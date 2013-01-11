using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
namespace baza
{
    public partial class addNewSchemeForm : Form
    {
        /// <summary>
        /// Обработка данных для формы добавления новой схемы
        /// </summary>

        Warnings.WarnMessages warnMess = new Warnings.WarnMessages();
        tabDiagEnv _tabDiagEnv = new tabDiagEnv();
        tabSurgeryEnv _tabSurgeryEnv = new tabSurgeryEnv();
        private int selectedSchemeId;
        public DataTable tlSchemeList;
        public DataTable tlSelDiags;
        public DataTable tlSelDrugs;

        public addNewSchemeForm()
        {
            InitializeComponent();
            this.comboBox1.DataSource = fillSchemeList();
            tlSelDiags = new DataTable();
            tlSelDiags.Columns.Add("scheme", System.Type.GetType("System.Int32"));
            tlSelDiags.Columns.Add("diag", System.Type.GetType("System.Int32"));
            tlSelDrugs = new DataTable();
            tlSelDrugs.Columns.Add("scheme", System.Type.GetType("System.Int32"));
            tlSelDrugs.Columns.Add("drug", System.Type.GetType("System.Int32"));
        }

        /// <summary>
        /// Заполняет табы схемы
        /// </summary>
        private void fillShemeDataTab()
        {
            List<string> diagList = _tabDiagEnv.getDiagGroupFromBase();
            foreach (string i in diagList)
            {
             
                this.treeView1.Nodes.Add(i);


            }
            this.treeView1.MouseClick += new MouseEventHandler(beginGetNodeData);
            
          
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


            if (this.treeView1.SelectedNode.Level == 0 && this.treeView1.SelectedNode.GetNodeCount(true) < 1)
            {
                Int16 selNodeIndx = Convert.ToInt16(this.treeView1.SelectedNode.Index);
                Cursor.Current = Cursors.WaitCursor;
                List<string> diagSubList = _tabDiagEnv.getDiagSubGroupFromBase(selNodeIndx);
                this.treeView1.BeginUpdate();
          
                foreach (string i in diagSubList)
                {

                    treeView1.Nodes[selNodeIndx].Nodes.Add(i);
                 
                    
                }
                Cursor.Current = Cursors.Default;

                this.treeView1.Update();
                this.treeView1.EndUpdate();

                this.treeView1.Refresh();

            }
            else if (this.treeView1.SelectedNode.Level == 1 && this.treeView1.SelectedNode.GetNodeCount(true) < 1)
            {
                Int16 selNodeIndx = Convert.ToInt16(this.treeView1.SelectedNode.Index);
                Cursor.Current = Cursors.WaitCursor;
                Int16 selGrpIndx = Convert.ToInt16(this.treeView1.SelectedNode.Parent.Index);

                List<string> diagMkbList = _tabDiagEnv.getMkbDiagFromBase(_tabDiagEnv.getDiagSubGroupIntFromBase(selGrpIndx)[selNodeIndx]);
                this.treeView1.BeginUpdate();
                var k=0;
                foreach (string i in diagMkbList)
                {
                    
                    TreeNode newNode = new TreeNode();
                    newNode.Text = i;
                    newNode.Tag = _tabDiagEnv.diagNumList[k];
                    treeView1.Nodes[selGrpIndx].Nodes[selNodeIndx].Nodes.Add(newNode);
                    this.treeView1.CheckBoxes = true;
                    k++;
                }
                Cursor.Current = Cursors.Default;

                this.treeView1.Update();
                this.treeView1.EndUpdate();

                this.treeView1.Refresh();

            }

            else if (this.treeView1.SelectedNode.Checked == true && this.treeView1.SelectedNode.Level == 2)
            {

            }
            

        }
        //Заполняет список всех лекарств для отметок
        private void fillSchemeDrugsCheckListBox()
        {
            if (this.checkedListBox1.Items.Count < 1)
            {
                Cursor.Current = Cursors.WaitCursor;
                List<string> drugList = _tabSurgeryEnv.fillSchemeAllDrugs();
                
                foreach (string i in drugList)
                {
                    
                    checkedListBox1.Items.Add(i);
                   
                }
                Cursor.Current = Cursors.Default;
            }

        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            fillShemeDataTab();
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            fillSchemeDrugsCheckListBox();
        }

        //Запись схемы в базу
        private void writeSchemeIntoBase()
        {
            NpgsqlCommand insEtapData = new NpgsqlCommand("insert into drug_scheme_descr (scheme_name, chemotherapy, immunotherapy, hormones, signal_transduction_inhibitors, monoclonals, tyrosinkinase_inhibitors) values ('"
                + this.textBox1.Text.Trim() + "','" + this.checkedListBox2.GetItemChecked(0) + "','" + this.checkedListBox2.GetItemChecked(1) + "','" + this.checkedListBox2.GetItemChecked(2) + "','"
                + this.checkedListBox2.GetItemChecked(3) + "','" + this.checkedListBox2.GetItemChecked(4) + "','" + this.checkedListBox2.GetItemChecked(5) + "');", DBExchange.Inst.connectDb);

            try
            {
                insEtapData.ExecuteNonQuery();

            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " writeSchemeIntoBase");
            }
        }

