using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Diagnostics;
using NpgsqlTypes;
using System.Reflection;


namespace baza
{
    public partial class StartupForm : Form
    {

        
        public DataTable tblPatientList;
        private DataTable tPatientDiags;
        public List<Int64> patDocuments;
        List<Int64> usrDocuments;
        public List<int> docNumList = new List<int>();
        List<string> docbuffer = new List<string>();
        List<string> bufferUsrPat = new List<string>();
        List<int> listNumUsrPatient = new List<int>();
        List<int> diagNumList = new List<int>();
        private List<string> listPatientDiags = new List<string>();
        private DataTable tPatientKartDiags;
        private DataTable tPatientUsr;
        private List<int> patDiagNumList;
        private Document.SampleType.LabList lab;
        private Document.SampleType.SList samTy;
        private Processing.LabTickets.PatientTicketList myPatientTicketList;
        Processing.DrugsClass.PatientDrugTreatment patientDrugTreatmentList;
        string currentDoctor;
        private Timer hidePanelTimer;
        private DataTable tlDiaDataLu;
        private DataTable tlDiaDataSu;
        private DataTable tlDiaDataFu;
        private DataTable tlDiaDataLa;
        private DataTable tlDiaDataMo;
        private DataTable tlMyPatDiaData;
        private DataTable tlPatientEtap;
        private string strDiaDataName;
        private DataTable tlDiagDataDt;
        
        private Processing.LabTickets.PatientTicketList ptList;
        Warnings.WarnMessages warnMess = new Warnings.WarnMessages();
        tabDiagEnv _tabDiagEnv = new tabDiagEnv();
        tabSurgeryEnv _tabSurgeryEnv = new tabSurgeryEnv();

        
        public StartupForm()
        {
            
            InitializeComponent();
            DBExchange.Inst.MkbDataSet = false;
            hidePanelTimer = new Timer();
            hidePanelTimer.Interval = 10000;
            hidePanelTimer.Start();
            hidePanelTimer.Tick += new EventHandler(hidePanelTimer_Tick);
            searchPatientBox1.setPatientSelected += new SearchPatientBox.setNewPatientSelected(this.fillPatientKartDocuments);

            dateTimePicker2.Value = dateTimePicker1.Value.AddDays(-5);
           
        }

        void hidePanelTimer_Tick(object sender, EventArgs e)
        {
            hidePanelTimer.Stop();
            hideLeftPanel();
            
        }

        public int _gotPatientId()
        {
            int thisPatId = 0;
            if (this.searchPatientBox1.pIdN > 0 && tabControl1.SelectedTab == tabPage3)
            {
                thisPatId = this.searchPatientBox1.pIdN;
            }
            else if (this.PatientList.SelectedIndex != -1)
            {
                thisPatId = (int)this.tblPatientList.Rows[this.PatientList.SelectedIndex]["pat_id"];
            }
            return thisPatId;
        }
        public void nowTryToSearchatId(int isThisPatId)
        {
            searchPatientBox1.searchById(isThisPatId);
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
            if (DBExchange.Inst.connectDb.State == ConnectionState.Open && DBExchange.Inst.CheckProgramVersion() == true)
            {

                if ((MessageBox.Show("Провести обновление сейчас?", "Есть обновленная версия программы", MessageBoxButtons.YesNo)) == DialogResult.Yes)
                {

                    if (File.Exists(Application.StartupPath + @"\setup.exe"))
                    {
                        File.Delete(Application.StartupPath + @"\setup.exe");
                    }



                    try
                    {

                                                //newcon.Open();
                                                //NpgsqlTransaction t = newcon.BeginTransaction();
                                                //LargeObjectManager lbm = new LargeObjectManager(newcon);
                                                //int noid = lbm.Create(LargeObjectManager.READWRITE);
                                                //LargeObject lo =  lbm.Open(noid,LargeObjectManager.READWRITE);

                                                //FileStream fs = File.OpenRead(args[0]);
                                                //byte[] buf = new byte[fs.Length];
                                                //fs.Read(buf,0,(int)fs.Length);

                                                //lo.Write(buf);
                                                //lo.Close();
                                                //t.Commit();        
        
                                                //t = newcon.BeginTransaction();        
                                                //lo =  lbm.Open(noid,LargeObjectManager.READWRITE);        
                                                //FileStream fsout = File.OpenWrite(args[0] + "database");        
                                                //buf = lo.Read(lo.Size());        
                                                //fsout.Write(buf, 0, (int)lo.Size());
                                                //fsout.Flush();
                                                //fsout.Close();
                                                //lo.Close();
                                                //t.Commit();
               
                                
                        NpgsqlCommand geetO = new NpgsqlCommand("Select MAX(prog_oid) from settings", DBExchange.Inst.connectDb);
                        long getOid = (long)geetO.ExecuteScalar();
                        SaveFileDialog sf1 = new SaveFileDialog();
                        sf1.FileName = Application.StartupPath + @"\setup.exe";
                        sf1.Filter = "exe (*.exe)|*.exe";
                        sf1.ShowDialog();
                        // FileStream fs = new FileStream(sf1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, lo.Size());
                        FileStream fs = new FileStream(sf1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        NpgsqlConnection cdb = DBExchange.Inst.connectDb;
                      //  cdb.Open();
                        NpgsqlTransaction t = cdb.BeginTransaction();
                        LargeObjectManager lbm = new LargeObjectManager(cdb);
                        
                        LargeObject lo = lbm.Open(Convert.ToInt32(getOid), LargeObjectManager.READ);
                        byte[] buf = new byte[lo.Size()];
                        buf = lo.Read(lo.Size());
                     //   MemoryStream ms = new MemoryStream();

                     //   ms.Write(buf, 0, lo.Size());

                        

                        

                      //  StreamWriter s = new StreamWriter(fs);
                        fs.Write(buf, 0, lo.Size());

                       // s.Write(buf, 0, lo.Size());

                        

                       // s.Close();
                        fs.Close();
                        lo.Close();
                        t.Commit();
                       // cdb.Close();
                        System.Diagnostics.Process.Start(sf1.FileName);
                        Application.Exit();

                    }
                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                        try
                        {
                            System.Net.WebClient Client = new System.Net.WebClient();

                            Client.DownloadFile("http://medx.spb.ru/latest/setup.exe", Application.StartupPath + @"\setup.exe");
                            System.Diagnostics.Process.Start(Application.StartupPath + @"\setup.exe");
                            Application.Exit();

                        }
                        catch (Exception exception1)
                        {
                            Warnings.WarnLog log1 = new Warnings.WarnLog();
                            log1.writeLog(MethodBase.GetCurrentMethod().Name, exception1.Message.ToString(), exception1.StackTrace.ToString());
                        }
                    }




                    //Client.DownloadFileCompleted += new AsyncCompletedEventHandler(AppUpdate);

                }

            }
        }

