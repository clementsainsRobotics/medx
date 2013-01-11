using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using Npgsql;

namespace trialDataApp
{
    public partial class RegisterForm : Form
    {
        private IList<string> divList;
        private IList<string> staList;
        private IList<string> speList;


        //Йнкции пользователя
        /// <summary>
        /// добавление\удаление пользователя
        /// добавление записи
        /// удаление своей незакрытой записи
        /// проверка
        /// изменение прав пользователя
        /// 
        /// удаление закрытой записи
        /// редактирование своей записи
        /// редактирование чужой записи
        /// 
        /// </summary>

        public RegisterForm()
        {

            InitializeComponent();
            divList = new List<string>();
            staList = new List<string>();
            speList = new List<string>();
            NpgsqlConnection cdbo = DBExchange.Inst.connectDb;
            NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select trim(name) from divisions order by division ASC", cdbo);
            DataTable divTable = new DataTable();
            div.Fill(divTable);
            foreach (DataRow roww in divTable.Rows )
            {
                divList.Add((string)roww[0]);
            }
            otdel.DataSource = divList;
            otdel.SelectedIndex = 1;
            NpgsqlDataAdapter sta = new NpgsqlDataAdapter("Select trim(name) from posts order by post ASC", cdbo);
            DataTable staTable = new DataTable();
            sta.Fill(staTable);
            foreach (DataRow roww in staTable.Rows)
            {
                staList.Add((string)roww[0]);
            }
            status.DataSource = staList;

            status.SelectedIndex = 39;
            //special.SelectedIndex = 0;
        }


        /// <summary>
        /// Запись пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

                DBExchange.Inst.ChkNameDbUser(u_name.Text);
                if (DBExchange.Inst.tblUsers.Rows.Count == 1)
                {

                    MessageBox.Show("Имя пользователя " + u_name.Text + " уже существует", "Выберите другое имя пользователя",
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    {

                
                            if (u_name.TextLength < 4)
                   {
                MessageBox.Show("Логин должен быть длиннее 4 букв", "Измените логин",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
                
                else
                {


                    if (r_pass.Text == r_pass_2.Text != null)
                    {
                        
                        DBExchange.Inst.CreateNewDbUser(u_name.Text.ToLower(), r_pass.Text, name.Text, f_name.Text, s_name.Text, dateTimePicker1.Value,
                            Convert.ToInt16(otdel.SelectedIndex), Convert.ToInt16(status.SelectedIndex+1), r_user_mail.Text, Convert.ToByte(comboBox1.SelectedIndex + 1));
                    }
                    else
                        MessageBox.Show("Пароли не соответствуют", "Проверьте пароль",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

      
        private void maskedTextBox4_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterForm.ActiveForm.Close();
        }

    
    }
}
