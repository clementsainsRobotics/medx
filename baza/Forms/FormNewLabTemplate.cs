using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using System.Reflection;

namespace baza.Forms
{
    public partial class FormNewLabTemplate : Form
    {
        private Document.SampleType.LabList labList;
        private DataTable dtLabTemp;
        private DataTable dtLabNorms;
        private DataTable dtLabs;
        private bool gotNorms;
        private DataTable dtTemplNorms;
        private int _gotLabId;
        private int _gotTemplateId; 
        private DataTable writeTheseNorms;
        private Document.SampleType.SList lTypes;

        public FormNewLabTemplate(int _gotLab)
        {
            InitializeComponent();
            gotNorms = false;
            
            loadLab();

            if (_gotLab > 0)
            {
                loadLabTemplates(_gotLab);
                loadLabNormLimits(_gotLab);
                int labI = labList.IndexOf( labList.Find(o => o.SampleType == _gotLab ));
                this.comboBoxLabName.SelectedIndex = labI;
            }
            else
            {
                loadLabTemplates(0);
                loadLabNormLimits(-1);
            }
            loadSampleType();
            
        }
        private void loadSampleType()
        {
            lTypes = new baza.Document.SampleType.SList();
            lTypes.SampleListGet();

            comboBoxTypes.Items.Clear();
            foreach (Document.SampleType.SampleItem i in lTypes)
            {
                comboBoxTypes.Items.Add(i.SampleName);
            }
            


            this.comboBoxTypes.Refresh();


        }