        //Заполнение списка схем
        public List<string> fillSchemeList()
        {
            List<string> drugScheme = new List<string>();

            tlSchemeList = new DataTable();
            NpgsqlDataAdapter selDrugs = new NpgsqlDataAdapter("Select * from drug_scheme_descr order by scheme_name Asc ", DBExchange.Inst.connectDb);
      

            try
            {
                selDrugs.Fill(tlSchemeList);


                foreach (DataRow row in tlSchemeList.Rows)
                {
                    drugScheme.Add((string)row["scheme_name"]);
                }

            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " fillSchemeList");
            }


            return drugScheme;
        }

        //Проверка перед записью схемы

        

        private void button1_Click(object sender, EventArgs e)
        {
            string newSchemeName = textBox1.Text;
            DataRow[] checkSchemeRows = tlSchemeList.Select("scheme_name = '"+ newSchemeName +"'", "");

            if (checkSchemeRows.Length < 1)
            {
                writeSchemeIntoBase();
                this.comboBox1.DataSource = fillSchemeList();
                int schItemIx = this.comboBox1.Items.IndexOf(newSchemeName);
                this.comboBox1.SelectedIndex = schItemIx;
            }
            else
            {
                warnMess.warnGotThisScheme();
            }


        }



        //Добавляет диагноз схемы из таблицы отмеченных в списке диагнозов
        private void addDiagToScheme()
        {
            

            foreach (TreeNode node in treeView1.Nodes)
            {
                foreach (TreeNode node2 in node.Nodes)
                {
                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        if (node3.Checked == true)
                        {
                            DataRow row = tlSelDiags.NewRow();
                            row["scheme"] = (int)this.selectedSchemeId;
                            row["diag"] = node3.Tag.ToString();
                            tlSelDiags.Rows.Add(row);
                        }
                    }
                }


            }

            NpgsqlTransaction beTr = DBExchange.Inst.connectDb.BeginTransaction();

            foreach (DataRow row in tlSelDiags.Rows )
                    {
                    NpgsqlCommand insDiagSch = new NpgsqlCommand("insert into scheme_diag_relation (scheme_id, diag_id) values ('"
                         + row["scheme"] + "','" + row["diag"] + "');", DBExchange.Inst.connectDb);

                        try
                        {
                            insDiagSch.ExecuteNonQuery();

                        }
                        catch (Exception exception)
                        {
                            System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " addDiagToScheme");
                        }
            }
            beTr.Commit();

            tlSelDiags.Clear();
        }

        public void addDrugToScheme()
        {
            for (int i = 0; i < checkedListBox1.Items.Count - 1; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    DataRow row = tlSelDrugs.NewRow();
                    row["scheme"] = (int)this.selectedSchemeId;
                    row["drug"] = _tabSurgeryEnv.tlFullDrugList.Rows[i]["drug_id"] ;
                    tlSelDrugs.Rows.Add(row);
                }
            }

            NpgsqlTransaction beTr = DBExchange.Inst.connectDb.BeginTransaction();

            foreach (DataRow row in tlSelDrugs.Rows)
            {
                NpgsqlCommand insDrugSch = new NpgsqlCommand("insert into relation_scheme_drug (scheme_id, drug_id) values ('"
                     + row["scheme"] + "','" + row["drug"] + "');", DBExchange.Inst.connectDb);

                try
                {
                    insDrugSch.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " addDrugToScheme");
                }
            }
            beTr.Commit();

            tlSelDrugs.Clear();

        }

        //Устанавливает индекс выбранной схемы
        private void setSchemeId()
        {

            int scItIx = this.comboBox1.SelectedIndex;
            selectedSchemeId = (int)tlSchemeList.Rows[scItIx]["scheme_id"];
            getSchemeDiagList(selectedSchemeId);
            getSchemeDrugList(selectedSchemeId);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            addNewSchemeForm.ActiveForm.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setSchemeId();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            addDiagToScheme();
        }

        //Получает список диагнозов для схемы и заполняет ими лист
        public void getSchemeDiagList(int gotScheme)
        {



        }
        //Получает список лекарств для выбранной схемы и нужно его передавать в листбокс
        public void getSchemeDrugList(int gotScheme)
        {
            tlSelDrugs = new DataTable();
            NpgsqlDataAdapter selSchemeDrugs = new NpgsqlDataAdapter("Select * from relation_scheme_drug where scheme_id = '"+gotScheme+"'order by drug_id Asc ", DBExchange.Inst.connectDb);


            try
            {
                selSchemeDrugs.Fill(tlSchemeList);


                foreach (DataRow row in tlSchemeList.Rows)
                {
                    
                }

            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " getSchemeDrugList");
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            addDrugToScheme();
        }



    }
}
