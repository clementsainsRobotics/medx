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
    public partial class FormChangeSampleType : Form
    {

        private Document.SampleType.SList lTypes;
        private baza.Processing.LabTickets.NormClassList nList;

        public FormChangeSampleType(int _thisType, int _thisSample)
        {
            InitializeComponent();
            loadSampleType();

            if (_thisType != 0 && _thisSample != 0)
            {
                this.comboBox1.SelectedIndex = lTypes.IndexOf(lTypes.Find(o => o.SampleType == _thisType));

                if (this.comboBox2.Items.Count > 0)
                {

                    this.comboBox2.SelectedIndex = nList.IndexOf(nList.Find(o => o.NormID == _thisSample));

                }

            }


        }

        private void loadSampleType()
        {
            lTypes = new baza.Document.SampleType.SList();
            lTypes.SampleListGet();

            comboBox1.Items.Clear();
            foreach (Document.SampleType.SampleItem i in lTypes)
            {
                comboBox1.Items.Add(i.SampleName);
            }

            comboBox3.Items.Clear();
            foreach (Document.SampleType.SampleItem i in lTypes)
            {
                comboBox3.Items.Add(i.SampleName);
            }

            this.comboBox1.Refresh();

            this.comboBox3.Refresh();

        }

        private void loadNorms(int _sampleType)
        {

            nList = new baza.Processing.LabTickets.NormClassList();
            nList.getNormsByType(_sampleType);


            comboBox2.Items.Clear();
            comboBox2.Text = "";
            foreach (Processing.LabTickets.NormClass i in nList)
            {
                comboBox2.Items.Add(i.NormName);
            }
            //  comboBox2.Items.Add("Добавить новый показатель ...");


            this.comboBox2.Refresh();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadNorms( lTypes [this.comboBox1.SelectedIndex].SampleType );
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Editor.FormChangeSampleType.ActiveForm.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (changeTypeNow() == true)
            {
                loadNorms(lTypes[this.comboBox1.SelectedIndex].SampleType);
            }
        }


        private bool changeTypeNow()
        {

            bool written = false;

            int _thisNorLimit = nList[this.comboBox2.SelectedIndex].NormID; 

           string  _command = "Update norms set type = '"+ lTypes[this.comboBox3.SelectedIndex].SampleType +"' where norm_id = '"+ _thisNorLimit +"' ;";
           NpgsqlCommand insNorm = new NpgsqlCommand(_command, DBExchange.Inst.connectDb);

                        
                
                        try
                        {
                            
                            insNorm.ExecuteNonQuery();

                            written = true;
                        }
                        catch
                            (Exception exception)
                        {
                            Warnings.WarnLog log = new Warnings.WarnLog(); 
                            log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString()); 
                            

                        }


            return written;
        }


    }
}