        private void AppUpdate(object sender, EventArgs e)
        {
           

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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void FamilySearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        //открывает карту пациента

        private void OpenPatientKart(object sender, EventArgs e)
        {
           
            
            if (this.PatientList.SelectedIndex != -1)
            {
                
                nowTryToSearchatId((int)this.tblPatientList.Rows[this.PatientList.SelectedIndex]["pat_id"]);
                this.tabControl1.SelectedTab = tabPage3;
               
            }
            
        }


        //Загружает список своих пацциентов
        private List<string> loadThisPatientList()
        {
            List<string> listMyPatients = new List<string>();
            tblPatientList = new DataTable();
            this.tblPatientList = DBExchange.Inst.loadUsrPatientList();
            foreach (DataRow row in tblPatientList.Rows)
            {
                if (String.IsNullOrEmpty(row[0].ToString().Trim()) == false || String.IsNullOrEmpty(row[0].ToString().Trim()) == false || String.IsNullOrEmpty(row[2].ToString().Trim()) == false)
                {
                    listMyPatients.Add(((string)row[0]) + " " + ((string)row[1]) + ". " + ((string)row[2]) + ".");
                }
                //     this.bPersonalMess.Items.Add(((string)row["m_date"].ToString()) + " " + ((string)row["m_from"]) + ": " + ((string)row["m_body"]));
            }
            return listMyPatients;
        }


        
        //Заполняет список документов для моего пациента

        private void fillPatientDocuments(int _selectedPatientId)
        {
            List<string> buffer = new List<string>();
            usrDocuments = new List<Int64>();
           
            try
            {

                NpgsqlCommand selectPatientDocuments = new NpgsqlCommand("Select dt.descr, d.document_date, trim(doc.family_name), trim(d.document_header), d.document_number as document_number "
                +"from documents d, docum_type dt, doctors doc where doc.doc_id = d.doc_id and dt.did = d.document_type and d.pat_id ='" 
                + _selectedPatientId + "' AND d.delete=false ORDER BY d.document_date DESC LIMIT 40", DBExchange.Inst.connectDb);
                NpgsqlDataReader readDocuments = selectPatientDocuments.ExecuteReader();

                int colNum = readDocuments.FieldCount;

                while (readDocuments.Read())
                {
                    string header = "";
                    if (readDocuments[3] is DBNull)
                    { }
                    else
                    { header = readDocuments[3].ToString(); }

                    string document = ((DateTime)readDocuments[1]).ToShortDateString() + " " + (string)readDocuments[0] + " " + (string)readDocuments[2] + " " + header;
                    buffer.Add(document);
                    usrDocuments.Add((Int64)readDocuments["document_number"]);

                }
                this.listBoxHistory.DataSource = buffer;
                readDocuments.Close();
               
                
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }

        }


        //загружает список назначений для своего пациента

        private void loadMyPatientTickets(int _selectedPatientId)
        {
           
                try
                {
                    this.listBoxMyPatientTickets1.Items.Clear();

                    Processing.LabTickets.PatientTicketList ptl = new Processing.LabTickets.PatientTicketList();
                    ptl.GetPatientTicketsNotFinished(_selectedPatientId);

                    foreach (Processing.LabTickets.PatientTicket i in ptl)
                    {
                        this.listBoxMyPatientTickets1.Items.Add(i.TicketDateIn.ToShortDateString() +" "+ i.TicketName + " "+ i.TicketDoctorFName );
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            

        }




        /// <summary>
        // Заполняет список дагнозов для выбранного в поле "мои паценты" пациента
        /// </summary>
        /// <param name="_selectedPatient"></param>

        private void fillPatientDiagList(int _selectedPatient)
        {
            tPatientDiags = new DataTable();
            NpgsqlDataAdapter selPatDiags = new NpgsqlDataAdapter("Select diag_date, (select trim(name) from diags where diag = diag_data.diag),diag, "
            + "trim(comment), usr_id from diag_data where pat_id = " + _selectedPatient + " and delete=false order by diag_date Asc ", DBExchange.Inst.connectDb);
            listViewDiags.Clear();

            try
            {
                selPatDiags.Fill(tPatientDiags);


                foreach (DataRow row in tPatientDiags.Rows)
                {
                    listViewDiags.Items.Add(((DateTime)row[0]).ToShortDateString()+" " + ((string)row[1]));
                }

            }
            finally
            {

            }


        }




        private void toolStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

       //открывает форму регстрацции нового пользователя
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


        /// <summary>
        // обновление общего чата
        /// </summary>
        private void updateAllMessageList()
        {
            DBExchange.Inst.updateMessagetbl();

            //Table mAllchat = new Table();
            mAllChat.Items.Clear();
            mAllChat.BeginUpdate();

            foreach (DataRow row in DBExchange.Inst.tblMessageAll.Rows)
            {
  
               mAllChat.Items.Add (((string)row["m_date"].ToString()) + " " + ((string)row["m_from"]) + ": " + ((string)row["m_body"]).Trim());
            }
           // this.mAllChat.TableModel = model;
            this.mAllChat.EndUpdate();
           // this.mAllChat.Invalidate();
            this.sendMessageTextBox1.Clear();
           
        }


        /// <summary>
        // Обновление объявлений
        /// </summary>
        public void updateAnnounceList()
        {
            DBExchange.Inst.updateAnnounceTbl();
            Announce.Items.Clear();
            foreach (DataRow row in DBExchange.Inst.tblMessageAnn.Rows)
            {
                Announce.Items.Add(((string)row["m_date"].ToString()) + " " + ((string)row["m_from"]) + ": " + ((string)row["m_body"]).Trim());

            }

            this.Announce.Invalidate();
            this.sendMessageTextBox1.Clear();
        }

        /// <summary>
        // Обновление списка карт пациентов
        /// </summary>

        public void updateKartList()
        {
            List<string> lKart = new List<string>();
            foreach (DataRow row in DBExchange.Inst.tblKartList.Rows)
            {
                lKart.Add(((string)row["nib"]) + " " + ((string)row["family_name"]) + ": " + ((string)row["first_name"]) + " " + ((string)row["last_name"]) + " " + ((string)row["birth_date"]));

            }
          //  this.AmbulatorKartList.DataSource = lKart;
        }

        public void StartupForm_Shown(object sender, EventArgs e)
        {
            DBExchange.Inst.getUsrData();
            this.UserName.Text = "Подпись: "+DBExchange.Inst.UsrSign;
            this.uNamelabel.Text = "Имя:\r\n"+DBExchange.Inst.uFName;
            this.fNameLabel.Text = "Фамилия:\r\n" + DBExchange.Inst.uName;
            DBExchange.Inst.GetDbObjType();
            refreshStartupData();
            this.Text += DBExchange.Inst.versionNumber;
          }


        //открывает форму ппросмотра
        private void viewForm()
        {
            Form prFC = new printFormGist(setDatatoViewForm(), false, false);
            prFC.Show();

        }

        /// <summary>
        // получает данные для просмотра
        /// </summary>
        /// <returns></returns>
        private string setDatatoViewForm()
        {

            WebBrowser wbC = new WebBrowser();
            DataTable documentRow = new DataTable();
            
            //транацкция? несколлько сначала получить таблицу к которой обращаться или держать её в памяти 
            //и с начала загрузки подменять данные без повторных обращений к базе

            NpgsqlDataAdapter dbGetDocument = new NpgsqlDataAdapter("SELECT trim(dt.descr) AS type,"
              //  +"(select descr from table where num=document_number) as text, document_date,"
                + "trim(pl.family_name) as patient_name,  pl.first_name as patient_fname"
                +" FROM documents ds, patient_list pl, docum_type dt WHERE dt.did = ds.document_type and pl.pat_id = ds.pat_id" 
                + " and ds.document_number = '" + DBExchange.Inst.tblWorkJournal.Rows[JournalList.CurrentRow.Index]["document_number"] + "'", DBExchange.Inst.connectDb);
            dbGetDocument.Fill(documentRow);
            
                       
            wbC.Navigate(Application.StartupPath + @"\Templates\generic.htm");
            string formData = wbC.DocumentText;
            formData = formData.Replace("issl", documentRow.Rows[0]["type"].ToString());
            formData = formData.Replace("patient", (string)documentRow.Rows[0]["patient_fname"] + " "+(string)documentRow.Rows[0]["patient_name"] +".");
           // formData = formData.Replace("cyto.doc", documentRow.Rows[0]["doc_id"].ToString());
            //formData = formData.Replace("cyto.text", documentRow.Rows[0]["text"].ToString());
            //formData = formData.Replace("cyto.date", documentRow.Rows[0]["document_date"].ToString());
           // formData = formData.Replace("cyto.numb", documentRow.Rows[0]["document_header"].ToString());

            return formData;


        }



        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (this.JournalList.RowCount > 0)
            {
            viewForm();
            }
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


        /// <summary>
        // поиск по справочникам диагнозы список услуг и тп
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void searchDictionnary()
        {
            if (this.tabControl2.SelectedTab == this.tabPage12)
                {

                    NpgsqlCommand selDiag = new NpgsqlCommand("select diag, trim(ds), trim(name)  from diags where lower(name) like '%" + this.textBoxSearchDict.Text.ToLower() + "%' ", DBExchange.Inst.connectDb);
                    this.DiagListBox.Items.Clear() ;
                try
                    {
                        NpgsqlDataReader readData = selDiag.ExecuteReader();
                        int colNum = readData.FieldCount;
                        while (readData.Read())
                        {

                            DiagListBox.Items.Add(readData[1] +" "+readData[2]);

                        }
                 
                    }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
                finally 
                {
                    
                    this.DiagListBox.Invalidate(); 
                }


                }
            else
                if (this.tabControl2.SelectedTab == this.tabPage14)
                {
                    NpgsqlCommand selDrug = new NpgsqlCommand("select trim(drug_name), trim(international) from drug where lower(drug_name) like '%" + this.textBoxSearchDict.Text.ToLower() + "%' or lower(international) like '%" + this.textBoxSearchDict.Text.ToLower() + "%' ", DBExchange.Inst.connectDb);
                    this.DrugListBox.Items.Clear();
                    try
                    {
                        NpgsqlDataReader readData = selDrug.ExecuteReader();
                        int colNum = readData.FieldCount;
                        while (readData.Read())
                        {
                            DrugListBox.Items.Add(readData[0] + " " + readData[1]);
                        }

                    }
                    catch (Exception exception)
                    {
                         Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    }
                    finally
                    {

                        this.DrugListBox.Invalidate();
                    }

                }

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
            refreshStartupData();
            
        }

        private void mSendButton_Click(object sender, EventArgs e)
        {

        }

        private void закрытьПрограммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBExchange.Inst.connectDb.Close();
            StartupForm.ActiveForm.Close();
            
        }

        private void ChangeUser(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + @"\medex.exe");
            Application.Exit();

            //DBExchange.Inst.connectDb.Close();
            //Login FormLogin = new Login();
            //FormLogin.ShowDialog();
        }

        private void SysExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + @"\medex.exe");
            Application.Exit();

            //DBExchange.Inst.StartupLogoff();
            //Login FormLogin = new Login();
            //FormLogin.ShowDialog();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            DBExchange.Inst.sendMessage(DBExchange.Inst.UsrSign, "ALL", sendMessageTextBox1.Text.Trim());
            updateAllMessageList();
        }

