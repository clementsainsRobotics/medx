using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;

namespace trialDataApp.Forms
{
    public partial class OneStringInputForm : Form
    {
        private Int16 _casen;
        private int _gotIntParm;

        public OneStringInputForm(string _header, string _label, Int16 _case, int _intParam)
        {
          
            InitializeComponent();
            this.Text = _header;
            this.label1.Text = _label;
            _gotIntParm = _intParam;
            _casen = _case;

        }

        private void writeThisToBase(string _command)
        {
            NpgsqlCommand wttb = new NpgsqlCommand(_command,DBExchange.Inst.connectDb);
            try
            {
                wttb.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + "");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OneStringInputForm.ActiveForm.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length > 1)
            {
                switch (_casen)
                {
                    case 1:
                        string _command = "Insert into trial_time_points (point_name, trial_id) values ('" + this.textBox1.Text + "','"+_gotIntParm+"')";
                        writeThisToBase(_command);
                        break;
                    case 2:
                        break;


                }
            }
            OneStringInputForm.ActiveForm.Close();
        }



    }
}
