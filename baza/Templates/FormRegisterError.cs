using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace baza.Templates
{
    public partial class FormRegisterError : Form
    {
        bool InsInBase = false;
        int errorLogId;
        public FormRegisterError(bool _gotDB, int errId)
        {
            InitializeComponent();
            InsInBase = _gotDB;
            errorLogId = errId;          
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text.Trim().Length > 2)
            {

                if (InsInBase == true)
                {

                    Npgsql.NpgsqlCommand nci = new Npgsql.NpgsqlCommand("update error_log set user_descr = '" + this.richTextBox1.Text.Trim() + "' where err_id = '"+errorLogId+"' ", DBExchange.Inst.connectDb);

                    nci.ExecuteNonQuery();
                }
                else
                {


                    if (!(System.IO.Directory.Exists(Application.StartupPath + "\\Errors\\")))
                    {

                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Errors\\");

                    }

                    FileStream fs = new FileStream(Application.StartupPath + "\\Errors\\errlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    StreamWriter s = new StreamWriter(fs);

                    s.Close();

                    fs.Close();

                    FileStream fs1 = new FileStream(Application.StartupPath + "\\Errors\\errlog.txt", FileMode.Append, FileAccess.Write);

                    StreamWriter s1 = new StreamWriter(fs1);

                    s1.Write("Date/Time: " + DateTime.Now.ToString());

                    s1.Write("\nКомментарий: " + this.richTextBox1.Text.Trim());





                    s1.Write("\n=========================================================================================== \n\n");

                    s1.Close();

                    fs1.Close();
                }
                
            }
            FormRegisterError.ActiveForm.Close();
        }
    }
}
