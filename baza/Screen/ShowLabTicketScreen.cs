using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;

namespace baza.Screen
{
    public partial class ShowLabTicketScreen : Form
    {
        Document.SampleType.LabList lab;
        Processing.LabTickets.TicketList ticketList;
        Processing.LabTickets.NormLimitList normList;

        public ShowLabTicketScreen()
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

        private void loadTickets()
        {

            if (this.comboBox1.SelectedIndex != -1)
            {
                DateTime datet = this.dateTimePicker1.Value;
                int labId = lab[this.comboBox1.SelectedIndex].SampleType;
                ticketList = new baza.Processing.LabTickets.TicketList();
                ticketList.Clear();
                ticketList.TicketListGet(labId, datet);
                this.dataGridView1.DataSource = ticketList;

                
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].Visible = false;
                this.dataGridView1.Columns[2].Visible = false;

                this.dataGridView1.Columns[3].ReadOnly = false;
                this.dataGridView1.Columns[4].ReadOnly = true;
                this.dataGridView1.Columns[5].ReadOnly = true;
                this.dataGridView1.Columns[6].ReadOnly = true;
                this.dataGridView1.Columns[7].ReadOnly = true;

                //listBox1.Items.Clear();
                
                //foreach (Processing.LabTickets.Ticket i in ticketList)
                //{
                //    listBox1.Items.Add(i.TemplateName.ToString()+" "+i.PatientFullName + " " + i.PatientBirth+" "+ i.PatientCart );
                //}

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadTickets();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            loadTickets();
        }

        private void loadTemplateNorm()
        {
            if (this.dataGridView1.CurrentRow.Index > -1 && ticketList.Count > 0)
            {
                Int64 _gotTicket = ticketList[this.dataGridView1.CurrentRow.Index].TicketId;

                normList = new baza.Processing.LabTickets.NormLimitList();
                normList.Clear();

                this.listBox2.Items.Clear();
                normList.getNorms(_gotTicket);

                foreach (Processing.LabTickets.NormLimit i in normList)
                {
                    this.listBox2.Items.Add(i.NormName);
                }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentRow.Index != -1)
            {

                loadTemplateNorm();

            }
        }


        private void writeNewSample()
        {
            Int64 template = ticketList[this.dataGridView1.CurrentRow.Index].TicketId;
            int pidn = ticketList[this.dataGridView1.CurrentRow.Index].PatientId;
            Forms.FormLabDataSample flds = new baza.Forms.FormLabDataSample(pidn, template, true);
            flds.ShowDialog();
            loadTickets();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
           // if (this.dataGridView1.CurrentRow.Index != null)
          //  {
           //     writeNewSample();
            //    Screen.FormLoadXmlForLab slxfl = new FormLoadXmlForLab();
            //    slxfl.ShowDialog();
            //}
            //else
            //{
                this.dataGridView1.EndEdit();
                Screen.FormLoadXmlForLab slxfl = new FormLoadXmlForLab();
                slxfl.ShowDialog();
          //  }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            Forms.FormSetNewTicket fsntr = new baza.Forms.FormSetNewTicket(0);
            fsntr.ShowDialog();
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void writeThisTicket(long _ticket, long _labSample)
        {

            NpgsqlCommand wtt = new NpgsqlCommand("Update ticket set lab_sample_number = '"+_labSample+"' where ticket_id = '"+_ticket+"' ;",DBExchange.Inst.connectDb);
           
            try
            {
                wtt.ExecuteNonQuery();
              
        
            }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog(); 
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString()); 
                    

                   
                }

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.dataGridView1.EndEdit();

                if(
                     (long)this.dataGridView1.Rows[e.RowIndex].Cells["LabTicketNumber"].Value != -999)
                //    (long)this.dataGridView1.Rows[e.RowIndex].Cells["LabTicketNumber"].Value != ticketList[this.dataGridView1.CurrentRow.Index].LabTicketNumber)
                {
                    writeThisTicket(ticketList[e.RowIndex].TicketId, ticketList[e.RowIndex].LabTicketNumber);
                }


            }
        }


    }
}