        private void patientKatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newPatientForm PatientKart = new newPatientForm(false,0);
            PatientKart.ShowDialog();
            fillDiaDataLists();
        }

        private void цитологиюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGistology gistology = new FormGistology(_gotPatientId());
            gistology.ShowDialog();
            fillDiaDataLists();
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            DBExchange.Inst.sendMessage(DBExchange.Inst.UsrSign, "ANN", sendMessageTextBox1.Text.Trim());
            updateAnnounceList();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartupForm.ActiveForm.Close();
        }

        private void цитологиюToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormCytology cytology = new FormCytology(_gotPatientId());
            cytology.ShowDialog();
            fillDiaDataLists();
        }

        private void томографиюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormKt kt = new FormKt(_gotPatientId(),0);
            kt.ShowDialog();
            fillDiaDataLists();
        }

        private void рентгенToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            formRg rg = new formRg(_gotPatientId(),0);
            rg.ShowDialog();
            fillDiaDataLists();
        }

        private void сцинтиографияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormScintio Scintio = new FormScintio(_gotPatientId(),0);
            Scintio.ShowDialog();
            fillDiaDataLists();
        }

        private void уЗИToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formUzi uzi = new formUzi(_gotPatientId(), 0);
            uzi.ShowDialog();
            fillDiaDataLists();
        }

        private void StartupForm_Activated(object sender, EventArgs e)
        {
            DBExchange.Inst.GetDbObjType();
            refreshStartupData();
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            searchDictionnary();
        }

        private void textBoxSearchDict_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchDictionnary();
            }
        }


        //Изменение данных пациента
        private void changePatientKartData()
        {
            if (this.tabControl1.SelectedTab == this.tabPage2)
            {
                  if (this.PatientList.SelectedIndex != -1)
                  {
                    int myPatientSelId = (int)this.tblPatientList.Rows[this.PatientList.SelectedIndex]["pat_id"];
                    newPatientForm PatientKart = new newPatientForm(true,myPatientSelId);
                    PatientKart.ShowDialog();
                  }
             }
            else if (this.tabControl1.SelectedTab == this.tabPage3)
            {
                newPatientForm PatientKart = new newPatientForm(true, this.searchPatientBox1.pIdN);
                PatientKart.ShowDialog();
            }
            else
            {
                warnMess.warnChoosePatient();
            }

            
            
        }

        //обновление данных в главной форме
        private void refreshStartupData()
        {
            if (this.tabControl1.SelectedTab == this.tabPage2)
            {
                this.JournalList.DataSource = DBExchange.Inst.tblJournal;
                this.PatientList.DataSource = loadThisPatientList();
                refreshMyPatientData();


            }
            else if (this.tabControl1.SelectedTab == this.tabPage7)
            {
                updateAllMessageList();
                updateAnnounceList();
            }
            
        }


        /// <summary>
        // Обновляет данные пациента на вкладке мои пациенты, возможно делать при клике на вкладку
        /// </summary>
        private void refreshMyPatientData()
        {
            if (this.PatientList.SelectedIndex != -1)
            {
                int myPatientSelId = (int)this.tblPatientList.Rows[this.PatientList.SelectedIndex]["pat_id"];
                
                fillPatientDocuments(myPatientSelId);
                loadMyPatientTickets(myPatientSelId);
                fillPatientDiagList(myPatientSelId);
                fillMyPatientDiagDataList(myPatientSelId);
                fillMyPatientAnalysis(myPatientSelId);
            }

        }


        private void PatientList_Enter(object sender, EventArgs e)
        {
            refreshStartupData();
        }

        private void PatientList_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshMyPatientData();
        }


        //импортировано из patientkart.cs

        //ззаполняет список документов пациента

        private void fillPatientKartDocuments(int _selectedPatientId)
        {
           
            List<string> buffer = new List<string>();
            patDocuments = new List<Int64>();
           
            try
            {

                NpgsqlCommand selectPatientDocuments = new NpgsqlCommand("Select trim(dt.descr), d.document_date, trim(doc.family_name), trim(d.document_header), d.document_number "
                + "from documents d, docum_type dt, doctors doc where doc.doc_id = d.doc_id and dt.did = d.document_type and d.pat_id ='" 
                + _selectedPatientId + "' AND d.delete='0' ORDER BY d.document_date DESC LIMIT 50", DBExchange.Inst.connectDb);
                NpgsqlDataReader readDocuments = selectPatientDocuments.ExecuteReader();

                int colNum = readDocuments.FieldCount;

                while (readDocuments.Read())
                {
                    string header = "";
                    if (readDocuments[3] is DBNull)
                    { }
                    else
                    { header = readDocuments[3].ToString(); }

                    string document = ((DateTime)readDocuments[1]).ToShortDateString() + " " + ((string)readDocuments[0]) + " " + ((string)readDocuments[2]) + " " + header;
                    buffer.Add(document);
                    patDocuments.Add(Convert.ToInt64(readDocuments["document_number"]));

                }
                this.listBox1.DataSource = buffer;
                readDocuments.Close();
                getCurrDoctorName();
                this.listBoxDiagnosis.DataSource = fillPatientKartDiagList(0);
                
            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }

            finally
            {

                loadDoctorsOfPatient();
                this.tabPage32.Refresh();
                this.tabControl3.Refresh();

            }



        }

        //получает имя лечащего врача
        private void getCurrDoctorName()
        {
            
            try
            {
                NpgsqlCommand selDoct = new NpgsqlCommand("Select trim(family_name), substr(first_name,1,1), substr(last_name,1,1) from doctors where doc_id = " 
                    + this.searchPatientBox1.currDoc + ";", DBExchange.Inst.connectDb);
                NpgsqlDataReader selectDoctor = selDoct.ExecuteReader();
                while (selectDoctor.Read())
                {
                    currentDoctor = selectDoctor[0] + " " + selectDoctor[1] + ". " + selectDoctor[2] + ".";
                }
                selectDoctor.Close();
            }
            catch { }

            this.label35.Text = currentDoctor;
            this.label35.Invalidate();
        }



        //загружает список всех докторов

        private void loadDoctors()
        {



            NpgsqlCommand selectDoc = new NpgsqlCommand("select trim(family_name), trim(first_name), trim(last_name), doc_id from doctors ", DBExchange.Inst.connectDb);
            try
            {

                NpgsqlDataReader readDoctors = selectDoc.ExecuteReader();
                int colNum = readDoctors.FieldCount;

                while (readDoctors.Read())
                {
                    string document = readDoctors[0].ToString() + " " + (readDoctors[1].ToString()) + " " + (readDoctors[2].ToString());
                    docbuffer.Add(document);
                    docNumList.Add(Convert.ToInt32(readDoctors[3]));

                }
                this.comboBox10.DataSource = docbuffer;



            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            finally
            {

            }
        }


        //загружает список назначенных докторов для пациента

        private void loadDoctorsOfPatient()
        {


            tPatientUsr = new DataTable();

            NpgsqlDataAdapter selectUsrPatData = new NpgsqlDataAdapter("select (select trim(family_name) from doctors where doc_id = user_data.usr_id ) as family_name,"
               + " (select substr(first_name,1,1) from doctors where doc_id = user_data.usr_id ) as first_name, usr_id, serial "
            + " from user_data where pat_id = " + this.searchPatientBox1.pIdN + " and approve = true", DBExchange.Inst.connectDb);



            bufferUsrPat = new List<string>();
            listNumUsrPatient = new List<int>();

           // this.listBox11.Items.Clear();
            try
            {
                selectUsrPatData.Fill(tPatientUsr);

                //NpgsqlDataReader readData = selectUsrPatData.ExecuteReader();
                //  int colNum = readData.FieldCount;

                if (tPatientUsr.Rows.Count > 0)
                {
                    foreach (DataRow readData in tPatientUsr.Rows)
                    {
                        string document = (string)readData[0] + " " + (string)readData[1] + ".";
                        bufferUsrPat.Add(document);
                        listNumUsrPatient.Add(Convert.ToInt32(readData[2]));

                    }
                }
                this.listBox11.DataSource = bufferUsrPat;



            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }

            finally
            {
                this.listBox11.Refresh();
            }


        }

        //установить лечащего врача
        private void setCurrDoctor()
        {

            NpgsqlCommand setCuDo = new NpgsqlCommand("update patient_list set curr_doctor = " + docNumList[this.comboBox10.SelectedIndex] 
                + " where pat_id = " + this.searchPatientBox1.pIdN + " ;", DBExchange.Inst.connectDb);
            try
            {
                setCuDo.ExecuteNonQuery();

            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            finally
            {
                nowTryToSearchatId(this.searchPatientBox1.pIdN);
            }
            getCurrDoctorName();

        }




        //записать пациента врачу
        private void insPatUsrInList()
        {
            if (listNumUsrPatient.Contains(docNumList[this.comboBox10.SelectedIndex]))
            { }
            else
            {

                NpgsqlCommand insPUiL = new NpgsqlCommand("insert into user_data (usr_id, pat_id, create_id) values ('" + docNumList[this.comboBox10.SelectedIndex] 
                    + "','" + this.searchPatientBox1.pIdN + "','" + DBExchange.Inst.dbUsrId + "') ;", DBExchange.Inst.connectDb);
                try
                {
                    insPUiL.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
                finally { }
                loadDoctorsOfPatient();
            }
        }


        //убрать врача из списка
        private void remUsrFromPatList()
        {
            if (listNumUsrPatient.Contains(docNumList[this.comboBox10.SelectedIndex]))
            {
                NpgsqlCommand remPUiL = new NpgsqlCommand("update user_data set approve = false, mod_id = " + DBExchange.Inst.dbUsrId 
                    + ", mod_date ='now()' where serial = '" + this.tPatientUsr.Rows[this.listBox11.SelectedIndex]["serial"] + "' ;", DBExchange.Inst.connectDb);
                try
                {
                    remPUiL.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
                finally { }

            }
            else
            {


            }
            loadDoctorsOfPatient();
        }








        /// <summary>
        // Заполняет список диагнозов пациента в карте пациента
        //0 Диагноз направившего учреждения
        //1 Диагноз при поступлении/обращении
        //2 Клинический диагноз
        //3 Уточненный клинический диагноз
        //4 Патологоанатомический диагноз
        //5 Сопутствующий диагноз
        //6 Причина смерти
        //


        /// </summary>
        private List<string> fillPatientKartDiagList(int _case)
        {
            string _command = "";
            switch (_case)
            {
                case 0:
                    _command = "Select distinct on (dd.diag) diag_date, trim(ds.name), dd.diag, serial, "
                    + " trim(comment), usr_id from diag_data dd, diags ds where pat_id = " + this.searchPatientBox1.pIdN
                    + " and delete = false and ds.diag = dd.diag order by dd.diag Asc ";
                    break;
                case 1:
                    _command = "Select diag_date, trim(ds.name), dd.diag, serial, "
                    + " trim(comment), usr_id from diag_data dd, diags ds where ds.diag = dd.diag and pat_id = " + this.searchPatientBox1.pIdN
                    + " and delete = false and main_mark=true order by diag_date desc ";
                    break;
                case 2:
                    _command = "Select diag_date, trim(ds.name), dd.diag, serial, "
                    + " trim(comment), usr_id from diag_data dd, diags ds where ds.diag = dd.diag and pat_id = " + this.searchPatientBox1.pIdN
                    + " and delete = false order by diag_date desc ";
                    break;
                case 3:
                    _command = "Select diag_date, trim(ds.name), dd.diag, serial, "
                    + " trim(comment), usr_id from diag_data dd, diags ds where ds.diag = dd.diag and pat_id = " + this.searchPatientBox1.pIdN
                    + " and delete = true order by diag_date desc ";
                    break;

            }


            tPatientKartDiags = new DataTable();
            NpgsqlDataAdapter selPatDiags = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
            listPatientDiags = new List<string>();
            patDiagNumList = new List<int>();
            try
            {
                selPatDiags.Fill(tPatientKartDiags);


                foreach (DataRow row in tPatientKartDiags.Rows)
                {
                    listPatientDiags.Add(((DateTime)row[0]).ToShortDateString() + " " + ((string)row[1]));
                    patDiagNumList.Add((int)row[3]);
                }

            }
            catch
            {
                
            }
            return listPatientDiags;

        }

        public void refreshDiagList()
        {
            this.listBoxDiagnosis.DataSource = fillPatientKartDiagList(0);
            this.listBoxDiagnosis.Invalidate(); 

        }


        //удалть диагноз из дагнозов пациента

        private void remDiagFromPatDiags()
        {
            string commandString;
            if (this.searchPatientBox1.patientSet == true && listBoxDiagnosis.SelectedIndex >= 0 && this.richTextBox1.Text.Trim() != "Причина удаления") 
               
                
                {
                    NpgsqlCommand getComment = new NpgsqlCommand("select comment from diag_data where serial = '" 
                        + this.patDiagNumList[listBoxDiagnosis.SelectedIndex] + "' ;", DBExchange.Inst.connectDb);
                    commandString = "update diag_data set delete = true and comment = 'Удаление " + DBExchange.Inst.UsrSign 
                        + " " + this.richTextBox1.Text.Trim() + "' where serial = '" + this.patDiagNumList[listBoxDiagnosis.SelectedIndex] + "' ;";
                 
                try
                {
                    var gotComment = getComment.ExecuteScalar();
                    if (Convert.IsDBNull(gotComment) == false)
                    {
                        string comTxt = gotComment.ToString();
                        commandString = "update diag_data set delete = true and comment = '" + comTxt + " Удаление " + DBExchange.Inst.UsrSign 
                            + " " + this.richTextBox1.Text.Trim() + "' where serial = '" + this.patDiagNumList[listBoxDiagnosis.SelectedIndex] + "' ;";

                    }
                    NpgsqlCommand remPatDiag = new NpgsqlCommand( commandString , DBExchange.Inst.connectDb);
                    remPatDiag.ExecuteNonQuery();
                    this.richTextBox1.Text = "Причина удаления";

                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
                finally { }
                refreshDiagList();


            }
            else
            { 
               System.Windows.Forms.MessageBox.Show ( "Укажите причину удаления, выберите пациента или диагноз в списке","Ошибка удаления");     
            }
     
                
        }



        private void RemFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            remUsrFromPatList();
        }

        private void AddToListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.searchPatientBox1.patientSet == true)
            {

                insPatUsrInList();
 
            }
            else
            {
                Warnings.WarnMessages ChoPa = new Warnings.WarnMessages();
                ChoPa.warnChoosePatient();
            }


        }

        private void SetCurDocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.searchPatientBox1.patientSet == true)
            {
                setCurrDoctor();

            }
            else
            {
                Warnings.WarnMessages ChoPa = new Warnings.WarnMessages();
                ChoPa.warnChoosePatient();
            }
        }

        //загружает данные в выбранный таб в tabcontrol3

        private void loadTabDataForTaControl3()
        {

            if (this.tabControl3.SelectedTab == this.tabPage32) //Врачи
            {
                loadDoctors();
                if (this.searchPatientBox1.patientSet == true)
                {
                    loadDoctorsOfPatient();
                }
            }
            if (this.tabControl3.SelectedTab == this.tabPage35) //Назначения
            {
                
                if (this.searchPatientBox1.patientSet == true)
                {
                    loadTicketsOfPatient(this.searchPatientBox1.pIdN);
                }
            }
            if (this.tabControl3.SelectedTab == this.tabPage23) //обследования
            {
                fillDiaDataLists();
                //if (this.comboBox6.SelectedIndex == -1)
                //{
                //    this.comboBox6.SelectedIndex = 1;
                //}
                //this.textBox2.SelectedIndex = 0;

            }
            if (this.tabControl3.SelectedTab == this.tabPagePatientKartDiagnosis) //Диагнозы
            {
               
                if (this.searchPatientBox1.patientSet == true)
                {
                    this.listBoxDiagnosis.DataSource = fillPatientKartDiagList(0);
                }
            }
            if (this.tabControl3.SelectedTab == this.tabPage4) //Лечение
            {
                if (this.searchPatientBox1.patientSet == true)
                {
                    refreshSurgeryTab();
                }
            }


        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadTabDataForTaControl3();
        }

        //загружает спписок назначений для пациента
        
        private void loadTicketsOfPatient(Int32 gotPatId)
        {
            if (this.searchPatientBox1.patientSet == true)
            {
                try
                {
                this.listBoxMyPatientTickets.Items.Clear();
                Processing.LabTickets.PatientTicketList ptl = new Processing.LabTickets.PatientTicketList();
                ptl.GetPatientTicketsNotFinished(gotPatId);
                    foreach (Processing.LabTickets.PatientTicket i in ptl)
                    {
                        this.listBoxMyPatientTickets.Items.Add(i.TicketDateIn.ToShortDateString() +" "+ i.TicketName + " "+ i.TicketDoctorFName );
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }

        }







        //private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    reziseFullDescrRichTB();
        //}

        //private void richTextBox1_MouseEnter(object sender, EventArgs e)
        //{
        //    reziseFullDescrRichTB();
        //}

        //private void reziseFullDescrRichTB()
        //{
        //    if (this.richTextBox1.Text == "Полное описание")
        //    {
        //        this.richTextBox1.Clear();
        //    }
        //    if ((string)this.comboBox6.SelectedItem == "КТ")
        //    {
        //        richTextBox1.Size = new Size(545, 300);
        //    }
        //    else
        //    {
        //        richTextBox1.Size = new Size(680, 300);
        //    }
        //    this.groupBox14.Invalidate();
        //}

        //private void reziseBackFullDescrRichTB()
        //{
        //    if ((string)this.comboBox6.SelectedItem == "КТ")
        //    {
        //        richTextBox1.Size = new Size(545, 70);
        //    }
        //    else
        //    {
        //        richTextBox1.Size = new Size(680, 60);
        //    }
        //    this.groupBox14.Invalidate();
        //}

        //private void richTextBox1_MouseLeave(object sender, EventArgs e)
        //{
        //    reziseBackFullDescrRichTB();
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.searchPatientBox1.patientSet == true)
            {
               // insDiagData();
            }
            else
            {
                Warnings.WarnMessages ChoPa = new Warnings.WarnMessages();
                ChoPa.warnChoosePatient();
            }
        }

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            DBExchange.Inst.sendMessage(DBExchange.Inst.UsrSign, "ANN", sendMessageTextBox1.Text);
            updateAnnounceList();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            DBExchange.Inst.sendMessage(DBExchange.Inst.UsrSign, "ALL", sendMessageTextBox1.Text);
            updateAllMessageList();
        }

        //загрузка данных о лечен пациента
        private void loadSurgeryDataForSelectedPatient()
        {

        }


        //зменяет сдержмое вкладк обследованиие ри выборе внутренних табов
        //private void tabControl5_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    fillDiaDataLists();
        //    if (this.tabControl5.SelectedTab == tabPage24)
        //    {
        //        this.comboBox6.Items.Clear();
        //        this.comboBox6.Items.AddRange(new string[] {"КТ","УЗИ","МРТ","ПЭТ","РГ","СГ"} );
        //        this.comboBox6.SelectedIndex = 0;
        //        this.comboBox6.Size = new Size(60, 20);
        //        this.comboBox6.Visible = true;
               
        //        this.richTextBox1.Visible = true;
        //        this.textBox2.Visible = true;
        //        this.button3.Visible = true;
        //        this.dateTimePicker5.Visible = true;
        //        this.comboBox12.Visible = true;
        //        this.button3.Visible = true;
        //        this.button9.Visible = true;
        //        this.button8.Visible = true;
        //        this.textBoxNumIss.Visible = true;
        //        this.toolStripLabResearch.Visible = false;
        //        this.toolStripMorphResearch.Visible = false;
        //    }
        //   else if (this.tabControl5.SelectedTab == tabPage25)
        //    {
        //        this.comboBox6.Items.Clear();
        //        this.comboBox6.Items.AddRange(new string[] { "ЭКГ", "ФВД", "ЭЭФ"});
        //        this.comboBox6.SelectedIndex = 0;
        //        this.comboBox6.Size = new Size(60, 20);
        //        this.comboBox6.Visible = true;
               
        //        this.richTextBox1.Visible = true;
        //        this.textBox2.Visible = true;
        //        this.button3.Visible = true;
        //        this.dateTimePicker5.Visible = true;
        //        this.comboBox12.Visible = true;
        //        this.button9.Visible = true;
        //        this.button8.Visible = true;
        //        this.textBoxNumIss.Visible = true;
        //        this.toolStripLabResearch.Visible = false;
        //        this.toolStripMorphResearch.Visible = false;
        //    }
        //    else if (this.tabControl5.SelectedTab == tabPage26)
        //    {

        //        this.comboBox6.Visible = false;
        //        this.richTextBox1.Visible = false;
        //        this.textBox2.Visible = false;
        //        this.button3.Visible = false;
        //        this.dateTimePicker5.Visible = false;
        //        this.comboBox12.Visible = false;
        //        this.button9.Visible = false;
        //        this.button8.Visible = false;
        //        this.textBoxNumIss.Visible = false;
        //        this.toolStripLabResearch.Visible = true;
        //        this.toolStripMorphResearch.Visible = false;
        //    }
        //    else if (this.tabControl5.SelectedTab == tabPage27)
        //    {

        //        this.comboBox6.Visible = false;
        //        this.richTextBox1.Visible = false;
        //        this.textBox2.Visible = false;
        //        this.button3.Visible = false;
        //        this.dateTimePicker5.Visible = false;
        //        this.comboBox12.Visible = false;
        //        this.button9.Visible = false;
        //        this.button8.Visible = false;
        //        this.textBoxNumIss.Visible = false;
        //        this.toolStripLabResearch.Visible = false;
        //        this.toolStripMorphResearch.Visible = true;
        //    }
        //    else if (this.tabControl5.SelectedTab == tabPage39)
        //    {
        //        this.comboBox6.Items.Clear();
        //        this.comboBox6.Items.AddRange(new string[] { "Осмотр" });
        //        this.comboBox6.SelectedIndex = 0;
        //        this.comboBox6.Visible = true;
        //        this.richTextBox1.Visible = true;
        //        this.textBox2.Visible = true;
        //        this.button3.Visible = true;
        //        this.dateTimePicker5.Visible = true;
        //        this.comboBox12.Visible = true;
        //        this.button3.Visible = true;
        //        this.button9.Visible = true;
        //        this.button8.Visible = true;
        //        this.textBoxNumIss.Visible = false;
        //        this.comboBox6.Size = new Size(143,20);
        //        this.toolStripLabResearch.Visible = false;
        //        this.toolStripMorphResearch.Visible = false;
        //    }


        //}





        private void tabControl3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            loadTabDataForTaControl3();
        }
        //открыть гстологию
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            FormGistology gistology = new FormGistology(this.searchPatientBox1.pIdN);
            gistology.ShowDialog();
            fillDiaDataLists();
        }

        private void toolStripButton16_Click_1(object sender, EventArgs e)
        {
            FormCytology cytology = new FormCytology(_gotPatientId());
            cytology.ShowDialog();
            fillDiaDataLists();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //setKTControls();
        }


        //Содаёт необходмые для КТ поля и чекбоксы
        //private void setKTControls()
        //{
        //    if ((string)this.comboBox6.SelectedItem == "КТ")
        //    {

        //        this.checkBox5.Visible = true;
        //        this.checkBox6.Visible = true;
        //        this.textBox3.Visible = true;
        //        this.richTextBox1.Size = new Size(545, 54);

        //    }
        //    else
        //    {

        //        this.checkBox5.Visible = false;
        //        this.checkBox6.Visible = false;
        //        this.textBox3.Visible = false;
        //        this.richTextBox1.Size = new Size(680, 54);
        //    }
        //}

        //заполнение таблиц обследований listbox 18-21 tabpage 24-27 tabcontrol5
        private void fillDiaDataLists()
        {

            //Лучевое
            if (this.tabControl5.SelectedTab == this.tabPage24) 
            {
                tlDiaDataLu = new DataTable();
            this.listBox18.Items.Clear();
            NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select d.document_type, dt.descr, d.document_date, trim(d.document_header), trim(doc.family_name) as doctor, d.doc_id, d.document_id, dt.table"
              + " from documents d, docum_type dt, doctors doc where d.pat_id = '" + this.searchPatientBox1.pIdN + 
              "' and document_type in (13, 17, 20, 19, 16, 12) and d.document_type=dt.did and d.doc_id=doc.doc_id order by document_date DESC", 
              DBExchange.Inst.connectDb); tlDiaDataLu = new DataTable();
            try
            {
                div.Fill(tlDiaDataLu);
                foreach (DataRow roww in tlDiaDataLu.Rows)
                {
                    //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                    this.listBox18.Items.Add(((DateTime)roww[2]).ToShortDateString() + " " + (string)roww[1] + " " + (string)roww[4] + ": " + (string)roww[3]);

                }
            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }

            }
            
                //Функциональное
            else if (this.tabControl5.SelectedTab == this.tabPage25)
                {

                    this.listBox19.Items.Clear();
              NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select d.document_type, dt.descr, d.document_date, trim(d.document_header), trim(doc.family_name) as doctor, d.doc_id, d.document_id, dt.table"
              + " from documents d, docum_type dt, doctors doc where d.pat_id = '" + this.searchPatientBox1.pIdN + 
              "' and document_type in (23, 22, 21) and d.document_type=dt.did and d.doc_id=doc.doc_id order by document_date DESC", DBExchange.Inst.connectDb);
                    tlDiaDataFu = new DataTable();
                    try
                    {
                        div.Fill(tlDiaDataFu);
                        foreach (DataRow roww in tlDiaDataFu.Rows)
                        {
                            //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                            this.listBox19.Items.Add(((DateTime)roww[2]).ToShortDateString() + " " + (string)roww[1] + " " + (string)roww[4] + ": " + (string)roww[3]);
                        }
                    }
                    catch (Exception exception)
                    {
                         Warnings.WarnLog log = new Warnings.WarnLog();    
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    }

                }
             //Лабораторное
                else  if (this.tabControl5.SelectedTab == this.tabPage26)
                    {
                        loadLab();
                        loadSampleType();
                        //this.listBox20.Items.Clear();
                        //  NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select d.document_type, dt.descr, d.document_date, trim(d.document_header), doc.family_name as doctor, d.doc_id, d.document_id, dt.table"
                        //  + " from documents d, docum_type dt, doctors doc where d.pat_id = '" + this.searchPatientBox1.pIdN + 
                        //  "' and document_type in (24,25) and d.document_type=dt.did and d.doc_id=doc.doc_id order by document_date ASC ", DBExchange.Inst.connectDb); tlDiaDataLa = new DataTable();
                        //try
                        //{
                        //    div.Fill(tlDiaDataLa);
                        //    foreach (DataRow roww in tlDiaDataLa.Rows)
                        //    {
                        //        //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                        //        this.listBox20.Items.Add((string)roww[1]);
                        //    }
                        //}
                        //catch (Exception exception)
                        //{
                        //     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                        //}
                        fillPatientlabList(this.searchPatientBox1.pIdN);
                    }
        //Осмотры
            else if (this.tabControl5.SelectedTab == this.tabPage39)
            {

                this.listBox23.Items.Clear();
                NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select d.document_type, dt.descr, d.document_date, trim(d.document_header), trim(doc.family_name) as doctor, d.doc_id, d.document_id, dt.table"
          + " from documents d, docum_type dt, doctors doc where d.pat_id = '" + this.searchPatientBox1.pIdN +
          "' and document_type = 27 and d.document_type=dt.did and d.doc_id=doc.doc_id order by document_date ASC", DBExchange.Inst.connectDb);
                tlDiaDataSu = new DataTable();
                try
                {
                    div.Fill(tlDiaDataSu);
                    foreach (DataRow roww in tlDiaDataSu.Rows)
                    {
                        //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                        this.listBox23.Items.Add(((DateTime)roww[2]).ToShortDateString() + " " + (string)roww[1] + " " + (string)roww[4] + ": " + (string)roww[3]);

                    }
                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }

            }

                    else
                      //Морфология
                        {

                            this.listBox21.Items.Clear();
              NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select d.document_type, dt.descr, d.document_date, trim(d.document_header), trim(doc.family_name) as doctor, d.doc_id, d.document_id, dt.table"
              +" from documents d, docum_type dt, doctors doc where d.pat_id = '" + this.searchPatientBox1.pIdN + 
              "' and document_type in (11,15) and d.document_type=dt.did and d.doc_id=doc.doc_id order by document_date ASC", DBExchange.Inst.connectDb);
                            tlDiaDataMo = new DataTable();
                            try
                            {
                                div.Fill(tlDiaDataMo);
                                foreach (DataRow roww in tlDiaDataMo.Rows)
                                {
                                    //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                                    this.listBox21.Items.Add(((DateTime)roww[2]).ToShortDateString() + " " + (string)roww[1] + " " + (string)roww[4] + ": " + (string)roww[3]);
                                }
                            }
                            catch (Exception exception)
                            {
                                System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name );
                            }

                        }
        }

        private void loadLab()
        {
            lab = new Document.SampleType.LabList();
            lab.LabListGet();
            foreach (Document.SampleType.LabItem i in lab)
            {
                this.comboBoxPatCartLab.Items.Add(i.SampleName);

            }
            if (this.comboBoxPatCartLab.Items.Count > 0)
            {
                this.comboBoxPatCartLab.SelectedIndex = 0;
            }
        }

        private void loadSampleType()
        {
            
                try
                {
                    comboBoxPatCartAnType.Items.Clear();

                    samTy = new baza.Document.SampleType.SList();
                    samTy.SampleListGet();
                    foreach (Document.SampleType.SampleItem i in samTy)
                    {
                        this.comboBoxPatCartAnType.Items.Add(i.SampleName);
                    }
                    if (this.comboBoxPatCartAnType.Items.Count >0 )
                    {
                        this.comboBoxPatCartAnType.SelectedIndex = 0;
                    }
                    //this.comboBoxPatCartAnType.Items.Add("Все показатели");
                    this.comboBoxPatCartAnType.Refresh();
                    

                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
           


        }

        //функция получает из списка выбранную строку, делает запрос в нужную таблицу и выводит хтмл окно с данными, либо печатает
        private void getDiagDataFromList()
        {

            tlDiagDataDt = new DataTable();
            int _patId = this.searchPatientBox1.pIdN;
            int fieldPosition;
            long documentId = 0;
            string documentTable = "";
            


            //Обследования
            if (this.tabControl5.SelectedTab == this.tabPage24)
            {
                if (listBox18.Items.Count > 0)
                {
                    fieldPosition = this.listBox18.SelectedIndex;
                    if (fieldPosition != -1)
                    {
                        documentTable = tlDiaDataLu.Rows[fieldPosition]["table"].ToString();
                        documentId = (long)tlDiaDataLu.Rows[fieldPosition]["document_id"];
                    }
                }
            }

            else if (this.tabControl5.SelectedTab == this.tabPage25)
            {

                if (listBox19.Items.Count > 0)
                {
                    fieldPosition = this.listBox19.SelectedIndex;
                    documentTable = tlDiaDataFu.Rows[fieldPosition]["table"].ToString();
                    documentId = (long)tlDiaDataFu.Rows[fieldPosition]["document_id"];

                }
               

            }
            //Лабораторное
            else if (this.tabControl5.SelectedTab == this.tabPage26)
            {
                this.dateTimePicker2.Value = this.dateTimePicker1.Value.AddDays(-14);
                if (listBox20.Items.Count > 0)
                {
                    fieldPosition = this.listBox20.SelectedIndex;
                    documentTable = tlDiaDataLa.Rows[fieldPosition]["table"].ToString();
                    documentId = (long)tlDiaDataLa.Rows[fieldPosition]["document_id"];

                }
                else
                {
                    fillPatientlabList(_patId);
                }
               

            }
            //Осмотры
            else if (this.tabControl5.SelectedTab == this.tabPage39)
            {
                if (listBox23.Items.Count > 0)
                {
                    fieldPosition = this.listBox23.SelectedIndex;
                    documentTable = tlDiaDataSu.Rows[fieldPosition]["table"].ToString();
                    documentId = (long)tlDiaDataSu.Rows[fieldPosition]["document_id"];

                }
               
            }

            else
            {
                if (listBox21.Items.Count > 0)
                {
                    fieldPosition = this.listBox21.SelectedIndex;
                    documentTable = tlDiaDataMo.Rows[fieldPosition]["table"].ToString();
                    documentId = (long)tlDiaDataMo.Rows[fieldPosition]["document_id"];

                }
                
            }
            //Лечение




            //формирруем запрос в таблицу из неё берём данныые
            if (documentId > 0)
            {
                NpgsqlDataAdapter getDiDa = new NpgsqlDataAdapter("Select * from " + documentTable + " where num = '" + documentId + "'", DBExchange.Inst.connectDb);
                getDiDa.Fill(tlDiagDataDt);
            }

        }

        private void fillMyPatientAnalysis(int _selectedPatient)
        {

            this.listBox12.Items.Clear();
            myPatientTicketList = new baza.Processing.LabTickets.PatientTicketList();
            myPatientTicketList.GetPatientTicketsFinished(_selectedPatient,DateTime.Now.AddDays(-14),DateTime.Now);

            foreach (Processing.LabTickets.PatientTicket i in myPatientTicketList)
            {

                this.listBox12.Items.Add(i.TicketDateOut.ToShortDateString() +" "+i.TicketName+" "+i.TicketDoctorFName);

            }

        }



        //запполняет таблицу обследований моего пациента
        private void fillMyPatientDiagDataList(int _selectedPatient)
        {
            this.listBox12.Items.Clear();

            NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select d.document_type, dt.descr, d.document_date, trim(d.document_header), doc.family_name as doctor, d.doc_id, d.document_id, dt.table"
              +" from documents d, docum_type dt, doctors doc where d.pat_id = '" + _selectedPatient + "' and d.document_type in (11, 15, 13, 20, 19, 16, 12, 24, 25, 23, 22, 21, 17)"
              +" and d.document_type=dt.did and d.doc_id=doc.doc_id order by document_date ASC ", DBExchange.Inst.connectDb);
            tlMyPatDiaData = new DataTable();
            try
            {
                div.Fill(tlMyPatDiaData);
                foreach (DataRow roww in tlMyPatDiaData.Rows)
                {
                    //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                    this.listBox12.Items.Add(((DateTime)roww[2]).ToShortDateString()+" "+ (string)roww[1] + " " + (string)roww[3] + " " + (string)roww[4]);
                }
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();    
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }

        }


        //Распечатка и формирование документа для печати

        //private void printDiagData()
        //{
        //    if (this.searchPatientBox1.patientSet == true)
        //    {
        //        if (this.textBoxNumIss.Visible == true && this.textBoxNumIss.Text == "Номер исслед" || (this.textBoxNumIss.Text.Trim()).Length < 1)
        //        {
        //            warnMess.warnSetNumIss();
        //        }
        //        else
        //        {
        //            if (this.comboBox6.Text.Trim() == "КТ" && (this.textBox3.Text == "Шаг, мм" || (this.textBox3.Text.Trim()).Length < 1))
        //            {
        //                warnMess.warnSetStepKT();
        //            }
        //            else
        //            {
        //                string diaNomer;

        //                NpgsqlCommand chkNamePat = new NpgsqlCommand("Select nomer FROM " + writeDiagDataTable() + " WHERE pat_id = '" + this.searchPatientBox1.pIdN +
        //                    "' AND nomer Like '" + this.textBoxNumIss.Text.Trim() + "' ;", DBExchange.Inst.connectDb);

        //                try
        //                {
        //                    diaNomer = (string)chkNamePat.ExecuteScalar();
        //                    if (diaNomer == null)
        //                    {
        //                        insDiagData();
        //                        Form prFC = new printFormGist(setDiagDataToForm(), true, false);
        //                    }
        //                }
        //                catch (Exception exception)
        //                {
        //                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
        //                }
        //            }
        //        }
  
        //    }
        //    else
        //    {
        //        Warnings.WarnMessages ChoPa = new Warnings.WarnMessages();
        //        ChoPa.warnChoosePatient();
        //    }
        //}

        //Просмотр данных обследования
        //private void viewDiagData()
        //{
        //    if (this.searchPatientBox1.patientSet == true)
        //    {
        //        if (this.textBoxNumIss.Visible == true && this.textBoxNumIss.Text == "Номер исслед" || (this.textBoxNumIss.Text.Trim()).Length < 1)
        //        {
        //            warnMess.warnSetNumIss();
        //        }
        //        else
        //        {
        //            if (this.comboBox6.Text.Trim() == "КТ" && this.textBox3.Text == "Шаг, мм" || (this.textBox3.Text.Trim()).Length < 1)
        //            {
        //                warnMess.warnSetStepKT();
        //            }
        //            else
        //            {
        //                string diaNomer;

        //                NpgsqlCommand chkNamePat = new NpgsqlCommand("Select nomer FROM " + writeDiagDataTable() + " WHERE pat_id = '" + this.searchPatientBox1.pIdN +
        //                    "' AND nomer Like '" + this.textBoxNumIss.Text.Trim() + "' ;", DBExchange.Inst.connectDb);

        //                try
        //                {
        //                    diaNomer = (string)chkNamePat.ExecuteScalar();
        //                    if (diaNomer == null)
        //                    {
        //                        Form prFC = new printFormGist(setDiagDataToForm(), false, false);
        //                        prFC.Show();
        //                    }
        //                }
        //                catch (Exception exception)
        //                {
        //                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
        //                }

        //            }
        //        }
        //    }
        //    else
        //    {
        //        Warnings.WarnMessages ChoPa = new Warnings.WarnMessages();
        //        ChoPa.warnChoosePatient();
        //    }
        //}

        //формирует тело html страницы для распечатки или просмотра
        //private string setDiagDataToForm()
        //{

        //    WebBrowser wbC = new WebBrowser();
        //    string cyto;

        //    if (this.comboBox6.Text.Trim() == "КТ")
        //    {
        //        wbC.Navigate(Application.StartupPath + @"\Templates\kt.htm");
        //        cyto = wbC.DocumentText;
        //        cyto = cyto.Replace("patient", this.searchPatientBox1.patName);
        //        cyto = cyto.Replace("cyto.doc", DBExchange.Inst.UsrSign);
        //        cyto = cyto.Replace("cyto.text", this.richTextBox1.Text);

        //        if (this.checkBox5.Checked == true)
        //        {
        //            cyto = cyto.Replace("contrast", "С контрастированием");
        //        }
        //        else
        //        {
        //            cyto = cyto.Replace("contrast", "без контрастирования");
        //        }

        //        if (this.checkBox6.Checked == true)
        //        {
        //            cyto = cyto.Replace("spiral", "Спиральная");
        //        }
        //        else
        //        {
        //            cyto = cyto.Replace("spiral", " ");
        //        }

        //        cyto = cyto.Replace("step", "Шаг "+this.textBox3.Text+" мм. ");

        //        cyto = cyto.Replace("cyto.date", this.dateTimePicker5.Value.ToShortDateString());
        //        cyto = cyto.Replace("cyto.numb", this.textBoxNumIss.Text.Trim());
                
        //    }
        //    else if (this.comboBox6.Text.Trim() == "Осмотр")
        //    {
        //        wbC.Navigate(Application.StartupPath + @"\Templates\generic.htm");
        //        cyto = wbC.DocumentText;
        //        cyto = cyto.Replace("issl", strDiaDataName);
        //        cyto = cyto.Replace("patient", this.searchPatientBox1.patName);
        //        cyto = cyto.Replace("cyto.doc", DBExchange.Inst.UsrSign);
        //        cyto = cyto.Replace("cyto.text", this.richTextBox1.Text);
        //        cyto = cyto.Replace("cyto.date", this.dateTimePicker5.Value.ToShortDateString());
        //        cyto = cyto.Replace("Заключение № cyto.numb :", "");
        
        //        cyto = cyto.Replace("mat", this.comboBox1.Text);
        //    }
        //    else
        //    {
        //        wbC.Navigate(Application.StartupPath + @"\Templates\generic.htm");
        //        cyto = wbC.DocumentText;
        //        cyto = cyto.Replace("issl", strDiaDataName);
        //        cyto = cyto.Replace("patient", this.searchPatientBox1.patName);
        //        cyto = cyto.Replace("cyto.doc", DBExchange.Inst.UsrSign);
        //        cyto = cyto.Replace("cyto.text", this.richTextBox1.Text);
        //        cyto = cyto.Replace("cyto.date", this.dateTimePicker5.Value.ToShortDateString());
        //        cyto = cyto.Replace("cyto.numb", this.textBoxNumIss.Text.Trim());
        //        cyto = cyto.Replace("mat", this.comboBox1.Text);
        //    }
        //    return cyto;


        //}


        
        ///Запсывает данные обследования, р выборе combobox6 меняется строка записи в базу
        ///а в конце она выполняется
        //////Добавить выбор назначения при записи обследования, 
        //private void insDiagData()
        //{
        //    if (this.textBoxNumIss.Visible == true && this.textBoxNumIss.Text.Trim() == "Номер исслед")
        //    {
        //     warnMess.warnSetNumIss() ;
        //    }
        //    else
        //    {

        //        NpgsqlCommand insDiaData;
        //        if (this.comboBox6.Text.Trim() == "КТ")
        //        {
        //            insDiaData = new NpgsqlCommand("insert into " + writeDiagDataTable() + " (doc_out, pat_id, nomer, descr, header, contrast, spiral, step ) values ('" + DBExchange.Inst.dbUsrId + "','"
        //            + this.searchPatientBox1.pIdN + "','" + this.textBoxNumIss.Text + "','" + this.richTextBox1.Text.Trim() + "','"
        //            + this.textBox2.Text + "', '" + this.checkBox5.Checked + "', '" + this.checkBox6.Checked + "','" + Convert.ToSingle(this.textBox3.Text.Trim()) + "') ;"
        //            , DBExchange.Inst.connectDb);
        //        }
        //        else if (this.comboBox6.Text.Trim() == "Осмотр")
        //        {
        //            insDiaData = new NpgsqlCommand("insert into " + writeDiagDataTable() + " (doc_out, pat_id, descr, header ) values ('" + DBExchange.Inst.dbUsrId + "','"
        //            + this.searchPatientBox1.pIdN + "','" + this.richTextBox1.Text.Trim() + "','"
        //            + this.textBox2.Text + "') ;"
        //            , DBExchange.Inst.connectDb);
        //        }
        //        else
        //        {
        //            insDiaData = new NpgsqlCommand("insert into " + writeDiagDataTable() + " (doc_out, pat_id, nomer, descr, header ) values ('" + DBExchange.Inst.dbUsrId + "','"
        //            + this.searchPatientBox1.pIdN + "','" + this.textBoxNumIss.Text + "','" + this.richTextBox1.Text.Trim() + "','" + this.textBox2.Text + "') ;", DBExchange.Inst.connectDb);
        //        }
        //        try
        //        {
        //            insDiaData.ExecuteNonQuery();

        //        }
        //        catch (Exception exception)
        //        {
        //             Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
        //        }
        //        finally { }
        //        fillDiaDataLists();
        //    }
        //}
        
        /// <summary>
        // выдает имя таблцы по выбранной форме и заполняет переменные есл таковые имеются
        /// </summary>
        /// <returns></returns>
        //private string writeDiagDataTable()
        //{
        //    string writeTable;
        //    if (this.comboBox6.Text.Trim() == "КТ")
        //    {
        //       writeTable = "diag_data_kt";
        //       strDiaDataName = "Компьютерная томография";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "МРТ")
        //    {
        //         writeTable = "diag_data_mrt";
        //         strDiaDataName = "Магнитно-резонансная томография";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "ПЭТ")
        //    {
        //        writeTable = "diag_data_pet";
        //        strDiaDataName = "Позитронно-эмиссионная томография";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "РГ")
        //    {
        //        writeTable = "diag_data_rg";
        //        strDiaDataName = "Рентгенография";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "СГ")
        //    {
        //        writeTable = "diag_data_scint";
        //        strDiaDataName = "Сцинтиография";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "УЗИ")
        //    {
        //        writeTable = "diag_data_uzi";
        //        strDiaDataName = "Ультразвуковое исследование";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "ЭКГ")
        //    {
        //        writeTable = "diag_data_ekg";
        //        strDiaDataName = "Электрокардиограмма";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "ЭЭФ")
        //    {
        //        writeTable = "diag_data_eeg";
        //        strDiaDataName = "Электрическая энцефалография";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "ФВД")
        //    {
        //        writeTable = "diag_data_fvd";
        //        strDiaDataName = "Функция внешнего дыхания";
        //    }
        //    else if ((string)comboBox6.SelectedItem == "Осмотр")
        //    {
        //        writeTable = "diag_data_survey";
        //        strDiaDataName = "Амбулаторный осмотр врача";
        //    }
        //    else
        //    { 
        //        writeTable = "не укзано"; 
        //    }
        //    return writeTable;
        //}


        //скрыть поля ввода обследований
        //private void hideDiaData()
        //{
        //    if (this.button3.Visible == true)
        //    {
        //        this.comboBox6.Visible = false;
        //        this.textBoxNumIss.Visible = false;
        //        this.textBox2.Visible = false;
        //        this.dateTimePicker5.Visible = false;
        //        this.comboBox12.Visible = false;
        //        this.button3.Visible = false;
        //        this.button8.Visible = false;
        //        this.richTextBox1.Visible = false;
        //        if ((string)this.comboBox6.SelectedItem == "КТ")
        //        {
        //            this.checkBox5.Visible = false;
        //            this.checkBox6.Visible = false;
        //            this.textBox3.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        this.richTextBox1.Visible = true;
        //        this.comboBox6.Visible = true;
        //        this.textBoxNumIss.Visible = true;
        //        this.textBox2.Visible = true;
        //        this.dateTimePicker5.Visible = true;
        //        this.comboBox12.Visible = true;
        //        this.button3.Visible = true;
        //        this.button8.Visible = true;
        //        if ((string)this.comboBox6.SelectedItem == "КТ")
        //        {
        //            this.checkBox5.Visible = true;
        //            this.checkBox6.Visible = true;
        //            this.textBox3.Visible = true;
        //        }
        //    }
        //}




        //private void textBoxNumIss_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (this.textBoxNumIss.Text == "Номер исслед")
        //    {
        //        this.textBoxNumIss.Clear();
        //    }
        //}

        private void свернутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.groupBox23.Visible = false;
        }

        private void развернутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.groupBox23.Visible = true;
        }

        private void скрытьМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideLeftPanel();
        }


        /// <summary>
        /// скрыть левую панель
        /// </summary>
        private void hideLeftPanel()
        {
            if (this.groupBoxLeft.Visible == true)
            {
                this.groupBoxLeft.Visible = false;
                this.скрытьМенюToolStripMenuItem.Text = "Показать меню";
            }
            else
            {
                this.скрытьМенюToolStripMenuItem.Text = "Скрыть меню";
                this.groupBoxLeft.Visible = true;
            }

        }

        //private void textBoxNumIss_MouseClick(object sender, EventArgs e)
        //{
        //    if (this.textBoxNumIss.Text == "Номер исслед")
        //    {
        //        this.textBoxNumIss.Clear();
        //    }
        //}

        //private void button9_Click(object sender, EventArgs e)
        //{
        //    hideDiaData();
        //}

        //private void button8_Click(object sender, EventArgs e)
        //{
        //    viewDiagData();
        //}

        //private void textBox3_Enter(object sender, EventArgs e)
        //{
        //    if (this.textBox3.Text == "Шаг, мм")
        //    {
        //        this.textBox3.Clear();
        //    }
        //}

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            workAddDictButton();
        }


        //работа кнопки добавить в справочнике
       private void workAddDictButton()
      {
          if (this.tabControl2.SelectedTab == tabPage14)
          {//препараты
              FormDrugAdd addDrug = new FormDrugAdd();
              addDrug.ShowDialog();
          }
          else if (this.tabControl2.SelectedTab == tabPage12)
          {//Диагнозы

          }
          else if (this.tabControl2.SelectedTab == tabPage36)
          {//Схемы
              addNewSchemeForm addScheme = new addNewSchemeForm();
              addScheme.ShowDialog();
          }
           
      }

       private void jnxToolStripMenuItem_Click(object sender, EventArgs e)
       {
           prntData prnD = new prntData(_gotPatientId());
           prnD.ShowDialog();
       }


      ///
      // Работа во вкладке пациенты - лечение - этапы, при выборе вкладки обновлять данные в листбоксах
      ///


      //Записывает новый этап для пациента

        //если дата конца меньше начала, то сделать предупреждение, иначе при выборе даты установить длительность или при выборе длительности установить дату, если дата конца не менялась
       //private void writePatientNewEtap()
       //{
       //    NpgsqlCommand insEtapData = new NpgsqlCommand("insert into treatment_step (doc_id, pat_id, line_start, line_stop, date_effect, date_e_stop, eff_lost, eff_short_comment, effect_id, comment, line_name, current) values ('"
       //       + DBExchange.Inst.dbUsrId + "','" + this.searchPatientBox1.pIdN + "','" + this.dateTimePicker10.Value.ToShortDateString() + "','" + this.dateTimePicker11.Value.ToShortDateString()
       //       + "','" + this.dateTimePicker12.Value.ToShortDateString() + "','" + this.dateTimePicker13.Value.ToShortDateString() + "','" + this.checkBox8.Checked.ToString() + "','"
       //       + this.textBox10.Text + "','" + this.comboBox26.SelectedIndex + "','" + this.richTextBox3.Text.Trim() + "','" + this.textBox8.Text + "','True');", DBExchange.Inst.connectDb);

       //    try
       //    {
       //        insEtapData.ExecuteNonQuery();

       //    }
       //    catch (Exception exception)
       //    {
       //        System.Windows.Forms.MessageBox.Show(exception.Message.ToString() + " writePatientNewEtap");
       //    }

       //}

       private void button15_Click(object sender, EventArgs e)
       {
           if (this.searchPatientBox1.patientSet == true)
           {
              //writePatientNewEtap();
              refreshSurgeryTab();
           }
           else { warnMess.warnChoosePatient(); }
          
       }
