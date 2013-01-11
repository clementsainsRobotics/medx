using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Linq;
using System.Text;
using Npgsql;
using System.Windows.Forms;

namespace trialDataApp
{
    public partial class Login : Form
    {


        public Login()
        {
            InitializeComponent();
            DBExchange.Inst.versionNumber = DBExchange.Inst.getAppVer();
                        
        }

        private void execOnFormLoad()
        {
         
            DBExchange.Inst.LoginSettings = new DataSet();
            DBExchange.Inst.LoginSettings.Tables.Add("login");
            DBExchange.Inst.LoginSettings.Tables.Add("server");
            DBExchange.Inst.LoginSettings.Tables["login"].Columns.Add("name");
            DBExchange.Inst.LoginSettings.Tables["server"].Columns.Add("ip");
            try
            {
                
                if (File.Exists("settings.xml"))
                {
                    
                    DBExchange.Inst.LoginSettings.ReadXml("settings.xml");
                    foreach (DataRow row in DBExchange.Inst.LoginSettings.Tables["login"].Rows)
                    {
                        DbLogin.Items.Add((string)row["name"]);

                    }
             
                    foreach (DataRow row1 in DBExchange.Inst.LoginSettings.Tables["server"].Rows)
                    {
                        DbServer.Items.Add((string)row1["ip"]);

                    }

                 }
                if (DbServer.Items.Count > 0)
                {
                    DbServer.SelectedIndex = 0;
                }
                if (DbLogin.Items.Count > 0)
                {
                    DbLogin.SelectedIndex = 0;
                }

                this.DbPass.Select();

            }
            catch
            {
            }
            finally { }

            
           




        }


        private void Login_Load(object sender, EventArgs e)
        {

            execOnFormLoad();
        }

        private void tryOpenDb()
        {
            
            DataTable tblLogin = DBExchange.Inst.LoginSettings.Tables["login"];
            DataTable tblServer = DBExchange.Inst.LoginSettings.Tables["server"];
            DataRow loginRow = tblLogin.NewRow();
            DataRow serverRow = tblServer.NewRow();
            try
            {
                DBExchange.Inst.DbInitConnection(DbServer.Text, DbLogin.Text, DbPass.Text);

                if (DBExchange.Inst.connectDb.State == ConnectionState.Open)
                {
                    loginRow["name"] = DbLogin.Text.ToLower();
                    if (DbLogin.Items.Contains(DbLogin.Text))
                    {
                        tblLogin.Rows.RemoveAt(DbLogin.SelectedIndex);
                        tblLogin.Rows.InsertAt(loginRow, 0);
                    }
                    else
                    {

                        tblLogin.Rows.InsertAt(loginRow, 0);
                    }

                    serverRow["ip"] = DbServer.Text;
                    if (DbServer.Items.Contains(DbServer.Text))
                    {
                        tblServer.Rows.RemoveAt(DbServer.SelectedIndex);
                        tblServer.Rows.InsertAt(serverRow, 0);
                    }
                    else
                    {
                        tblServer.Rows.InsertAt(serverRow, 0);
                    }
                        
                        
                            if (File.Exists("settings.xml"))
                            {
                                DBExchange.Inst.LoginSettings.WriteXml("settings.xml");
                            
                            }
                            else
                            {
                                File.Create("settings.xml");

                                DBExchange.Inst.LoginSettings.WriteXml("settings.xml");

                            }

                   
                        
                    
                   
                }


            }

            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
            }
            finally
            {
                if (DBExchange.Inst.connectDb.State == ConnectionState.Open)
                {
                    DBExchange.Inst.getUsrData();
                    Login.ActiveForm.Close();
                }

            }

        }

        private void DbOpen(object sender, EventArgs e)
        {
            tryOpenDb();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DbPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               tryOpenDb();
            }
        }

    }
}
