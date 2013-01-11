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
    public partial class FormLabDataSample : Form
    {
        private DataTable dtSampleType;
        private int _gotPatient;
        private Int64 _gotTicket;
        private bool _labImport;

        public FormLabDataSample(int _PatientId, Int64 _TicketId, bool _gotLabImport)
        {
            InitializeComponent();
            _gotPatient = 0;
            if (_PatientId > 0)
            {
                _gotPatient = _PatientId;
                this.searchPatientBox1.searchById(_gotPatient);
            }
            _gotTicket = 0;
            if (_TicketId > 0)
            {
                _gotTicket = _TicketId;
            }
            _labImport = _gotLabImport;
            getSampleTypes();
        }

        private void getSampleTypes()
        {
            dtSampleType = new DataTable();
            NpgsqlDataAdapter getMes = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '3' ;", DBExchange.Inst.connectDb);
            try
            {
                getMes.Fill(dtSampleType);
                comboBox1.Items.Clear();
                foreach (DataRow ro in dtSampleType.Rows)
                {
                    comboBox1.Items.Add((string)ro["txt"]);
                }
                comboBox1.Items.Add("Добавить новый тип ...");

            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            this.comboBox1.Refresh();
        }

        private void writeNewType()
        {
            if (this.comboBox1.SelectedItem.ToString().Trim() == "Добавить новый тип ...")
            {
                Forms.FormAddNewLab fanl = new FormAddNewLab(2, 0, 0);
                fanl.ShowDialog();
                getSampleTypes();
            }
            else
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            writeNewType();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormLabDataSample.ActiveForm.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            WriteAnalysisData();
          
        }

        private void WriteAnalysisData()
        {
            if (this.textBox1.Text.Trim().Length > 0)
            {
                DateTime Sdate = this.dateTimePicker1.Value;
                string Sname = this.textBox1.Text.Trim();
                int pidn = this.searchPatientBox1.pIdN;
                if (this.comboBox1.SelectedIndex != -1)
                {
                    Int16 Stype = Convert.ToInt16((int)dtSampleType.Rows[this.comboBox1.SelectedIndex]["code_id"]);


                    string _to = "sample_date, sample_name, sample_type, pat_id";
                    string _val = " '" + Sdate + "','" + Sname + "','" + Stype + "','" + pidn + "'";

                    string _writeSample = "";
                    if (this.richTextBox1.Text.Trim().Length > 0)
                    {
                        string Scomment = this.richTextBox1.Text.Trim();

                        _to += " , comment";
                        _val += ",'" + Scomment + "' ";

                    }

                    if (_labImport == true)
                    {
                        _to += ", lab_import";
                        _val += ", '" + _labImport.ToString() + "' ";
                    }

                    string _command = "insert into bio_sample (" + _to + ") VALUES (" + _val + ") RETURNING sample_id;";
                    NpgsqlCommand wSample = new NpgsqlCommand(_command, DBExchange.Inst.connectDb);

                    try
                    {
                        Int64 _gotSample = (Int64)wSample.ExecuteScalar();
                        if (_gotTicket > 0)
                        {
                            _writeSample = "update lab_results set sample_id = '" + _gotSample + "' where ticket_id = '" + _gotTicket + "' ;";
                            NpgsqlCommand uSample = new NpgsqlCommand(_writeSample, DBExchange.Inst.connectDb);
                            uSample.ExecuteNonQuery();
                            string _writeTicket = "update ticket set doc_out = '" + DBExchange.Inst.dbUsrId + "', date_out = '" + Sdate + "' where ticket_id = '" + _gotTicket + "' ; ";
                            NpgsqlCommand uTicket = new NpgsqlCommand(_writeTicket, DBExchange.Inst.connectDb);
                            uTicket.ExecuteNonQuery();
                        }

                        this.richTextBox1.Clear();
                        this.textBox1.Clear();
                        _labImport = false;
                        _gotTicket = 0;
                    }
                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog(); log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    }

                }


                else
                {
                    System.Windows.Forms.MessageBox.Show("Выберите тип образца");
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Введите название образца");
            }


        }


    }
}
