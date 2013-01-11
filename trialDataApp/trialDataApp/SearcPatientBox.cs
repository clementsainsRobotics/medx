using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace trialDataApp
{
    public partial class SearchPatientBox : UserControl
    {
        public bool patientSet;
        public DataTable tblPatList;
        public int pIdN;  //идентификация пациента
        public string patName;
        public int currDoc;
        public List<string> lstPatients;
        private bool searchPat;
        public int patBirthDate;
        public string patInitials;
        public TimeSpan tpatBirthDate;
        Timer searchPatientTimer;
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
                searchNow();
            }

                  

                  if (searchPatientTimer.Enabled == false)
                  {                     
                      searchPatientTimer.Interval = 1300;
                      searchPatientTimer.Start();                      
                  }
                  else
                  {
                      searchPatientTimer.Stop();
                      searchPatientTimer.Interval = 1300;
                      searchPatientTimer.Start();
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

            + " birth_date, nib, pat_id, curr_doctor FROM patient_list where pat_id = '" + gotId + "';", DBExchange.Inst.connectDb);

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


            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " searchById");
            }


            finally
            {


                this.searchPatientComboBox1.DataSource = lstPatients;
              //  this.searchPatientComboBox1.SelectedItem = 0 ;
                searchPat = true;
                try
                {
                    pIdN = gotId;
                }
                catch
                { }

            }
            setPatSelected();

        }



        //собственно сабж
        private void searchNow()
        {
            int txtPos = 0;
            if (searchPatientComboBox1.Text.Trim().Length >= 3)
            {
                txtPos = searchPatientComboBox1.Text.Trim().Length;

                NpgsqlConnection psDBC = DBExchange.Inst.connectDb;

                NpgsqlDataAdapter selPat = new NpgsqlDataAdapter("SELECT family_name, first_name, last_name, birth_date, nib, pat_id, curr_doctor FROM patient_list where Upper(family_name) like '"
                    + (searchPatientComboBox1.Text.Trim()).ToUpper() + "%' order by family_name, first_name, last_name ASC ;", psDBC);
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


                }
                catch (Exception exception)
                {
                    System.Windows.Forms.MessageBox.Show(exception.Message.ToString() +" searchNow");
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
                    currDoc = Convert.ToInt32(tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["curr_doctor"]);
                    
                    patName = (string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["family_name"] + ""
                        + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["first_name"])[0] + ""
                        + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["last_name"])[0] + "";
                   
                    patInitials = ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["family_name"])[0] + ""
                        + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["first_name"])[0] + ""
                        + ((string)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["last_name"])[0] ;
                    TimeSpan pbd = DateTime.Now - ((DateTime)tblPatList.Rows[this.searchPatientComboBox1.SelectedIndex]["birth_date"]);
                    patBirthDate = Convert.ToInt32((long)(pbd.Days / 365.25));
                    this.label1.Text = patBirthDate.ToString() + " лет";
                }
                catch { }
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
                patientSet = true;
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
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() +" searchPatientDoc");
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
