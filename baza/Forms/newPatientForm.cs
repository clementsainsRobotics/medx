using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;

namespace baza
{
    

    public partial class newPatientForm : Form
    {
        public bool is_man;
        public bool resus;
        private bool birth;
        private bool isComment;
        private bool isTrustMan;
        private bool m1;
        private bool m2;
        private bool m3;
        private bool nnib;
        private bool editFlag;
        private DataTable tblPatient;
        private DataTable tblRegion;
        private DataTable tblCountry;
        private DataTable tblCity;
        private DataTable tblStreet;
        private DataTable tblDoc;
        private Int16 valCity;
        private Int16 valOkrug;
        private Int16 valRegion;
        private Int16 valCountry;
        private Int32 valStreet;
        private Int16 valRayon;
        private List<string> lstCiId;
        private List<string> lstCoId;
        private List<string> lstReId;
        private List<string> lstRaId;
        private List<string> lstStId;
        private List<string> lstStreet;
        private List<string> lstOkrug;
        private List<string> lstOkId;
        private int gotPatientId;
        private string strPatAddress;

        private int DocId;


        public newPatientForm(bool editData, int patIdN)
        {
            
            InitializeComponent();
            editFlag = false;
            DBExchange.Inst.chkRegion();

            List<string> lstCountry = new List<string>();


            foreach (DataRow row in DBExchange.Inst.tblCountry.Rows)
            {
                lstCountry.Add((string)row["text_value"]);
            }

            this.CountryBox.DataSource = lstCountry;
            if (editData == true)
            {
                editFlag = true;
                getPatientDataFromBase(patIdN);
                gotPatientId = patIdN;
            }
            else
            {
                this.CountryBox.SelectedIndex = 0;
                valCountry = 1;

                changeCountry(0);
                this.CountryBox.SelectedItem = 0;
                changeOkrug(1);
                this.comboBoxOkrug.SelectedIndex = 1;

                changeRegion(9);
                this.RegionBox.SelectedIndex = 9;

                chngCity(9);
                this.CityBox.SelectedIndex = 9;
            }

            DocId = DBExchange.Inst.dbUsrId;
            birth = false;

            updateAddrText();

         //   comboBox1.Items.Add(DBExchange.Inst.UsrSign);
         //   this.comboBox1.SelectedIndex = 0;
            
        }

        private void updateAddrText()
        {
            try
            {

                strPatAddress = this.CountryBox.SelectedValue + ", " + this.CityBox.SelectedValue + ", " + this.StreetBox.SelectedValue + " "
                    + this.textBox10.Text.Trim() + " корп." + this.textBox9.Text.Trim() + " кв." + this.maskedTextBox1.Text.Trim();
                if (editFlag == false)
                {

                    this.textBoxAddr.Text = strPatAddress;
                }
                else
                {
                    if (Convert.IsDBNull(tblPatient.Rows[0]["address"]) == false)
                    {
                        if (tblPatient.Rows[0]["address"].ToString().Length > 0 && this.StreetBox.SelectedIndex >= 0)
                        {
                            if (((string)tblPatient.Rows[0]["address"]).Contains((string)this.StreetBox.SelectedValue) == true)
                            {
                                this.textBoxAddr.Text = strPatAddress;
                            }
                            else
                            {
                                this.textBoxAddr.Text = (string)tblPatient.Rows[0]["address"];
                            }
                        }
                    }
                }
                this.textBoxAddr.Invalidate();
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                
            }
        }


