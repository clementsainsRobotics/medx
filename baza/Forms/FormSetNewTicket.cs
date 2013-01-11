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
    public partial class FormSetNewTicket : Form
    {

        private DataTable dtLabNorms;
        private DataTable dtWriteThis;
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

        private long gotTicket;


        public FormSetNewTicket(int _gotPidn)
        {
            
             gotSampleTypes = false;
            InitializeComponent();
           // this.dateTimePicker1.Value = DateTime.Now.AddDays(-14);
            loadSampleType();
            if (_gotPidn > 0)
            {
                this.searchPatientBox1.searchById(_gotPidn);
            }
            if (this.searchPatientBox1.patientSet == true)
            {

            }
            else
            {
                
            }
        }

                




        private void comboBoxLabName_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Добавить новую лабораторию ...")
            {
                Forms.FormAddNewLab fanl = new FormAddNewLab(1, 0, 0);
                fanl.ShowDialog();
                if (_gotType > 0)
                {
                    loadLabsForSampleType(_gotType);
                }
            }
            else if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Без лаборатории")
            {
               
                loadLabTemplates(0);
            }
            else if (this.comboBoxLabName.SelectedItem.ToString().Trim() == "Все показатели")
            {
                
                loadLabTemplates(-1);
            }
            else
            {
                _gotLabId = (int)dtLabs.Rows[this.comboBoxLabName.SelectedIndex]["code_id"];
             
                loadLabTemplates(_gotLabId);
            }

            
        }


        private void loadLabTemplates(int _thisLab)
        {

            dtLabTemp = new DataTable();

            NpgsqlDataAdapter getLabs = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '5' and code ='" + _thisLab + "' order by code_id ASC;", DBExchange.Inst.connectDb);
            try
            {
                getLabs.Fill(dtLabTemp);
                comboBoxTemplateName.Items.Clear();
                comboBoxTemplateName.Text = "";
                foreach (DataRow ro in dtLabTemp.Rows)
                {
                    comboBoxTemplateName.Items.Add((string)ro["txt"]);
                }
                comboBoxTemplateName.Items.Add("Добавить новый шаблон ...");

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                
            }
            this.comboBoxTemplateName.Refresh();


        }

        private void setTemplateNorms(int _thisTemplate)
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

                    ro["value"] = "-9999";

                }
               
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            

        }

        private void comboBoxTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxTemplateName.SelectedItem.ToString().Trim() == "Добавить новый шаблон ...")
            {
                Forms.FormNewLabTemplate fnlt = new FormNewLabTemplate(_gotLabId);
                fnlt.ShowDialog();
                loadLabTemplates(_gotLabId);
                if (dtLabTemp.Rows.Count > 0)
                {
                    this.comboBoxTemplateName.SelectedIndex = dtLabTemp.Rows.Count - 1;
                }
            }
            else
            {
                int _gotT = (int)dtLabTemp.Rows[comboBoxTemplateName.SelectedIndex]["code_id"];
                getTemplateNorms(_gotT);
            }
        }


        private void loadTemplateNorms()
        {

        }



        private void searchPatientBox1_gotPatientId(int _newPatientId)
        {

           
        }

        private void loadLabsForSampleType(int _thisType)
        {



            dtLabs = new DataTable();
            NpgsqlDataAdapter getLabs = new NpgsqlDataAdapter("Select DISTINCT txt, code_id from codes JOIN norm_limits ON codes.code_id = norm_limits.lab_id "
                +" where norm_limits.type = '"+Convert.ToInt16(_thisType)+"' ;",DBExchange.Inst.connectDb);
            try
            {
                getLabs.Fill(dtLabs);
                comboBoxLabName.Items.Clear();
                foreach (DataRow ro in dtLabs.Rows)
                {
                    comboBoxLabName.Items.Add((string)ro["txt"]);
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
                loadSampleType();

            }
            else if (this.searchPatientBox1.patientSet == true)
            {
                
                Document.SampleType.SampleItem SI = samTy[comboBoxSampleType.SelectedIndex];
                _gotType =  SI.SampleType ;
               
                loadLabsForSampleType(_gotType);
            }

        }

        private void loadSampleType()
        {
            if (gotSampleTypes == false)
            {
                try
                {
                     comboBoxSampleType.Items.Clear();

                    samTy = new baza.Document.SampleType.SList() ;
                    samTy.Clear();
                    samTy.SampleListGet();
                    foreach (Document.SampleType.SampleItem i in samTy )
                        {
                            this.comboBoxSampleType.Items.Add(i.SampleName);
                        }
                    comboBoxSampleType.Items.Add("Добавить новый тип ...");
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

        private void getTemplateNorms(int _thisTemplate)
        {
            dtTemplNorms = new DataTable();
            _gotTemplateId = _thisTemplate;
            checkedListBox1.Items.Clear();
  

            string _command = "Select norm_limits_test_id, template_codes_id, serial_key, test_name from lab_template JOIN norm_limits ON "
                +" lab_template.norm_limits_test_id = norm_limits.test_name_id where template_codes_id = '" + _thisTemplate + "';";
            NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
            try
            {
                getNorm.Fill(dtTemplNorms);
               
                foreach (DataRow ro in dtTemplNorms.Rows)
                {
                    this.checkedListBox1.Items.Add(ro["test_name"]);
                    this.checkedListBox1.SetItemChecked ( ro.Table.Rows.IndexOf(ro) , true) ;
                    
                }
                dtWriteThis = new DataTable();
                //dtWriteThis.Columns.Add("test_name_id", System.Type.GetType("System.Int32"));
                //dtWriteThis.Columns.Add("ticket_id", System.Type.GetType("System.Int64"));
                //dtWriteThis.Columns.Add("date", System.Type.GetType("System.DateTime"));

                NpgsqlDataAdapter da = new NpgsqlDataAdapter("Select test_name_id, ticket_id, date, test_id from lab_results where test_id = '2';", DBExchange.Inst.connectDb);
                da.Fill(dtWriteThis);

                foreach (int i in checkedListBox1.CheckedIndices )
                {
                    DataRow ro = dtWriteThis.NewRow();
                    ro["test_name_id"] = (int)dtTemplNorms.Rows[i]["norm_limits_test_id"];
                    ro["ticket_id"] = 0;
                    ro["date"] = this.dateTimePicker1.Value;
                    dtWriteThis.Rows.Add(ro);
                }
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name );
            }
            this.checkedListBox1.Refresh();

        }


        private void writeDataToBase()
        {

            NpgsqlConnection conn = DBExchange.Inst.connectDb;

            int patid = this.searchPatientBox1.pIdN;
            int docin = DBExchange.Inst.dbUsrId;
            int serviceid = _gotTemplateId;
            gotTicket = 0;

            string _command = "Insert into ticket (pat_id, doc_in, service_id, template, date_app, lab_id) values "
            +"('"+patid+"','"+docin+"','"+serviceid+"','true', '"+this.dateTimePicker1.Value.ToShortDateString()+"', '"+_gotLabId+"') RETURNING ticket_id ;";
            NpgsqlCommand writeTicket = new NpgsqlCommand(_command,conn);

            DataTable ds = new DataTable();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter("Select test_name_id, ticket_id, date, test_id from lab_results where test_id = '2';", conn);

            da.InsertCommand = new NpgsqlCommand("insert into lab_results (test_name_id, ticket_id, date ) " +
                                    " values (:a, :b, :c)", conn);
           

            da.InsertCommand.Parameters.Add(new NpgsqlParameter("a", NpgsqlDbType.Integer));

            da.InsertCommand.Parameters.Add(new NpgsqlParameter("b", NpgsqlDbType.Bigint));
            da.InsertCommand.Parameters.Add(new NpgsqlParameter("c", NpgsqlDbType.Date));






            da.InsertCommand.Parameters[0].Direction = ParameterDirection.Input;
            da.InsertCommand.Parameters[1].Direction = ParameterDirection.Input;
            da.InsertCommand.Parameters[2].Direction = ParameterDirection.Input;

            da.InsertCommand.Parameters[0].SourceColumn = "test_name_id";
            da.InsertCommand.Parameters[1].SourceColumn = "ticket_id";
            da.InsertCommand.Parameters[2].SourceColumn = "date";

            try
            {
               gotTicket = (long)writeTicket.ExecuteScalar();
              
               da.Fill(ds);
               foreach (DataRow ro in dtWriteThis.Rows)
               {
                   if (ro["test_id"] is DBNull)
                   {
                       ro["ticket_id"] = gotTicket;
                   }
                 //  ds.Rows.InsertAt(ro, ds.Rows.Count + 1);
               }
               ds = dtWriteThis;
               DataTable ds2 = ds.GetChanges();
               
               da.Update(ds2);

               ds.Merge(ds2);
               ds.AcceptChanges();

               checkedListBox1.Items.Clear();
               foreach (DataRow ro in dtTemplNorms.Rows)
               {
                   this.checkedListBox1.Items.Add(ro["test_name"]);
               }
               dtWriteThis.Clear();

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }


        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            writeDataToBase();
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            FormSetNewTicket.ActiveForm.Close();
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
        private void gotItemChecked()
        {
            if (this.comboBoxTemplateName.SelectedIndex != -1 && this.checkedListBox1.SelectedIndex != -1)
            {
                DataRow ro = dtWriteThis.NewRow();
                ro["test_name_id"] = (int)dtTemplNorms.Rows[this.checkedListBox1.SelectedIndex]["norm_limits_test_id"];
                ro["ticket_id"] = 0;
                ro["date"] = this.dateTimePicker1.Value;
                dtWriteThis.Rows.Add(ro);
            }
            else if (this.comboBoxTemplateName.SelectedIndex == -1)
            { System.Windows.Forms.MessageBox.Show("Выберите шаблон из списка"); }
        }

        private void gotItemUnchecked()
        {
            try
            {
                int _thisItem = (int)dtTemplNorms.Rows[this.checkedListBox1.SelectedIndex]["norm_limits_test_id"];

                DataRow[] ro = dtWriteThis.Select("test_name_id = '" + _thisItem + "'");
                foreach (DataRow i in ro)
                {
                    i.Delete();
                }

                //  this.checkedListBox1.SetItemChecked(this.checkedListBox1.SelectedIndex, false);
            }
            catch (Exception exception)
            {
               Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            writeDataToBase();
            
            Forms.FormAddAnalysis fa = new FormAddAnalysis(gotTicket,this.searchPatientBox1.pIdN,_gotType);
                   
            fa.ShowDialog();
            FormSetNewTicket.ActiveForm.Close();   
            
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            updateDateInTable();
        }

        private void updateDateInTable()
        {

            foreach (DataRow row0 in dtWriteThis.Rows)
            {

                row0["date"] = this.dateTimePicker1.Value;
            }

        }

    }
    
}
