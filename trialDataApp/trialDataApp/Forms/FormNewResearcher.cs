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
    public partial class FormNewResearcher : Form
    {
        private DataTable DTuserTrials;
        private DataTable DTTrialDocs;
        private DataTable DTStatus;
        private DataTable DTDoctors;
        private List<string> fDoc;
        int gotTrial;

        public FormNewResearcher()
        {
            InitializeComponent();
            loadStatusList();

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormNewResearcher.ActiveForm.Close();
        }

        private void getTrialsForUser()
        {
            int _thisUser = DBExchange.Inst.dbUsrId;
            DTuserTrials = new DataTable();
            NpgsqlDataAdapter getTrial = new NpgsqlDataAdapter("Select tr.trial_id as tr_id, trim(td.nickname) as nickname from " +
            "trial_role tr INNER JOIN trial_descr td ON td.trial_id = tr.trial_id where tr.doc_id = " + _thisUser + " and tr.role < 3;", DBExchange.Inst.connectDb);
            try
            {
                getTrial.Fill(DTuserTrials);

            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " getTrialsForUser");
            }

        }

        /// <summary>
        /// Заполняет список исследований для пользователя
        /// </summary>
        /// <returns></returns>
        private List<string> loadUserTrials()
        {
            
            getTrialsForUser();
            List<string> ut = new List<string>();
            foreach (DataRow ro in DTuserTrials.Rows)
            {
                ut.Add((string)ro["nickname"]);
            }
            return ut;
        }


        private List<string> getFreeDoctors()
        {

            DTDoctors = new DataTable();
            string gfd = "Select doc_id, trim(family_name) as family_name, trim(first_name) as first_name from doctors where special < 4";
            fDoc = new List<string>();
            if (this.comboBox1.Items.Count > 0)
            {
                int _trial = (int)DTuserTrials.Rows[this.comboBox1.SelectedIndex]["tr_id"];
                gfd = "Select doc_id, trim(family_name) as family_name, trim(first_name) as first_name from doctors ds where ds.special < 4 and ds.doc_id not in "
                +"(select doc_id from trial_role where trial_id = '"+_trial+"')";
            }

            NpgsqlDataAdapter getfd = new NpgsqlDataAdapter(gfd,DBExchange.Inst.connectDb);
            try
            {
                getfd.Fill(DTDoctors);


            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " getFreeDoctors");
            }

            foreach (DataRow ro in DTDoctors.Rows)
            {
                fDoc.Add(ro["family_name"] + " " + ro["first_name"]);

            }

            return fDoc;

        }

        private List<string> loadTrialDocs(int _thisTrial)
        {
            List<string> ltd = new List<string>();
            DTTrialDocs = new DataTable();

            NpgsqlDataAdapter getltd = new NpgsqlDataAdapter("Select tr.doc_id as doc_id, tr.role as role_id, trim(ds.family_name) as faname, trim(ds.first_name) as finame, " +
            "trim(ds.last_name) as lname from trial_role tr, doctors ds where tr.trial_id = '"
            + _thisTrial + "' and ds.doc_id = tr.doc_id ;", DBExchange.Inst.connectDb);
            try
            {
                getltd.Fill(DTTrialDocs);

            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " loadTrialDocs");
            }
                foreach (DataRow ro in DTTrialDocs.Rows)
                {
                    ltd.Add((string)ro["faname"] + " " + (string)ro["finame"]);
                }
            

            return ltd;
        }


        private void loadStatusList()
        {
            DTStatus = new DataTable();
            NpgsqlDataAdapter getStatus = new NpgsqlDataAdapter("Select trim(txt) as txt, code from codes where grp=1 ;",DBExchange.Inst.connectDb );
            getStatus.Fill(DTStatus);

            foreach (DataRow ro in DTStatus.Rows)
            {
                comboBox3.Items.Add(ro["txt"]);
            }

        }

        private void loadTrialsInForm()
        {
            this.comboBox1.DataSource = loadUserTrials();
            
        }

        private void getResearchers(int _thisTrial)
        {
            gotTrial = _thisTrial;

            this.listBox1.DataSource = loadTrialDocs(_thisTrial);
        }

        private void FormNewResearcher_Load(object sender, EventArgs e)
        {
            loadTrialsInForm();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Items.Count > 0)
            {
                int _trial = (int)DTuserTrials.Rows[this.comboBox1.SelectedIndex]["tr_id"];
                getResearchers(_trial);
                this.comboBox2.Items.Clear();
                this.comboBox2.DataSource = getFreeDoctors();
            }
        }


        private void writeDocToTrial()
        {
            if (this.comboBox3.SelectedIndex != -1)
            {
                int trial = gotTrial;
                int doc = (int)DTDoctors.Rows[this.comboBox2.SelectedIndex]["doc_id"];
                DateTime start = this.dateTimePicker1.Value;
                DateTime end = this.dateTimePicker2.Value;
                Int16 role = (Int16)DTStatus.Rows[this.comboBox3.SelectedIndex]["code"];

                NpgsqlCommand wdtt = new NpgsqlCommand("Insert into trial_role (trial_id, doc_id, role, start_role, end_role)"
                    + " VALUES ('" + trial + "','" + doc + "','" + role + "','" + start + "','" + end + "')", DBExchange.Inst.connectDb);
                try
                {
                    wdtt.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " writeDocToTrial");
                }
                getResearchers(trial);
                this.comboBox2.DataSource = getFreeDoctors();
            }
            else
            { System.Windows.Forms.MessageBox.Show("Выберите роль пользователя в исследовании"); }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeDocToTrial();
        }

    }
}
