using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;


namespace baza.Forms
{
    public partial class FormAddNewSqlTemplateCode : Form
    {
        public FormAddNewSqlTemplateCode()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        private void writeData()
        {
            if (this.textBox1.Text.Length > 0 && this.textBox2.Text.Length > 0)
            {

                NpgsqlCommand ins = new NpgsqlCommand("insert into codes_sql (code_text, code_change, code_description) values "+
                   " ('" + this.textBox1.Text.Trim() + "','" + this.textBox2.Text.Trim() + "','" + this.textBox3.Text.Trim() + "') ;", DBExchange.Inst.connectDb);
                try
                {
                    ins.ExecuteNonQuery();
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog(); 
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                    
                }


            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeData();
        }


    }
}
