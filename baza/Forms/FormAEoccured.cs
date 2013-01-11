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

namespace baza.Forms
{
    public partial class FormAEoccured : Form
    {
        private Documentation.DocumentsList documentList;

        public FormAEoccured(int _pidn, long _document)
        {
            InitializeComponent();

            if (_pidn > 0)
            {               
                this.searchPatientBox1.searchById(_pidn);
                
            }
        }

        private void loadDocumentsForPatients(int _pid)
        {
            comboBox1.Items.Clear();
            documentList = new Documentation.DocumentsList();
            documentList.Get50PatientDocumentsWithoutAE(_pid);
            foreach (Documentation.Document i in documentList)
            {
                comboBox1.Items.Add(i.Date + " " + i.DocumentHeader + " " + i.DocumentShortName + " " + i.DocumentDoctor);
            }

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormAEoccured.ActiveForm.Close();
        }

        private void searchPatientBox1_setPatientSelected(int _selectedPatient)
        {
            loadDocumentsForPatients(_selectedPatient);
        }

        private void writeThisAe()
        {
              NpgsqlCommand writeAE = new NpgsqlCommand("Insert Into ae_occured (start_ae, stop_ae, sae, ncs, casuality, resolution, actions, pat_id, doc_id, ae_comment) values "+
                  "('"+this.dateTimePicker1.Value+"','"+this.dateTimePicker2.Value+"','"+this.checkBox1.Checked+"','"+this.checkBox2.Checked+
                  "','"+this.textBox1.Text.Trim()+"','"+this.textBox2.Text.Trim()+"','"+this.textBox3.Text.Trim()+"','"+this.searchPatientBox1.pIdN+"','"
                  +DBExchange.Inst.dbUsrId+"','"+this.richTextBox1.Text.Trim()+"') returning ae_id ;", DBExchange.Inst.connectDb);
              try
              {
                 long ae_id = writeAE.ExecuteNonQuery();
                 NpgsqlCommand updateDocuments = new NpgsqlCommand("Update documents set ae_occured = '"+ae_id+"' where document_number = '"
                     +documentList[this.comboBox1.SelectedIndex].Serial+"';",DBExchange.Inst.connectDb);
                 updateDocuments.ExecuteNonQuery();
              }
              catch (Exception exception)
              {
                  Warnings.WarnLog log = new Warnings.WarnLog();
                  log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
              }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeThisAe();
        }

    }
}
