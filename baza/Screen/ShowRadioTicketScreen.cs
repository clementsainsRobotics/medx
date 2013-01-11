using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace baza.Screen
{
    public partial class ShowRadioTicketScreen : Form
    {

        private Editor.ClassRadiologyItem.ResearchList rl;
        private Editor.ClassRadiologyItem.ZoneList zl;
        private Processing.LabTickets.TicketListRadio ticketList ;
        public ShowRadioTicketScreen()
        {
            InitializeComponent();

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


        private void loadTickets()
        {

            if (this.comboBox1.SelectedIndex != -1)
            {
                DateTime datet = this.dateTimePicker1.Value;
                int labId = rl[this.comboBox1.SelectedIndex].ResearchId;
                ticketList = new baza.Processing.LabTickets.TicketListRadio();
                ticketList.Clear();
                listBox1.Items.Clear();
                ticketList.RadioListGet(labId, datet);
                foreach (Processing.LabTickets.RadioTicket i in ticketList)
                {
                    listBox1.Items.Add( i.PatientFullName + " " + i.PatientBirth + " " + i.PatientCart );
                }

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


        private void loadTicketResearch()
        {
            Int64 _gotTicket = ticketList[this.listBox1.SelectedIndex].TicketId;

            zl = new baza.Editor.ClassRadiologyItem.ZoneList();
            zl.Clear();

            this.listBox2.Items.Clear();
            zl.GetPatientZoneList(_gotTicket);

            

            foreach (Editor.ClassRadiologyItem.ResearchZones i in zl)
            {
                this.listBox2.Items.Add(i.ZoneName );
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadTicketResearch();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                switch (rl[this.comboBox1.SelectedIndex].ResearchGroupCode)
                {
                    case 4:

                        formUzi fu = new formUzi(ticketList[this.listBox1.SelectedIndex].PatientId, ticketList[this.listBox1.SelectedIndex].TicketId);
                        fu.ShowDialog();

                        break;
                    case 5:

                        FormScintio fs = new FormScintio(ticketList[this.listBox1.SelectedIndex].PatientId, ticketList[this.listBox1.SelectedIndex].TicketId);
                        fs.ShowDialog();

                        break;
                    case 3:

                        formRg fr = new formRg(ticketList[this.listBox1.SelectedIndex].PatientId, ticketList[this.listBox1.SelectedIndex].TicketId);
                        fr.ShowDialog();

                        break;

                    case 2:

                        //form fr = new formRg(ticketList[this.listBox1.SelectedIndex].PatientId, ticketList[this.listBox1.SelectedIndex].TicketId);
                        //fr.ShowDialog();

                        break;
                    case 1:

                        FormKt fk = new FormKt(ticketList[this.listBox1.SelectedIndex].PatientId, ticketList[this.listBox1.SelectedIndex].TicketId);
                        fk.ShowDialog();

                        break;
                }



        }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            Forms.FormSetNewTicketRadio fsntr = new baza.Forms.FormSetNewTicketRadio(0);
            fsntr.ShowDialog();
        }



    }
}
