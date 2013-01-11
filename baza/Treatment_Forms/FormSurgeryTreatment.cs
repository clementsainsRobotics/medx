using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;

namespace baza.Forms
{
    public partial class FormSurgeryTreatment : Form
    {
        private bool patSel;
        private bool documWritten;

        public FormSurgeryTreatment(int patID)
        {
            InitializeComponent();
            if (patID != -1)
            {
                this.searchPatientBox1.searchById(patID);
            }
        }

        public void writeSurgeryToBase()
        {



        }

        //распечатка
        private void printForm()
        {
            if (patSel == true)
            {
                if (documWritten == false)
                {
                    insCytZ();
                }

                Form prFC = new printFormGist(setDataCy(), true, false);
            }
            else
            { System.Windows.Forms.MessageBox.Show("Выберите пациента"); }
        }

        private void viewForm()
        {
            Form prFC = new printFormGist(setDataCy(), false, false);
            prFC.Show();

        }
        private void clearForm()
        {
            this.richTextBox2.Clear();
            this.searchPatientBox1.ResetText();
            documWritten = false;
          
        }


        //формирует тело html страницы для распечатки или просмотра
        private string setDataCy()
        {

            WebBrowser wbC = new WebBrowser();

            wbC.Navigate(Application.StartupPath + @"\Templates\generic.htm");
            string cyto = wbC.DocumentText;
            cyto = cyto.Replace("issl", "- Хирургическое вмешательство");
            cyto = cyto.Replace("patient", this.searchPatientBox1.patName);
            cyto = cyto.Replace("cyto.doc", DBExchange.Inst.UsrSign);
            string brTxt = this.richTextBox2.Text.Replace("\n", "<br>");
            cyto = cyto.Replace("cyto.text", brTxt);
            cyto = cyto.Replace("cyto.date", DateTime.Now.ToShortDateString());
            cyto = cyto.Replace("cyto.numb", "");

            return cyto;


        }


        //формирует запрос в баззу данных
        private void insCytZ()
        {

            int PatientId = this.searchPatientBox1.pIdN;
            DateTime aDate = DateTime.Now;

            try
            {
               


                string textZakl = this.richTextBox2.Text.Trim();
                string header = this.textBox12.Text.Trim();



                NpgsqlCommand insRow = new NpgsqlCommand("Insert into treatment_data_surgery (doctor_id, surgery_date, surgery_comment, pat_id, surgery_name) VALUES ('"
                    + DBExchange.Inst.dbUsrId + "','" + aDate + "', '" + textZakl + "','" + PatientId + "','" + header + "');", DBExchange.Inst.connectDb);
                try
                {

                    insRow.ExecuteNonQuery();
                    documWritten = true;
                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    
                }

                finally
                {

                }
                clearForm();

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Выберите пациента");
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            viewForm();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FormSurgeryTreatment.ActiveForm.Close();

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            insCytZ();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            viewForm();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }





    }
}
