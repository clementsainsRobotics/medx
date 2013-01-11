using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Npgsql;


namespace baza
{
    public partial class prntData : Form
    {
        private string snilsNum;
        private string patAddr;
        private string patBDate;
        private string patSerial;
        private string patNumb;
        private string patGiven;
        private string patWork;
        private string patName;

        
        
        DataTable tPatientDiags;


        ///Найти ошибку
        public prntData(int _patId)
        {
            InitializeComponent();
            this.checkBox1.Checked = true;
            this.checkBox2.Checked = true;
            this.checkBox3.Checked = true;
            if (_patId != -1)
            {
                this.searchPatientBox1.searchById(_patId);
            }
            this.comboBox3.SelectedIndex = 0;
            searchPatientBox1.setPatientSelected += new SearchPatientBox.setNewPatientSelected(this.fillPatient);
            loadOtdelData();
            
        }

        private void fillPatient(int _selPat)
        {

            try
            {

                NpgsqlCommand selectPatient = new NpgsqlCommand("Select * from patient_list where pat_id = '"+_selPat+"'", DBExchange.Inst.connectDb);
                NpgsqlDataReader readDocuments = selectPatient.ExecuteReader();

                int colNum = readDocuments.FieldCount;

                while (readDocuments.Read())
                {
                    patName = (string)readDocuments["family_name"] + " " + (string)readDocuments["first_name"] + " " + (string)readDocuments["last_name"];
                    if (Convert.IsDBNull(readDocuments["snils"]) == false)
                    {
                        snilsNum = (string)readDocuments["snils"];
                    }
                    else
                    {
                        snilsNum = "";
                    }
                    if (Convert.IsDBNull(readDocuments["address"]) == false)
                    {
                        patAddr = (string)readDocuments["address"];
                    }
                    else
                    {
                        patAddr = "";
                    }
                    if (Convert.IsDBNull(readDocuments["birth_date"]) == false)
                    {
                        patBDate = ((DateTime)readDocuments["birth_date"]).ToShortDateString();
                    }
                    else
                    {
                        patBDate = "";
                    }
                    if (Convert.IsDBNull(readDocuments["doc_ser"]) == false)
                    {
                        patSerial = (string)readDocuments["doc_ser"];
                    }
                    else
                    {
                        patSerial = "";
                    }
                    if (Convert.IsDBNull(readDocuments["doc_num"]) == false)
                    {
                        patNumb = (string)readDocuments["doc_num"];
                    }
                    else
                    {
                        patNumb = "";
                    }
                    if (Convert.IsDBNull(readDocuments["doc_date"]) == false)
                    {
                        patGiven = ((DateTime)readDocuments["doc_date"]).ToShortDateString() + " " + (string)readDocuments["doc_org"];
                    }
                    else
                    {
                        patGiven = "";
                    }
                    if (Convert.IsDBNull(readDocuments["job"]) == false)
                    {
                        patWork = (string)readDocuments["job"];
                    }
                    else
                    {
                        patWork = "";
                    }
                }

                readDocuments.Close();

                tPatientDiags = new DataTable();
                NpgsqlDataAdapter selPatDiags = new NpgsqlDataAdapter("Select diag_date, (select trim(name) from diags where diag = diag_data.diag) as name, "
                + "(select trim(ds) from diags where diag = diag_data.diag) as ds, usr_id from diag_data where pat_id = " + _selPat + " and delete=false order by diag_date Asc ", DBExchange.Inst.connectDb);
                comboBox1.Items.Clear();


                    selPatDiags.Fill(tPatientDiags);


                    foreach (DataRow row in tPatientDiags.Rows)
                    {
                        comboBox1.Items.Add(((string)row[2]) + " " + ((string)row[1]));
                    }


                    if (this.comboBox1.Items.Count > 0)
                    {
                        this.comboBox1.SelectedIndex = 0;
                    }
               

     
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog(); 
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString()); 
                

            }

        }

        private void printForms()
        {
            if  (this.checkBox1.Checked == true)
            {
                //HTMLPageAwaitingList.html
                Form prFC = new printFormGist(setAwaitList(), true, false);
                

            }
            if (this.checkBox2.Checked == true)
            {
                //HTMLPageSend.htm
                Form prPS = new printFormGist(setKomission(), true, false);
               

            }
            if (this.checkBox3.Checked == true)
            {
                //HTMLPageKomission.html
                Form prKo = new printFormGist(setPageSend(), true, true);

            }
            
        }



        private void showForms()
        {
            if (this.checkBox1.Checked == true)
            {
                //HTMLPageAwaitingList.html
                Form prFC = new printFormGist(setAwaitList(), false, false);
                prFC.Show();
                
            }
            if (this.checkBox2.Checked == true)
            {
                //HTMLPageSend.htm
                Form prPS = new printFormGist(setKomission(), false, false);
                prPS.Show();
                
            }
            if (this.checkBox3.Checked == true)
            {
                //HTMLPageKomission.html
                Form prKo = new printFormGist(setPageSend(), false, false);
                prKo.Show();
            }
        }

        /// <summary>
        /// Заполняет список дагнозов для выбранного в поле "мои паценты" пациента
        /// </summary>
        /// <param name="_selectedPatient"></param>





