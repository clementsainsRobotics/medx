using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace baza
{
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void Login(object sender, EventArgs e)
        {
           Login FormLogin = new Login();
           FormLogin.ShowDialog();
           
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {
            Login FormLogin = new Login();
            FormLogin.ShowDialog();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox13_Enter(object sender, EventArgs e)
        {

        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void оТправитьСообщениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void FamilySearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void OpenPatientKart(object sender, EventArgs e)
        {
            PatientKart OpenPatientKart = new PatientKart();
            OpenPatientKart.ShowDialog();
        }

        private void toolStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void RegisterDbUser(object sender, EventArgs e)
        {
            RegisterForm FormRegisterDbUser = new RegisterForm();
            FormRegisterDbUser.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void PatientList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                editContextMenuStrip.Show(PatientList, new Point(e.X, e.Y));
            }
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void updateAllMessageList()
        {
            DBExchange.Inst.updateMessagetbl();
            List<string> lMess = new List<string>();
            foreach (DataRow row in DBExchange.Inst.tblMessageAll.Rows)
            {
                lMess.Add(((string)row["m_date"].ToString()) + " " + ((string)row["m_from"]) + ": " + ((string)row["m_body"]));
           //     this.bPersonalMess.Items.Add(((string)row["m_date"].ToString()) + " " + ((string)row["m_from"]) + ": " + ((string)row["m_body"]));
            }

            this.mAllChat.DataSource = lMess;
            this.mAllChat.Invalidate();
            this.sendMessageTextBox1.Clear();
           
        }

        public void updateAnnounceList()
        {
            DBExchange.Inst.updateAnnounceTbl();
            List<string> lAnn = new List<string>();
            foreach (DataRow row in DBExchange.Inst.tblMessageAnn.Rows)
            {
                lAnn.Add(((string)row["m_date"].ToString()) + " " + ((string)row["m_from"]) + ": " + ((string)row["m_body"]));

            }
            this.Announce.DataSource = lAnn;
            this.Announce.Invalidate();
            this.sendMessageTextBox1.Clear();
        }

        public void updateKartList()
        {
            List<string> lKart = new List<string>();
            foreach (DataRow row in DBExchange.Inst.tblKartList.Rows)
            {
                lKart.Add(((string)row["nib"]) + " " + ((string)row["family_name"]) + ": " + ((string)row["first_name"]) + " " + ((string)row["second_name"]) + " " + ((string)row["birth_date"]));

            }
            this.AmbulatorKartList.DataSource = lKart;
        }

        public void StartupForm_Shown(object sender, EventArgs e)
        {
            DBExchange.Inst.getUsrData();
            this.UserName.Text = DBExchange.Inst.UsrSign;
            this.uNamelabel.Text = DBExchange.Inst.uFName;
            this.fNameLabel.Text = DBExchange.Inst.uName;
            DBExchange.Inst.GetDbObjType();
            updateAllMessageList();
            updateAnnounceList();
            DBExchange.Inst.loadPatListXml();

            
            this.JournalList.DataSource = DBExchange.Inst.tblJournal;
           
           
            //  List<object> journalItems = new List<object>();
          //  List<object[]> dbItems = DBExchange.Inst.DbSelect("m_date, m_from, m_body", "message_center");
           // List<object[]> dbItems = DBExchange.Inst.DbSelect("m_body", "message_center");
           // foreach (object[] dbItemsIt in dbItems)
           // {
            //    journalItems.Add(dbItemsIt[0]);
         //       journalItems.Add(dbItemsIt[0] + " " + dbItemsIt[1].ToString() + " " + dbItemsIt[2].ToString());
           // }
           // this.JournalList.Items.AddRange(journalItems.ToArray());
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
         //   dbExchange.DbSelect("*", "tDocuments");
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
        //    Order m_send = new Order
        //    {
            //    m_from = UserName;
            //    m_text = sendMessageTextBox1.Text;
            //    m_date = DateTime.Now;
            //};


        }

        private void sendMessageTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Messages_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void StartupForm_Enter(object sender, EventArgs e)
        {
        
            
        }

        private void mSendButton_Click(object sender, EventArgs e)
        {

        }

        private void закрытьПрограммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartupForm.ActiveForm.Close();
        }

        private void ChangeUser(object sender, EventArgs e)
        {
            Login FormLogin = new Login();
            FormLogin.ShowDialog();
        }

        private void SysExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBExchange.Inst.StartupLogoff();
            Login FormLogin = new Login();
            FormLogin.ShowDialog();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            DBExchange.Inst.sendMessage(DBExchange.Inst.dBUserName, "ALL", sendMessageTextBox1.Text);
            updateAllMessageList();
        }

        private void patientKatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newPatientForm PatientKart = new newPatientForm();
            PatientKart.ShowDialog();
        }

        private void цитологиюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGistology gistology = new FormGistology();
            gistology.ShowDialog();
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            DBExchange.Inst.sendMessage(DBExchange.Inst.dBUserName, "ANN", sendMessageTextBox1.Text);
            updateAnnounceList();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartupForm.ActiveForm.Close();
        }

        private void цитологиюToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormCytology cytology = new FormCytology();
            cytology.ShowDialog();
        }

        private void томографиюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormKt kt = new FormKt();
            kt.ShowDialog();
        }

        private void рентгенToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            formRg rg = new formRg();
            rg.ShowDialog();
        }

        private void сцинтиографияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormScintio Scintio = new FormScintio();
            Scintio.ShowDialog();
        }

        private void уЗИToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formUzi uzi = new formUzi();
            uzi.ShowDialog();
        }


    }
}
