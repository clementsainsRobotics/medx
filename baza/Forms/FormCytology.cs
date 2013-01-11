using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using System.Reflection;

namespace baza
{
    public partial class FormCytology : Form
    {
        private UserClass.Doctors.IdentityList DocList;
        private bool documWritten;
        public FormCytology(int patID)
        {
            InitializeComponent();
            if (patID != -1)
            {
                this.searchPatientBox1.searchById(patID);
            }
            loadDoctors();

        }

        private void просмотретьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form prFC = new printFormGist(setDataCy(), false, false);
            prFC.Show();
        }

        private void FormCytology_Load(object sender, EventArgs e)
        {

        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCytology.ActiveForm.Close();
        }

        private void loadDoctors()
        {

            short _group = 4;
            DocList = new UserClass.Doctors.IdentityList();
            DocList.getIdentityListByGroupID(_group);
            if (DocList.Count > 0)
            {
                foreach (UserClass.Doctors.Identity i in DocList)
                {
                    this.comboBox1.Items.Add(i.DoctorFamilyName + " " + i.DoctorFirstName);
                    this.comboBox3.Items.Add(i.DoctorFamilyName + " " + i.DoctorFirstName);
                }
            }
            
       }


        //формирует тело html страницы для распечатки или просмотра
        private string setDataCy()
        {

            WebBrowser wbC = new WebBrowser();

            wbC.Navigate(Application.StartupPath + @"\Templates\Cytology.htm");
            string cyto = wbC.DocumentText;
            cyto = cyto.Replace("patient", this.searchPatientBox1.Text.Trim());
            cyto = cyto.Replace("cyto.doc", DBExchange.Inst.UsrSign);
            string brTxt = this.bodyRichTextBox1.thisText.Text.Replace("\n", "<br>");
            cyto = cyto.Replace("cyto.text", brTxt);
            cyto = cyto.Replace("cyto.date", dateTimePicker1.Value.ToShortDateString());
            //cyto = cyto.Replace("cyto.numb", this.nomerTextBox1.Text.Trim());
            //cyto = cyto.Replace("mat", this.comboBox1.Text);
            //cyto = cyto.Replace("prep", this.textBox1.Text.Trim());
            //cyto = cyto.Replace("prep2", this.textBox4.Text.Trim());


            return cyto;


        }

        //формирует запрос в баззу данных
        private void insCytZ()
        {
            NpgsqlConnection cdbo = DBExchange.Inst.connectDb;
            int PatientId = this.searchPatientBox1.pIdN;
            DateTime aDate = DateTime.Now;
            DateTime iDate = dateTimePicker1.Value;

            string _to = "";
            string _value = "";

            if (this.comboBox1.SelectedIndex >= 0)
            {

                _to += ", doc_out_c ";
                _value += ", '"+DocList[this.comboBox1.SelectedIndex].DoctorId+"'";

            }

            if (this.comboBox3.SelectedIndex >= 0)
            {

                _to += ", doc_out ";
                _value += ", '" + DocList[this.comboBox3.SelectedIndex].DoctorId + "'";

            }


            try
            {


                
                NpgsqlCommand insRow = new NpgsqlCommand("Insert into diag_data_cyto (descr, header, pat_id, date_out, doc_out "+_to+") VALUES (:descr, :header, '"+PatientId+"', '"+iDate+"', '" 
                    + DBExchange.Inst.dbUsrId + "' "+_value+" );", cdbo);
                using (insRow)
                {
                    insRow.Parameters.Add(new NpgsqlParameter("descr", NpgsqlDbType.Text));
                    insRow.Parameters[0].Value = this.bodyRichTextBox1.thisText.Text.Trim();
                    insRow.Parameters.Add(new NpgsqlParameter("header", NpgsqlDbType.Varchar, 100));
                    insRow.Parameters[1].Value = this.headerTextBox1.thisText.Text.Trim();
                }


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
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                
            }
        }


        private void clearForm()
        {
            this.bodyRichTextBox1.thisText.Text = "Полное описание";
            this.headerTextBox1.thisText.Text = "Краткое описание";
        }


        private void распечататьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (documWritten == false)
            {
                insCytZ();
            }

            Form prFC = new printFormGist(setDataCy(), true, false);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void записатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insCytZ();
        }

        //просмотр выписки



    }
}
