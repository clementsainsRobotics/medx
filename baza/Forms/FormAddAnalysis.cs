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


    public partial class FormAddAnalysis : Form
    {
        private DataTable dtTemplNorms;
        private DataTable dtLabTemp;
        private DataTable dtPatSamples;
        private DataTable dtSampleTypes;
        public bool gotSampleTypes;
        private Document.SampleType.SList samTy;
        private DataTable dtLabs;
        private int _gotTemplateId;
        private int _gotLabId;
        private int _gotType;
        private Int64 _gotTicketId;
        private Int64 _gotSampleId;
        private int _gotPatId;
        private Processing.LabTickets.PatientTicketList ptList;
        private Processing.LabTickets.PatientAnalysisList paList;
             
        public FormAddAnalysis(Int64 _gotTicket, int _gotPIdN, int _gType)
        {
            gotSampleTypes = false;
            InitializeComponent();
            
            

            if (_gotPIdN != 0)
            {
                _gotPatId = _gotPIdN;
                this.searchPatientBox1.searchById(_gotPIdN);
                _gotTicketId = _gotTicket;
                _gotType = _gType;
            }
            loadSampleType();
        }

        private void comboBoxSampleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxSampleName.SelectedItem.ToString().Trim() == "Добавить новый образец ...")
            {
                Forms.FormLabDataSample flds = new FormLabDataSample(_gotPatId,_gotTicketId,false);
                flds.ShowDialog();
                if (this.searchPatientBox1.patientSet == true)
                {

                    Document.SampleType.SampleItem SI = samTy[comboBoxSampleType.SelectedIndex];
                    _gotType = SI.SampleType;
                    loadPatSamples(searchPatientBox1.pIdN, _gotType);
                }
            }
            else
            {
                if (this.comboBoxSampleName.SelectedIndex != -1)
                {
                    foreach (Processing.LabTickets.PatientAnalysis i in paList)
                    {
                        _gotSampleId = (Int64)dtPatSamples.Rows[this.comboBoxSampleName.SelectedIndex]["sample_id"];
                        i.ASample = _gotSampleId;
                    }
                    this.dataGridView1.Refresh();
                }
            }
        }

        private void fillPatientTickets()
        {

            this.comboBoxTicket.Items.Clear();
            this.comboBoxTicket.ResetText();
            this.comboBoxTicket.SelectedText = "";
            ptList = new baza.Processing.LabTickets.PatientTicketList();
            _gotPatId = this.searchPatientBox1.pIdN;
            if (_gotPatId > 0 )
            {
                int _gT = 0;
                int _pl = 0;
                int _pidn = _gotPatId;
                
                ptList.GetPatientTicketsNotFinished(_pidn);
                foreach (Processing.LabTickets.PatientTicket i in ptList)
                {
                    if (_gotTicketId > 0)
                    {
                        if (_gotTicketId == i.TicketId)
                        {
                            this.comboBoxTicket.Items.Add(i.TicketDateApp + " " + i.TicketName + " " + i.TicketDoctorFName);
                            this.comboBoxTicket.SelectedIndex = 0;
                            getTicketNorms(_gotTicketId);
                        }
                    }
                    else
                    {
                        this.comboBoxTicket.Items.Add(i.TicketDateApp + " " + i.TicketName + " " + i.TicketDoctorFName);
                    }

                }

                

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Выберите пациента");
            }


        }


        private void comboBoxLabName_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Добавить новую лабораторию ...")
            //{
            //    Forms.FormAddNewLab fanl = new FormAddNewLab(1, 0);
            //    fanl.ShowDialog();
            //    if (_gotType > 0)
            //    {
            //        loadLabsForSampleType(_gotType);
            //    }
            //}
            //else if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Без лаборатории")
            //{
               
            //    loadLabTemplates(0);
            //}
            //else if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Все показатели")
            //{
                
            //    loadLabTemplates(-1);
            //}
            //else
            //{
            //    _gotLabId = (int)dtLabs.Rows[this.comboBoxLabName.SelectedIndex]["code_id"];
             
            //    loadLabTemplates(_gotLabId);
            //}

            
        }


        private void loadLabTemplates(int _thisLab)
        {

            //dtLabTemp = new DataTable();

            //NpgsqlDataAdapter getLabs = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '5' and code ='" + _thisLab + "' ;", DBExchange.Inst.connectDb);
            //try
            //{
            //    getLabs.Fill(dtLabTemp);
            //    comboBoxTemplateName.Items.Clear();
            //    comboBoxTemplateName.Text = "";
            //    foreach (DataRow ro in dtLabTemp.Rows)
            //    {
            //        comboBoxTemplateName.Items.Add((string)ro["txt"]);
            //    }
            //    comboBoxTemplateName.Items.Add("Добавить новый шаблон ...");

            //}
            //catch (Exception exception)
            //{
            //     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            //}
            //this.comboBoxTemplateName.Refresh();


        }

        private void getTicketNorms(Int64 _ticket)
        {
            paList = new baza.Processing.LabTickets.PatientAnalysisList();
            
            paList.GetTicketAnalysisForFill(_ticket);
            //this.comboBoxTicket.Text = "";

           // dataGridView1 = new DataGridView();
            if (paList.Count > 0)
            {
                dataGridView1.Columns.Clear();
             dataGridView1.DataSource = paList;
             dataGridView1.Columns[0].ReadOnly = true;
             dataGridView1.Columns[2].ReadOnly = false;
             dataGridView1.Columns[3].ReadOnly = false;
             dataGridView1.Columns[4].ReadOnly = true;
             dataGridView1.Columns[1].ReadOnly = false;


             dataGridView1.Columns[2].Visible = false;
             dataGridView1.Columns[4].Visible = false;
             dataGridView1.Columns[5].Visible = false;
             dataGridView1.Columns[6].Visible = false;
             dataGridView1.Columns[7].Visible = false;

             if (this.dataGridView1.Columns.Count == 8)
             {
                 this.Column1.Items.Clear();
             
                 this.Column1.HeaderText = "Результат";
                 this.Column1.Items.AddRange(new object[] { "Не выбрано",
                                                            "Положительно",
                                                            "Отрицательно"});
                 this.Column1.Name = "bool_data";

                 this.Column1.ReadOnly = true;
                 this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.Column1 });
             }

                 dataGridView1.Columns["bool_data"].ReadOnly = false;
             
             dataGridView1.Columns[3].Resizable = DataGridViewTriState.True ;
             dataGridView1.Columns[0].FillWeight = 200;
             dataGridView1.Columns[3].FillWeight = 200;
             //int gridW = dataGridView1.Size.Width;
             //int colmnW = dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width + dataGridView1.Columns[2].Width;
             //dataGridView1.Columns[3].Width = gridW - colmnW - 5;

                foreach (Processing.LabTickets.PatientAnalysis i in paList)
                {

                    if (i.AValueBool == null)
                    {
         //            this.dataGridView1[8, paList.IndexOf(i)].Value = DataGridCell. "Не выбран" ;
        //                // this.dataGridView1.Column

        //                  ;
        //cell.Items.Clear();
        //cell.Items.Add(e.FormattedValue);
        //dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        //cell.Value = e.FormattedValue;

                        //DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dataGridView1.CurrentCell;
                       
                        
                        //ComboBox ctrl = (ComboBox)cell.EditType. Control;
                        //// Get Currently selected value...
                        //string curValue = String.Empty;
                        //if (cell.Value != null)
                        //    curValue = cell.Value.ToString();

                        ////bind data
                        //ctrl.DataSource = dataBaseData;
                        ////set selected value
                        //ctrl.SelectedItem = curValue;


                    }

                }
            }

        }


        private void getTemplateNorms(int _thisTemplate)
        {
            dtTemplNorms = new DataTable();
            _gotTemplateId = _thisTemplate;


            string _command = "Select test_name_id, test_name, unit_id from norm_limits JOIN lab_template ON "
            + " lab_template.norm_limits_test_id = norm_limits.test_name_id where template_codes_id = '" + _thisTemplate + "';";
            NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
            try
            {
                getNorm.Fill(dtTemplNorms);
                dtTemplNorms.Columns.Add("value", System.Type.GetType("System.String"));
                foreach (DataRow ro in dtTemplNorms.Rows)
                {
                    if (Convert.IsDBNull(ro["value"]) == true)
                    {
                        ro["value"] = "-9999";
                    }
                }
                this.dataGridView1.DataSource = dtTemplNorms;
            }
            catch (Exception exception)
            {
                
            }
            this.dataGridView1.Refresh();

        }

        private void comboBoxTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.comboBoxTemplateName.SelectedItem.ToString().Trim() == "Добавить новый шаблон ...")
            //{
            //    Forms.FormNewLabTemplate fnlt = new FormNewLabTemplate();
            //    fnlt.ShowDialog();
            //}
            //else
            //{
            //    int _gotT = (int)dtLabTemp.Rows[comboBoxTemplateName.SelectedIndex]["code_id"];
            //    getTemplateNorms(_gotT);
            //}
        }


        private void loadTemplateNorms()
        {

        }



        private void searchPatientBox1_gotPatientId(int _newPatientId)
        {
            DataTable dta = new DataTable();
            this.dataGridView1.DataSource = dta;
            fillPatientTickets();
            loadPatSamples(this.searchPatientBox1.pIdN, _gotType);
            _gotTicketId = 0;
        }

        private void loadLabsForSampleType(int _thisType)
        {



            //dtLabs = new DataTable();
            //NpgsqlDataAdapter getLabs = new NpgsqlDataAdapter("Select DISTINCT txt, code_id from codes JOIN norm_limits ON codes.code_id = norm_limits.lab_id "
            //    +" where norm_limits.type = '"+Convert.ToInt16(_thisType)+"' ;",DBExchange.Inst.connectDb);
            //try
            //{
            //    getLabs.Fill(dtLabs);
            //    comboBoxLabName.Items.Clear();
            //    foreach (DataRow ro in dtLabs.Rows)
            //    {
            //        comboBoxLabName.Items.Add((string)ro["txt"]);
            //    }
            //    comboBoxLabName.Items.Add("Все показатели");
            //    comboBoxLabName.Items.Add("Без лаборатории");
            //    comboBoxLabName.Items.Add("Добавить новую лабораторию ...");

            //}
            //catch (Exception exception)
            //{
            //     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            //}
            //this.comboBoxLabName.Refresh();

       

        }


        private void loadPatSamples(int _thisPat, int _type)
        {

            dtPatSamples = new DataTable();
            this.comboBoxSampleName.Text = "";
            bool _gotType;
            _gotType = false;
            string _command = "";
            string _where = "";
            int _days = -2;

            if (_type != -1)
            {
                _gotType = true;
                _where += " and sample_type = '" + Convert.ToInt16(_type) + "' ";

            }
            if (this.textBox1.Text.Trim().Length > 1)
            {
                _days = Convert.ToInt32(this.textBox1.Text.Trim());
                
            }
            else
            {

            }
            _where += " and sample_date between '" + dateTimePicker2.Value.AddDays(_days).ToShortDateString() + "' and '" + dateTimePicker2.Value.AddDays(1).ToShortDateString() + "'";
            _command = "Select sample_id, sample_name, sample_date, sample_type from bio_sample where pat_id = '"
                 + _thisPat + "' "+_where+" and (status = false or status is null);";

            NpgsqlDataAdapter getps = new NpgsqlDataAdapter(_command,DBExchange.Inst.connectDb);
            try
            {
                getps.Fill(dtPatSamples);
                comboBoxSampleName.Text = "";
                comboBoxSampleName.Items.Clear();
                foreach (DataRow ro in dtPatSamples.Rows)
                {
                
                     int st = Convert.ToInt32((Int16)ro["sample_type"]);
                     string Stype ="";
                     foreach (Document.SampleType.SampleItem i in samTy)
                     {
                         if (i.SampleType == st)
                         {
                             Stype = i.SampleName;
                             comboBoxSampleName.Items.Add(Stype + " " + (string)ro["sample_name"] + " " + ((DateTime)ro["sample_date"]).ToShortDateString());
                            
                         }
                     }
                    
                      //  string Stype = samTy. sampleName;
                        
                   
                }
                comboBoxSampleName.Items.Add("Добавить новый образец ...");

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormAddAnalysis.ActiveForm.Close();
        }

        private void comboBoxSampleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxSampleType.SelectedItem.ToString().Trim() == "Добавить новый тип ...")
            {

                Forms.FormAddNewLab fanl = new FormAddNewLab(2, 0, 0);
                fanl.ShowDialog();
                gotSampleTypes = false;
                loadSamples();
                loadSampleType();
            }
            else
            {
                if (this.searchPatientBox1.patientSet == true)
                {

                    loadSamples();
                    loadLabsForSampleType(_gotType);
                }
            }

        }

        private void loadSampleType()
        {
            if (gotSampleTypes == false)
            {
                try
                {
                    comboBoxSampleType.Items.Clear();
                    int _sT = 0;
                    int _pl = 0;
                    samTy = new baza.Document.SampleType.SList() ;
                    samTy.SampleListGet();
                    foreach (Document.SampleType.SampleItem i in samTy )
                        {
                            this.comboBoxSampleType.Items.Add(i.SampleName);
                            if (_gotType > 0)
                            {
                                if (i.SampleType == _gotType)
                                {
                                   _sT = _pl;
                                }
                                _pl++;
                            }
                        }
                    comboBoxSampleType.Items.Add("Добавить новый тип ...");
                    
                    this.comboBoxSampleType.SelectedIndex = _sT;
                    this.comboBoxSampleType.Refresh();
                    gotSampleTypes = true;
                               
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeDataToBase();
        }

        private void writeDataToBase()
        {
            
                if (this.comboBoxTicket.SelectedIndex != -1)
                {
                    NpgsqlConnection conn = DBExchange.Inst.connectDb;
                        int iPa = 0;
                    DataTable tNorma = new DataTable();
                    tNorma.Columns.Add("date", System.Type.GetType("System.DateTime"));
                    tNorma.Columns.Add("sample_id", System.Type.GetType("System.Int64"));
                    tNorma.Columns.Add("value", System.Type.GetType("System.Decimal"));
                    tNorma.Columns.Add("test_id", System.Type.GetType("System.Int64"));
                    tNorma.Columns.Add("test_positive", System.Type.GetType("System.Boolean"));
                    tNorma.Columns.Add("txt_data", System.Type.GetType("System.String"));


                    DataTable ds = new DataTable();

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter("Select test_id, sample_id, test_name_id, value, test_positive, txt_data, date from lab_results where ticket_id = '"+_gotTicketId+"' ;", conn);

                    da.InsertCommand = new NpgsqlCommand("insert into lab_results (test_name_id, sample_id, value, got_data ) values (:a, :b, :c, true)", conn);
                    da.UpdateCommand = new NpgsqlCommand("update lab_results set date = :a, sample_id = :b, value = :c, test_positive = :boval, txt_data = :txval, got_data = true where test_id = :d", conn);



                    da.InsertCommand.Parameters.Add(new NpgsqlParameter("a", NpgsqlDbType.Integer));
                    da.InsertCommand.Parameters.Add(new NpgsqlParameter("b", NpgsqlDbType.Bigint));
                    da.InsertCommand.Parameters.Add(new NpgsqlParameter("c", NpgsqlDbType.Numeric));

                    da.InsertCommand.Parameters[0].Direction = ParameterDirection.Input;
                    da.InsertCommand.Parameters[1].Direction = ParameterDirection.Input;
                    da.InsertCommand.Parameters[2].Direction = ParameterDirection.Input;

                    da.InsertCommand.Parameters[0].SourceColumn = "test_name_id";
                    da.InsertCommand.Parameters[1].SourceColumn = "sample_id";
                    da.InsertCommand.Parameters[2].SourceColumn = "value";

                    da.UpdateCommand.Parameters.Add(new NpgsqlParameter("a", NpgsqlDbType.Date));
                    da.UpdateCommand.Parameters.Add(new NpgsqlParameter("b", NpgsqlDbType.Bigint));
                    da.UpdateCommand.Parameters.Add(new NpgsqlParameter("c", NpgsqlDbType.Numeric));
                    da.UpdateCommand.Parameters.Add(new NpgsqlParameter("d", NpgsqlDbType.Bigint));
                    da.UpdateCommand.Parameters.Add(new NpgsqlParameter("boval", NpgsqlDbType.Boolean));
                    da.UpdateCommand.Parameters.Add(new NpgsqlParameter("txval", NpgsqlDbType.Text));

                    da.UpdateCommand.Parameters[0].Direction = ParameterDirection.Input;
                    da.UpdateCommand.Parameters[1].Direction = ParameterDirection.Input;
                    da.UpdateCommand.Parameters[2].Direction = ParameterDirection.Input;
                    da.UpdateCommand.Parameters[3].Direction = ParameterDirection.Input;
                    da.UpdateCommand.Parameters[4].Direction = ParameterDirection.Input;
                    da.UpdateCommand.Parameters[5].Direction = ParameterDirection.Input;


                    da.UpdateCommand.Parameters[0].SourceColumn = "date";
                    da.UpdateCommand.Parameters[1].SourceColumn = "sample_id";
                    da.UpdateCommand.Parameters[2].SourceColumn = "value";
                    da.UpdateCommand.Parameters[3].SourceColumn = "test_id";
                    da.UpdateCommand.Parameters[4].SourceColumn = "test_positive";
                    da.UpdateCommand.Parameters[5].SourceColumn = "txt_data";



                try
                {
                    da.Fill(ds);
                    foreach (Processing.LabTickets.PatientAnalysis ro in paList)
                    {
                        int roCount = paList.IndexOf(ro); 
                        if (ro.AValue != -9999 || ro.AValueText.Trim().Length > 0 || ro.AValueBool != null )
                        {


                            DataRow[] drm = ds.Select("test_id = "+ro.ATestId);
                            foreach (DataRow dri in drm)
                            {
                                dri["date"] = this.dateTimePicker1.Value;
                                if (this.comboBoxSampleName.SelectedIndex != -1)
                                {
                                    dri["sample_id"] = _gotSampleId;
                                }
                                else
                                {
                                    dri["sample_id"] = -1;
                                }

                                dri["value"] = ro.AValue;
                                if (ro.AValueBool == null)
                                {
                                    //dri["test_positive"] = "DBnull";
                                }
                                else
                                {
                                    dri["test_positive"] = ro.AValueBool;
                                }
                                dri["txt_data"] = ro.AValueText;



                            }
                            iPa++;
                        }
                    }


                   
                    DataTable ds2 = ds.GetChanges();

                    da.Update(ds2);

                    ds.Merge(ds2);
                    ds.AcceptChanges();
                    if (iPa == paList.Count )
                    {
                        NpgsqlCommand CloseTicket = new NpgsqlCommand("Update ticket set date_out = '"+DateTime.Now.ToShortDateString()+"', doc_out = '"
                            +DBExchange.Inst.dbUsrId+"' where ticket_id = '"+_gotTicketId+"' ;",DBExchange.Inst.connectDb);
                        CloseTicket.ExecuteNonQuery();
                    }

                    this.searchPatientBox1.searchById(_gotPatId);

                }
                catch (Exception exception)
                {
                   Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    
                }
                }
            else
            { System.Windows.Forms.MessageBox.Show("Выберите назначение"); }


        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (this.searchPatientBox1.patientSet == true)
            {
                loadSamples();
            }
        }

        private void loadSamples()
        {
            Document.SampleType.SampleItem SI = samTy[comboBoxSampleType.SelectedIndex];
            _gotType = SI.SampleType;
            loadPatSamples(searchPatientBox1.pIdN, _gotType);
        }


        private void comboBoxTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            _gotTicketId = ptList[comboBoxTicket.SelectedIndex].TicketId;
            getTicketNorms(_gotTicketId);      
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
           dataGridView1.EndEdit() ;
           if (e.ColumnIndex == 8 && e.RowIndex >=0 )
           {
               if (this.dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
               {
                   if (this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Trim() == "Положительно")
                   {
                       paList[e.RowIndex].AValueBool = true;
                   }
                   else if (this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Trim() == "Отрицательно")
                   {
                       paList[e.RowIndex].AValueBool = false;
                   }
                   else
                   {
                       paList[e.RowIndex].AValueBool = null;
                   }

               }         


           }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            loadSamples();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadSamples();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                // DataGridBoolColumn dgbc = new DataGridBoolColumn();
             //   this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = this.dataGridView1.Columns.Add( );

            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.EndEdit();
            if (e.ColumnIndex == 8 && e.RowIndex >= 0)
            {
                if (this.dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
                {
                    if (this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Trim() == "Положительно")
                    {
                        paList[e.RowIndex].AValueBool = true;
                    }
                    else if (this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Trim() == "Отрицательно")
                    {
                        paList[e.RowIndex].AValueBool = false;
                    }
                    else
                    {
                        paList[e.RowIndex].AValueBool = null;
                    }

                }


            }
        }


    }
}
