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
    public partial class FormAddNewSQLTemplateDocument : Form
    {

        int gotTemplate = -1;
        public FormAddNewSQLTemplateDocument(int _template)
        {
            InitializeComponent();
            gotTemplate = _template;
            if (gotTemplate > 0)
            {
                loadTemplateData(gotTemplate);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        void writeData()
        {
            if (this.textBox1.Text.Length > 0)
            {
                string _command = "";
                if (gotTemplate > 0)
                {
                    _command = "Update codes_sql_templates set template_text = '" + this.richTextBox1.Text.Trim() + "', template_file = '" + this.textBox2.Text.Trim() + 
                        "', template_description = '" + this.textBox1.Text.Trim() + "', template_type = 1 where template_id = '"+gotTemplate+"' ;";
                }
                else
                {
                    _command = "insert into codes_sql_templates (template_text, template_file, template_description, template_type) values " +
                   " ('" + this.richTextBox1.Text.Trim() + "','" + this.textBox2.Text.Trim() + "','" + this.textBox1.Text.Trim() + "', '1') ;";
                }
                NpgsqlCommand ins = new NpgsqlCommand(_command, DBExchange.Inst.connectDb);
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

        void loadTemplateData(int _template)
        {
            string _command = "Select * from codes_sql_templates where template_id = '"+_template+"' ;";

            try
            {
                NpgsqlDataAdapter getTemplate = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                DataTable gt = new DataTable();
                getTemplate.Fill(gt);
                this.richTextBox1.Text = (string)gt.Rows[0]["template_text"];
                this.textBox2.Text = (string)gt.Rows[0]["template_file"];
                this.textBox1.Text = (string)gt.Rows[0]["template_description"];
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            this.textBox2.Text = openFileDialog1.FileName;
            WebBrowser wbC = new WebBrowser();

            wbC.Navigate(this.openFileDialog1.FileName);
            this.richTextBox1.Text = wbC.DocumentText;

            Form prFC = new printFormGist(this.richTextBox1.Text.Trim(), false, false);
            prFC.Show();


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text.Trim().Length > 0)
            {
                
                Form prFC = new printFormGist(this.richTextBox1.Text.Trim(), false, false);
                prFC.Show();
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeThisTemplate();
        }

        void writeThisTemplate()
        {

            if (this.textBox1.Text.Length > 0)
            {

                string _txt = "";
                if (textBox2.Text.Length == 0)
                {
                    _txt += "<head><title>" + textBox1.Text.Trim() + "</title><meta http-equiv='content-type' content='text/html; charset=utf-8' /></head>";
                    _txt += this.richTextBox1.Text.Trim();

                    _txt = _txt.Replace("\n", "<br>");
                }
                else
                {
                    _txt = this.richTextBox1.Text.Trim();
                }

                NpgsqlCommand ins = new NpgsqlCommand("insert into codes_sql_templates (template_text, template_file, template_description) values " +
                   " (:newtext ,'" + this.textBox2.Text.Trim() + "','" + this.textBox1.Text.Trim() + "') ;", DBExchange.Inst.connectDb);
                ins.Parameters.Add(new NpgsqlParameter("newtext", NpgsqlDbType.Text));
                ins.Parameters[0].Direction = ParameterDirection.Input;
                ins.Parameters[0].Value = _txt;
                try
                {
                    ins.ExecuteNonQuery();
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                    this.richTextBox1.Text = "";
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }


            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            updateData();
        }

        void updateData()
        {
            if (this.textBox1.Text.Length > 0)
            {
                string _command = "";
                string _txt = this.richTextBox1.Text.Trim();

                if (textBox2.Text.Length == 0)
                {
                    _txt = _txt.Replace("\n", "<br>");
                }
                    //_command = "Update codes_sql_templates set template_text = '" + _txt + "', template_file = '" + this.textBox2.Text.Trim() +
                    //    "', template_description = '" + this.textBox1.Text.Trim() + "', template_type = 1 where template_id = '" + gotTemplate + "' ;";
                    NpgsqlCommand ins = new NpgsqlCommand("Update codes_sql_templates set template_text = :newtext, template_file = '" + this.textBox2.Text.Trim() +
                            "', template_description = '" + this.textBox1.Text.Trim() + "', template_type = 1 where template_id = '" + gotTemplate + "' ;", DBExchange.Inst.connectDb);
                ins.Parameters.Add(new NpgsqlParameter("newtext", NpgsqlDbType.Text));
                ins.Parameters[0].Direction = ParameterDirection.Input;
                ins.Parameters[0].Value = _txt;
                try
                {
                    ins.ExecuteNonQuery();
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                    this.richTextBox1.Text = "";
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }


            }

        }
    }


}
