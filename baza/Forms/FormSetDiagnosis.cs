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
    public partial class FormSetDiagnosis : Form
    {
        tabDiagEnv _tabDiagEnv = new tabDiagEnv();
        private DataTable tlDiagData;
        private bool MkbDataSet = DBExchange.Inst.MkbDataSet;
        private bool EditFlag;
        private Diag.Diagnosis gotDiagnosis;
        private int diagId;

        public FormSetDiagnosis(int patID, bool _editFlag, int _thisDiag)
        {      
           
            InitializeComponent();
            if (patID != -1)
            {
                this.searchPatientBox1.searchById(patID);
            }
            if (this.MkbDataSet == false)
            {
                fillDiagMkb();
            }
            if (_editFlag == true)
            {
                getThisDiagData(_thisDiag);
                diagId = _thisDiag;
            }

            EditFlag = _editFlag;
        }


        private void getThisDiagData(int _thisDiag)
        {
            gotDiagnosis = new Diag.Diagnosis();
            gotDiagnosis = gotDiagnosis.getDiag(_thisDiag);

           
                comboBox8.SelectedIndex = gotDiagnosis.DiagType;           
                this.dateTimePicker1.Value = gotDiagnosis.DiagDate;           
                checkBox1.Checked = gotDiagnosis.DiagMain;
                this.textBox1.Text = gotDiagnosis.DiagBody;
                this.comboBoxDiagDescr.DataSource = _tabDiagEnv.getDiagById(gotDiagnosis.DiagMkbId); 
            
     
           

        }

        ///Проверка существует ли диагноз в базе
        ///
        private bool checkPatDiag()
        {
            
                int gotDiag = -1;
                bool isDiag = true;
                NpgsqlCommand getDiag = new NpgsqlCommand("select min(serial) from diag_data  "
                + "where pat_id = '" + this.searchPatientBox1.pIdN + "' and  diag = '" + _tabDiagEnv.diagNumList[comboBoxDiagDescr.SelectedIndex] + "' and type = '" 
                + this.comboBox8.SelectedIndex + "' and main_mark = " + checkBox1.Checked.ToString().ToLower() + " and delete = false ;", DBExchange.Inst.connectDb);

                try
                {
                   var getDiagnosis = getDiag.ExecuteScalar();
                   if (Convert.IsDBNull( getDiagnosis) == false )
                   {
                       gotDiag = Convert.ToInt32(getDiagnosis);
                   }
                    if (gotDiag > 0) {isDiag = true;}
                    else
                        isDiag = false;

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
                return isDiag;
             

            
        }


        /// <summary>
        // Запись диагноза пациента в базу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void diagDataAddToBase()
        {
            if (this.searchPatientBox1.pIdN >= 0 && comboBoxDiagDescr.SelectedIndex >= 0)
            {
                if (checkPatDiag() == false)
                    {
                        string insCom;
                        string txtCom;
                        if (this.textBox1.Text.Trim().Length > 1)
                        {
                            insCom = ", comment";
                            txtCom = ", '" + this.textBox1.Text.Trim() + "'";
                        }
                        else
                        {
                            insCom = "";
                            txtCom = "";
                        }
                        NpgsqlCommand insPatDiag = new NpgsqlCommand("insert into diag_data (usr_id, pat_id, diag_date, diag, type, main_mark " + insCom + ") "
                        + "values ('" + DBExchange.Inst.dbUsrId + "','" + this.searchPatientBox1.pIdN + "', '" + this.dateTimePicker1.Value.Date + "','" + _tabDiagEnv.diagNumList[comboBoxDiagDescr.SelectedIndex] + "',"
                        + " '" + this.comboBox8.SelectedIndex + "', '" + checkBox1.Checked.ToString().ToLower() + "' " + txtCom + ");", DBExchange.Inst.connectDb);
                        try
                        {
                            insPatDiag.ExecuteNonQuery();
                            System.Windows.Forms.MessageBox.Show("Диагноз добавлен", "Отлично!");
                        }
                        catch (Exception exception)
                        {
                             Warnings.WarnLog log = new Warnings.WarnLog();    
                            log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                        }
                       
                    }
                    else
                    { 
                         System.Windows.Forms.MessageBox.Show("Диагноз для пациента уже существует.", "Ошибка!!! Такой диагноз установлен"); 
                    }

                }
               
            else

           {
                Warnings.WarnMessages ChoPa = new Warnings.WarnMessages();
                ChoPa.warnChoosePatient();
           }
            
        }

        private void comboBoxDiagGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectDiagSubGroup();
        }

        //Заполняет диагнозы МКБ
        private void fillDiagMkb()
        {
            comboBoxDiagGroup.DataSource = _tabDiagEnv.getDiagGroupFromBase();
            this.comboBoxDiagGroup.SelectedIndex = 1;
            this.comboBoxDiagSubGroup.SelectedIndex = 6;
            this.comboBox8.DataSource = _tabDiagEnv.fillDiagTypeList();
            MkbDataSet = true;
        }

        /// <summary>
        /// Заполняет подгруппу диагнозов
        /// </summary>
        private void selectDiagSubGroup()
        {
            this.comboBoxDiagSubGroup.DataSource = _tabDiagEnv.getDiagSubGroupFromBase(Convert.ToInt16(comboBoxDiagGroup.SelectedIndex));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (EditFlag == false)
            {
                diagDataAddToBase();
            }
            else
            {
                deleteDiag(diagId);
                diagDataAddToBase();
                diagId = 0;
                EditFlag = false;
            }

        }
        /// <summary>
        // получает список диагнозов, указанных в группе
        /// </summary>

        private void selectDiagData()
        {
            this.comboBoxDiagDescr.DataSource = _tabDiagEnv.getMkbDiagFromBase(_tabDiagEnv.diagSubGroupNumList[comboBoxDiagSubGroup.SelectedIndex]);

        }


        private void comboBoxDiagSubGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectDiagData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           Forms.FormSetDiagnosis.ActiveForm.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (EditFlag == true)
            {
                deleteDiag(diagId);
                this.textBox1.Text = "";
                System.Windows.Forms.MessageBox.Show("Диагноз удалён", "Успешно!");

                diagId = 0;
                EditFlag = false;
            }
        }

        private void deleteDiag(int _thisDiag)
        {
            NpgsqlCommand delDi = new NpgsqlCommand("Update diag_data set delete = true where serial = '"+_thisDiag+"' ;",DBExchange.Inst.connectDb);
            try
            {
                delDi.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }
        }

    }
}
