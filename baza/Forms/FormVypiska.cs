using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;


namespace baza.Forms
{
    public partial class FormVypiska : Form
    {

        List<Int64> usrDocuments;

        private string snilsNum;
        private string patAddr;
        private string patBDate;
        private string patSerial;
        private string patNumb;
        private string patGiven;
        private string patWork;
        private string patName;
        private string patPhone;
        DataTable tPatientDiags;
        DataTable tDocuments;

        public FormVypiska(int patID)
        {
            InitializeComponent();
            this.comboBox2.SelectedIndex = 0;
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-730);
            if (patID != -1)
            {
                this.searchPatientBox1.searchById(patID);
            }
        }

        private void fillPatientDocuments(int _selectedPatientId)
        {
            List<string> buffer = new List<string>();
            usrDocuments = new List<Int64>();
            tDocuments = new DataTable();
            try
            {

                string dateFrom = dateTimePicker1.Value.ToShortDateString();
                string toDate = (dateTimePicker2.Value.AddDays(1)).ToShortDateString(); 

                NpgsqlDataAdapter selectPatientDocuments = new NpgsqlDataAdapter("Select dt.descr, d.document_date, trim(doc.family_name), trim(d.document_header), d.document_number as document_number, dt.table as doc_table, d.document_id as docum_id, dt.did "
                + "from documents d, docum_type dt, doctors doc where d.delete=false AND d.pat_id ='"
                + _selectedPatientId + "' AND dt.did = d.document_type and d.document_date between '" + dateFrom + "' and '" + toDate + "' and dt.type != 1 and doc.doc_id = d.doc_id ORDER BY d.document_id DESC", DBExchange.Inst.connectDb);
               //NpgsqlDataReader readDocuments = selectPatientDocuments.ExecuteReader();

               // int colNum = readDocuments.FieldCount;
               // while (readDocuments.Read())
               // {
               //     string header = "";
               //     if (readDocuments[3] is DBNull)
               //     { }
               //     else
               //     { header = readDocuments[3].ToString(); }

               //     string document = ((DateTime)readDocuments[1]).ToShortDateString() + " " + (string)readDocuments[0] + " " + (string)readDocuments[2] + " " + header;
               //     buffer.Add(document);
               //     usrDocuments.Add((Int64)readDocuments["document_number"]);

               // }
               // this.checkedListBoxHistory.DataSource = buffer;
               // readDocuments.Close();

                // 0 - descr
                // 1 - document_date
                // 2 - doc.family_name
                // 3 - document_headere
                // 4 - document_number
                // 5 - doc_table
                // 6 - document_id

                    selectPatientDocuments.Fill(tDocuments);
                    foreach (DataRow roww in tDocuments.Rows)
                    {
                        string header = "";
                        if (roww[3] is DBNull)
                        { }
                        else
                        { header = roww[3].ToString(); }
                        //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                        string document =((DateTime)roww[1]).ToShortDateString() + " " + (string)roww[0] + " " + (string)roww[2] + " " + header;
                        buffer.Add(document);
                        usrDocuments.Add((Int64)roww["document_number"]);
                    }
                    this.checkedListBoxHistory.DataSource = buffer;


            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }




        }

        private void searchPatientBox1_setPatientSelected(int _selectedPatient)
        {
            fillPatient(_selectedPatient);
            fillPatientDocuments(_selectedPatient);
        }

