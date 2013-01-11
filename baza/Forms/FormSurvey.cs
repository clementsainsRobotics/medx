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
    public partial class FormSurvey : Form
    {
        public FormSurvey()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormSurvey.ActiveForm.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (checkData() == true)
            {
                insertDataIntoBase();
                clearFom();
            }
        }

        private void insertDataIntoBase()
        {


            NpgsqlCommand insRow = new NpgsqlCommand("Insert into diag_data_survey (date_out, doc_out, pat_id, header, descr) VALUES ('"+this.dateTimePicker1.Value+"', '"+DBExchange.Inst.dbUsrId+"', '"+
            this.searchPatientBox1.pIdN+"', :header_p, :descr_p) ",DBExchange.Inst.connectDb);
            using (insRow)
            {
                insRow.Parameters.Add(new NpgsqlParameter("header_p", NpgsqlDbType.Varchar, 100));
                insRow.Parameters[0].Value = this.headerTextBox1.thisText.Text.Trim();
                insRow.Parameters.Add(new NpgsqlParameter("descr_p", NpgsqlDbType.Text));
                insRow.Parameters[1].Value = this.bodyRichTextBox1.thisText.Text.Trim();
            }
            try
            {

                insRow.ExecuteNonQuery();
                
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                
            }

        }

        private bool checkData()
        {
            bool all_is_ok = false;

            if (this.searchPatientBox1.patientSet == true)
            {
                all_is_ok = true;
                
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Выберите пациента");
            }


            return all_is_ok;
        }

        private void clearFom()
        {

            this.headerTextBox1.thisText.Text = "Краткое описание";
            this.bodyRichTextBox1.thisText.Text = "Полное описание";

        }

    }
}
