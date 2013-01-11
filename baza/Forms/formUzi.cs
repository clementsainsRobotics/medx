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
    public partial class formUzi : Form
    {
        private bool patSel;
        private bool documWritten;
        private Editor.ClassRadiologyItem.ResearchList rl;
        private Editor.ClassRadiologyItem.ZoneList zl;
        private Editor.ClassRadiologyItem.ZoneTemplateList ztl;

        NpgsqlConnection cdbo = DBExchange.Inst.connectDb;
        private long _gotTicket;


        public formUzi(int patID, long _ticket)
        {
            InitializeComponent();
            documWritten = false;
            patSel = false;
            searchPatientBox1.gotPatientId += new SearchPatientBox.getPatientIdHandler(gotPatientIdInForm);

            rl = new Editor.ClassRadiologyItem.ResearchList();
            rl.GetResearchList();
            fillZoneList();

            _gotTicket = _ticket;
            if (patID != -1)
            {
                this.searchPatientBox1.searchById(patID);
                patSel = true;
            }
            if (_gotTicket != 0)
            {
                fillTemplateList(_ticket);
            }


            loadVars();

        }

        private void fillZoneTemplate()
        {
            ztl = new Editor.ClassRadiologyItem.ZoneTemplateList();

            ztl.GetZoneTemplate(zl[comboBoxZone.SelectedIndex].ZoneId, this.checkBox1.Checked);
            this.comboBoxTemplate.Items.Clear();
            foreach (Editor.ClassRadiologyItem.ZoneTemplate i in ztl)
            {

                this.comboBoxTemplate.Items.Add(i.TemplateName);

            }
        
        
        }


        private void fillZoneList()
        {
            zl = new Editor.ClassRadiologyItem.ZoneList();
            zl.GetZoneList(rl.Find(o => o.ResearchGroupCode == 4).ResearchId, false);
            foreach (Editor.ClassRadiologyItem.ResearchZones i in zl)
            {

                this.comboBoxZone.Items.Add(i.ZoneName);

            }

        }




        private void fillTemplateList(long _ticket)
        {
            ztl = new baza.Editor.ClassRadiologyItem.ZoneTemplateList();
            ztl.GetTicketTemplate(_ticket);

            foreach (Editor.ClassRadiologyItem.ZoneTemplate i in ztl)
            {
                this.comboBoxTemplate.Items.Add(i.TemplateName);
            }


        }



        private void writeForm()
        {
            if (patSel == true)
            {
                if (documWritten == false)
                {
                    insCytZ();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Исследование уже существует", "Запись также происходит при распечатывании",
                                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
            }
            else
            { System.Windows.Forms.MessageBox.Show("Выберите пациента"); }
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
            this.richTextBox1.Text = "";
            this.searchPatientBox1.ResetText();
            documWritten = false;
            loadVars();

        }

        private void loadVars()
        {
            
            NpgsqlCommand getNom = new NpgsqlCommand("Select MAX(nomer) from diag_data_uzi", cdbo);
            int _newNomer = 0;

            try
            {
                _newNomer = (int)getNom.ExecuteScalar();
            }
            catch { }
                this.textBox1.Text = (_newNomer + 1).ToString();
                _gotTicket = 0;

        }




        //формирует тело html страницы для распечатки или просмотра
        private string setDataCy()
        {

            WebBrowser wbC = new WebBrowser();

            wbC.Navigate(Application.StartupPath + @"\Templates\generic.htm");
            string cyto = wbC.DocumentText;
            cyto = cyto.Replace("issl", "- Ульразвуковое обследование пациента ");
            cyto = cyto.Replace("otdel", "лучевой диагностики");
            cyto = cyto.Replace("patient", this.searchPatientBox1.patFullName+ ", "+this.searchPatientBox1.patYears + " лет");
            cyto = cyto.Replace("cyto.doc", DBExchange.Inst.UsrSign);
            cyto = cyto.Replace("cyto.text", this.richTextBox1.Text);
            cyto = cyto.Replace("cyto.date", DateTime.Now.ToShortDateString());
            cyto = cyto.Replace("cyto.numb", this.textBox1.Text.Trim());

            string thisBody = cyto;
            thisBody = thisBody.Replace("{ПациентФИО}", searchPatientBox1.patName);
            thisBody = thisBody.Replace("{Пациент}", searchPatientBox1.patFullName);
            thisBody = thisBody.Replace("{ПациентДР}", searchPatientBox1.patRealBirthDate.ToString());
            thisBody = thisBody.Replace("{ПациентВсегоЛет}", searchPatientBox1.patYears.ToString());        
            thisBody = thisBody.Replace("{ПациентПасспорт}", searchPatientBox1.patPassport.ToString());
            thisBody = thisBody.Replace("{ПациентПасспортВыдан}", searchPatientBox1.patPasspOrg.ToString());
            thisBody = thisBody.Replace("{ПациентАдрес}", searchPatientBox1.patAddr.ToString());
            thisBody = thisBody.Replace("{ПациентТелефон}", searchPatientBox1.patPhone.ToString());
            thisBody = thisBody.Replace("{Пользователь}", DBExchange.Inst.UsrSign);
            thisBody = thisBody.Replace("{ТекущаяДата}", DateTime.Now.ToShortDateString());
           // thisBody = thisBody.Replace("{ПациентДиагноз}", comboBox3.SelectedItem.ToString());
            //thisBody = thisBody.Replace("ДатаДокумента",);
            //.searchPatientBox1.thisBody = thisBody.Replace("ДиагнозПациента",searchPatientBox1.);
            thisBody = thisBody.Replace("{ПациентКарта}", searchPatientBox1.PatientKart);

            cyto = thisBody;
            return cyto;


        }


        private void gotPatientIdInForm(int _patient)
        {
            patSel = true;

        }


        //Шаблоны заполняются из директории Templates, в котором выбирать по расширению .uzi.html



        //Список услуг заполняется из реестра, шаблон записывается в базу и в директорию программы, 
        //в базе задается соответствие услуги и шаблона, но выводится в списке из директории, 
        //чтобы получить в директорию нужно выбрать шаблоны из списка шаблонов




        //формирует запрос в баззу данных
        private void insCytZ()
        {

            int PatientId = this.searchPatientBox1.pIdN;
            DateTime aDate = DateTime.Now;

            try
            {
                string nomer = this.textBox1.Text.Trim();

                string textZakl = this.richTextBox1.Text.Trim();
                string hheader = this.textBox2.Text.Trim();
                if (hheader.Length > 100)
                {
                    hheader = hheader.Substring(0, 99);
                }



                NpgsqlCommand insRow = new NpgsqlCommand("Insert into diag_data_uzi (doc_out, date_out, descr, pat_id, nomer, header) VALUES ('"
                    + Convert.ToInt16(DBExchange.Inst.dbUsrId) + "','" + aDate.ToShortDateString() + "',  :descr ,'" 
                    + PatientId + "', '" + nomer + "',  :header );", DBExchange.Inst.connectDb);
                using (insRow)
                {
                    insRow.Parameters.Add(new NpgsqlParameter("descr", NpgsqlDbType.Text));
                insRow.Parameters[0].Direction = ParameterDirection.Input;
                    insRow.Parameters[0].Value = textZakl;
                    insRow.Parameters.Add(new NpgsqlParameter("header", NpgsqlDbType.Varchar, 100));
                insRow.Parameters[1].Direction = ParameterDirection.Input;
                    insRow.Parameters[1].Value = hheader;
                }

                    
                    insRow.ExecuteNonQuery();
                    documWritten = true;
                

                clearForm();

            }
            catch (Exception exception)
            {
              Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());      
            }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formUzi.ActiveForm.Close();
        }

        private void просмотретьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewForm();
        }

        private void распечататьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printForm();
        }

        private void записатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBoxTemplate.SelectedIndex != -1)
            {
                this.richTextBox1.Text += "\n" + ztl[this.comboBoxTemplate.SelectedIndex].TemplateBody;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillZoneTemplate();
        }
    }
}
