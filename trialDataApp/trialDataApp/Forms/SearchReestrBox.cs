using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace trialDataApp
{
    public partial class SearchReestrBox : UserControl
    {
        Timer searchReestrTimer;
        private bool searchR;
        public DataTable getReestrTable;
        public int _gotReestrId;

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

        }

        private void searchPatientComboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            searchR = false;
            int txtPos = 0;
            if (comboBox1.Text.Trim().Length >= 3)
            {
                txtPos = comboBox1.Text.Trim().Length;
                if (e.KeyCode == Keys.Enter)
                //  || e.KeyCode == Keys.Space
                {
                    searchNow();
                }

                if (searchReestrTimer.Enabled == false)
                {
                    searchReestrTimer.Interval = 1300;
                    searchReestrTimer.Start();
                }
                else
                {
                    searchReestrTimer.Stop();
                    searchReestrTimer.Interval = 1300;
                    searchReestrTimer.Start();
                }
                this.comboBox1.Select(txtPos, 0);
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
            NpgsqlDataAdapter searchList = new NpgsqlDataAdapter("Select reestr, name, code from reestr where upper(name) like '%" + searchThis.ToUpper() + "%' or code like '%" + searchThis.ToUpper() + "%' ", DBExchange.Inst.connectDb);
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
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " searchReestr");
            } 
            return getList;
            
       }




        private void searchNow()
        {
            this.comboBox1.DataSource = searchReestrListbyName(this.comboBox1.Text);

        }

        private void comboBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.Text = "";
            this.comboBox1.Refresh();
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "Введите название услуги")
            {
                this.comboBox1.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex > 0)
            {
                _gotReestrId = (int)getReestrTable.Rows[this.comboBox1.SelectedIndex]["reestr"];
            }
        }



    }
}
