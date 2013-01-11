using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;


namespace baza
{
    public partial class PatientKart : Form
    {
        public List<Int64> patDocuments;
        public List<int> docNumList = new List<int>();
        List<string> docbuffer = new List<string>();
        List<string> bufferUsrPat = new List<string>();
        List<int> listNumUsrPatient = new List<int>();
        List<int> diagNumList = new List<int>();
        private List<string> listPatientDiags = new List<string>();
        private DataTable tPatientDiags;
        private DataTable tPatientUsr;
        string currentDoctor;

        NpgsqlConnection cdbopk = DBExchange.Inst.connectDb;

        public PatientKart()
        {
            InitializeComponent();
            searchPatientBox1.setPatientSelected += new SearchPatientBox.setNewPatientSelected(this.fillPatientDocuments);
           
        }

        private void PatientKartTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_BottomToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public void nowTryToSearchatId(int isThisPatId)
        {
            searchPatientBox1.searchById(isThisPatId);
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatientKart.ActiveForm.Close();
        }

        public void getPatientKartData(object sender, SearchPatientBox e)
        {
            MessageBox.Show("Пользователь создан", "Спасибо за регистрацию",
     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }



        //ззаполняет список документов пациента

        private void fillPatientDocuments(int _selectedPatientId)
        {
            List<string> buffer = new List<string>();
            patDocuments = new List<Int64>();
            NpgsqlConnection cdbo = DBExchange.Inst.connectDb;
            try
            {

                NpgsqlCommand selectPatientDocuments = new NpgsqlCommand("Select (Select descr from docum_type where docum_type.did = documents.document_type) as document_type,document_date,"
                + "(Select family_name from doctors where doctors.doc_id = documents.doc_id) as doc_id,document_header,document_number from documents where pat_id ='" + _selectedPatientId + "' AND delete='0' ORDER BY document_date DESC LIMIT 20", cdbo);
                NpgsqlDataReader readDocuments = selectPatientDocuments.ExecuteReader();

                int colNum = readDocuments.FieldCount;

                while (readDocuments.Read())
                {
                    string document = Convert.ToDateTime(readDocuments[1]).ToShortDateString() + " " + (readDocuments[0].ToString()) + " " + (readDocuments[2].ToString()) + " " + readDocuments[3];
                    buffer.Add(document);
                    patDocuments.Add(Convert.ToInt64(readDocuments["document_number"]));

                }
                this.listBox6.DataSource = buffer;
                readDocuments.Close();
                NpgsqlCommand selDoct = new NpgsqlCommand("Select trim(family_name), substr(first_name,1,1), substr(last_name,1,1) from doctors where doc_id = " + this.searchPatientBox1.currDoc + ";", cdbo);
                NpgsqlDataReader selectDoctor = selDoct.ExecuteReader();
                while (selectDoctor.Read())
                {
                    currentDoctor = selectDoctor[0] + " " + selectDoctor[1] + ". " + selectDoctor[2] + ".";
                }
                selectDoctor.Close();
                fillPatientDiagList();
            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }

            finally
            {

                this.label35.Text = currentDoctor;
                this.label35.Invalidate();
                loadDoctorsOfPatient();
                this.tabPage8.Refresh();
                this.tabControl1.Refresh();
                
            }
            


        }



        //загружает список всех докторов

        private void loadDoctors()
        {



            NpgsqlCommand selectDoc = new NpgsqlCommand("select trim(family_name), trim(first_name), trim(last_name), doc_id from doctors ", cdbopk);
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
            +" from user_data where pat_id = " + this.searchPatientBox1.pIdN + " and approve = true", cdbopk);
            
            
            
            bufferUsrPat = new List<string>();
            listNumUsrPatient = new List<int>();

            this.listBox4.ResetText();
            try
            {
                selectUsrPatData.Fill(tPatientUsr);

                //NpgsqlDataReader readData = selectUsrPatData.ExecuteReader();
              //  int colNum = readData.FieldCount;


                foreach (DataRow readData in tPatientUsr.Rows )
                {
                    string document = (string)readData[0] + " " + (string)readData[1]+".";
                    bufferUsrPat.Add(document);
                    listNumUsrPatient.Add(Convert.ToInt32(readData[2]));

                }
                this.listBox4.DataSource = bufferUsrPat;



            }

            finally
            {
                this.listBox4.Refresh();
            }


        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabPage8)
            {
                loadDoctors();
                if (this.searchPatientBox1.patientSet == true)
                {
                    loadDoctorsOfPatient();
                }
            }
        }