        public void createDataForPatient()
        {
            try
            {

                DBExchange.Inst.chkPassport = false;
                if (checkPassport() == false || editFlag == true)
                {

                    String f_name = this.pFamilyNameTextBox1.Text.Trim();
                    String fi_name = this.pNameTextBox2.Text.Trim();
                    String se_name = this.p_s_name_textBox3.Text.Trim();



                    if (birth == false && editFlag == false)
                    {
                        MessageBox.Show("Укажите дату рождения", "Дата рождения указана неверно",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        DateTime bi_d = this.pdateTimePicker1.Value;

                        if (this.radioButton1.Checked == false && this.radioButton2.Checked == false)
                        {
                            MessageBox.Show("Укажите половую принадлежность пациента", "Это женщина или мужчина?",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            is_man = this.radioButton1.Checked;



                            if (f_name.Length < 2 || se_name.Length < 2 || fi_name.Length < 2)
                            {
                                MessageBox.Show("Проверьте ФИО пациента", "Неправильно заполнены данные",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                String korp = this.textBox9.Text;
                                DBExchange.Inst.getRegion();

                                DBExchange.Inst.getPatientStruct();
                                DataRow wnpRow = DBExchange.Inst.writeNewPatient.NewRow();

                                wnpRow["family_name"] = f_name;
                                wnpRow["first_name"] = fi_name;
                                wnpRow["last_name"] = se_name;
                                wnpRow["birth_date"] = bi_d;
                                if (this.PassBox.Text.Length == 12)
                                {
                                    DBExchange.Inst.chkPassport = true;
                                    Int64 pass = Convert.ToInt64(this.PassBox.Text.Remove(8, 1).Remove(4, 1));
                                    wnpRow["pass"] = pass;
                                    wnpRow["doc_ser"] = this.PassBox.Text.Substring(0, 4);
                                    wnpRow["doc_num"] = pass.ToString().Substring(4, 6);
                                    wnpRow["doc_date"] = this.dateTimePicker2.Value;
                                }
                                else
                                {
                                    MessageBox.Show("Проверьте паспортные данные ", "Возникла ошибка",
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                if (this.textBox1.Text.Trim().Length > 1)
                                {
                                    wnpRow["doc_org"] = textBox1.Text.Trim();

                                }


                                if (pHistoryNumbertextBox5.Text.Length > 3)
                                {
                                    wnpRow["nib"] = Convert.ToInt32(this.pHistoryNumbertextBox5.Text.Trim());
                                    nnib = true;
                                    wnpRow["nib_date"] = this.dateTimePicker1.Value;
                                }

                                wnpRow["is_man"] = is_man;
                                wnpRow["creator_id"] = DocId;
                                wnpRow["region1"] = valRegion;
                                wnpRow["state"] = valOkrug;
                                wnpRow["country"] = valCountry;
                                wnpRow["town_id"] = valCity;
                                wnpRow["area"] = valRayon;
                                if (this.StreetBox.SelectedIndex >= 0)
                                {
                                    wnpRow["street_n"] = Convert.ToInt32(lstStId[this.StreetBox.SelectedIndex]);
                                }
                                else
                                {
                                    wnpRow["street_n"] = DBNull.Value;

                                }
                                wnpRow["address"] = strPatAddress;
                                wnpRow["snils"] = this.maskedTextBoxSnils.Text.Trim();

                                if (this.textBox10.Text.Length > 0)
                                {

                                    wnpRow["house"] = this.textBox10.Text.Trim();
                                }
                                wnpRow["building"] = textBox9.Text.Trim();
                                if (maskedTextBox1.Text.Length > 0)
                                {
                                    wnpRow["flat"] = this.maskedTextBox1.Text.Trim();
                                }
                                if (m1Box.Text.Trim().Length == 16)
                                {
                                    m1 = true;
                                    Int64 ph1 = Convert.ToInt64(this.m1Box.Text.Remove(11, 1).Remove(6, 2).Remove(2, 1).Remove(0, 1));
                                    wnpRow["phone1"] = ph1;
                                }
                                if (m2Box.Text.Trim().Length == 16)
                                {
                                    Int64 ph2 = Convert.ToInt64(this.m2Box.Text.Remove(11, 1).Remove(6, 2).Remove(2, 1).Remove(0, 1));
                                    m2 = true;
                                    wnpRow["phone2"] = ph2;
                                }
                                if (m3Box.Text.Trim().Length == 16)
                                {
                                    Int64 ph3 = Convert.ToInt64(this.m3Box.Text.Remove(11, 1).Remove(6, 2).Remove(2, 1).Remove(0, 1));
                                    m3 = true;
                                    wnpRow["phone3"] = ph3;
                                }
                                if (textBox15.Text.Trim().Length > 2)
                                {
                                    isComment = true;
                                    wnpRow["comment"] = textBox15.Text.Trim();
                                }
                                if (textBox13.Text.Length > 4)
                                {
                                    isTrustMan = true;
                                    wnpRow["trust_man"] = textBox13.Text.Trim();
                                    wnpRow["trust_phone"] = textBox14.Text.Trim();
                                }
                                DBExchange.Inst.writeNewPatient.Rows.Add(wnpRow);

                                try
                                {
                                    if (editFlag == true)
                                    {
                                        createUpdatePatientString();
                                    }
                                    else
                                    {
                                        DBExchange.Inst.crtNewPatient(isComment, isTrustMan, m1, m2, m3, nnib);
                                    }
                                    clearData();

                                }
                                catch (Exception exception)
                                {
                                    Warnings.WarnLog log = new Warnings.WarnLog();
                                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                
            }
        
                        
        }

        private void createUpdatePatientString()
        {
            try 
            {
            string sqlString = "";

            DataTable wrTbl = DBExchange.Inst.writeNewPatient;

            if (wrTbl.Rows[0]["family_name"].ToString().Trim() != tblPatient.Rows[0]["family_name"].ToString().Trim())
            {
                sqlString = " family_name = '" + wrTbl.Rows[0]["family_name"].ToString().Trim() + "' , ";
            }
            else
            {
            }

            if (wrTbl.Rows[0]["first_name"].ToString().Trim() != tblPatient.Rows[0]["first_name"].ToString().Trim())
            {
                sqlString = sqlString + " first_name = '" + wrTbl.Rows[0]["first_name"].ToString().Trim() + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["last_name"].ToString().Trim() != tblPatient.Rows[0]["last_name"].ToString().Trim())
            {
                sqlString = sqlString + "last_name = '" + wrTbl.Rows[0]["last_name"].ToString().Trim() + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["comment"].ToString().Trim() != tblPatient.Rows[0]["comment"].ToString().Trim())
            {
                sqlString = sqlString + "comment = '" + wrTbl.Rows[0]["comment"].ToString().Trim() + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["trust_man"].ToString().Trim() != tblPatient.Rows[0]["trust_man"].ToString().Trim())
            {
                sqlString = sqlString + "trust_man = '" + wrTbl.Rows[0]["trust_man"].ToString().Trim() + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["trust_phone"].ToString().Trim() != tblPatient.Rows[0]["trust_phone"].ToString().Trim())
            {
                sqlString = sqlString + "trust_phone = '" + wrTbl.Rows[0]["trust_phone"].ToString().Trim() + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["phone1"].ToString().Trim() != tblPatient.Rows[0]["phone1"].ToString().Trim())
            {
                sqlString = sqlString + "phone1 = '" + wrTbl.Rows[0]["phone1"] + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["phone2"].ToString().Trim() != tblPatient.Rows[0]["phone2"].ToString().Trim())
            {
                sqlString = sqlString + "phone2 = '" + wrTbl.Rows[0]["phone2"] + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["phone3"].ToString().Trim() != tblPatient.Rows[0]["phone3"].ToString().Trim())
            {
                sqlString = sqlString + "phone3 = '" + wrTbl.Rows[0]["phone3"] + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["nib"].ToString().Trim() != tblPatient.Rows[0]["nib"].ToString().Trim())
            {
                sqlString = sqlString + "nib = '" + wrTbl.Rows[0]["nib"] + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["birth_date"].ToString().Trim() != tblPatient.Rows[0]["birth_date"].ToString().Trim())
            {
                sqlString = sqlString + "birth_date = '" + wrTbl.Rows[0]["birth_date"] + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["pass"].ToString().Trim() != tblPatient.Rows[0]["pass"].ToString().Trim())
            {
                sqlString = sqlString + "pass = '" + wrTbl.Rows[0]["pass"] + "', doc_ser ='" + wrTbl.Rows[0]["doc_ser"] +
                    "', doc_num ='" + wrTbl.Rows[0]["doc_num"] + "', doc_org ='" + wrTbl.Rows[0]["doc_org"] + "', doc_date ='" + wrTbl.Rows[0]["doc_date"] + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["doc_date"].ToString().Trim() != tblPatient.Rows[0]["doc_date"].ToString().Trim())
            {
                sqlString += " doc_date ='" + wrTbl.Rows[0]["doc_date"] + "', ";
            }
            if (wrTbl.Rows[0]["doc_org"].ToString().Trim() != tblPatient.Rows[0]["doc_org"].ToString().Trim())
            {
                sqlString += " doc_org ='" + wrTbl.Rows[0]["doc_org"] + "', ";
            }

            if (wrTbl.Rows[0]["is_man"].ToString().Trim() != tblPatient.Rows[0]["is_man"].ToString().Trim())
            {
                sqlString = sqlString + "is_man = '" + wrTbl.Rows[0]["is_man"] + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["region1"].ToString().Trim() != tblPatient.Rows[0]["region1"].ToString().Trim())
            {
                sqlString = sqlString + "region1 = '" + wrTbl.Rows[0]["region1"] + "', ";
            }
            else
            {
            }
            if (wrTbl.Rows[0]["state"].ToString().Trim() != tblPatient.Rows[0]["state"].ToString().Trim())
            {
                sqlString = sqlString + "state = '" + wrTbl.Rows[0]["state"] + "', ";
            }

            if (wrTbl.Rows[0]["country"].ToString().Trim() != tblPatient.Rows[0]["country"].ToString().Trim())
            {
                sqlString = sqlString + "country = '" + wrTbl.Rows[0]["country"] + "', ";
            }

            if (wrTbl.Rows[0]["town_id"].ToString().Trim() != tblPatient.Rows[0]["town_id"].ToString().Trim())
            {
                sqlString = sqlString + "town_id = '" + wrTbl.Rows[0]["town_id"] + "', ";
            }

            if (wrTbl.Rows[0]["area"].ToString().Trim() != tblPatient.Rows[0]["area"].ToString().Trim())
            {
                sqlString = sqlString + "area = '" + wrTbl.Rows[0]["area"] + "', ";
            }

            if (wrTbl.Rows[0]["street_n"].ToString().Trim() != tblPatient.Rows[0]["street_n"].ToString().Trim())
            {
                sqlString = sqlString + "street_n = '" + wrTbl.Rows[0]["street_n"] + "', ";
            }

            if (wrTbl.Rows[0]["house"].ToString().Trim() != tblPatient.Rows[0]["house"].ToString().Trim())
            {
                sqlString = sqlString + "house = '" + wrTbl.Rows[0]["house"] + "', ";
            }

            if (wrTbl.Rows[0]["building"].ToString().Trim() != tblPatient.Rows[0]["building"].ToString().Trim())
            {
                sqlString = sqlString + "building = '" + wrTbl.Rows[0]["building"] + "', ";
            }

            if (wrTbl.Rows[0]["flat"].ToString().Trim() != tblPatient.Rows[0]["flat"].ToString().Trim())
            {
                sqlString = sqlString + "flat = '" + wrTbl.Rows[0]["flat"] + "', ";
            }

            if (wrTbl.Rows[0]["address"].ToString().Trim() != tblPatient.Rows[0]["address"].ToString().Trim())
            {
                sqlString = sqlString + "address = '" + wrTbl.Rows[0]["address"].ToString().Trim() + "' ,";
            }
            if (wrTbl.Rows[0]["snils"].ToString().Trim() != tblPatient.Rows[0]["snils"].ToString().Trim())
            {
                sqlString = sqlString + "snils = '" + wrTbl.Rows[0]["snils"].ToString().Trim() + "' ,";
            }


                sqlString = sqlString + " creator_id = '" + DBExchange.Inst.dbUsrId + "' ";
            

          
                sqlString = "UPDATE patient_list SET " + sqlString + " where pat_id = '" + gotPatientId + "'";

                DBExchange.Inst.execSqlCommand(sqlString);
            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            finally
            {

            }
        }


        private void clearData()
        {
            this.pFamilyNameTextBox1.ResetText();
            this.pNameTextBox2.ResetText();
            this.p_s_name_textBox3.ResetText();
            this.pdateTimePicker1.ResetText();
            this.pHistoryNumbertextBox5.ResetText();
            this.dateTimePicker1.ResetText();
            this.radioButton1.Checked = false;
            this.radioButton2.Checked = false;
            this.PassBox.ResetText();
            this.StreetBox.ResetText();
            this.textBox10.ResetText();
            this.textBox9.ResetText();
            this.m1Box.ResetText();
            this.m2Box.ResetText();
            this.m3Box.ResetText();
            this.textBox13.ResetText();
            this.textBox14.ResetText();
            this.textBox15.ResetText();
            this.maskedTextBox1.ResetText();
            this.StreetBox.ResetText();
            this.textBox1.ResetText();
            this.dateTimePicker2.Value = DateTime.Now;

        }

        public void insertPatientData()
        {
            createDataForPatient();
            
        }

        private void записатьToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            insertPatientData();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newPatientForm.ActiveForm.Close();
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearData();
        }

        private void pdateTimePicker1_Enter(object sender, EventArgs e)
        {
            birth = true;
        }

        private void PassBox_Leave(object sender, EventArgs e)
        {
            if (editFlag == false)
            {
                checkPassport();
            }
        }

        bool checkPassport()
        {
           bool gotThisPassport = false;
            if (this.PassBox.Text.Trim().Length == 12)
            {
                Int64 chkPassPatient = Convert.ToInt64(this.PassBox.Text.Remove(8, 1).Remove(4, 1));
                DBExchange.Inst.chkPatInBaseByPass(chkPassPatient);
                if
                (DBExchange.Inst.chkPassport == true && editFlag == false)
                {
                    MessageBox.Show("Проверьте введённый паспорт или пациента в базе данных", "Пациент с таким паспортом уже существует",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    gotThisPassport = true;
                }
            }
            else
            {
                MessageBox.Show("Проверьте введённый паспорт или оставьте поле пустым", "Такого пасспорта нет",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return gotThisPassport;
        }

        private void changeCountry(Int16 countryItem)
        {
            this.comboBoxOkrug.ResetText();
            tblCountry = DBExchange.Inst.tblCountry;
           
            Int16 coId = Convert.ToInt16(tblCountry.Rows[countryItem]["int_value"]);
            valCountry = coId;


            lstOkrug = new List<string>();
            lstOkId = new List<string>();

           if (coId == 1)
           {
               foreach (DataRow row in DBExchange.Inst.tblState.Select("c_id = 1"))
               {
                   lstOkrug.Add(((string)row["text_value"]).Trim());
                   lstOkId.Add(Convert.ToString(row["int_value"]));

               }

           }

           else
           {

               foreach (DataRow row in DBExchange.Inst.tblState.Select("c_id = 2"))
               {
                   lstOkrug.Add(((string)row["text_value"]).Trim());
                   lstOkId.Add(Convert.ToString(row["int_value"]));

               }
           }

           this.comboBoxOkrug.DataSource = lstOkrug;

            this.RegionBox.ResetText();
            this.CityBox.ResetText();
            this.RayonBox.ResetText();
            this.StreetBox.ResetText();

            changeOkrug(0);

            
        }


        private void changeOkrug(Int16 countryItem)
        {
            

            Int16 stateId = Convert.ToInt16(lstOkId[countryItem]);
            valOkrug = stateId;
           
            tblRegion = DBExchange.Inst.tblRegion;
            List<string> lstRegion = new List<string>();
            lstReId = new List<string>();
            foreach (DataRow row in tblRegion.Select("state = '" + stateId + "'"))
            {
                lstRegion.Add(((string)row["text_value"]).Trim());
                lstReId.Add(Convert.ToString(row["int_value"]));

            }

            this.RegionBox.ResetText();
            if (valCountry == 1)
            {
                this.RegionBox.DataSource = lstRegion;
                this.RegionBox.Invalidate();
                this.CityBox.ResetText();
                this.RayonBox.ResetText();
                this.StreetBox.ResetText();
                
                changeRegion(0);
            }
            else
            {
                this.RegionBox.DataSource = null;
                this.RegionBox.Invalidate();
                this.CityBox.ResetText();
                this.RayonBox.ResetText();
                this.StreetBox.ResetText();
            }



        }




        private void CountryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeCountry(Convert.ToInt16(this.CountryBox.SelectedIndex));
        }

        
        private void changeRegion(Int16 regionItem)
        {
            
            Int16 reId = Convert.ToInt16(lstReId[regionItem]);
            valRegion = reId;
            List<string> lstCity = new List<string>();
            lstCiId = new List<string>();
            try
            {
                
                foreach (DataRow row in DBExchange.Inst.tblCities.Select("c_id='"+reId+"'"))
                {
                    lstCity.Add(((string)row["text_value"]).Trim());
                    lstCiId.Add(Convert.ToString(row["int_value"]));
                    
                }
                this.CityBox.DataSource = lstCity;
            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            
            this.CityBox.Invalidate();
            chngRayon(0);
      //       chngCity(0);

        }

        

        private void RegionBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            changeRegion(Convert.ToInt16(this.RegionBox.SelectedIndex));
        }

        private void chngCity(Int16 cityItem)

        {
            this.StreetBox.ResetText();
            Int16 vCi = Convert.ToInt16(lstCiId[cityItem]);
            List<string> listStreet = new List<string>();
            DBExchange.Inst.getStreetAddr(vCi);
            DataTable tSt = DBExchange.Inst.tblStreet;
            lstStId = new List<string>();
            foreach (DataRow rowS in tSt.Rows)
            {

                lstStId.Add(Convert.ToString((int)rowS["serial"]));
                listStreet.Add(((string)rowS["addr"]).Trim());
            }
            this.StreetBox.DataSource = listStreet;
            this.StreetBox.Invalidate();
            if (editFlag == true)
            {
                if (tblPatient.Rows[0]["street_n"].ToString() != "")
                {
                int strId = (int)tblPatient.Rows[0]["street_n"];

                    int stre = (int)lstStId.IndexOf((strId).ToString());

                    this.StreetBox.SelectedIndex = stre;

                
                }

            }

            valCity = vCi;

            chngRayon(vCi);
            updateAddrText();
        }

        private void chngRayon(Int16 CiId)

        {
            this.RayonBox.ResetText();
            lstRaId = new List<string>();
            List<string> lstRayon = new List<string>();

            //valCity = Convert.ToInt16(lstCiId[CiId]);

            foreach (DataRow rowR in DBExchange.Inst.tblArea.Select(" c_id = '" + CiId + "'"))
            {
                lstRaId.Add(Convert.ToString(rowR["int_value"]));
                lstRayon.Add(Convert.ToString(rowR["text_value"]).Trim());
            }

            this.RayonBox.DataSource = lstRayon;

            this.RayonBox.Invalidate();
        }


        private void CityBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            chngCity(Convert.ToInt16(this.CityBox.SelectedIndex));
           // chngRayon(Convert.ToInt16(this.CityBox.SelectedIndex));
        }

 
        private void getNames()
        {
            DataTable tblNames = new DataTable();
            AutoCompleteStringCollection lNames = new AutoCompleteStringCollection();
            AutoCompleteStringCollection lFNames = new AutoCompleteStringCollection();
            if (this.radioButton1.Checked == true)
            {


               
                NpgsqlDataAdapter gN = new NpgsqlDataAdapter("Select m_name, m_sur from names", DBExchange.Inst.connectDb);
                gN.Fill(tblNames);

                foreach (DataRow nRow in tblNames.Rows)
                {
                    lNames.Add(Convert.ToString(nRow["m_name"]));
                    lFNames.Add(Convert.ToString(nRow["m_sur"]));
                }
               
            }
            else
            {
                
                NpgsqlDataAdapter gN = new NpgsqlDataAdapter("Select f_name, f_sur from names", DBExchange.Inst.connectDb);
                gN.Fill(tblNames);

                foreach (DataRow nRow in tblNames.Rows)
                {
                    lNames.Add(Convert.ToString(nRow["f_name"]));
                    lFNames.Add(Convert.ToString(nRow["f_sur"]));
                }
              
            }

            this.pNameTextBox2.AutoCompleteCustomSource = lNames;
            this.p_s_name_textBox3.AutoCompleteCustomSource = lFNames ;
            
            }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            getNames();
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            getNames();
        }


        private void setStreet()
        {
            
            try


            {
               valStreet = Convert.ToInt16(this.StreetBox.SelectedIndex);     
            }
            catch
            { }
            if (valStreet == -1)
            {
                String txtStreet = this.StreetBox.Text;
                if (txtStreet != null)
                {
                    try
                    {
                       
                        NpgsqlCommand selStr = new NpgsqlCommand("Select serial from streets where city = '"+valCity+"' and UPPER(addr) = '%" + txtStreet.ToUpper() + "%';", DBExchange.Inst.connectDb);
                        try
                        {
                            Int32 StrAddr = (Int32)selStr.ExecuteScalar();
                        }
                        catch
                        //  if (StrAddr == 0)
                        
                        //  {if (valRayon)
                        

                        {
                            if ((MessageBox.Show("добавить адрес в справочник? " + CityBox.Text + " " + StreetBox.Text , "Улица не найдена", MessageBoxButtons.YesNo)) == DialogResult.Yes)
                            {

                                NpgsqlCommand insStr = new NpgsqlCommand("Insert into streets (addr, city) Values ('" + txtStreet + "','" + lstCiId[this.CityBox.SelectedIndex] + "');", DBExchange.Inst.connectDb);
                            insStr.ExecuteNonQuery();
                        
                            }
                        }
                        finally
                        {
                           
                        }

                        chngCity(Convert.ToInt16(this.CityBox.SelectedIndex));

                        }





                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                        
                    }
                }
                else 
                {
                    valStreet = Convert.ToInt32(lstStId[valStreet]);
                }
           }
    
            updateAddrText();
           

        }

        private void StreetBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            setStreet();
    
        }

        private void RayonBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            valRayon = Convert.ToInt16(lstRaId[this.RayonBox.SelectedIndex]);
        }

        private void добавитьАдресToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setStreet();
        }

        private void StreetBox_Leave(object sender, EventArgs e)
        {
            setStreet();
        
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            tblDoc = new DataTable();
            List<string> lstDoc = new List<string>();
            NpgsqlConnection cdbo = DBExchange.Inst.connectDb;
            try
            {
                NpgsqlDataAdapter getDoc = new NpgsqlDataAdapter("Select family_name, first_name, last_name, doc_id from doctors where status ='' ",cdbo);
                
                getDoc.Fill(tblDoc);
               
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                
            }
            //нужно получить лист имен докторов и при выборе в боксе устанавливать DocNum
           // this.comboBox1.DataSource = lstDoc;

            }

        private void comboBoxOkrug_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeOkrug(Convert.ToInt16(this.comboBoxOkrug.SelectedIndex));
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            updateAddrText();
        }

        private void getPatientDataFromBase(int thisId)
        {
            
            
            NpgsqlConnection cdbo = DBExchange.Inst.connectDb;
            tblPatient = new DataTable();
            try
            {
                NpgsqlDataAdapter getPatData = new NpgsqlDataAdapter("Select * from patient_list where pat_id ='" + thisId + "' ", cdbo);

                getPatData.Fill(tblPatient);
                gotPatientId = thisId;
                if (Convert.IsDBNull(tblPatient.Rows[0]["pass"]) == false)
                {
                    if ((Int64)tblPatient.Rows[0]["pass"] > 0)
                    {
                        this.PassBox.Text = tblPatient.Rows[0]["pass"].ToString();
                    }
                    else
                    {
                        this.PassBox.Text = tblPatient.Rows[0]["doc_ser"].ToString().Trim() + "" + tblPatient.Rows[0]["doc_num"].ToString().Trim();
                    }
                }
                this.pFamilyNameTextBox1.Text = tblPatient.Rows[0]["family_name"].ToString();
                this.pNameTextBox2.Text = tblPatient.Rows[0]["first_name"].ToString();
                this.p_s_name_textBox3.Text = tblPatient.Rows[0]["last_name"].ToString();
                this.maskedTextBoxSnils.Text = tblPatient.Rows[0]["snils"].ToString().Trim();
                this.dateTimePicker2.Value = (DateTime)tblPatient.Rows[0]["doc_date"];
                this.textBox1.Text = (string)tblPatient.Rows[0]["doc_org"].ToString().Trim();
                if ((bool)tblPatient.Rows[0]["is_man"] == true)
                {
                    this.radioButton1.Checked = true;
                }
                else
                {
                    this.radioButton2.Checked = true;
                }
                pdateTimePicker1.Value = (DateTime)tblPatient.Rows[0]["birth_date"];

                if (Convert.IsDBNull(tblPatient.Rows[0]["nib_date"])==true || ((DateTime)tblPatient.Rows[0]["nib_date"]).Year.ToString() == "1001")
                {
                    dateTimePicker1.Value = DateTime.Today;
                }
                else
                {
                    dateTimePicker1.Value = (DateTime)tblPatient.Rows[0]["nib_date"];
                }

                if ((int)tblPatient.Rows[0]["nib"] > 0)
                {
                    pHistoryNumbertextBox5.Text = ((int)tblPatient.Rows[0]["nib"]).ToString();
                }
                int country_id = 1;
                if (Convert.IsDBNull(tblPatient.Rows[0]["country"]) == false)
                {
                    country_id = (int)tblPatient.Rows[0]["country"];
                }

                if (Convert.IsDBNull(tblPatient.Rows[0]["town_id"]) == false)
                {
                    if ((int)tblPatient.Rows[0]["town_id"] == 210)
                    {
                        this.CountryBox.SelectedIndex = 0;
                        valCountry = 1;

                        changeCountry(0);
                        this.CountryBox.SelectedItem = 0;

                        changeOkrug(1);
                        this.comboBoxOkrug.SelectedIndex = 1;

                        changeRegion(9);
                        this.RegionBox.SelectedIndex = 9;

                        chngCity(9);
                        this.CityBox.SelectedIndex = 9;



                    }
                    else
                    {
                        if (country_id != 1)
                        {
                            changeCountry(Convert.ToInt16(DBExchange.Inst.tblCountry.Select("int_value=" + country_id + "").GetEnumerator().Current));

                        }
                        else
                        {
                            changeCountry(0);
                            this.CountryBox.SelectedItem = 0;
                        }
                        changeOkrug(Convert.ToInt16(lstOkId.IndexOf((tblPatient.Rows[0]["state"]).ToString())));
                        Int16 regId = Convert.ToInt16(lstReId.IndexOf((tblPatient.Rows[0]["region1"]).ToString()));
                        changeRegion(regId);
                        this.RegionBox.SelectedIndex = regId;
                        Int16 cityId = Convert.ToInt16(lstCiId.IndexOf((tblPatient.Rows[0]["town_id"]).ToString()));
                        chngCity(cityId);
                        this.CityBox.SelectedIndex = cityId;



                    }
                }

                if (Convert.IsDBNull(tblPatient.Rows[0]["house"]) == false)
                {
                    textBox10.Text = (string)tblPatient.Rows[0]["house"];
                }
                                
                if (Convert.IsDBNull(tblPatient.Rows[0]["building"]) == false)
                {
                
                    textBox9.Text = (string)tblPatient.Rows[0]["building"];
                                                    
                }

                                
                if (Convert.IsDBNull(tblPatient.Rows[0]["flat"]) == false)
                {
                
                                    
                    maskedTextBox1.Text = (string)tblPatient.Rows[0]["flat"];
                                                    
                }

                                
                if (Convert.IsDBNull(tblPatient.Rows[0]["area"]) == false)
                                {
                                    if (tblPatient.Rows[0]["area"].ToString() != "")
                                    {
                                        Int16 areaId = (Int16)tblPatient.Rows[0]["area"];

                                        if (areaId > 0 & lstRaId.Count > 0)
                                        {
                                            Int16 cri = Convert.ToInt16(lstRaId.IndexOf((areaId).ToString()));
                                            //chngRayon(cri);
                                            RayonBox.SelectedIndex = cri;

                                        }
                                    }
                                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                
            }

        }




    }
}
