using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;

namespace trialDataApp
{
    public partial class MainForm : Form
    {
        public DataTable DTuserTrials;
        private DataTable DTtrialPatients;
        private int _gotTrial;
        private DataTable DTtrialTimePoints;

        public MainForm()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// Заполгяет список тсследований для пользователя
        /// </summary>
        /// <returns></returns>
        private List<string> loadUserTrials()
        {
            int _thisUser = DBExchange.Inst.dbUsrId;
            getTrialsForUser(_thisUser);
            List<string> ut = new List<string>();
            foreach (DataRow ro in DTuserTrials.Rows)
            {
                ut.Add((string)ro["nickname"] );
            }
            return ut;
        }

        
        /// <summary>
        /// Получает список пациентов в комбобокс формы
        /// </summary>
        private void refreshMainFormData()
        {

            this.comboBox3.DataSource = loadUserTrials();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Login FormLogin = new Login();
            FormLogin.ShowDialog();

        }
/// <summary>
/// Скрывает панель
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void скрытьМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideLeftPanel();
        }
        /// <summary>
        /// скрыть левую панель
        /// </summary>
        private void hideLeftPanel()
        {
            if (this.groupBoxLeft.Visible == true)
            {
                this.groupBoxLeft.Visible = false;
                this.скрытьМенюToolStripMenuItem.Text = "Показать меню";
            }
            else
            {
                this.скрытьМенюToolStripMenuItem.Text = "Скрыть меню";
                this.groupBoxLeft.Visible = true;
            }

        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm.ActiveForm.Close();
        }


