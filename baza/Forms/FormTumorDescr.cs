using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using System.Reflection;

namespace baza.Forms
{
    public partial class FormTumorDescr : Form
    {
        public FormTumorDescr()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormTumorDescr.ActiveForm.Close();
        }

        private void clearForm()
        {

            foreach (int i in checkedListBox1.CheckedIndices)
            {
                checkedListBox1.SetItemChecked(i, false);
            }

            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.textBox6.Text = "";
            this.richTextBox1.Text = "Комментарий";
            this.dateTimePicker1.Value = DateTime.Now;


        }

        private void insertIntoBase()
        {
            try
            {

            short total = Convert.ToInt16( this.textBox1.Text.Trim());
            int s_sum = Convert.ToInt32(this.textBox2.Text.Trim());
            short d_sum = Convert.ToInt16(this.textBox3.Text.Trim());

            short measure = Convert.ToInt16(this.textBox4.Text.Trim());
            int _s = Convert.ToInt32(this.textBox5.Text.Trim());
            short _d = Convert.ToInt16(this.textBox6.Text.Trim());

            string _comm = this.richTextBox1.Text.Trim();

            NpgsqlCommand _command = new NpgsqlCommand("Insert into tumor_size (total, d_sum, s_sum, s, d, measurable, pat_id, rasp_data, got_comment, brain, kidney, skin, bones, bone_marrow, lung, lymphatic, adrenal, os, hepar, lien, other, doc_id) values "
                + "( '" + total + "','" + d_sum + "','" + s_sum + "', '" + _s + "','" + _d + "','" + measure + "','" + this.searchPatientBox1.pIdN + "','" + this.dateTimePicker1.Value + "', :descr ,'" + this.checkedListBox1.GetItemChecked(0)
                + "','" + this.checkedListBox1.GetItemChecked(1) + "','" + this.checkedListBox1.GetItemChecked(2) + "','" + this.checkedListBox1.GetItemChecked(3) + "','" + this.checkedListBox1.GetItemChecked(4) + "','" + this.checkedListBox1.GetItemChecked(5)
                + "','" + this.checkedListBox1.GetItemChecked(6) + "','" + this.checkedListBox1.GetItemChecked(7) + "','" + this.checkedListBox1.GetItemChecked(8) + "','" + this.checkedListBox1.GetItemChecked(9) + "','" + this.checkedListBox1.GetItemChecked(10) 
                + "','" + this.checkedListBox1.GetItemChecked(11) + "' , '" + DBExchange.Inst.dbUsrId + "') ;", DBExchange.Inst.connectDb);
                using (_command)
                {
                    _command.Parameters.Add(new NpgsqlParameter("descr", NpgsqlDbType.Text));
                    _command.Parameters[0].Value = _comm;
                }

                _command.ExecuteNonQuery();
                clearForm();
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
               
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            insertIntoBase();
        }


    }
}
