using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Npgsql;

namespace baza
{
    public partial class SearchPatientBox : UserControl
    {
        public bool patientSet;
        public DataTable tblPatList;
        public int pIdN;  //идентификация пациента
        public string patName;
        public string patFullName;
        public string patPassport;
        public string patPasspOrg;
        public string patPhone;
        public string patAddr;
        public string patRealBirthDate;
        public int currDoc;
        public List<string> lstPatients;
        private bool searchPat;
        public int patYears;
        public TimeSpan tpatBirthDate;
        Timer searchPatientTimer;
        public string PatientKart;

        public SearchPatientBox()

        {


            InitializeComponent();
            pIdN = -1;
            searchPat = false;
            patientSet = false;
            this.searchPatientComboBox1.Text = "Введите фамилию пациента";
            
            searchPatientTimer = new Timer();
            searchPatientTimer.Tick += new EventHandler(searchTimer_Tick);
    
        }

 




        private void button1_Click(object sender, EventArgs e)
        {

        }



        private void searchPatientComboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            searchPat = false;

            if (e.KeyCode == Keys.Enter )
            //  || e.KeyCode == Keys.Space
            {
                searchPatientTimer.Stop();
                searchNow();
            }
             else
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {

                    searchPatientTimer.Stop();
                   
                }
                else
                {
                    if (searchPatientTimer.Enabled == false)
                    {

                        searchPatientTimer.Interval = 2300;
                        searchPatientTimer.Start();


                    }
                    else
                    {
                       // searchPatientTimer.Stop();
                        searchPatientTimer.Interval = 2300;
                       // searchPatientTimer.Start();
                    }
                }

           
        }

        void searchTimer_Tick(object sender, EventArgs e)
        {
            searchPatientTimer.Stop();
            searchNow();
            
        }
 
       

        private void searchPatientComboBox1_Enter(object sender, EventArgs e)
        {
        //    searchPatientComboBox1.ResetText();
        //    patientSet = false;
        }


        public void searchById(int gotId)
        {
            tblPatList = new DataTable();
            DateTime birth;
            NpgsqlConnection psDBC = DBExchange.Inst.connectDb;

            NpgsqlDataAdapter selPat = new NpgsqlDataAdapter("SELECT family_name, first_name, last_name,"

            + "birth_date, nib, pat_id, curr_doctor, doc_ser, doc_num, phone, address, doc_date, doc_org FROM patient_list where pat_id = '" + gotId + "';", DBExchange.Inst.connectDb);

            try
            {
                            
                selPat.Fill(tblPatList);

                lstPatients = new List<string>();

                if (tblPatList != null)
                {
                    foreach (DataRow row in tblPatList.Rows)
                    {
                        if ((DateTime)row["birth_date"] != null)
                        {
                            birth = (DateTime)row["birth_date"];
                        }
                        else
                        {
                            birth = DateTime.Today;
                        }
                        if (String.IsNullOrEmpty(row["nib"].ToString()))
                        {
                            lstPatients.Add(((string)row["family_name"]).Trim() + " " + ((string)row["first_name"])[0]
                                + ". " + ((string)row["last_name"])[0] + ". " + birth.Year.ToString()
                                + " г. ");
                        }
                        else
                        {
                            lstPatients.Add(((string)row["family_name"]).Trim() + " " + ((string)row["first_name"])[0]
                                + ". " + ((string)row["last_name"])[0] + ". " + birth.Year.ToString()
                                + " г. " + ((int)row["nib"]));
                        }

                    }


                }
                this.searchPatientComboBox1.DataSource = lstPatients;
                //  this.searchPatientComboBox1.SelectedItem = 0 ;
                if (lstPatients.Count > 0)
                {
                    searchPat = true;
                    pIdN = gotId;
                 //   setPatSelected();
                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog(); 
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString()); 
                

            }


            

        }



