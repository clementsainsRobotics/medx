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
    public partial class FormCreateTrial : Form
    {

        /// <summary>
        /// trial_descr
        /// </summary>
        public FormCreateTrial()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormCreateTrial.ActiveForm.Close();
        }

        private void writeTrial()
        {
            string name = this.textBox2.Text.Trim();
            string nickname = this.textBox1.Text.Trim();
            string incl = this.textBox3.Text.Trim();
            DateTime start = this.dateTimePicker1.Value;
            DateTime stop = this.dateTimePicker2.Value;
            string trial_number = this.textBox7.Text.Trim();
            string sponsor = this.textBox4.Text.Trim();
            string cro = this.textBox5.Text.Trim();
            string cra = this.textBox6.Text.Trim();
            DateTime dataz = this.dateTimePicker4.Value;
            DateTime datar = this.dateTimePicker3.Value;
            DateTime fin = this.dateTimePicker5.Value;
            string num = this.textBox8.Text.Trim();
            string com = this.richTextBox2.Text.Trim();

            NpgsqlCommand insTrial = new NpgsqlCommand("insert into trial_descr (name, nickname, inclusion, start, stop, trial_number, sponsor, cro, cra, "+
                "data_zayavki, data_reshenia, number_to_incl, fin, comment, doc_id) VALUES ('" + name + "','" + nickname + "','" + incl + "','" + start
                + "','" + stop + "','" + trial_number + "','" + sponsor + "','" + cro + "','" + cra +"','" + dataz +"','" + datar +"','" + num +"','" + fin +"','" 
                + com +"','"+DBExchange.Inst.dbUsrId+"') ;", DBExchange.Inst.connectDb);

            try
            {
                insTrial.ExecuteNonQuery();
            }

            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " writeTrial");
            }
        
        
        }

        private bool checkTrialName(string _trialName)
        {
            bool gotTrial;
            gotTrial = true;


            return gotTrial;
        }


        private void textBox7_Enter(object sender, EventArgs e)
        {
            if (this.textBox7.Text.Trim() == "Номер исследования")
            {
                this.textBox7.Text = "";
            }

        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (this.textBox4.Text.Trim() == "Спонсор")
            {
                this.textBox4.Text = "";
            }
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            if (this.textBox8.Text.Trim() == "Количество включенных")
            {
                this.textBox8.Text = "";
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (this.textBox5.Text.Trim() == "CRO")
            {
                this.textBox5.Text = "";
            }
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            if (this.textBox6.Text.Trim() == "CRA")
            {
                this.textBox6.Text = "";
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeTrial();
            this.textBox2.Text = "";
            this.textBox1.Text = "";
            this.textBox3.Text = "";
        }


    }
}
