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

namespace baza.Screen
{
    public partial class FormLoadXmlForLab : Form
    {
        private Document.SampleType.LabList lab;

        public FormLoadXmlForLab()
        {
            InitializeComponent();
            loadLab();
        }

        private void loadLab()
        {
            lab = new Document.SampleType.LabList();
            lab.LabListGet();
            foreach (Document.SampleType.LabItem i in lab)
            {
                this.comboBox1.Items.Add(i.SampleName);

            }
            if (this.comboBox1.Items.Count > 0)
            {
                this.comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void openFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.DefaultExt = "xml";
            openFileDialog1.Title = "Выберите файл для загрузки";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Screen.FormLoadXmlForLab.ActiveForm.Close();
        }

        private void writeData()
        {

            DataSet gotData = new DataSet();
            if (this.textBox1.Text.Trim().Length > 2)
            {
                gotData.ReadXml(textBox1.Text.Trim());
                System.Windows.Forms.MessageBox.Show("Загружено " + gotData.Tables[0].Rows.Count.ToString() + " образцов");

                NpgsqlConnection conn = DBExchange.Inst.connectDb;



                DataTable ds = new DataTable();

                // NpgsqlDataAdapter da = new NpgsqlDataAdapter("Select lr.test_id, lr.value, bs.sample_id as sample_id,  lr.ticket_id, trim(nl.test_code) as test_code, " 
                //    +"trim(bs.sample_name) as sample_comment from lab_results lr, norm_limits nl, bio_sample bs, ticket tt where lr.date = '"
                //    + this.dateTimePicker1.Value.ToShortDateString() + "' and lr.import_value = false and lr.test_name_id = nl.test_name_id and nl.lab_id = '" 
                //    + lab[this.comboBox1.SelectedIndex].SampleType + "' and lr.sample_id = bs.sample_id and bs.lab_import = true ;", conn);

                //da.InsertCommand = new NpgsqlCommand("insert into lab_results ( sample_id, value, ticket_id, got_data ) values ( :b, :c, :d, true)", conn);
                //da.UpdateCommand = new NpgsqlCommand("update lab_results set value = :c, import_value=true, date='"+DateTime.Now.ToShortDateString()+"', got_data = true where test_id = :a ", conn);



                NpgsqlDataAdapter da = new NpgsqlDataAdapter("Select lr.test_id, lr.value, tt.lab_sample_number as sample_id, lr.ticket_id, trim(nl.test_code) as test_code " 
                    +" from lab_results lr, norm_limits nl, ticket tt where tt.date_app = '"
                    + this.dateTimePicker1.Value.ToShortDateString() + "' and "
               + "lr.import_value = false and lr.test_name_id = nl.test_name_id and nl.lab_id = '" 
                    + lab[this.comboBox1.SelectedIndex].SampleType + "' and tt.ticket_id = lr.ticket_id ;", conn);

                da.InsertCommand = new NpgsqlCommand("insert into lab_results ( sample_id, value, ticket_id, got_data ) values ( :b, :c, :d, true)", conn);
                da.UpdateCommand = new NpgsqlCommand("update lab_results set sample_id = :b, value = :c, import_value=true, date = '"+ dateTimePicker1.Value.ToShortDateString()+"', got_data = true where test_id = :a ", conn);

                da.InsertCommand.Parameters.Add(new NpgsqlParameter("a", NpgsqlDbType.Bigint));
                da.InsertCommand.Parameters.Add(new NpgsqlParameter("b", NpgsqlDbType.Bigint));
                da.InsertCommand.Parameters.Add(new NpgsqlParameter("c", NpgsqlDbType.Numeric));
                da.InsertCommand.Parameters.Add(new NpgsqlParameter("d", NpgsqlDbType.Bigint));

                da.InsertCommand.Parameters[0].Direction = ParameterDirection.Input;
                da.InsertCommand.Parameters[1].Direction = ParameterDirection.Input;
                da.InsertCommand.Parameters[2].Direction = ParameterDirection.Input;
                da.InsertCommand.Parameters[3].Direction = ParameterDirection.Input;

                da.InsertCommand.Parameters[0].SourceColumn = "test_id";
                da.InsertCommand.Parameters[1].SourceColumn = "sample_id";
                da.InsertCommand.Parameters[2].SourceColumn = "value";
                da.InsertCommand.Parameters[3].SourceColumn = "ticket_id";

                da.UpdateCommand.Parameters.Add(new NpgsqlParameter("a", NpgsqlDbType.Bigint));
                da.UpdateCommand.Parameters.Add(new NpgsqlParameter("b", NpgsqlDbType.Bigint));
                da.UpdateCommand.Parameters.Add(new NpgsqlParameter("c", NpgsqlDbType.Numeric));
                da.UpdateCommand.Parameters.Add(new NpgsqlParameter("d", NpgsqlDbType.Bigint));

                da.UpdateCommand.Parameters[0].Direction = ParameterDirection.Input;
                da.UpdateCommand.Parameters[1].Direction = ParameterDirection.Input;
                da.UpdateCommand.Parameters[2].Direction = ParameterDirection.Input;
                da.UpdateCommand.Parameters[3].Direction = ParameterDirection.Input;

                da.UpdateCommand.Parameters[0].SourceColumn = "test_id";
                da.UpdateCommand.Parameters[1].SourceColumn = "sample_id";
                da.UpdateCommand.Parameters[2].SourceColumn = "value";
                da.UpdateCommand.Parameters[3].SourceColumn = "ticket_id";

                try
                {
                    da.Fill(ds);
                    int writtenNorm = 0;

                    if (ds.Rows.Count > 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Найдено " + ds.Rows.Count.ToString() + " показателей");
                        foreach (DataRow dr in ds.Rows)
                        {
                            Int64 _Sample = (long)dr["sample_id"];
                           

                            if (_Sample != -999)
                            {

                                DataRow[] ro = gotData.Tables["SAMPLE"].Select("SampleID = " + _Sample);
                                //Получен массив из хмл файла
                                foreach (DataRow gdr in ro)
                                {

                                    //DataRow gdr = ro[1];
                                    int sample_key = (int)gdr["SAMPLE_ID"];


                                    if (Convert.IsDBNull(dr["test_code"]) == false)
                                    {
                                        string tCode = (string)dr["test_code"];
                                        DataRow[] ResultR = gotData.Tables["RESULTS"].Select("SPECIMENS_Id =" + sample_key + " and TestID LIKE '" + tCode + "'");

                                        foreach (DataRow r1 in ResultR)
                                        {
                                            dr["value"] = Convert.ToDecimal(((string)r1["Result"]).Trim().Replace('.', ','));
                                            writtenNorm++;
                                        }
                                    }

                                }
                            }


                        }



                        //ds = dtTemplNorms;
                        if (writtenNorm > 0)
                        {

                            DataTable ds2 = ds.GetChanges();

                            da.Update(ds2);

                            ds.Merge(ds2);
                            ds.AcceptChanges();
                            System.Windows.Forms.MessageBox.Show("Записано " + writtenNorm.ToString() + " показателей");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Не вписаны номера образцов в базе");
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("1 Не найдено назначений или \n2 проверьте лабораторию и выбранную дату");
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    
                    
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Выберите файл для загрузки");
            }
    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            writeData();
        }



    }
}
