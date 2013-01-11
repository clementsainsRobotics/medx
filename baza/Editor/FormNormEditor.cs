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

namespace baza.Editor
{
    public partial class FormNormEditor : Form
    {
        Document.SampleType.SList lTypes;
        Document.SampleType.MeasureList MTypes;
        Processing.LabTickets.NormClassList nList ;
        Processing.LabTickets.NormFullList thisNorm;
        
        public FormNormEditor()
        {
            InitializeComponent();
            loadSampleType();
           
        }

        
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex == 0)
            {
                if (checkName() == false)
                {
                    writeData();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Показатель уже существует в базе");
                }
            }
            else
            {
                writeData();
            }
        }

        private bool checkName()
        {
            bool gotName = false;

            string gName = this.textBox2.Text.Trim();
            if (gName.Length > 1)
            {
                NpgsqlCommand askName = new NpgsqlCommand("Select norm_id from norms where norm_name = '"+gName+"' ;",DBExchange.Inst.connectDb);
              //  DataTable dtName = new DataTable();
                
                try
                {
                    NpgsqlDataReader readName = askName.ExecuteReader();
                    readName.Read();
                    
                    if (readName.HasRows == true)                  
                    {
                        gotName = true;
                    }
                    readName.Close();
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }


            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Введите название показателя");
            }

            return gotName;
        }




        private void writeData()
        {
            string _niName = this.textBox1.Text.Trim();
            string _nName = this.textBox2.Text.Trim();
            string _nsName = this.textBox3.Text.Trim();
          //  string _tCode = this.textBox4.Text.Trim();



            short nType = 0;

            if (this.comboBoxTypes.SelectedIndex >= 0)
            {
                nType = Convert.ToInt16(lTypes[this.comboBoxTypes.SelectedIndex].SampleType);

            }
            int restId = 0;

            if (this.searchReestrBox1.gotReestrCode >= 0)
            {
                restId = this.searchReestrBox1.gotReestrCode;

            }

            NpgsqlCommand writeNorm = new NpgsqlCommand();
            if (this.listBox1.SelectedIndex == 0)
            {
                writeNorm = new NpgsqlCommand("Insert into norms (norm_name, norm_short_name, norm_int_name,  type, reestr_id)  VALUES "
                    + " ( :noName, :nosName , :noiName , '" + nType + "', '" + restId + "'  ) ;", DBExchange.Inst.connectDb);
            }
            else
            {
                writeNorm = new NpgsqlCommand("UPDATE norms set norm_name = :noName, norm_short_name = :nosName , norm_int_name =  :noiName ,  type = '" + nType + "', reestr_id = '" + restId + "' where norm_id = '"+thisNorm[0].NormID+"';", DBExchange.Inst.connectDb);
            }
            using (writeNorm)
            {
                writeNorm.Parameters.Add(new NpgsqlParameter("noName", NpgsqlDbType.Varchar, 255));
                writeNorm.Parameters[0].Value = _nName;
                writeNorm.Parameters.Add(new NpgsqlParameter("nosName", NpgsqlDbType.Varchar, 15));
                writeNorm.Parameters[1].Value = _nsName;
                writeNorm.Parameters.Add(new NpgsqlParameter("noiName", NpgsqlDbType.Varchar, 255));
                writeNorm.Parameters[2].Value = _niName;
 //               writeNorm.Parameters.Add(new NpgsqlParameter("teCode", NpgsqlDbType.Varchar, 15));
 //               writeNorm.Parameters[3].Value = _tCode;
            }


            try
            {
                this.toolStripStatusLabel1.Text = "Устанавливаем связь с базой данных";
                writeNorm.ExecuteNonQuery();
                this.toolStripStatusLabel1.Text = "Данные записаны";
                clearForm();

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }

        }

        private void clearForm()
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
          //  this.textBox4.Text = "";
            this.toolStripStatusLabel1.Text = "Данные записаны, введите новый показатель или закройте форму.";
            int nType = lTypes[this.comboBoxTypes.SelectedIndex].SampleType;
            loadExistingNorms(nType);



        }


        private void loadSampleType()
        {
            lTypes = new baza.Document.SampleType.SList();    
            lTypes.SampleListGet();
                
                comboBoxTypes.Items.Clear();
                foreach (Document.SampleType.SampleItem i in lTypes )
                {
                    comboBoxTypes.Items.Add(i.SampleName);
                }
                comboBoxTypes.Items.Add("Добавить новый тип ...");

            
            this.comboBoxTypes.Refresh();


        }


        private void loadExistingNorms(int _sampleType)
        {

            nList = new baza.Processing.LabTickets.NormClassList();
            nList.getNormsByType(_sampleType);


            listBox1.Items.Clear();
            listBox1.Text = "";
            listBox1.Items.Add("Добавить новый показатель ...");
            foreach (Processing.LabTickets.NormClass i in nList)
            {
                listBox1.Items.Add(i.NormName);
            }
            
            this.listBox1.Refresh();
            this.listBox1.SelectedIndex = 0;

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormNormEditor.ActiveForm.Close();
        }

        private void comboBoxTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxTypes.SelectedItem.ToString().Trim() == "Добавить новый тип ...")
            {
                Forms.FormAddNewLab fanl = new Forms.FormAddNewLab(2, 0, 0);
                fanl.ShowDialog();
                loadSampleType();
            }
            else
            {
                if (this.comboBoxTypes.SelectedIndex >= 0)
                    {
                        int nType = lTypes[this.comboBoxTypes.SelectedIndex].SampleType; 
                        loadExistingNorms(nType);
                    }                
            }
        }
        
        private void loadNormData()
        {



        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex == 0)
            {
                this.textBox1.Text = "";
                this.textBox2.Text = "";
                this.textBox3.Text = "";
            }
            else if (this.listBox1.SelectedIndex > 0)
            {
               
               int normId = nList[this.listBox1.SelectedIndex-1].NormID;
               thisNorm = new Processing.LabTickets.NormFullList();
               thisNorm.getNormByType(normId);
               if (thisNorm.Count > 0)
               {
                   this.textBox2.Text = thisNorm[0].NormName;
                   this.textBox3.Text = thisNorm[0].NormShortName;
                   this.textBox1.Text = thisNorm[0].NormIntName;
                   this.searchReestrBox1.searchThisReestrId(thisNorm[0].NormReestrId);
               }

            }


        }

    }
}
