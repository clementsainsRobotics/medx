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
    public partial class FormTumorDetails : Form
    {
        public FormTumorDetails()
        {
            InitializeComponent();
        }


        private void clearForm()
        {

            this.checkBox1.Checked = true;
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.richTextBox1.Text = "Комментарий";

        }

        private void writeData()
        {
            
            try
            {

                short _number = Convert.ToInt16(this.textBox5.Text.Trim());
                decimal _l = Convert.ToDecimal(this.textBox1.Text.Trim());
                decimal _s = Convert.ToDecimal(this.textBox2.Text.Trim());
                decimal _h = Convert.ToDecimal(this.textBox4.Text.Trim());

                string _les_descr = this.textBox3.Text.Trim();
                string _comment = this.richTextBox1.Text.Trim();

                bool _measure = this.checkBox1.Checked;
                int _pat_id = this.searchPatientBox1.pIdN;

                int _method = 0; //?
                int _region = 0; //?
                int _diag = 0;

                int _eval_id = 0;

                DateTime _date = this.dateTimePicker1.Value;
             

                NpgsqlCommand _command = new NpgsqlCommand("Insert into tumor_size_details (l, s, h, lesion_descr, date, method, number, measurable, comment, eval_id, diag_data_id, region, pat_id, doc_id) values "
                    + "( '" + _l+ "','" + _s + "','" + _h + "', '" + _les_descr + "','" + _date + "','" + _method + "','" + _number + "', '"+_measure+"' , :descr , '"+_eval_id+"' , '"+_diag+"' , '"+_region+"' , '"+_pat_id
                    +"' , '" + DBExchange.Inst.dbUsrId + "') ;", DBExchange.Inst.connectDb);
                using (_command)
                {
                    _command.Parameters.Add(new NpgsqlParameter("descr", NpgsqlDbType.Text));
                    _command.Parameters[0].Value = _comment;
                }

                _command.ExecuteNonQuery();
              
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                

            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormTumorDetails.ActiveForm.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeData();
            clearForm();
        }
    }
}
