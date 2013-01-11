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
    public partial class FormTicket : Form
    {
        
        public FormTicket(int patID)
        {
            InitializeComponent();
            loadOtdelData();
            if (patID != -1)
            {
                this.searchPatientBox1.searchById(patID);
            }
        }

        private void loadOtdelServiceDataByGroup()
        {
            this.comboBox9.Items.Clear();
            NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select trim(code), trim(name), reestr from reestr where substr(code,1,3) like '" + this.comboBox11.SelectedItem + "' order by code ASC", DBExchange.Inst.connectDb);
             DBExchange.Inst.tlServiceTable = new DataTable();
            try
            {
                div.Fill(DBExchange.Inst.tlServiceTable);
                foreach (DataRow roww in DBExchange.Inst.tlServiceTable.Rows)
                {
                    //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                    this.comboBox9.Items.Add((string)roww[1]);
                }
            }
            catch { }

        }

        private void loadOtdelServiceData()
        {
            this.comboBox9.Items.Clear();
            NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select trim(code), trim(name), reestr from reestr where division ='" + (this.comboBox7.SelectedIndex) + "' order by code ASC", DBExchange.Inst.connectDb);
            DBExchange.Inst.tlServiceTable = new DataTable();
            try
            {
                div.Fill(DBExchange.Inst.tlServiceTable);
                foreach (DataRow roww in DBExchange.Inst.tlServiceTable.Rows)
                {
                    this.comboBox9.Items.Add((string)roww[1]);
                }
            }
            catch { }

        }

        //загружает список услуг по отделению





        private void writeTicketForPatient()
        {
            if (this.searchPatientBox1.patientSet == true)
            {

                NpgsqlCommand insTicket = new NpgsqlCommand("insert into ticket (doc_in, pat_id, service_id) values (" + DBExchange.Inst.dbUsrId + "," + this.searchPatientBox1.pIdN + "," + DBExchange.Inst.tlServiceTable.Rows[this.comboBox9.SelectedIndex]["reestr"] + ") ;", DBExchange.Inst.connectDb);
                try
                {
                    insTicket.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
                finally { }
               

            }
        }

        private void loadTicketsOfPatient(Int32 gotPatId)
        {
            if (this.searchPatientBox1.patientSet == true)
            {

                this.listBoxMyPatientTickets.DataSource = null;
                List<string> patTckt = DBExchange.Inst.rtnPatTcktLst(gotPatId);
                this.listBoxMyPatientTickets.DataSource = patTckt;

                //this.comboBox12.DataSource = null;
                //this.comboBox12.DataSource = patTckt;
            }

        }

        //загружает список отделений из которых потом формруется список услуг
        private void loadOtdelData()
        {
            try
            {
                NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select trim(name) from divisions order by division ASC", DBExchange.Inst.connectDb);
                DataTable divTable = new DataTable();
                div.Fill(divTable);
                foreach (DataRow roww in divTable.Rows)
                {
                    this.comboBox7.Items.Add((string)roww[0]);
                }
                NpgsqlDataAdapter group = new NpgsqlDataAdapter("select substr(code,1,3) from reestr group by substr(code,1,3) order by substr(code,1,3) ASC", DBExchange.Inst.connectDb);
                DataTable groupTable = new DataTable();
                group.Fill(groupTable);
                foreach (DataRow roww in groupTable.Rows)
                {
                    this.comboBox11.Items.Add((string)roww[0]);
                }


            }
            catch { }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadOtdelServiceData();
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadOtdelServiceDataByGroup();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (this.searchPatientBox1.patientSet == true)
            {
                writeTicketForPatient();
            }
            else
            {
                Warnings.WarnMessages ChoPa = new Warnings.WarnMessages();
                ChoPa.warnChoosePatient();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormTicket.ActiveForm.Close();
        }









    }
}