        //установить лечащего врача
        private void setCurrDoctor()
        {

            NpgsqlCommand setCuDo = new NpgsqlCommand("update patient_list set curr_doctor = " + docNumList[this.comboBox10.SelectedIndex] + " where pat_id = " + this.searchPatientBox1.pIdN + " ;", cdbopk);
            try
            {
                setCuDo.ExecuteNonQuery();

            }
            catch { }
            finally { }

        }




        //записать пациента врачу
        private void insPatUsrInList()
        {
            if (listNumUsrPatient.Contains(docNumList[this.comboBox10.SelectedIndex]))
            { }
            else
            {

                NpgsqlCommand insPUiL = new NpgsqlCommand("insert into user_data (usr_id, pat_id, create_id) values ('" + docNumList[this.comboBox10.SelectedIndex] + "','" + this.searchPatientBox1.pIdN + "','"+DBExchange.Inst.dbUsrId+"') ;", cdbopk);
                try
                {
                    insPUiL.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
                finally { }
            }
        }


        //убрать врача из списка
        private void remUsrFromPatList()
        {
            if (listNumUsrPatient.Contains(docNumList[this.comboBox10.SelectedIndex]))
            {
                NpgsqlCommand remPUiL = new NpgsqlCommand("update user_data set approve = false, mod_id = "+DBExchange.Inst.dbUsrId+", mod_date ='now()' where serial = '" + this.tPatientUsr.Rows[this.listBox4.SelectedIndex]["serial"] + "' ;", cdbopk);
                try
                {
                    remPUiL.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
                finally { }

            }
            else
            {


            }
            loadDoctorsOfPatient();
        }




        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectDiagData();
        }


        /// <summary>
        /// получает список диагнозов, указанных в группе
        /// </summary>

        private void selectDiagData()
        {
            NpgsqlCommand selDiDa = new NpgsqlCommand("select diag, trim(name) from diags where ds like '" + comboBoxDiagGroup.Text + "%' ", cdbopk);
            diagNumList = new List<int>();
            List<string> listDiagDescr = new List<string>();
            this.comboBoxDiagDescr.ResetText();
            try
            {
                NpgsqlDataReader readData = selDiDa.ExecuteReader();
                int colNum = readData.FieldCount;

                while (readData.Read())
                {
                    string document = (string)readData[1];
                    listDiagDescr.Add(document);
                    diagNumList.Add((int)readData[0]);

                }

            }
            catch { }
            finally { }

            this.comboBoxDiagDescr.DataSource = listDiagDescr;

        }

        /// <summary>
        /// Заполняет список диагнозов пациента
        /// </summary>
        private void fillPatientDiagList()
        {
            tPatientDiags = new DataTable();
            NpgsqlDataAdapter selPatDiags = new NpgsqlDataAdapter("Select diag_date, (select name from diags where diag = diag_data.diag),diag, "
            +"status, location, trim(comment), usr_id from diag_data where pat_id = " + this.searchPatientBox1.pIdN + " order by diag_date Asc ", cdbopk);
            listPatientDiags = new List<string>();

            try
            {
                selPatDiags.Fill(tPatientDiags);


                foreach (DataRow row in tPatientDiags.Rows)
                {
                    listPatientDiags.Add(((string)row[1]) + " " + ((DateTime)row[0]));
                }

            }
            finally
                {
                    this.listBox5.DataSource = listPatientDiags;
                    this.listBox5.Invalidate();
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
                MessageBox.Show("Выберите пациента", "Не выбран пациент",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AddToListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.searchPatientBox1.patientSet == true)
            {

                insPatUsrInList();
                loadDoctorsOfPatient();
            }
            else
            {
                MessageBox.Show("Выберите пациента", "Не выбран пациент",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void RemFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            remUsrFromPatList();
        }

    }
}
