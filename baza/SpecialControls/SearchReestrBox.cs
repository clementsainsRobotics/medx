using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Npgsql;

namespace baza.Forms
{
    public partial class SearchReestrBox : UserControl
    {
        Timer searchReestrTimer;
        private bool searchR;
        public DataTable getReestrTable;
        public int gotReestrCode;

        public SearchReestrBox()
        {
            InitializeComponent();
            searchR = false;
            searchReestrTimer = new Timer();
            searchReestrTimer.Tick += new EventHandler(searchTimer_Tick);


        }

        void searchTimer_Tick(object sender, EventArgs e)
        {
            searchReestrTimer.Stop();
            searchNow();
            searchR = true;
        }

        private void searchPatientComboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            searchR = false;
            
                int txtPos = 0;
                txtPos = this.comboBox1.Text.Trim().Length;
                if (txtPos >= 3)
                {    
                        if (e.KeyCode == Keys.Enter)
                        //  || e.KeyCode == Keys.Space
                        {
                            searchReestrTimer.Stop();
                            searchNow();
                            searchR = true;
                           
                        }

                        else
                            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back  || e.KeyCode == Keys.Return)
                            {

                                searchReestrTimer.Stop();

                            }
                            else
                            {
                                            if (searchReestrTimer.Enabled == false)
                                            {
                                                searchReestrTimer.Interval = 2300;
                                                searchReestrTimer.Start();
                                                
                                            }
                                            else
                                            {
                                              //  searchReestrTimer.Stop();
                                                searchReestrTimer.Interval = 2300;
                                               // searchReestrTimer.Start();
                                               
                                            }
                                            
                                }

                
                
            }
            
        }

               

        /// <summary>
        /// Поиск в реестре услуг заданной услуги
        /// </summary>
        /// <param name="searchThis"></param>
        /// <returns></returns>

        public List<string> searchReestrListbyName(string searchThis)
        {
            List<string> getList = new List<string>();
            getReestrTable = new DataTable();
            NpgsqlDataAdapter searchList = new NpgsqlDataAdapter("Select reestr, name, code from reestr where upper(name) like '%" 
                + searchThis.ToUpper() + "%' or code like '%" + searchThis.ToUpper() + "%' ", DBExchange.Inst.connectDb);
            try
            {
                searchList.Fill(getReestrTable);
                foreach (DataRow row in getReestrTable.Rows)
                {
                    getList.Add(row["code"].ToString().Trim() + " " + row["name"].ToString().Trim());
                }

                
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                
            }
            
            return getList;

       }


        public void searchThisReestrId(int _Id)
        {
            List<string> getList = new List<string>();
            getReestrTable = new DataTable();
            NpgsqlDataAdapter searchList = new NpgsqlDataAdapter("Select reestr, name, code from reestr where reestr = '"+_Id+"' ", DBExchange.Inst.connectDb);
            try
            {
                searchList.Fill(getReestrTable);
                if (getReestrTable.Rows.Count > 0)
                {
                    foreach (DataRow row in getReestrTable.Rows)
                    {
                        getList.Add(row["code"].ToString().Trim() + " " + row["name"].ToString().Trim());
                    }
                    this.comboBox1.DataSource = getList;
                    this.comboBox1.SelectedIndex = 0;
                }
                else
                {
                    this.comboBox1.DataSource = getList;
                    this.comboBox1.Text = "";
                }
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                
            }


        }


        private void searchNow()
        {
           int txtPos = this.comboBox1.Text.Trim().Length;
           List<String>gotReestrList = searchReestrListbyName(this.comboBox1.Text);
           this.comboBox1.DataSource = gotReestrList;
           this.comboBox1.Refresh();
          
           this.comboBox1.Select(txtPos, 0);   
           searchR = false;


        }

        private void comboBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.Text = "";
            this.comboBox1.Refresh();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex != -1)
            {
                gotReestrCode = (int)getReestrTable.Rows[this.comboBox1.SelectedIndex]["reestr"];
            }
        }



    }
}