/// <summary>
/// обновление вкладки хирургического лечения
/// </summary>

       private void refreshSurgeryTab()
       {
           if (this.tabControlSurgery.SelectedTab == tabPage10)//Лекарственное лечение
           {
               loadDrugTreatmentTabData();
  
           }
           else if (this.tabControlSurgery.SelectedTab == tabPage37) //Этапы
           {
               loadTreatmStepList();
           }

           else if (this.tabControlSurgery.SelectedTab == tabPage38 || this.tabControlSurgery.SelectedTab == tabPage22)//Схемы
           {
               List<string> SchemeData = new addNewSchemeForm().fillSchemeList();
               this.comboBox22.DataSource = SchemeData;
               this.comboBox32.DataSource = SchemeData;
           }


           else if (this.tabControlSurgery.SelectedTab == tabPage22)
           {
               fillDiaDataLists();
               List<string> pDiags = fillPatientKartDiagList(0);
               //this.comboBox3.DataSource = pDiags;
               //this.comboBox24.DataSource = pDiags;
               //this.comboBox18.DataSource = pDiags;
               this.comboBox21.DataSource = pDiags;
               List<string> SurgeryE = _tabSurgeryEnv.fillListSurgeryEtap(this.searchPatientBox1.pIdN);
               this.listBoxSurgeryEtap.DataSource = SurgeryE;
               //this.comboBox27.DataSource = SurgeryE;
               //this.comboBox28.DataSource = SurgeryE;
               //this.comboBox23.DataSource = SurgeryE;
               //this.comboBox19.DataSource = SurgeryE;


               //this.comboBox2.DataSource = SchemeData;

           }
       }

       private void loadDrugTreatmentTabData()
       {
           listBox14.Items.Clear();
           patientDrugTreatmentList = new Processing.DrugsClass.PatientDrugTreatment ();
           patientDrugTreatmentList.getPatientDrugs(_gotPatientId());

           foreach (Processing.DrugsClass.DrugTreatment i in patientDrugTreatmentList)
           {
               listBox14.Items.Add(i.WrittenDate +" " + i.drugName + " " +i.drugDose +" "+i.drugDoseName+" "+i.doctorName);

           }

       }

       private void loadTreatmStepList()
       {
           listBoxSurgeryEtap.Items.Clear();
           Processing.Treatment.TreatmentStepsList tsList = new Processing.Treatment.TreatmentStepsList();
           tsList.getTreatmentStepByPatientId(_gotPatientId());
           foreach (Processing.Treatment.TreatmentStep ts in tsList)
           {
               this.listBoxSurgeryEtap.Items.Add(ts.LineName);
           }
           listBoxSurgeryEtap.Refresh();
       }

       private void tabPage36_Enter(object sender, EventArgs e)
       {
          
       }