        /// <summary>
        /// Создаёт таблицу исследований для пользователя
        /// </summary>
        /// <param name="_thisUser"></param>
        private void getTrialsForUser(int _thisUser)
        {
            _thisUser = DBExchange.Inst.dbUsrId;
            DTuserTrials = new DataTable();
            NpgsqlDataAdapter getTrial = new NpgsqlDataAdapter("Select tr.trial_id as tr_id, trim(td.nickname) as nickname from " +
            "trial_role tr INNER JOIN trial_descr td ON td.trial_id = tr.trial_id where tr.doc_id = " + _thisUser + " ;", DBExchange.Inst.connectDb);
            try
            {
                getTrial.Fill(DTuserTrials);

            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + "");
            }

        }


        /// <summary>
        /// Заполняет список пациентов из таблицы 
        /// </summary>
        /// <param name="_thisTrial"></param>
        /// <returns></returns>
        private List<String> loadTrialPatients(int _thisTrial)
        {
            
            List<String> _trialPAtients = new List<string>();
            getPatientsForTrial(_thisTrial);
            if (DTtrialPatients.Rows.Count > 0)
            {
                foreach (DataRow row1 in DTtrialPatients.Rows)
                {
                    _trialPAtients.Add((string)row1["family_name"] + " " + (string)row1["pat_initials"] + " " + ((DateTime)row1["birth_date"]).Year);
                }
            }
            return _trialPAtients;
        }


        /// <summary>
        /// Создаёт таблицу пациентов для исследования
        /// </summary>
        /// <param name="_thisTrial"></param>

        private void getPatientsForTrial(int _thisTrial)
        {
            
            DTtrialPatients = new DataTable();
            NpgsqlDataAdapter getPAtients = new NpgsqlDataAdapter("Select tpil.pat_id, trim(tpil.pat_initials) as pat_initials," 
                +" trim(pl.family_name) as family_name, pl.birth_date as birth_date from " +
            "trial_pat_id_list tpil JOIN patient_list pl ON tpil.pat_id = pl.pat_id where tpil.trial_id = " + _thisTrial + " ;", DBExchange.Inst.connectDb);
            try
            {
                getPAtients.Fill(DTtrialPatients);
            }

            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + "");
            }

        }

        private void редактироватьВизитыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.FormVisitEditor fve = new Forms.FormVisitEditor(DTuserTrials);
            fve.ShowDialog();
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            refreshMainFormData();
        }

        private void исследованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.FormCreateTrial fct = new trialDataApp.Forms.FormCreateTrial();
            fct.ShowDialog();
        }

        private void исследовательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.FormNewResearcher fnr = new Forms.FormNewResearcher();
            fnr.ShowDialog();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox3.Items.Count > 0)
            {
                _gotTrial = (int)DTuserTrials.Rows[this.comboBox3.SelectedIndex]["tr_id"];
                this.listBox13.DataSource = loadTrialPatients(_gotTrial);
                getFirstNodes();
            }

        }

        private void участникаИсследованияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.FormPatientIdList fpil = new trialDataApp.Forms.FormPatientIdList();
            fpil.ShowDialog();
        }



        /// <summary>
        /// Получает первую строку нодов, точки исследования для выбранного исследования
        /// </summary>
        private void getFirstNodes()
        {
            List<string> firstNode = getTrialPoints(_gotTrial);
            this.treeView1.BeginUpdate();
            foreach (string i in firstNode)
            {
                this.treeView1.Nodes.Add(i);
            }
         //   this.treeView1.MouseClick += new MouseEventHandler(beginGetNodeData);
            this.treeView1.Update();
            this.treeView1.EndUpdate();
            this.treeView1.Refresh();


        }

        private List<string> getTrialPoints(int _thisTrial)
        {                                                 

            List<string> triPo = new List<string>();
            NpgsqlDataAdapter gtp = new NpgsqlDataAdapter("Select * from trial_time_points where trial_id ='" + _thisTrial + "'", DBExchange.Inst.connectDb);
            DTtrialTimePoints = new DataTable();

            try
            {
                gtp.Fill(DTtrialTimePoints);
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name);
            }

            foreach (DataRow ro in DTtrialTimePoints.Rows )
            {
                triPo.Add((string)ro["point_name"]);
            }

            return triPo;
        }


        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {

                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        // NOTE   This code can be added to the BeforeCheck event handler instead of the AfterCheck event.
        // After a tree node's Checked property is changed, all its child nodes are updated to the same value.
        private void node_AfterCheck(object sender, TreeViewEventArgs e)
        {

            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {

                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }


        //Получение списка диагнозов при выборе соответствующей записи
        private void beginGetNodeData(object sender, EventArgs e)
        {


            //if (this.treeView1.SelectedNode.Level == 0 && this.treeView1.SelectedNode.GetNodeCount(true) < 1)
            //{
            //    Int16 selNodeIndx = Convert.ToInt16(this.treeView1.SelectedNode.Index);
            //    Cursor.Current = Cursors.WaitCursor;
            //   // List<string> diagSubList = _tabDiagEnv.getDiagSubGroupFromBase(selNodeIndx);
            //    this.treeView1.BeginUpdate();

            //    foreach (string i in diagSubList)
            //    {

            //        treeView1.Nodes[selNodeIndx].Nodes.Add(i);


            //    }
            //    Cursor.Current = Cursors.Default;

            //    this.treeView1.Update();
            //    this.treeView1.EndUpdate();

            //    this.treeView1.Refresh();

            //}
            //else if (this.treeView1.SelectedNode.Level == 1 && this.treeView1.SelectedNode.GetNodeCount(true) < 1)
            //{
            //    Int16 selNodeIndx = Convert.ToInt16(this.treeView1.SelectedNode.Index);
            //    Cursor.Current = Cursors.WaitCursor;
            //    Int16 selGrpIndx = Convert.ToInt16(this.treeView1.SelectedNode.Parent.Index);

            //    List<string> diagMkbList = _tabDiagEnv.getMkbDiagFromBase(_tabDiagEnv.getDiagSubGroupIntFromBase(selGrpIndx)[selNodeIndx]);
            //    this.treeView1.BeginUpdate();
            //    var k = 0;
            //    foreach (string i in diagMkbList)
            //    {

            //        TreeNode newNode = new TreeNode();
            //        newNode.Text = i;
            //        newNode.Tag = _tabDiagEnv.diagNumList[k];
            //        treeView1.Nodes[selGrpIndx].Nodes[selNodeIndx].Nodes.Add(newNode);
            //        this.treeView1.CheckBoxes = true;
            //        k++;
            //    }
            //    Cursor.Current = Cursors.Default;

            //    this.treeView1.Update();
            //    this.treeView1.EndUpdate();

            //    this.treeView1.Refresh();

            //}

            //else if (this.treeView1.SelectedNode.Checked == true && this.treeView1.SelectedNode.Level == 2)
            //{

            //}


        }

        private void закрытьПрограммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm.ActiveForm.Close();
        }



    }
}
