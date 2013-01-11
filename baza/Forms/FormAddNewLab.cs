using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Npgsql;
using NpgsqlTypes;

namespace baza.Forms
{
    public partial class FormAddNewLab : Form
    {
        private int gotCase;
        private int _gotIntAddon;
        private short _service_code;
        public FormAddNewLab(int _case, int _addon, short _service_code)
        {
            InitializeComponent();
            gotCase = _case;
            switch (_case)
            {
                case 1:
                    
                    break;
                case 2:
                    this.Text = "Добавление типа показателя";
                    this.label1.Text = "Тип показателя";
                    this.toolStripButton1.Text = "Записать тип";
                    break;
                case 3:
                    this.Text = "Добавление единицы измерения показателя";
                    this.label1.Text = "Единица измерения";
                    this.toolStripButton1.Text = "Записать единицу";
                    break;
                case 4:
                    break;
                case 5:
                    _gotIntAddon = _addon;
                    this.Text = "Добавление шаблона анализов";
                    this.label1.Text = "Название шаблона";
                    this.toolStripButton1.Text = "Записать шаблон";
                    break;

            }


        }



        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormAddNewLab.ActiveForm.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                
                writeLabIntoBase();
                               
                
                FormAddNewLab.ActiveForm.Close();
            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
        }




        private void writeLabIntoBase()
        {
            string labName = this.textBox1.Text.Trim();
            if (labName.Length > 1)
            {

                string _code = "";

                switch (gotCase)
                {
                    case 1:
                        _code = "2";
                        break;
                    case 2:
                        _code = "3";
                        break;
                    case 3:
                        _code = "4";
                        break;
                    case 4:
                        _code = "";
                        break;
                    case 5:
                        _code = "5";
                        break;

                }
                NpgsqlCommand insLab = new NpgsqlCommand("Insert into codes (txt, grp) values ( :lab , '"+_code+"') ;", DBExchange.Inst.connectDb);
               
                if (gotCase == 5)
                {
                    insLab = new NpgsqlCommand("Insert into codes (txt, grp, code) values ( :lab , '" + _code + "', '"+_gotIntAddon+"') ;", DBExchange.Inst.connectDb);

                  
                }
                using (insLab)
                {
                    insLab.Parameters.Add(new NpgsqlParameter("lab", NpgsqlDbType.Text));
                    insLab.Parameters[0].Value = labName;
                 
                }
                
                try
                {
                    insLab.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }

            }
            else
            { System.Windows.Forms.MessageBox.Show("Введите название лаборатории"); }
        }

    }
}