        private string fillForm()
        {

            string selectedDiag = this.comboBox1.Text;
            string setCode = "";
            string setOsnovanie = "";
            
            WebBrowser wbC = new WebBrowser();
            string prntFormData;
            //HTMLPageAwaitingList.html HTMLPageKomission.html  HTMLPageSend.htm


            wbC.Navigate(Application.StartupPath + @"\Templates\"+this.comboBox2.SelectedItem+"\\Header.htm");
            prntFormData = wbC.DocumentText;

            prntFormData = prntFormData.Replace("PATIENT", patName);
            prntFormData = prntFormData.Replace("BIRTH", patBDate);

            prntFormData = prntFormData.Replace("SERIAL", patSerial);
            prntFormData = prntFormData.Replace("NUMB", patNumb);
            prntFormData = prntFormData.Replace("GIVEN", patGiven);
            prntFormData = prntFormData.Replace("LIVING", patAddr);
            prntFormData = prntFormData.Replace("PHONE", patPhone);

            prntFormData = prntFormData.Replace("DIAG", selectedDiag);
            prntFormData = prntFormData.Replace("CODE", setCode);
            prntFormData = prntFormData.Replace("OSNOVA", setOsnovanie);


            


            foreach (int i in checkedListBoxHistory.CheckedIndices)
            {
                
                
                
                WebBrowser HReport = new WebBrowser();
                HReport.Navigate(Application.StartupPath + @"\Templates\"+this.comboBox2.SelectedItem+"\\Body.htm");

                string repotThis;
                repotThis = HReport.DocumentText;
                // 0 - type descr
                // 1 - document_date
                // 2 - doc.family_name
                // 3 - document_headere
                // 4 - document_number
                // 5 - doc_table - where documnt located
                // 6 - document_id - from table
                // 7 - document_type_id
                string documentTable = (string)tDocuments.Rows[i][5];
                Int64 documentId = (Int64)tDocuments.Rows[i][6];
                
                string thisCommand = "Select descr from "+documentTable+" where num = "+ Convert.ToInt32(documentId)+" ";

                if ((int)tDocuments.Rows[i][7] == 29)
                {
                    thisCommand = "Select lr.txt_data, lr.value, lr.test_positive, nl.test_name from lab_results lr, norm_limits nl where lr.ticket_id = " + documentId + " and nl.test_name_id = lr.test_name_id ";

                }

                NpgsqlCommand getDocDescr = new NpgsqlCommand(thisCommand, DBExchange.Inst.connectDb);
                var getDescr = (string)tDocuments.Rows[i][3] +" ";
                if ((int)tDocuments.Rows[i][7] == 29)
                {
                    NpgsqlDataReader getIt = getDocDescr.ExecuteReader();

                    int colNum = getIt.FieldCount;
                     while (getIt.Read())
                    {
                        getDescr = "<br> <span>" + (string)getIt[3];
                        if ((decimal)getIt[1] == -9999)
                        { }
                        else
                        { getDescr += ": "+getIt[1].ToString(); }
                        if ((string)getIt[0] == "''")
                        { }
                        else
                        { getDescr += ": " + (string)getIt[0]; }

                        if (getIt[2] is DBNull)
                        { }
                        else
                        {
                            if ((Boolean)getIt[2] == true)
                            {
                                getDescr += ": Положительно";
                            }
                            else if ((bool)getIt[2] == false)
                            {
                                getDescr += ": Отрицательно";
                            }
                        }
                        

                     }
                     getDescr += "</span>";
                    getIt.Close();
                }
                else
                {
                   getDescr = "<br> <span>"+getDocDescr.ExecuteScalar().ToString()+"</span>";
                }
                
                                 
                     string getDocumentName = ((string)tDocuments.Rows[i][3]+" "+(string)tDocuments.Rows[i][0]);
                
                     string getDocumentDate = ((DateTime)tDocuments.Rows[i][1]).ToShortDateString();
                
                     string getDocumentBody = Convert.ToString(getDescr) ;             


                repotThis = repotThis.Replace("DOCUMENT", getDocumentName);
                repotThis = repotThis.Replace("DATE", getDocumentDate);
                repotThis = repotThis.Replace("BODY", getDocumentBody);

                prntFormData += repotThis;
            }


            WebBrowser FooterReport = new WebBrowser();
            FooterReport.Navigate(Application.StartupPath + @"\Templates\"+this.comboBox2.SelectedItem+"\\Footer.htm");
            string repotFooter;
            repotFooter = FooterReport.DocumentText;
            prntFormData += repotFooter;
            prntFormData = prntFormData.Replace("DATE", this.dateTimePicker1.Value.ToLongDateString());
            return prntFormData;

        }


        private void fillPatient(int _selPat)
        {

            try
            {

                NpgsqlCommand selectPatient = new NpgsqlCommand("Select * from patient_list where pat_id = '" + _selPat + "'", DBExchange.Inst.connectDb);
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
                    if (Convert.IsDBNull(readDocuments["phone"]) == false)
                    {
                        patPhone = (string)readDocuments["phone"];
                    }
                    else
                    {
                        patPhone = "";
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


        private void SampleFabric()
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form prFC = new printFormGist(fillForm(), false, false);
            prFC.Show();
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form prFC = new printFormGist(fillForm(), true, false);
            prFC.Show();
        }
         


    }
}
