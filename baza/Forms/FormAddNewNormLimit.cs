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
    public partial class FormAddNewNormLimit : Form
    {
        private DataTable dtLabs;

        private baza.Processing.LabTickets.NormClassList nList;

        private Document.SampleType.SList lTypes;
        private Document.SampleType.MeasureList MTypes;
        private Processing.LabTickets.NormAgeList NAList;
        private Processing.LabTickets.NormLimitList NLList;
        private Processing.LabTickets.NormLimitClassFullList gotNormLimit;

        public FormAddNewNormLimit()
        {
            InitializeComponent();
            this.dateTimePicker2.Value = this.dateTimePicker2.Value.AddYears(5);
            loadLab();
            loadSampleType();
            loadMeasureTypes();
       //    loadSampleMeasure();
            // getNormAges();
        }

        //загружает лаборатории
        private void loadLab()
        {

            dtLabs = new DataTable();
            NpgsqlDataAdapter getLabs = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '2' ;",DBExchange.Inst.connectDb);
            try
            {
                getLabs.Fill(dtLabs);
                comboBoxLabName.Items.Clear();
                foreach (DataRow ro in dtLabs.Rows)
                {
                    comboBoxLabName.Items.Add((string)ro["txt"]);
                }
                comboBoxLabName.Items.Add("Добавить новую лабораторию ...");

            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();   
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                
            }
            this.comboBoxLabName.Refresh();

        }

        //загружает типы материала
        private void loadSampleType()
        {
            lTypes = new baza.Document.SampleType.SList();
            lTypes.SampleListGet();

            comboBoxTypes.Items.Clear();
            foreach (Document.SampleType.SampleItem i in lTypes)
            {
                comboBoxTypes.Items.Add(i.SampleName);
            }
            comboBoxTypes.Items.Add("Добавить новый тип ...");


            this.comboBoxTypes.Refresh();


        }

        //загружает измерители показателей
        //private void loadSampleMeasure()
        //{


        //    MTypes = new baza.Document.SampleType.MeasureList();
        //    MTypes.MeasureListGet();

        //    comboBox1.Items.Clear();
        //    foreach (Document.SampleType.MeasureItem i in MTypes)
        //    {
        //        comboBox1.Items.Add(i.SampleName);
        //    }
        //    comboBox1.Items.Add("Добавить новую единицу ...");


        //    this.comboBox1.Refresh();



        //}


        private void loadMeasureTypes()
        {

            MTypes = new baza.Document.SampleType.MeasureList();
            MTypes.MeasureListGet();

            comboBox1.Items.Clear();
            foreach (Document.SampleType.MeasureItem i in MTypes)
            {
                comboBox1.Items.Add(i.SampleName);
            }
            comboBox1.Items.Add("Добавить новый тип ...");
            this.comboBox1.Refresh();


        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedItem != null)

            {
                if (this.comboBox1.SelectedItem.ToString().Trim().Contains("Добавить новый тип ..."))
                {
                    Forms.FormAddNewLab fanl = new Forms.FormAddNewLab(3, 0, 0);
                    fanl.ShowDialog();
                    loadMeasureTypes();
                }
            }
        }


        //загружает нормы по типу материала
        private void loadNorms(int _sampleType)
        {

            nList = new baza.Processing.LabTickets.NormClassList();
            nList.getNormsByType(_sampleType);

            checkedListBox1.Items.Clear();
          
            //comboBox2.Items.Clear();
            //comboBox2.Text = "";
            foreach (Processing.LabTickets.NormClass i in nList)
            {
                checkedListBox1.Items.Add(i.NormName);
            }
            //  comboBox2.Items.Add("Добавить новый показатель ...");


            this.checkedListBox1.Refresh();
        }


        //Получает нормы для лаборатории

        private void loadNormLimitList()
        {
            try
            {
                NLList = new Processing.LabTickets.NormLimitList();
                foreach (int i in checkedListBox1.CheckedIndices)
                {
                   
                        checkedListBox1.SetItemChecked(i, false);
                    
                }

                if (this.comboBoxLabName.SelectedIndex != -1 && this.comboBoxTypes.SelectedIndex != -1)
                {
                    short _type = Convert.ToInt16(lTypes[comboBoxTypes.SelectedIndex].SampleType);
                    int _lab = (int)dtLabs.Rows[this.comboBoxLabName.SelectedIndex]["code_id"];
                    NLList.loadLabNormLimits(_lab, _type);
                    foreach (Processing.LabTickets.NormLimit i in NLList)
                    {
                        if (nList.Count > 0)
                        {
                            int kL = nList.IndexOf(nList.Find(o => o.NormID == i.NormLimitId));
                            if (kL >= 0)
                            {
                                checkedListBox1.SetItemChecked(kL, true);
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



        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                gotItemChecked();
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                gotItemUnchecked();
            }
          //  loadNormLimitList();
            this.checkedListBox1.Refresh();
        }


        private void gotItemChecked()
        {
            if (NLList.Count == this.checkedListBox1.CheckedItems.Count)
            {
                int thisInd = this.checkedListBox1.SelectedIndex;
                if (thisInd >= 0)
                {

                    Processing.LabTickets.NormLimit nLimit = new Processing.LabTickets.NormLimit();
                    nLimit.NormLimitId = nList[thisInd].NormID;
                    nLimit.NormName = nList[thisInd].NormName;
                    if (NLList == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Выберите лабораторию");
                    }
                    else
                    {
                        int typeId = lTypes[this.comboBoxTypes.SelectedIndex].SampleType;
                        int labId = (int)dtLabs.Rows[this.comboBoxLabName.SelectedIndex]["code_id"];

                        string _to = "";
                        string _values = "";

                        if (this.comboBox1.SelectedIndex >= 0)
                        {
                           short uId = Convert.ToInt16(MTypes[this.comboBox1.SelectedIndex].SampleType);
                            _to += ", unit_id";
                            _values += ", '" + uId + "'";
                        }
                        string insText = nLimit.NormName;
                        if (insText.Length >= 50)
                        {
                            insText = insText.Substring(0, 50);
                        }

                        NpgsqlCommand checkItem = new NpgsqlCommand("Insert Into norm_limits (from_ot, to_do, test_name, lab_id, norm_id, type "+_to+") values ('" 
                            + this.dateTimePicker1.Value + "', '" + this.dateTimePicker2.Value + "', '"+ insText +"', '"+labId+"','"+nLimit.NormLimitId+"','"+typeId+"' "+_values+") ;", DBExchange.Inst.connectDb);
                        try
                        {
                            checkItem.ExecuteNonQuery();
                            NLList.Add(nLimit);
                        }
                        catch (Exception exception)
                        {
                            Warnings.WarnLog log = new Warnings.WarnLog();
                            log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                            
                        }
                    }

                }

                else if (this.checkedListBox1.SelectedIndex == -1)
                { System.Windows.Forms.MessageBox.Show("Выберите показатель из списка"); }
            }
        }

        private void gotItemUnchecked()
        {
            try
            {
                if (this.checkedListBox1.SelectedIndex >= 0 && NLList.Count > 0)
                {   
                    int thisInd = this.checkedListBox1.SelectedIndex;

                        int i = NLList.IndexOf(NLList.Find(o => o.NormLimitId == nList[thisInd].NormID));
                        int labId = (int)dtLabs.Rows[this.comboBoxLabName.SelectedIndex]["code_id"];
                        NpgsqlCommand uncheckItem = new NpgsqlCommand("Update norm_limits set delete_this = true where lab_id = '" + labId + "' and norm_id = '" + nList[thisInd].NormID + "' ;", DBExchange.Inst.connectDb);
                        uncheckItem.ExecuteNonQuery();
                        NLList.RemoveAt(i);
                    }
                
                //  this.checkedListBox1.SetItemChecked(this.checkedListBox1.SelectedIndex, false);
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog(); 
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString()); 
                
            }
        }



        //получает\проверяет данные для записи
        private void getDataForInsert()
        {

            string _upperLimit= "";
            string _lowerLimit="";
        //    try
        //    {
        //        if (this.textBox2.Text.Trim().Length > 0)
        //        {
        //            _upperLimit = this.textBox2.Text.Trim().Replace(',', '.');
        //        }
        //        else { _upperLimit = "NULL"; }
        //        if (this.textBox3.Text.Trim().Length > 0)
        //        {
        //            _lowerLimit = this.textBox3.Text.Trim().Replace(',','.');
        //        }
        //        else { _lowerLimit = "NULL"; }
                insertNormIntoBase(_upperLimit, _lowerLimit);
        //    }
        //    catch
        //        (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show("Неверный тип данных в полях предела показателя",ex.Message.ToString() 
        //        + " " + MethodBase.GetCurrentMethod().Name );
                
        //    }
            


       }

        //получает возрастные группы
        //public void getNormAges()
        //{

        //    NAList = new Processing.LabTickets.NormAgeList();
        //    NAList.getNormAges();

        //    foreach (Processing.LabTickets.NormAges i in NAList )
        //    {

        //        comboBox3.Items.Add(i.NormAgeId + ". " + i.NormAgeFrom + " " + i.NormAgeTo);

        //    }
        //    comboBox3.Refresh();

        //}

        //проверка имени в базе
        private bool checkNormName(string _norm, int _lab_id)
        {
            bool gotName = false;
            this.toolStripStatusLabel1.Text = "Проверяем показатель в базе.";
            if (NLList.Exists(o => o.NormLimitId == nList[this.checkedListBox1.SelectedIndex].NormID) == true)
            {
                gotName = true;
            }

            //string _selNorm = "Select test_name_id from norm_limits where trim(lower(test_name)) = '"+_norm.Trim().ToLower()+"' ;";
            //if (_lab_id != -1)
            //{
            //    _selNorm = "Select test_name_id from norm_limits where trim(lower(test_name)) = '" + _norm.Trim().ToLower() + "' and lab_id = '"+_lab_id+"';";
            //}
            //NpgsqlCommand getNorm = new NpgsqlCommand(_selNorm,DBExchange.Inst.connectDb);
            //try
            //{
            //    int gotNorm = -1;
            //    gotNorm = (int)getNorm.ExecuteScalar();
            //    if (gotNorm > 0)
            //    {
            //        gotName = true;
            //    }

            //}
            //catch { };

            return gotName;
        }

        //Запись нормы в базу данных
        private void insertNormIntoBase(string _ul, string _ll)
        {
            bool gotLab = false;
            this.toolStripStatusLabel1.Text = "Подготовка к записи.";
            string _normName = nList[this.checkedListBox1.SelectedIndex].NormName; //this.textBox1.Text.Trim() ;
            int normId = nList[this.checkedListBox1.SelectedIndex].NormID;
            int typeId = -1;
            int labId = -1;
            short uId = -1;
            if (this.comboBox1.SelectedIndex >= 0)
            {
                uId = Convert.ToInt16(MTypes[this.comboBox1.SelectedIndex].SampleType);
            }
            bool gotType = false;
            bool gotComment = false;
            string _command = "";
            string _to;
            string _values;
            string  _gotCode = this.textBoxCode.Text.Trim();
            
             if (this.comboBoxLabName.SelectedIndex != -1)
                {
                    labId = (int)dtLabs.Rows[this.comboBoxLabName.SelectedIndex]["code_id"];

                    if (checkedListBox1.SelectedIndex == -1)
                        {

                            if (checkNormName(_normName, labId) == false)
                            {
                             _to = "test_name, norm_id, from_ot, to_do ";
                             _values = "'" +_normName + "', '"+normId+"', '" + this.dateTimePicker1.Value + "', '" + this.dateTimePicker2.Value +"'";
                       
                                gotLab = true;
                                _to += ", lab_id";
                                _values += ", '" + labId + "'";


                                //if (this.radioButton3.Checked == false)
                                //{
                                //    _to += ", is_man";
                                //    _values += ",'"+this.radioButton1.Checked+ "'";
                                //}

                               
                                if (this.comboBox1.SelectedIndex >= 0)
                                {
                                    uId = Convert.ToInt16(MTypes[this.comboBox1.SelectedIndex].SampleType);
                                    _to += ", unit_id";
                                    _values += ", '" + uId + "'";
                                }

                                if (this.comboBoxTypes.SelectedIndex != -1)
                                {
                                    typeId = lTypes[this.comboBoxTypes.SelectedIndex].SampleType;
                                    gotType = true;
                                    _to += ", type";
                                    _values += ", '" + typeId + "'";
                                }

                                //if (this.comboBox1.SelectedIndex != -1)
                                //{
                                //    string MeasureId = this.comboBox1.SelectedItem.ToString().Trim(); ;
                                //    int mId = MTypes[this.comboBox1.SelectedIndex].SampleType;
                                //    _to += ", unit_id";
                                //    _values += ", '" + mId + "'";
                                //}


                                //if (this.richTextBox1.Text.Length > 1)
                                //{
                                //    gotComment = true;
                                //    _to += ", comment";
                                //    _values += ", '" + this.richTextBox1.Text.Trim()+"'";
                                //}



                                if (_gotCode.Length >= 1)
                                {
                                   
                                    _to += ", test_code";
                                    _values += ", '" + _gotCode + "'";
                                }

                                //     if (this.maskedTextBox1.Text.Length >= 1)
                                //     {
                                //        _to += ", age_from_ot, age_to_do ";
                                //         _values += ", '" + Convert.ToDecimal( maskedTextBox2.Text.ToString()) + "', '" + Convert.ToDecimal( maskedTextBox1.Text.ToString()) + "' ";

                                //     }
                      


                                _command = "insert into norm_limits (" + _to + ") VALUES (" + _values + ") ;";

                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Такой показатель уже существует в лаборатории");
                                return;
                               
                            }
                        }
                        else
                        {
                            _command = "Update norm_limits set test_name = '"+_normName+"', from_ot = '"+this.dateTimePicker1.Value+"', to_do = '"+this.dateTimePicker2.Value
                                +"', test_code = '"+_gotCode+"', unit_id = '"+uId+"' where test_name_id = '"+gotNormLimit[0].NormID +"' ;";
                        }
                
                        try
                        {
                            NpgsqlCommand insNorm = new NpgsqlCommand(_command, DBExchange.Inst.connectDb);
                            insNorm.ExecuteNonQuery();
                            //this.textBox1.Text = "";
                            //this.richTextBox1.Text = "";
                            //this.radioButton3.Select();
                            //this.textBox2.Text = "";
                            //this.textBox3.Text = "";
                            this.toolStripStatusLabel1.Text = "Показатель успешно записан в базу.";
                            this.textBoxCode.Text = "";
                            loadNormLimitList();
                            this.checkedListBox1.SelectedIndex = 0;
                       //     this.checkedListBox1.SelectedIndex = 0;
                        }
                        catch
                            (Exception exception)
                        {
                            Warnings.WarnLog log = new Warnings.WarnLog(); 
                            log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                        }
                   }
              else
                {
                    System.Windows.Forms.MessageBox.Show("Выберите лабораторию");
                }
                    

        }

        //проверка мужчина женщина общий
        //private void radioButton1_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.radioButton2.Checked = false;
        //    this.radioButton3.Checked = false;
        //    this.radioButton1.Select();
        //    this.radioButton1.Refresh();
        //}

        //private void radioButton2_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.radioButton3.Checked = false;
        //    this.radioButton1.Checked = false;
        //    this.radioButton2.Select();
        //    this.radioButton2.Refresh();
           
        //}

        //private void radioButton3_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.radioButton2.Checked = false;
        //    this.radioButton1.Checked = false;
        //    this.radioButton3.Select();
        //    this.radioButton3.Refresh();
        //}

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Forms.FormAddNewNormLimit.ActiveForm.Close();
        }

        private void comboBoxTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxTypes.SelectedItem.ToString().Trim() == "Добавить новый тип ...")
            {
                Forms.FormAddNewLab fanl = new FormAddNewLab(2,0,0);
                fanl.ShowDialog();
                loadSampleType();
            }
            else
            {
                loadNorms(lTypes[comboBoxTypes.SelectedIndex].SampleType);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            getDataForInsert();
        }

        private void comboBoxLabName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Добавить новую лабораторию ...")
            {
                Forms.FormAddNewLab fanl = new FormAddNewLab(1, 0, 0);
                fanl.ShowDialog();
                loadLab();
            }
            else
            {
                loadNormLimitList();    
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxLabName.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Выберите лабораторию");
            }
            else
            {
                if (this.checkedListBox1.SelectedIndex >= 0)
                {
                    int selectedListNorm = nList[this.checkedListBox1.SelectedIndex].NormID;
                    int nlId = NLList.IndexOf(NLList.Find(o => o.NormLimitId == selectedListNorm));
                    if (nlId >= 0)
                    {
                        //if (this.comboBox2.SelectedIndex != nlId)
                        //{
                        //    this.comboBox2.SelectedIndex = nlId;
                        gotNormLimit = new Processing.LabTickets.NormLimitClassFullList();
                        gotNormLimit.loadLabNormLimits(NLList[nlId].NormID);
                        if (gotNormLimit.Count > 0)
                        {
                            if (gotNormLimit[0].NormUnitId != 0)
                            {
                                this.comboBox1.SelectedIndex = MTypes.IndexOf(MTypes.Find(o => o.SampleType == gotNormLimit[0].NormUnitId));
                            }
                            else
                            {
                                this.comboBox1.SelectedIndex = -1;
                            }
                            this.dateTimePicker1.Value = gotNormLimit[0].NormFrom;
                            this.dateTimePicker2.Value = gotNormLimit[0].NormTo;

                            this.textBoxCode.Text = gotNormLimit[0].NormLimitCode;
                        }
                        //}
                    }
                }
            }

        }



        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.comboBox2.SelectedIndex >= 0)
            //{
            //    int listIx = NLList.IndexOf(NLList.Find(o => o.NormLimitId == nList[this.comboBox2.SelectedIndex].NormID));

            //    if (listIx >= 0)
            //    {
            //        if (this.checkedListBox1.SelectedIndex != listIx + 1)
            //        {
            //            this.checkedListBox1.SelectedIndex = listIx + 1;
            //        }
            //    }
            //    else
            //    { this.checkedListBox1.SelectedIndex = 0; }
            //}
            //else
            //{
            //    this.checkedListBox1.SelectedIndex = 0;
            //}
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.SelectedIndex >= 0)
            {
                Editor.FormChangeSampleType fcst = new Editor.FormChangeSampleType(lTypes[this.comboBoxTypes.SelectedIndex].SampleType, nList[this.checkedListBox1.SelectedIndex].NormID);
                fcst.ShowDialog();
            }
            else
            {
                Editor.FormChangeSampleType fcst = new Editor.FormChangeSampleType(0,0);
                fcst.ShowDialog();
            }
        }


        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.comboBox1.SelectedItem.ToString().Trim() == "Добавить новую единицу ...")
        //    {
        //        Forms.FormAddNewLab fanl = new FormAddNewLab(3,0);
        //        fanl.ShowDialog();
        //        loadSampleMeasure();
        //    }
        //}




    }
}