        //собственно сабж
        private void searchNow()
        {
            int txtPos = 0;
            if (searchPatientComboBox1.Text.Trim().Length >= 3)
            {
                txtPos = searchPatientComboBox1.Text.Trim().Length;

                string _thisText = searchPatientComboBox1.Text.Trim().ToUpper();

                NpgsqlConnection psDBC = DBExchange.Inst.connectDb;
                NpgsqlDataAdapter selPat = new NpgsqlDataAdapter("SELECT family_name, first_name, last_name, birth_date, nib, pat_id, curr_doctor, doc_ser, doc_num, phone, address, doc_date, doc_org FROM patient_list where Upper(family_name) like '"
                        + _thisText + "%' order by family_name, first_name, last_name ASC ;", psDBC);

                double _thisNum;
                try
                {

                    bool _gotNum = double.TryParse(_thisText, out _thisNum);
                    if (_gotNum)
                    {
                        selPat = new NpgsqlDataAdapter("SELECT family_name, first_name, last_name, birth_date, nib, pat_id, curr_doctor, doc_ser, doc_num, phone, address, doc_date, doc_org FROM patient_list where doc_num like '"
                            + _thisNum + "%' or nib = '" + _thisNum + "' order by family_name, first_name, last_name ASC ;", psDBC);
                    }
                }
                catch { }
                //   NpgsqlCommand selPat = new NpgsqlCommand("Select family_name,birth_date,first_name,last_name,serial f
                //rom patient_list where Upper(family_name) like '%" + searchPatientComboBox1.Text.ToUpper() + "%';", psDBC);
                try
                {


                    tblPatList = new DataTable();
                    selPat.Fill(tblPatList);
                    DateTime birth;
                    lstPatients = new List<string>();

                    if (tblPatList.Rows.Count >= 1)
                    {
                        foreach (DataRow row in tblPatList.Rows)
                        {
                            if (String.IsNullOrEmpty(row["birth_date"].ToString()))
                            {
                                birth = DateTime.Today;                                 
                            }
                            else
                            {
                                birth = (DateTime)row["birth_date"];
                            }

                            if (String.IsNullOrEmpty(row["nib"].ToString()))
                            {
                                lstPatients.Add(((string)row["family_name"]).Trim() + " " + ((string)row["first_name"])[0]
                                    + ". " + ((string)row["last_name"])[0] + ". " + birth.Year.ToString() + " г. ");
                            }
                            else
                            {
                                lstPatients.Add(((string)row["family_name"]).Trim() + " " + ((string)row["first_name"])[0]
                                    + ". " + ((string)row["last_name"])[0] + ". " + birth.Year.ToString() + " г. " + ((int)row["nib"]));
                            }


                        }


                    }


                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog(); log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString()); 

                }


                finally
                {


                    this.searchPatientComboBox1.DataSource = lstPatients;
                    this.searchPatientComboBox1.Select(txtPos, 0);
                    searchPat = true;
                    try
                    {
                        pIdN = (Int32)tblPatList.Rows[0]["pat_id"];
                        
                    }
                    catch
                    { }

                }



            }
            
        }

        
        public delegate void getPatientIdHandler(int _newPatientId);
        public delegate void getPatientDocHandler(int _patientDocId, string _patientDocName);
        public delegate void setNewPatientSelected(int _selectedPatient);

        public event setNewPatientSelected setPatientSelected;
        public event getPatientIdHandler gotPatientId;
        public event getPatientDocHandler gotPatientDoc;


        //protected void OngotPatientdId(object , EventArgs )
        //{
        //    if (gotPatientId != null)
        //    {
        //        gotPatientId( , )
        //    }

        //}

        //Устанавливает пациента - выбранным изз списка

        private void setPatSelected()
        {

            if (this.searchPatientComboBox1.SelectedIndex != -1)
            {
                try
                {
                    pIdN = Convert.ToInt32(tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["pat_id"]);
                    if (Convert.IsDBNull(tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["curr_doctor"]) == false)
                    {
                        currDoc = Convert.ToInt32(tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["curr_doctor"]);
                    }
                    patName = ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["family_name"]).Trim() + " "
                        + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["first_name"])[0] + ". "
                        + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["last_name"])[0] + ". ";
                    patFullName = ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["family_name"]).Trim() + " "
                        + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["first_name"]).Trim() + " "
                        + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["last_name"]).Trim() ;
                    TimeSpan pbd = DateTime.Now - ((DateTime)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["birth_date"]);
                    PatientKart = ((Int32)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["nib"]).ToString().Trim();
                    patYears = Convert.ToInt32((long)(pbd.Days / 365.25));
                    patRealBirthDate = ((DateTime)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["birth_date"]).ToShortDateString();
                    if (Convert.IsDBNull(tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["doc_ser"]) == false)
                    {
                        patPassport = "Пасспорт cерии " + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["doc_ser"]).Trim() + " номер " + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["doc_num"]).Trim();

                    }
                    if (Convert.IsDBNull(tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["doc_date"]) == false)
                    {
                        patPasspOrg = "Выдан " + ((DateTime)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["doc_date"]).ToShortDateString() + " " + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["doc_org"]).Trim();

                    }
                    if (Convert.IsDBNull(tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["phone"]) == false)
                    {
                        patPhone = ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["phone"]).Trim();

                    }
                    if (Convert.IsDBNull(tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["address"]) == false)
                    {
                        patAddr = ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["address"]).Trim();

                    } 
                    this.label1.Text = patYears.ToString() + " лет";
                    patientSet = true;
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog(); 
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());

                }
            }

            if (gotPatientId != null)
            {
                gotPatientId(pIdN);
                
            }
            if (gotPatientDoc != null)
            {
                searchPatientDoc();
                
            }
            if (setPatientSelected != null)
            {
                setPatientSelected(pIdN);
                
            }

        }


        private void searchPatientComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPatSelected();
            
        }


        //Ищет доктора пациента чтобы вписывать его в списки или сравнить с вошедшим пользователем
        private void searchPatientDoc()
        {
          
            NpgsqlConnection cdbo = DBExchange.Inst.connectDb;
            string patDoctor;
            patDoctor = "";
            int docNum;
            try
            {
                
                ///Получение номера лечащего врача
                NpgsqlCommand getDocNumb = new NpgsqlCommand("SELECT curr_doctor FROM patient_list WHERE pat_id = '" + pIdN + "';", cdbo);

                docNum = (int)getDocNumb.ExecuteScalar();


                    NpgsqlCommand getDocName = new NpgsqlCommand("SELECT family_name, first_name, last_name FROM doctors WHERE doc_id = '" + docNum + "';", cdbo);
                    NpgsqlDataReader pd = getDocName.ExecuteReader();
                    while (pd.Read())
                    {
                        patDoctor = pd[0] + " " + (pd[1].ToString()[1]) + ". " + (pd[2].ToString()[1]) + ".";

                    }
                   
                        gotPatientDoc(docNum, patDoctor);
                    }
                

            
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog(); log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString()); 

            }
            finally
            {
                
            }
          }

        private void SearchPatientBox_DoubleClick(object sender, EventArgs e)
        {
               searchPatientComboBox1.ResetText();
               patientSet = false;
        }

        //private void searchPatientComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        //{

        //        if (setPatientSelected != null)
        //        {
               
        //                setPatientSelected(pIdN);
        //                patientSet = true;
                    
           
        //        }
            
        //}

        //private void searchPatientComboBox1_Leave(object sender, EventArgs e)
        //{
           
        //        if (setPatientSelected != null)
        //        {
                   
        //            setPatientSelected(pIdN);
        //            patientSet = true;
                    
        //        }
            
        //}
        


    }
}