        private string setAwaitList()
        {

            WebBrowser wbC = new WebBrowser();
            string prntFormData;
            //HTMLPageAwaitingList.html HTMLPageKomission.html  HTMLPageSend.htm
            // if (this.checkBox1.Checked == true)

            wbC.Navigate(Application.StartupPath + @"\Templates\HTMLPageAwaitingList.html");
                prntFormData = wbC.DocumentText;
                prntFormData = prntFormData.Replace("PATIENT", patName);
                prntFormData = prntFormData.Replace("SNILS", snilsNum);
                prntFormData = prntFormData.Replace("LIVING", patAddr);
                prntFormData = prntFormData.Replace("birth", patBDate);
                prntFormData = prntFormData.Replace("DIAG", this.textBox1.Text +" "+richTextBox1.Text);
               
                prntFormData = prntFormData.Replace("DATE", this.dateTimePicker1.Value.ToLongDateString());

                return prntFormData;
   
        }

        private void diagSelected()
        {
            //if (this.textBox1.Text.Trim() == "Код")
            //{
                this.textBox1.Text = (string)tPatientDiags.Rows[this.comboBox1.SelectedIndex]["ds"];
                this.richTextBox1.Text = (string)tPatientDiags.Rows[this.comboBox1.SelectedIndex]["name"];
            //}
            //else
            //{
            //    this.textBox1.Text += ", "+(string)tPatientDiags.Rows[this.comboBox1.SelectedIndex]["ds"];
            //    this.richTextBox1.Text += ", "+(string)tPatientDiags.Rows[this.comboBox1.SelectedIndex]["name"];
            //}

        }

        //загружает список отделений из которых потом формруется список услуг
        private void loadOtdelData()
        {
            try
            {
                NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select trim(name) from divisions order by division ASC", DBExchange.Inst.connectDb);
                DataTable divTable = new DataTable();
                div.Fill(divTable);
                foreach (DataRow roww in divTable.Rows)
                {
                    this.comboBox2.Items.Add((string)roww[0]);
                }
                short otdel = (short)DBExchange.Inst.tblUsrData.Rows[0]["otdel"];
                if (otdel > 0)
                {
                    this.comboBox2.SelectedIndex = otdel;
                }
                else
                { this.comboBox2.SelectedIndex = 11; }
            }
            catch { }
            finally
            {

            }
        }


        private string setKomission()
        {

            WebBrowser wbC = new WebBrowser();
            string prntFormData;
            //HTMLPageAwaitingList.html HTMLPageKomission.html  HTMLPageSend.htm


            wbC.Navigate(Application.StartupPath + @"\Templates\HTMLPageKomission.html");
            prntFormData = wbC.DocumentText;

            prntFormData = prntFormData.Replace("PATIENT", patName);
            prntFormData = prntFormData.Replace("BIRTH", patBDate);

            prntFormData = prntFormData.Replace("SERIAL", patSerial);
            prntFormData = prntFormData.Replace("NUMB", patNumb);
            prntFormData = prntFormData.Replace("GIVEN", patGiven);
            prntFormData = prntFormData.Replace("LIVING", patAddr);

            prntFormData = prntFormData.Replace("DIAG", this.textBox1.Text);
            prntFormData = prntFormData.Replace("CODE", this.richTextBox1.Text);
            prntFormData = prntFormData.Replace("OSNOVA", this.richTextBox3.Text);
        
            prntFormData = prntFormData.Replace("DATE", this.dateTimePicker1.Value.ToLongDateString());

            return prntFormData;

        }

        private string setPageSend()
        {

            WebBrowser wbC = new WebBrowser();
            string prntFormData;
            //HTMLPageAwaitingList.html HTMLPageKomission.html  HTMLPageSend.htm


            wbC.Navigate(Application.StartupPath + @"\Templates\HTMLPageSend.htm");
            prntFormData = wbC.DocumentText;

            if (this.comboBox3.SelectedIndex == 0)
            {
                prntFormData = prntFormData.Replace("NAPRAVLENIE", 
                "<strong><u><b>на госпитализацию</b></u></strong>, обследование, консультацию");
            }
            else if (this.comboBox3.SelectedIndex == 1)
            {
                prntFormData = prntFormData.Replace("NAPRAVLENIE",
                "на госпитализацию, <strong><u><b>обследование</b></u></strong>, консультацию");
            }
            else if (this.comboBox3.SelectedIndex == 2)
            {
                prntFormData = prntFormData.Replace("NAPRAVLENIE",
                "на госпитализацию, обследование, <strong><u><b>консультацию</b></u></strong>");
            }
            prntFormData = prntFormData.Replace("OTDEL", this.comboBox2.Text);
            prntFormData = prntFormData.Replace("PATIENT", patName);
            prntFormData = prntFormData.Replace("BIRTH", patBDate);
            prntFormData = prntFormData.Replace("ADDRESS", patAddr);
            prntFormData = prntFormData.Replace("WORK", patWork);
           
            prntFormData = prntFormData.Replace("DIAG_CODE", this.textBox1.Text );
            prntFormData = prntFormData.Replace("DIAG", this.richTextBox1.Text);
            prntFormData = prntFormData.Replace("OSNOVA", this.richTextBox3.Text);

            prntFormData = prntFormData.Replace("DATE", this.dateTimePicker1.Value.ToLongDateString());


            return prntFormData;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            showForms();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prntData.ActiveForm.Close();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            diagSelected();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printForms();
        }
       
    }
}