        private void loadLabTemplates(int _thisLab)
        {

            dtLabTemp = new DataTable();
           
            NpgsqlDataAdapter getLabs = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '5' and code ='"+_thisLab+"' ;", DBExchange.Inst.connectDb);
            try
            {
                getLabs.Fill(dtLabTemp);
                comboBox1.Items.Clear();
                comboBox1.Text = "";
                foreach (DataRow ro in dtLabTemp.Rows)
                {
                    comboBox1.Items.Add((string)ro["txt"]);
                }
                comboBox1.Items.Add("Добавить новый шаблон ...");

            }
            catch (Exception exception)
            {
                  Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            this.comboBox1.Refresh();


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int labID = -2;
            short _analysisType = 0;
            if (this.comboBox1.SelectedItem.ToString().Trim() == "Добавить новый шаблон ...")
            {
                Forms.FormAddNewLab fanl = new FormAddNewLab(5, 0, 0);

                if (this.comboBoxLabName.SelectedIndex >= 0)
                {
                    if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Все показатели")
                {
                    labID = -1;  
                }
                else if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Без лаборатории" )
                {
                    labID = 0;
                }
                        
                    else  if (this.comboBoxLabName.SelectedItem.ToString().Trim() != "Без лаборатории" || this.comboBoxLabName.SelectedItem.ToString().Trim() != "Все показатели")
                    {
                        labID = labList[this.comboBoxLabName.SelectedIndex].SampleType;                       
                    }
                }

           
                
                else
                {
                    labID = 0;
                }


                fanl = new FormAddNewLab(5, labID, _analysisType);
                fanl.ShowDialog();
                loadLabTemplates(labID);
            }
            else
            {
                if (this.comboBox1.SelectedIndex >= 0 && this.comboBox1.SelectedItem.ToString().Trim() != "Добавить новый шаблон ...")
                {

                    getTemplateNorms((int)dtLabTemp.Rows[this.comboBox1.SelectedIndex]["code_id"]);

                }
            }
        }


        private void loadLabNormLimits(int _labID)
        {
            dtLabNorms = new DataTable();
           
            string _command = "Select test_name_id, test_name from norm_limits where lab_id is null ;";
            string _gType = "";
            if (this.comboBoxTypes.SelectedIndex >= 0)
            {
                int _gotType = lTypes[this.comboBoxTypes.SelectedIndex].SampleType;
                _gType = " and type = '"+_gotType+"'";
            }
            if (_labID == -2)
            {
                _command = "Select test_name_id, test_name from norm_limits ;";
            }
            
            else
                if (_labID > 0)
                {
                   _command = "Select test_name_id, test_name from norm_limits where lab_id = '" + _labID + "' "+_gType+" ;";
                }
            
            

            NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
            try
            {
                getNorm.Fill(dtLabNorms);
                checkedListBox1.Items.Clear();
                foreach (DataRow ro in dtLabNorms.Rows)
                {
                    checkedListBox1.Items.Add((string)ro["test_name"]);
                }
                

            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            this.checkedListBox1.Refresh();


        }

        private void loadLab()
        {

            labList = new Document.SampleType.LabList();
            try
            {
                labList.LabListGet();
                comboBoxLabName.Items.Clear();
                foreach (Document.SampleType.LabItem i in labList )
                {
                    comboBoxLabName.Items.Add(i.SampleName);
                }
                comboBoxLabName.Items.Add("Все показатели");
                comboBoxLabName.Items.Add("Без лаборатории");
                comboBoxLabName.Items.Add("Добавить новую лабораторию ...");

            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            this.comboBoxLabName.Refresh();

        }

        private void comboBoxLabName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Добавить новую лабораторию ...")
            {
                Forms.FormAddNewLab fanl = new FormAddNewLab(1,0,0);
                fanl.ShowDialog();
                loadLab();
            }
            else if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Без лаборатории")
            {
                loadLabNormLimits(-1);
                loadLabTemplates(0);
            }
            else if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Все показатели")
            {
                loadLabNormLimits(-2);
                loadLabTemplates(-1);
            }
            else
            {
                _gotLabId = labList[this.comboBoxLabName.SelectedIndex].SampleType;
                loadLabNormLimits(_gotLabId);
                loadLabTemplates(_gotLabId);
            }

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormNewLabTemplate.ActiveForm.Close();
        }


        private void writeTemplate()
        {
            try
            {
                if (this.comboBox1.SelectedIndex >= 0)
                {

                    NpgsqlConnection conn = DBExchange.Inst.connectDb;

                    DataTable ds = new DataTable();

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter("Select norm_limits_test_id, template_codes_id, serial_key from lab_template where template_codes_id = '" + _gotTemplateId + "';", conn);

                    da.InsertCommand = new NpgsqlCommand("insert into lab_template (norm_limits_test_id, template_codes_id) values (:a, :b)", conn);
                    da.DeleteCommand = new NpgsqlCommand("Delete from lab_template where serial_key = :a ", conn);

                    da.DeleteCommand.Parameters.Add(new NpgsqlParameter("a", NpgsqlDbType.Integer));
                    da.DeleteCommand.Parameters[0].SourceVersion = DataRowVersion.Original;
                    da.DeleteCommand.Parameters[0].SourceColumn = "serial_key";


                    da.InsertCommand.Parameters.Add(new NpgsqlParameter("a", NpgsqlDbType.Integer));
                    da.InsertCommand.Parameters.Add(new NpgsqlParameter("b", NpgsqlDbType.Integer));

                    da.InsertCommand.Parameters[0].Direction = ParameterDirection.Input;
                    da.InsertCommand.Parameters[1].Direction = ParameterDirection.Input;

                    da.InsertCommand.Parameters[0].SourceColumn = "norm_limits_test_id";
                    da.InsertCommand.Parameters[1].SourceColumn = "template_codes_id";

                    da.Fill(ds);
                    ds = dtTemplNorms;

                    DataTable ds2 = ds.GetChanges();

                    da.Update(ds2);

                    ds.Merge(ds2);
                    ds.AcceptChanges();

                    getTemplateNorms(_gotTemplateId);
                }
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                

            }




        }

        //При изменении индекса шаблона, загружать в список норм отмеченные и делать апдейт в базе всех измененных записей
        //записывать сразу весь список с проверкой на уже существующие записи
        //Выбор шаблонов для лаборатории при смене лаборатории и записи шаблона, при выборе шаблона грузится таблица с его нормами и нормы отмечаются 
        //в списке, если загружается полный список норм, то остальные записи нужно сортировать в памяти а не из базы

        private void gotItemChecked()
        {
            if (this.comboBox1.SelectedIndex != -1 && this.checkedListBox1.SelectedIndex != -1)
            {
                DataRow ro = dtTemplNorms.NewRow();
                ro["template_codes_id"] = _gotTemplateId;
                ro["norm_limits_test_id"] = (int)dtLabNorms.Rows[this.checkedListBox1.SelectedIndex]["test_name_id"];
                dtTemplNorms.Rows.Add(ro);
            }
            else if (this.comboBox1.SelectedIndex == -1 )
            { System.Windows.Forms.MessageBox.Show("Выберите шаблон из списка"); }
        }

        private void gotItemUnchecked()
        {
            try
            {
                if (this.checkedListBox1.SelectedIndex != -1 && dtTemplNorms.Rows.Count >0)
                {
                    
                int _thisItem = (int)dtLabNorms.Rows[this.checkedListBox1.SelectedIndex]["test_name_id"];


                    DataRow[] ro = dtTemplNorms.Select("norm_limits_test_id = '" + _thisItem + "'");
                    foreach (DataRow i in ro)
                    {
                        i.Delete();
                    }
                }

              //  this.checkedListBox1.SetItemChecked(this.checkedListBox1.SelectedIndex, false);
            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeTemplate();
        }

        private void getTemplateNorms(int _thisTemplate)
        {
           
            _gotTemplateId = _thisTemplate;
            //foreach (int i in checkedListBox1.CheckedIndices)
            //{
            //    checkedListBox1.SetItemChecked(i, false);
            //}
            checkedListBox1.Items.Clear();
            foreach (DataRow ro in dtLabNorms.Rows)
            {
                checkedListBox1.Items.Add((string)ro["test_name"]);
            }


            string _command = "Select norm_limits_test_id, template_codes_id, serial_key from lab_template where template_codes_id = '" + _thisTemplate + "';";
            NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
            try
            {
                dtTemplNorms = new DataTable();
                getNorm.Fill(dtTemplNorms);
                if (dtTemplNorms.Rows.Count > 0)
                {
                    foreach (DataRow ro in dtTemplNorms.Rows)
                    {
                        DataRow[] ros = dtLabNorms.Select("test_name_id = " + ro[0]);

                        foreach (DataRow i in ros)

                        {
                            int _getNormRow = dtLabNorms.Rows.IndexOf(i);
                            this.checkedListBox1.SetItemChecked(_getNormRow, true);
                        }

                    }

                }
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();   
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                
            }
            this.checkedListBox1.Refresh();

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
            this.checkedListBox1.Refresh();
        }

        private void comboBoxTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxLabName.SelectedIndex >= 0)
            {
                _gotLabId = labList[this.comboBoxLabName.SelectedIndex].SampleType;
                if (this.comboBoxTypes.SelectedIndex >= 0)
                {
                    loadLabNormLimits(_gotLabId);
                }
            }
        }


    }
}
