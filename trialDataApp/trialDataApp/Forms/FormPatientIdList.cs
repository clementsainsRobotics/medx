using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace trialDataApp.Forms
{
    public partial class FormPatientIdList : Form
    {
        private DataTable DTuserTrials;
        private DataTable DTtrialPatients;
        private int gotTrial;
        private int gotPatient;

        /// <summary>
        /// после логина получать список исследоований для участника добавлять пациентов и удалять
        /// trial_pat_id_list
        /// номер пациента инициалы группа фаза лечения рассчет по датам
        /// </summary>

        public FormPatientIdList()
        {
            InitializeComponent();
            loadTrialsInForm();
        }

        private void getTrialsForUser(int _thisUser)
        {
             
            DTuserTrials = new DataTable();
            NpgsqlDataAdapter getTrial = new NpgsqlDataAdapter("Select tr.trial_id as id, trim(td.nickname) as name from "+
            "trial_role tr INNER JOIN trial_descr td ON td.trial_id = tr.trial_id where tr.doc_id = "+ _thisUser +" ;", DBExchange.Inst.connectDb);
            try
            {
                getTrial.Fill(DTuserTrials);

            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " getTrialsForUser");
            }

        }

        private void loadTrialsInForm()
        {
            getTrialsForUser(DBExchange.Inst.dbUsrId);
            comboBox1.Items.Clear();
            foreach (DataRow row1 in DTuserTrials.Rows)
            {
                comboBox1.Items.Add((string)row1["name"]);
            }


        }

        private List<String> loadTrialPatients(int _thisTrial)
        {
            List<String> _trialPAtients = new List<string>();
            getPatientsForTrial(_thisTrial);
            foreach (DataRow row1 in DTtrialPatients.Rows)
            {

                _trialPAtients.Add((string)row1["family_name"] + " " + (string)row1["pat_initials"] + " " + ((DateTime)row1["birth_date"]).Year);
            }


            return _trialPAtients;
        }


        private void getPatientsForTrial(int _thisTrial)
        {

            DTtrialPatients = new DataTable();
            NpgsqlDataAdapter getPAtients = new NpgsqlDataAdapter("Select tpil.pat_id, tpil.pat_initials, pl.family_name, pl.birth_date from "+
            "trial_pat_id_list tpil JOIN patient_list pl ON tpil.pat_id = pl.pat_id where tpil.trial_id = "+_thisTrial+" ;",DBExchange.Inst.connectDb);
            try
            {
                getPAtients.Fill(DTtrialPatients);
                if (DTtrialPatients.Rows.Count > 0)
                {
                    this.dataGridView1.DataSource = DTtrialPatients;
                }

            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " getPatientsForTrial");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Items.Count > 0)
            {
                gotTrial = (int)DTuserTrials.Rows[this.comboBox1.SelectedIndex]["id"];
                getPatientsForTrial(gotTrial);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
             if (this.searchPatientBox1.pIdN > 0)
             {
                 writePatientInTrial();
             }
        }

        private void writePatientInTrial()
        {

            DateTime start = this.dateTimePicker3.Value;
            DateTime stop = this.dateTimePicker4.Value;
            DateTime offs = this.dateTimePicker5.Value;
            DateTime sign = this.dateTimePicker1.Value;
            string initials = this.searchPatientBox1.patInitials;

            NpgsqlCommand wpt = new NpgsqlCommand("Insert into trial_pat_id_list (pat_id, trial_id, start, stop, pat_initials, off_study, date_of_sign) VALUES ('"
                + this.searchPatientBox1.pIdN +"','"+ gotTrial +"','"+start+"','"+stop+"','"+initials+"','"+offs+"','"+sign+"')",DBExchange.Inst.connectDb);
            try
            {
                wpt.ExecuteNonQuery();
                getPatientsForTrial(gotTrial);
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " writePatientInTrial");
            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FormPatientIdList.ActiveForm.Close();
        }

    }
}
