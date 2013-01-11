using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace trialDataApp.Forms
{
    public partial class FormVisitEditor : Form
    {
        private DataTable dtTrialPoints;
        private DataTable dtTrialForms;
        private DataTable dtUserTrials;
        private List<string> lstTrials;
        private int gotTrialSelected;
        /// <summary>
        /// Trial_Time_points
        /// выбирать список форм для исследования
        /// </summary>

        public FormVisitEditor(DataTable userTrials)
        {
            InitializeComponent();
            this.comboBox3.DataSource = getUserTrials(userTrials);
        }


        /// <summary>
        /// Получает точки исследования
        /// </summary>
        /// <param name="_thisTrial"></param>
        /// <returns></returns>
        private DataTable getPointsForTrial(int _thisTrial)
        {
            dtTrialPoints = new DataTable();
            gotTrialSelected = _thisTrial;

            NpgsqlDataAdapter getForms = new NpgsqlDataAdapter("Select point_id, point_name from trial_time_points " +
                " where trial_id = '" + _thisTrial + "' ;", DBExchange.Inst.connectDb);
            try
            {
                getForms.Fill(dtTrialPoints);
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " getFormsForTrial");
            }
           
                return dtTrialPoints;

        }

        /// <summary>
        /// Грузит точки в контррол
        /// </summary>
        /// <param name="_thisTrial"></param>
        private void loadPoints(int _thisTrial)
        {
            DataTable gotPoints = getPointsForTrial(_thisTrial);
            this.comboBox1.Items.Clear();
            if (gotPoints != null)
            {
                foreach (DataRow ro in gotPoints.Rows)
                {
                    this.comboBox1.Items.Add((string)ro["point_name"]);
                }
            }
            this.comboBox1.Items.Add("Добавить новую точку ...");
            
        }

        /// <summary>
        /// Поплучает список исследований
        /// </summary>
        /// <param name="ust"></param>
        /// <returns></returns>
        private List<string> getUserTrials(DataTable ust)
        {
            dtUserTrials = new DataTable();
            dtUserTrials = ust;
            lstTrials = new List<string>();
            foreach (DataRow ro in dtUserTrials.Rows)
            {
                lstTrials.Add((string)ro["nickname"]);
            }
            return lstTrials;
        }


        /// <summary>
        /// Получает форрмы для исследования
        /// </summary>
        /// <param name="_thisTrial"></param>
        /// <returns></returns>
        private DataTable getFormsForTrial(int _thisTrial)
        {
            dtTrialForms = new DataTable();

            NpgsqlDataAdapter getForms = new NpgsqlDataAdapter("Select docum_type_did, dt.descr as descr from trial_forms tf JOIN docum_type dt "+
                "ON tf.docum_type_did = dt.did where point_id = "+_thisTrial+" ;",DBExchange.Inst.connectDb);
            try
            {
                getForms.Fill(dtTrialForms);
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " getFormsForTrial");
            }
            return dtTrialForms;
        }




        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox3.Items.Count > 0)
            {
                int fp = this.comboBox3.SelectedIndex;
                loadPoints((int)dtUserTrials.Rows[fp]["tr_id"]);
            }


        }


        /// <summary>
        /// Загрузка форм в контрол
        /// </summary>
        /// <param name="_thisTrial"></param>
        private void loadForms(int _thisTrial)
        {
            DataTable gotForms = getFormsForTrial(_thisTrial);
            this.listBox1.Items.Clear();
            foreach (DataRow ro in gotForms.Rows)
            {
                this.listBox1.Items.Add((string)ro["descr"]);
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormVisitEditor.ActiveForm.Close();
        }

        /// <summary>
        /// Загрузка списка форм исследования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "Добавить новую точку ...")
            {
                OneStringInputForm osif = new OneStringInputForm("Запись новой точки исследования","Введите название новой точки",1,gotTrialSelected);
                osif.ShowDialog();
                loadPoints(gotTrialSelected);
            }
                     
            else if (this.comboBox1.Items.Count > 1)
            {

                int fp = this.comboBox1.SelectedIndex;
                loadForms((int)dtTrialPoints.Rows[fp]["point_id"]);
            }

        }



        private void FormVisitEditor_Load(object sender, EventArgs e)
        {

        }
        private void writePointForms()
        {
            if (this.comboBox1.SelectedIndex > 0)
            {
                Int64 _point = (Int64)dtTrialPoints.Rows[this.comboBox1.SelectedIndex]["point_id"];

            }
            

        }




    }
}