//Лучевое
       private void listBox18_DoubleClick(object sender, EventArgs e)
       {
           getDiagDataFromList();
           this.richTextBox1Radio.Text = (string)tlDiagDataDt.Rows[0]["descr"];
           this.richTextBox1Radio.Invalidate();

       }
        /// <summary>
        /// Функциональное
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void listBox19_DoubleClick(object sender, EventArgs e)
       {
           getDiagDataFromList();
           this.richTextBoxFunc.Text = (string)tlDiagDataDt.Rows[0]["descr"];
           this.richTextBoxFunc.Invalidate();
       }
        //Лабораторное
       private void listBox20_DoubleClick(object sender, EventArgs e)
       {
           //getDiagDataFromList();
           //this.richTextBoxLab.Text = (string)tlDiagDataDt.Rows[0]["descr"];
           //this.richTextBoxLab.Invalidate();

           changeAnalysisInList();

       }

       private void changeAnalysisInList()
       {
           Int64 _ticketId = ptList[listBox20.SelectedIndex].TicketId;
           listBox25.Items.Clear();

           Processing.LabTickets.PatientAnalysisList pal = new baza.Processing.LabTickets.PatientAnalysisList();
           if (ptList[listBox20.SelectedIndex].TicketLabSampleId == -999)
           {

               pal.GetTicketAnalysis(_ticketId);
               foreach (Processing.LabTickets.PatientAnalysis i in pal)
               {
                   string boolval = "";
                   if (i.AValueBool == true)
                   {
                       boolval = " Положительно ";
                   }
                   else if (i.AValueBool == false)
                   {
                       boolval =  " Отрицательно " ;
                   }

                   listBox25.Items.Add(i.ADate.ToShortDateString() + " " + i.AName + " Значение: " + i.AValue.ToString() + " " + i.AValueText + boolval + " Материал: " + i.ASampleName);
               }
           }
           else
           {
               pal.GetImportdTicketAnalysis(_ticketId);
               foreach (Processing.LabTickets.PatientAnalysis i in pal)
               {
                   listBox25.Items.Add(i.ADate.ToShortDateString() + " " + i.AName + " Значение: " + i.AValue.ToString() + " Материал: " + i.ASampleName);
               }
           }

       }


        //морфология
       private void listBox21_DoubleClick(object sender, EventArgs e)
       {
           getDiagDataFromList();
           this.richTextBoxMorph.Text = (string)tlDiagDataDt.Rows[0]["descr"];
           this.richTextBoxMorph.Invalidate();
       }
        //осмотры
       private void listBox23_DoubleClick(object sender, EventArgs e)
       {
           getDiagDataFromList();
           this.richTextBoxOsm.Text = (string)tlDiagDataDt.Rows[0]["descr"];
           this.richTextBoxOsm.Invalidate();
       }

       private void данныеПациентаToolStripMenuItem_Click(object sender, EventArgs e)
       {
           changePatientKartData();
       }

       private void button1_Click(object sender, EventArgs e)
       {           
            remDiagFromPatDiags();
       }

       private void button2_Click(object sender, EventArgs e)
       {
        
           Forms.FormSetDiagnosis fSetDiag = new Forms.FormSetDiagnosis(_gotPatientId(), false, 0);
           fSetDiag.ShowDialog();
           refreshDiagList();

       }

       private void другиеУслугиToolStripMenuItem_Click(object sender, EventArgs e)
       {
            
            Forms.FormTicket ft = new baza.Forms.FormTicket(_gotPatientId());
            ft.ShowDialog();
           

       }

       private void диагнозToolStripMenuItem_Click(object sender, EventArgs e)
       {

           Forms.FormSetDiagnosis fSetDiag = new Forms.FormSetDiagnosis(_gotPatientId(), false, 0);
           fSetDiag.ShowDialog();
           refreshDiagList();

       }

       private void tabControl5_SelectedIndexChanged(object sender, EventArgs e)
       {
           if (this.searchPatientBox1.patientSet == true)
           {
               fillDiaDataLists();
           }
       }

       private void хирургическоеToolStripMenuItem_Click(object sender, EventArgs e)
       {
        
           Forms.FormSurgeryTreatment fst = new Forms.FormSurgeryTreatment(_gotPatientId());
           fst.ShowDialog();
           
           refreshSurgeryTab();
       }

       private void лабораториюToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormAddNewLab fanl = new Forms.FormAddNewLab(1,0,0);
           fanl.ShowDialog();
       }

       private void показательToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormAddNewNormLimit fannl = new baza.Forms.FormAddNewNormLimit();
           fannl.ShowDialog();
       }

       private void шаблонАнализовToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormNewLabTemplate fnlt = new baza.Forms.FormNewLabTemplate(0);
           fnlt.ShowDialog();
       }

       private void записьОбразцаДляИсследованияToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormLabDataSample flds = new baza.Forms.FormLabDataSample(0,0,false);
           flds.ShowDialog();
       }

       private void вводДанныхАнализаToolStripMenuItem_Click(object sender, EventArgs e)
       {
           int _gp = 0;
          if (_gotPatientId() > 0)
          {
              _gp = _gotPatientId();
          }
           Forms.FormAddAnalysis faa = new baza.Forms.FormAddAnalysis(0,_gp,0);
           faa.ShowDialog();
       }

       private void обследованиеToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormSetNewTicketRadio fsnt = new baza.Forms.FormSetNewTicketRadio(_gotPatientId());
           fsnt.ShowDialog();
           refreshStartupData();
       }

       private void лабораторияToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Screen.ShowLabTicketScreen slts = new baza.Screen.ShowLabTicketScreen();
           slts.ShowDialog();
       }

       private void listBox10_SelectedIndexChanged(object sender, EventArgs e)
       {
           Int64 _thisTicket = myPatientTicketList[listBox10.SelectedIndex].TicketId;
           getMyPatientTicketAnalysis(_thisTicket);
       }

       private void getMyPatientTicketAnalysis(Int64 _ticket)
       {
           listBox24.Items.Clear();

           Processing.LabTickets.PatientAnalysisList pal = new baza.Processing.LabTickets.PatientAnalysisList();
           pal.GetTicketAnalysis(_ticket);
           foreach (Processing.LabTickets.PatientAnalysis i in pal)
           {
               listBox24.Items.Add(i.ADate.ToShortDateString() + " " + i.AName + " " + i.AValue.ToString() + " " + i.ASample.ToString());
           }


       }

       private void getLabsList()
       {
           
           if (this.lab.Count <= 0)
                    
           {
               lab = new baza.Document.SampleType.LabList();
               lab.LabListGet();
           }
           comboBoxPatCartLab.Items.Clear();
           foreach (Document.SampleType.LabItem i in lab)
           {
               this.comboBoxPatCartLab.Items.Add(i.SampleName);
           }

       }

       private void getAnalysisTypes()
       {
           if (this.samTy.Count <= 0)

           {
               samTy = new baza.Document.SampleType.SList();
               samTy.SampleListGet();
           }
           comboBoxPatCartAnType.Items.Clear();

           foreach (Document.SampleType.SampleItem i in samTy)
           {
               this.comboBoxPatCartAnType.Items.Add(i.SampleName);
           }

       }


       private void updatePatientTicketList(int _patId)
       {
           listBox20.Items.Clear();
           ptList = new baza.Processing.LabTickets.PatientTicketList();
           ptList.GetPatientTicketsFinished(_patId, dateTimePicker2.Value, dateTimePicker1.Value);
           listBox20.Items.Clear();
           foreach (Processing.LabTickets.PatientTicket i in ptList)
           {
               listBox20.Items.Add(i.TicketDateOut + " " + i.TicketDoctorFName + " " + i.TicketName);
           }
       }

       private void fillPatientlabList(int _patId)
       {
           if (comboBoxPatCartLab.Items.Count != lab.Count || lab.Count <= 0)
           {               
               getLabsList();
           }
           if (comboBoxPatCartAnType.Items.Count != samTy.Count || samTy.Count <= 0)
           {
               getAnalysisTypes();
           }

           if (ptList != null && ptList.Count > 0)
           {
               if ( ptList[0].TicketPatientId != _patId)
               {
                   updatePatientTicketList(_patId);
               }
           }
           else
           {
               updatePatientTicketList(_patId);
           }

       }

       private void шаблонДляЛучевыхИсследованийToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Editor.FormNewTemplateEditor fnte = new baza.Editor.FormNewTemplateEditor() ;
           fnte.ShowDialog();
       }

       private void анализКровиToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormSetNewTicket fsnt = new baza.Forms.FormSetNewTicket(_gotPatientId());
           fsnt.ShowDialog();
       }

       private void эЛИсследованияToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Screen.ShowRadioTicketScreen srts = new baza.Screen.ShowRadioTicketScreen();
           srts.ShowDialog();
       }

       private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
       {
           refreshStartupData();
       }

       private void сменаПароляToolStripMenuItem_Click(object sender, EventArgs e)
       {
           FormChangePassword fcp = new FormChangePassword();
           fcp.ShowDialog();
       }

       private void новыйПоказательToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Editor.FormNormEditor fne = new baza.Editor.FormNormEditor();
           fne.ShowDialog();
       }

       private void описаниеОчаговToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormTumorDescr ftd = new Forms.FormTumorDescr();
           ftd.ShowDialog();
       }

       private void деталиРаспространенияToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormTumorDetails ftds = new Forms.FormTumorDetails();
           ftds.ShowDialog();
       }

       private void осмотрToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormSurvey fsu = new Forms.FormSurvey();
           fsu.ShowDialog();
       }

       private void лекарственноеToolStripMenuItem_Click(object sender, EventArgs e)
       {
       
           Forms.FormDrugTreatment fdt = new Forms.FormDrugTreatment(_gotPatientId());
           fdt.ShowDialog();
       }

       private void лучевоеToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormRadioTreatment frt = new Forms.FormRadioTreatment(_gotPatientId());
           frt.ShowDialog();
       }

       private void toolStripMenuItem1_Click(object sender, EventArgs e)
       {
           if (this.tabControl3.SelectedTab == tabPagePatientKartDiagnosis)
           {
             
               Forms.FormSetDiagnosis fSetDiag = new Forms.FormSetDiagnosis(_gotPatientId(), false, 0);
               fSetDiag.ShowDialog();
               refreshDiagList();
           }
       }

       private void последниеToolStripMenuItem_Click(object sender, EventArgs e)
       {
           this.listBoxDiagnosis.DataSource = fillPatientKartDiagList(0);
       }

       private void основныеToolStripMenuItem_Click(object sender, EventArgs e)
       {
           this.listBoxDiagnosis.DataSource = fillPatientKartDiagList(1);
       }

       private void всеToolStripMenuItem_Click(object sender, EventArgs e)
       {
           this.listBoxDiagnosis.DataSource = fillPatientKartDiagList(2);
       }

       private void удаленныеToolStripMenuItem_Click(object sender, EventArgs e)
       {
           this.listBoxDiagnosis.DataSource = fillPatientKartDiagList(3);
       }

       private void listBoxDiagnosis_SelectedIndexChanged(object sender, EventArgs e)
       {
           if (tabControl3.SelectedTab == tabPagePatientKartDiagnosis)
           {
               getDiag();
           }
       }

       private void getDiag()
       {
           if (this.listBoxDiagnosis.SelectedIndex >= 0 && tPatientKartDiags.Rows.Count > 0)
           {
               int _gotDiag = (int)tPatientKartDiags.Rows[this.listBoxDiagnosis.SelectedIndex]["serial"];
               richTextBox1.Text = getThisDiagBody(_gotDiag);
           }
       }

       private string getThisDiagBody(int _diagSerial)
       {
           string _body = "";
           NpgsqlCommand getDiag = new NpgsqlCommand("Select comment from diag_data where serial = '"+_diagSerial+"'", DBExchange.Inst.connectDb);
           try
           {
               _body = getDiag.ExecuteScalar().ToString();
           }
           catch (Exception exception)
           {
               Warnings.WarnLog log = new Warnings.WarnLog();
               log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
           }
           return _body;
       }

       private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
       {
           if (tabControl3.SelectedTab == tabPagePatientKartDiagnosis)
           {
               int _gotDiag = (int)tPatientKartDiags.Rows[this.listBoxDiagnosis.SelectedIndex]["serial"];
               editDiagnosis(_gotDiag);
           }
       }

       private void editDiagnosis(int _thisDiag)
       {
           Forms.FormSetDiagnosis fSetDiag = new Forms.FormSetDiagnosis(_gotPatientId(), true, _thisDiag);
           fSetDiag.ShowDialog();
           refreshDiagList();
       }

       private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
       {
           int thisPatId = this.searchPatientBox1.pIdN;
           if (thisPatId > 0)
           {
               updatePatientTicketList(thisPatId);
           }

       }

       private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
       {
           int thisPatId = this.searchPatientBox1.pIdN;
           if (thisPatId > 0)
           {
               updatePatientTicketList(thisPatId);
           }
       }

       private void comboBoxPatCartLab_SelectedIndexChanged(object sender, EventArgs e)
       {
           int thisPatId = this.searchPatientBox1.pIdN;
           if (thisPatId > 0)
           {
               updatePatientTicketList(thisPatId);
           }
       }

       private void осложнениеToolStripMenuItem_Click(object sender, EventArgs e)
       {
          

           Forms.FormAEoccured fae = new Forms.FormAEoccured(_gotPatientId(), 0);
           fae.ShowDialog();
       }

       private void tabControlSurgery_SelectedIndexChanged(object sender, EventArgs e)
       {
           refreshSurgeryTab();
       }

       private void выпискаToolStripMenuItem_Click(object sender, EventArgs e)
       {

           Forms.FormVypiska fv = new Forms.FormVypiska(_gotPatientId());
           fv.ShowDialog();
       }

       private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
       {
           if (tabControl3.SelectedTab == tabPagePatientKartDiagnosis)
           {
               int _gotDiag = (int)tPatientKartDiags.Rows[this.listBoxDiagnosis.SelectedIndex]["serial"];
               editDiagnosis(_gotDiag);
           }
       }

       private void печатьДокументовToolStripMenuItem_Click(object sender, EventArgs e)
       {
           FormPrintDocuments fpd = new FormPrintDocuments(_gotPatientId());
           fpd.ShowDialog();
       }

       private void поПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
       {
          // WebBrowser wb = new WebBrowser();
           
           //wb.Url = http://medx.spb.ru/;
          // wb.Navigate("http://medx.spb.ru/");
           Process.Start("http://medx.spb.ru/");
       }

       private void открытьФормуToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Forms.FormTreatmentBase fs = new Forms.FormTreatmentBase(_gotPatientId());
           fs.ShowDialog();
       }


    }
}
