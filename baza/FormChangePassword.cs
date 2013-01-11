using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


namespace baza
{
    public partial class FormChangePassword : Form
    {
        public FormChangePassword()
        {
            
            InitializeComponent();
            this.label5.Text = DBExchange.Inst.dBUserName.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormChangePassword.ActiveForm.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pass = this.textBox1.Text.Trim();
            try
            {
                if (textBox2.Text == textBox3.Text)
                {

                    if (DBExchange.Inst.checkUserPassword(pass) == true)
                    {
                        string password = textBox3.Text.Trim();
                        DBExchange.Inst.changePass(password);
                        FormChangePassword.ActiveForm.Close();
                        
                        
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Старый пароль введён не правильно");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Новый пароль не соответствует проверке");
                }
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                

            }


        }
    }
}
