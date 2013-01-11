using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;

namespace baza.Forms
{
    public partial class FormSetNewTicketRadio : Form
    {

        private Editor.ClassRadiologyItem.ResearchList rl;
        private Editor.ClassRadiologyItem.ZoneList zl;

        public FormSetNewTicketRadio(int _gotPidn)
        {
            InitializeComponent();
            if (_gotPidn > 0)
            {
                this.searchPatientBox1.searchById(_gotPidn);
            }
            fillTypeList();
        }

        private void fillTypeList()
        {
            rl = new Editor.ClassRadiologyItem.ResearchList();
            rl.GetResearchList();
            this.comboBox1.Items.Clear();

            foreach (Editor.ClassRadiologyItem.MagneticResearches i in rl)
            {
                this.comboBox1.Items.Add(i.ResearchName);
            }

            if (this.comboBox1.Items.Count > 0)
            {
                this.comboBox1.SelectedIndex = 0;
            }
        }


        private void fillTemplateList()
        {
            zl = new Editor.ClassRadiologyItem.ZoneList();
            this.checkedListBox1.Items.Clear();
            int _zType = rl[this.comboBox1.SelectedIndex].ResearchId;

          
                zl.GetZoneList(_zType, false);
            

            foreach (Editor.ClassRadiologyItem.ResearchZones i in zl)
            {

                this.checkedListBox1.Items.Add(i.ZoneName);

            }                      

        }



        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormSetNewTicketRadio.ActiveForm.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillTemplateList();
        }

        private void wrtiteData()
        {
            DataTable dwt = new DataTable();

            NpgsqlConnection conn = DBExchange.Inst.connectDb;

            dwt.Columns.Add("ticket_id", System.Type.GetType("System.Int64"));
            dwt.Columns.Add("template_id", System.Type.GetType("System.Int32"));
            

            int patid = this.searchPatientBox1.pIdN;
            int docin = DBExchange.Inst.dbUsrId;

            int serviceid = rl[this.comboBox1.SelectedIndex].ResearchId;

            string _command = "Insert into ticket_radio (pat_id, doc_in, type_id, date_app, status) values "
           + "('" + patid + "','" + docin + "','" + serviceid + "', '" + this.dateTimePicker1.Value.ToShortDateString() + "', 'false') RETURNING ticket_id ;";
            NpgsqlCommand writeTicket = new NpgsqlCommand(_command, conn);

            DataTable ds = new DataTable();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter("Select ticket_id, template_id from lab_results_radio where ticket_id = (Select max(ticket_id) from lab_results_radio);", conn);

            da.InsertCommand = new NpgsqlCommand("insert into lab_results_radio (ticket_id, template_id ) " +
                                    " values (:a, :b)", conn);


            da.InsertCommand.Parameters.Add(new NpgsqlParameter("a", NpgsqlDbType.Bigint));
            da.InsertCommand.Parameters.Add(new NpgsqlParameter("b", NpgsqlDbType.Integer));
           
            da.InsertCommand.Parameters[0].Direction = ParameterDirection.Input;
            da.InsertCommand.Parameters[1].Direction = ParameterDirection.Input;

            da.InsertCommand.Parameters[0].SourceColumn = "ticket_id";
            da.InsertCommand.Parameters[1].SourceColumn = "template_id";




            try
            {
                Int64 gotInt6 = (Int64)writeTicket.ExecuteScalar();

                foreach (int i in checkedListBox1.CheckedIndices)
                {
                    DataRow dwtNewRo = dwt.NewRow();
                    dwtNewRo["ticket_id"] = gotInt6;
                    dwtNewRo["template_id"] = zl[i].ZoneId;

                    dwt.Rows.Add(dwtNewRo);
                }


                da.Fill(ds);

                ds = dwt;
                DataTable ds2 = ds.GetChanges();

                da.Update(ds2);

                ds.Merge(ds2);
                ds.AcceptChanges();

                foreach (int i in checkedListBox1.CheckedIndices)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
                dwt.Clear();


            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            wrtiteData();
        }

    }
}
