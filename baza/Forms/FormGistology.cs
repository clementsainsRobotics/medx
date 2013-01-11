using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using System.IO;

namespace baza
{
    public partial class FormGistology : Form
    {
        
        public int gisNum;
        private bool documWritten;
        public int docNumb;
        public List<int> docIdList = new List<int>();
        private bool patSel;
        private UserClass.Doctors.IdentityList DocIdentList;

        public FormGistology(int patID)
        {
            InitializeComponent();
            patSel = false;
            searchPatientBox2.gotPatientDoc += new SearchPatientBox.getPatientDocHandler(this.ThrowMessageBoxOnPatientChange);
            if (patID != -1)
            {
              //  this.searchPatientBox1.searchById(patID);
                this.searchPatientBox2.searchById(patID);
            }

            
            documWritten = false;
            docNumb = DBExchange.Inst.dbUsrId;
            this.doc_in.Items.Add(DBExchange.Inst.UsrSign);
            GetGistolgyDoctors();
            docIdList.Add(docNumb);
            this.doc_in.SelectedIndex = 0;
            
            
        }

        private void GetGistolgyDoctors()
        {

            DocIdentList = new baza.UserClass.Doctors.IdentityList();
            DocIdentList.getIdentityListByGroupID(4);
            foreach (UserClass.Doctors.Identity i in DocIdentList)
            {
                this.comboBox6.Items.Add(i.DoctorFullName);
            }
            if (this.comboBox6.Items.Count > 0)
            {
                this.comboBox6.SelectedIndex = 0;
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {

            //  cytoInsertData(pat_id, Convert.ToDateTime(dateTimePicker1.Value), comboBox2.Text, Convert.ToInt32(nomerTextBox1.Text), zaklRichTextBox1.Text);
        }

        private void FormCytology_Load(object sender, EventArgs e)
        {
            loadVars();

        }

        private void loadVars()
        {
            gistLoadData();

            nomerTextBox1.Text = gisNum.ToString();
            textBox3.Text = gisNum.ToString();
            comboBox1.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        ///Загрузка данных в форму цитологии
        public void gistLoadData()
        {
            
            try
            {
                NpgsqlCommand selCytNumb = new NpgsqlCommand("SELECT MAX(nomer) FROM gistology;", DBExchange.Inst.connectDb);
                gisNum = (Int32)selCytNumb.ExecuteScalar();
                gisNum += 1;
             }
            catch
            {

            }
            finally
            {

            }

            try
            {
                NpgsqlDataAdapter doc = new NpgsqlDataAdapter("Select trim(Family_name), substr(first_name,1,1), substr(last_name,1,1), doc_id from doctors where otdel = '39' order by family_name ASC", 
                DBExchange.Inst.connectDb);
                DataTable docTable = new DataTable();
                doc.Fill(docTable);
                foreach (DataRow roww in docTable.Rows)
                {
                    doc_in.Items.Add((string)roww[0] + " " + (string)roww[1] + ". " + (string)roww[2] + ".");
                    comboBox6.Items.Add((string)roww[0] + " " + (string)roww[1] + ". " + (string)roww[2] + ".");
                    docIdList.Add((int)roww["doc_id"]);

                }

                NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select trim(name) from divisions order by division ASC", DBExchange.Inst.connectDb);
                DataTable divTable = new DataTable();
                div.Fill(divTable);
                foreach (DataRow roww in divTable.Rows)
                {
                    this.comboBox2.Items.Add((string)roww[0]);
                    this.comboBox5.Items.Add((string)roww[0]);
                }
                this.comboBox2.SelectedIndex = 11;
                this.comboBox5.SelectedIndex = 11;
            }
            catch { }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGistology.ActiveForm.Close();
        }



        //проверка номера исследования
        private void chkCN()
        {
            NpgsqlConnection cdbo = DBExchange.Inst.connectDb;
            int gistNumb;
            if (this.tabControl1.SelectedTab == tabPage2)
            {
                gistNumb = Convert.ToInt32(nomerTextBox1.Text.Trim());
            }
            else 
            {
                gistNumb = Convert.ToInt32(textBox3.Text.Trim());
            }
            
            if (gistNumb != gisNum)
            {
                int chCyNum;
                try
                {

                    ///Проверка номера исследования
                    NpgsqlCommand chkCytNumb = new NpgsqlCommand("SELECT nomer FROM diag_data_gistology WHERE nomer = '" + gistNumb + "' and to_char(adatetime, 'YYYY') = '" + DateTime.Now.Year + "';", cdbo);

                    chCyNum = (int)chkCytNumb.ExecuteScalar();
                    if (chCyNum == gistNumb)
                    {

                        System.Windows.Forms.MessageBox.Show("Исследование № " + gistNumb + " уже существует", "Выберите другой номер или проверьте последние записи",
                                         System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
                finally
                {

                }
            }
        }


        private void записатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (patSel == true)
            {
            if (documWritten == false)
            {
                insCytZ();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Исследование уже существует", "Запись также происходит при распечатывании",
                                 System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
                     }
            else
            { System.Windows.Forms.MessageBox.Show("Выберите пациента"); }
        }


        //формирует запрос в баззу данных
        private void insCytZ()
        {
            NpgsqlConnection cdbo = DBExchange.Inst.connectDb;

            int PatientId;
            DateTime aDate;
            DateTime iDate ;
            int nomer ;
            //int nomerP = Convert.ToInt32(this.textBox1.Text.Trim());
            string nomerP ;
            char material ;
            //int shifr1 = Convert.ToInt32(this.textBox2.Text.Trim());
            //string shifr2 = Convert.ToInt16(this.textBox3.Text.Trim());
            int otd ;
            string strDiagnos ;
            string previous ;

            string textZakl ;
            string header ;
            int ans_doc ;

            string macro_descr;
            string kass_num;

            try
            {
                if (this.tabControl1.SelectedTab == tabPage2)
                {
                    PatientId = this.searchPatientBox1.pIdN;
                    aDate = dateTimePicker1.Value;
                    iDate = dateTimePicker2.Value;
                   nomer = Convert.ToInt32(this.nomerTextBox1.Text.Trim());
                    //int nomerP = Convert.ToInt32(this.textBox1.Text.Trim());
                     nomerP = this.textBox1.Text.Trim();
                  material = this.comboBox1.Text[0];
                    //int shifr1 = Convert.ToInt32(this.textBox2.Text.Trim());
                    //string shifr2 = Convert.ToInt16(this.textBox3.Text.Trim());
                  otd = this.comboBox2.SelectedIndex;
                    strDiagnos = this.diagnos.Text.Trim();
                     previous = this.textBox2.Text.Trim();

                   textZakl = this.zaklRichTextBox1.Text.Trim();
                    header = this.textBox5.Text.Trim();
                    ans_doc = docIdList[this.doc_in.SelectedIndex];
                                    
                    macro_descr = richTextBox6.Text.Trim();
                    kass_num = richTextBox8.Text.Trim();
                }
                else
                {
                    PatientId = this.searchPatientBox2.pIdN;
                    aDate = dateTimePicker4.Value;
                    iDate = dateTimePicker3.Value;
                    nomer = Convert.ToInt32(this.textBox3.Text.Trim());
                    //int nomerP = Convert.ToInt32(this.textBox1.Text.Trim());
                    nomerP = this.textBox4.Text.Trim();
                    material = this.comboBox4.Text[0];
                    //int shifr1 = Convert.ToInt32(this.textBox2.Text.Trim());
                    //string shifr2 = Convert.ToInt16(this.textBox3.Text.Trim());
                    otd = this.comboBox5.SelectedIndex;
                    strDiagnos = this.richTextBox1.Text.Trim();
                    previous = this.richTextBox2.Text.Trim();

                    textZakl = this.richTextBox3.Text.Trim();
                    header = this.textBox6.Text.Trim();
                    ans_doc = DocIdentList[this.comboBox6.SelectedIndex].DoctorId;

                    macro_descr = richTextBox4.Text.Trim();
                    kass_num = richTextBox5.Text.Trim();
                }

                NpgsqlCommand insRow = new NpgsqlCommand("Insert into diag_data_gistology (zakluch, pat_id, ans_date, inc_date, nomer, preparat, material, header, otdel, previous, ans_doc, clinic_diag, macro_descr, kass_num, doc_in ) VALUES "
                    +" (:zakl ,'" + PatientId + "','" + aDate + "','" + iDate + "','" + nomer + "','" + nomerP + "','" + material + "', :head ,'" + otd + "','" + previous + "','" + ans_doc + "', :clin_diag , :macrod , :kass , '"+DBExchange.Inst.dbUsrId+"'  );", cdbo);
                using (insRow)
                {
                    insRow.Parameters.Add(new NpgsqlParameter("zakl", NpgsqlDbType.Text));
                    insRow.Parameters[0].Value = textZakl;
                    insRow.Parameters.Add(new NpgsqlParameter("head", NpgsqlDbType.Varchar, 100));
                    insRow.Parameters[1].Value = header;
                    insRow.Parameters.Add(new NpgsqlParameter("macrod", NpgsqlDbType.Text));
                    insRow.Parameters[2].Value = macro_descr;
                    insRow.Parameters.Add(new NpgsqlParameter("clin_diag", NpgsqlDbType.Text));
                    insRow.Parameters[3].Value = strDiagnos;
                    insRow.Parameters.Add(new NpgsqlParameter("kass", NpgsqlDbType.Text));
                    insRow.Parameters[4].Value = kass_num;
                }
                
                
                
                try
                {

                    insRow.ExecuteNonQuery();

                    documWritten = true;
                    clearForm();
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    
                }


                

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("В поле порядковый номер допустимы только цифры");
            }
        }

        private void nomerTextBox1_Leave(object sender, EventArgs e)
        {
            gisNum = Convert.ToInt32(this.nomerTextBox1.Text);
            chkCN();
        }


        //распечатка
        private void рапечататьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (patSel == true)
            {
                if (documWritten == false)
                {
                    insCytZ();
                }

                Form prFC = new printFormGist(setDataCy(), true, false);
            }
            else
            { System.Windows.Forms.MessageBox.Show("Выберите пациента"); }

        }



        //формирует тело html страницы для распечатки или просмотра
        private string setDataCy()
        {

            WebBrowser wbC = new WebBrowser();
            wbC.Navigate(Application.StartupPath + @"\Templates\Gistology.htm");
            string cyto = wbC.DocumentText;
            if (this.tabControl1.SelectedTab == tabPage2)
            {
                cyto = cyto.Replace("patient", this.searchPatientBox1.patName);
                cyto = cyto.Replace("cyto.doc", doc_in.Text);
                string brTxt = this.zaklRichTextBox1.Text.Replace("\n", "<br>");
                cyto = cyto.Replace("zakl", brTxt);
                cyto = cyto.Replace("cyto.date", dateTimePicker1.Value.ToShortDateString());
                cyto = cyto.Replace("nomer", this.nomerTextBox1.Text.Trim());
                cyto = cyto.Replace("mat", this.comboBox1.Text);
                cyto = cyto.Replace("previous", this.textBox2.Text);
                cyto = cyto.Replace("secNom", this.textBox1.Text.Trim());
                cyto = cyto.Replace("otdel", this.comboBox2.Text.Trim());
                cyto = cyto.Replace("kasset", this.richTextBox8.Text.Trim());
                cyto = cyto.Replace("macro", this.richTextBox6.Text.Trim());
                cyto = cyto.Replace("diagnos", this.diagnos.Text.Trim());
                cyto = cyto.Replace("coming", this.dateTimePicker2.Value.ToShortDateString());
            }
            else if (this.tabControl1.SelectedTab == tabPage1)
            {
                cyto = cyto.Replace("patient", this.searchPatientBox2.patName);
                cyto = cyto.Replace("cyto.doc", comboBox6.Text);
                cyto = cyto.Replace("zakl", this.richTextBox3.Text);
                cyto = cyto.Replace("cyto.date", dateTimePicker4.Value.ToShortDateString());
                cyto = cyto.Replace("nomer", this.textBox3.Text.Trim());
                cyto = cyto.Replace("mat", this.comboBox4.Text);
                cyto = cyto.Replace("previous", this.richTextBox2.Text);
                cyto = cyto.Replace("secNom", this.textBox4.Text.Trim());
                cyto = cyto.Replace("otdel", this.comboBox5.Text.Trim());
                cyto = cyto.Replace("kasset", this.richTextBox5.Text.Trim());
                cyto = cyto.Replace("macro", this.richTextBox4.Text.Trim());
                cyto = cyto.Replace("diagnos", this.richTextBox1.Text.Trim());
                cyto = cyto.Replace("coming", this.dateTimePicker3.Value.ToShortDateString());
            }


            return cyto;


        }

        //просмотр выписки
        private void просмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form prFC = new printFormGist(setDataCy(), false, false);
            prFC.Show();
        }

        private void clearForm()
        {
            this.zaklRichTextBox1.Clear();
            this.searchPatientBox1.ResetText();
            documWritten = false;
            loadVars();

        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearForm();
       }

        private void ThrowMessageBoxOnPatientChange(int _docId, string _docName)
        {

            patSel = true;
                if (_docId != docNumb)
                {
                    if (docIdList.Contains(_docId))
                    { }
                    else
                    {   this.doc_in.Items.Add(_docName);
                        this.doc_in.SelectedItem = _docName;
                        docNumb = _docId;
                        docIdList.Add(_docId);
                    }


                }
               

            }

        private void doc_in_SelectionChangeCommitted(object sender, EventArgs e)
        {
            docNumb = docIdList[this.doc_in.SelectedIndex];
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void zaklRichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchPatientBox1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text.Trim() == "Клинический диагноз")
            { this.richTextBox1.Clear(); }
        }

        private void richTextBox2_Enter(object sender, EventArgs e)
        {
            if (this.richTextBox2.Text.Trim() == "Предыдущее исследование")
            { this.richTextBox2.Clear(); }
        }

        private void richTextBox3_Enter(object sender, EventArgs e)
        {
            if (this.richTextBox3.Text.Trim() == "Патологоанатомическое заключение")
            { this.richTextBox3.Clear(); }
        }

        private void richTextBox4_Enter(object sender, EventArgs e)
        {
            if (this.richTextBox4.Text.Trim() == "Макроскопическое описание")
            { this.richTextBox4.Clear(); }
        }

        private void richTextBox5_Enter(object sender, EventArgs e)
        {
            if (this.richTextBox5.Text.Trim() == "Номера кассет")
            { this.richTextBox5.Clear(); }
        }

        private void richTextBox8_Enter(object sender, EventArgs e)
        {
            if (this.richTextBox8.Text.Trim() == "Номера кассет")
            { this.richTextBox8.Clear(); }
        }

        private void richTextBox6_Enter(object sender, EventArgs e)
        {
            if (this.richTextBox6.Text.Trim() == "Макроскопическое описание")
            { this.richTextBox6.Clear(); }
        }

        private void zaklRichTextBox1_Enter(object sender, EventArgs e)
        {
            if (this.zaklRichTextBox1.Text.Trim() == "Заключение")
            { this.zaklRichTextBox1.Clear(); }
        }

        private void diagnos_Enter(object sender, EventArgs e)
        {
            if (this.diagnos.Text.Trim() == "Диагноз")
            { this.diagnos.Clear(); }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (this.textBox2.Text.Trim() == "Предыдущее исследование")
            { this.textBox2.Clear(); }
        }
           
        

    }
}
