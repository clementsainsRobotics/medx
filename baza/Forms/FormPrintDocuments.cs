using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;

namespace baza
{
    public partial class FormPrintDocuments : Form
    {

        DataTable tblSqlCodes;
        PrintingTemplates.TemplateList templList;
        TemplateCodes.TempCodeList templateCodeList;
        PrintingCodes.PrintCodesList printCodes;
        Forms.FormSimpleInputForm fsif = new Forms.FormSimpleInputForm();
        Forms.FormSimpleInputForm fsif1 = new Forms.FormSimpleInputForm();
        Forms.FormDocumentsList fdl ;
        Documentation.DocumentsList DocumentList;
        Processing.Diagnosis.DiagList diags;


        private string newTextData;
        private string newTextData1;

        public FormPrintDocuments(int _patient)
        {
            

            InitializeComponent();
            this.comboBox2.Hide();
            loadSqlCodes();
            refreshDocumentsList();
            fsif.ParentText += new EventHandler<Forms.FormParentEventArgs>(getClass);
            fsif1.ParentText += new EventHandler<Forms.FormParentEventArgs>(getClass1);

            if (_patient != 0)
            {
                searchPatientBox1.searchById(_patient);
            }
            dateTimePicker2.Value = dateTimePicker2.Value.AddMonths(-3);

        }

        public void getClass(object sender, Forms.FormParentEventArgs e)
        {
            newTextData = e.ClassName;
        }

        public void getClass1(object sender, Forms.FormParentEventArgs e)
        {
            newTextData1 = e.ClassName;
        }



        private void FormPrintDocuments_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormPrintDocuments.ActiveForm.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.toolStripButton1.Text == "Исследование")
            {
                this.toolStripButton1.Text = "Пациент";
                this.searchPatientBox1.Hide();
                this.label3.Text = "Исследование";
                this.comboBox2.Show();

            }
            else
            {
                this.toolStripButton1.Text = "Исследование";
                this.searchPatientBox1.Show();
                this.label3.Text = "Пациент";
                this.comboBox2.Hide();
            }
        }

        private void loadDocumentTemplates()
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Forms.FormAddNewSqlTemplateCode fanstc = new Forms.FormAddNewSqlTemplateCode();
            fanstc.ShowDialog();
            loadSqlCodes();
        }

        private void loadSqlCodes()
        {
            this.checkedListBox1.Items.Clear();

            //NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select sql_code_id, code_text, code_description, code_change from codes_sql ", DBExchange.Inst.connectDb);
            //tblSqlCodes = new DataTable();
            try
            {
                printCodes = new PrintingCodes.PrintCodesList();
                printCodes.getCodesList();
                foreach (PrintingCodes.PrintCode i in printCodes )
                {
                    this.checkedListBox1.Items.Add(i.CodeDescr);
                }
                //div.Fill(tblSqlCodes);
                //foreach (DataRow roww in tblSqlCodes.Rows)
                //{
                //    //  this.comboBox9.Items.Add((string)roww[0] + " " + (string)roww[1]);
                //    this.checkedListBox1.Items.Add( (string)roww[2] );
                //}
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }


        }

        private void refreshDocumentsList()
        {
            templList = new PrintingTemplates.TemplateList();

            if (this.toolStripButton1.Text == "Исследование")
            {
                templList.getTemplates(false);
            }
            else
            {
                templList.getTemplates(true);
            }
            comboBox1.Items.Clear();
                       
            foreach (PrintingTemplates.Template tl in templList  )
            {
                this.comboBox1.Items.Add(tl.TemplateDescription );
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

        }



        private void checkDocumentSet()
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Forms.FormAddNewSQLTemplateDocument fanstd = new Forms.FormAddNewSQLTemplateDocument(-1);
            fanstd.ShowDialog();
            refreshDocumentsList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDocumentsParameters();
        }

        void loadDocumentsParameters()
        {
            PrintingTemplates.Template thisDocument = templList[this.comboBox1.SelectedIndex];
            templateCodeList = new TemplateCodes.TempCodeList();
            templateCodeList.getTempCodes(thisDocument.TemplateId);

            this.dataGridView1.DataSource = templateCodeList;
            this.dataGridView1.Columns.Remove("RelationId");
            this.dataGridView1.Columns.Remove("CodeId");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            writeNewCodesIntoDocument();
        }

        private void writeNewCodesIntoDocument()
        {
            if (this.comboBox1.SelectedIndex != -1)
            {
                if (checkedListBox1.SelectedItems.Count > 0)
                {
                    PrintingTemplates.Template thisDocument = templList[this.comboBox1.SelectedIndex];
                    
                    foreach (int ic in this.checkedListBox1.CheckedIndices)
                    {
                        
                        PrintingCodes.PrintCode pc = printCodes[ic];
                        TemplateCodes.TempCode temc = templateCodeList.Find(o => o.CodeId == pc.CodeId);
                        if (temc == null)
                        {
                            NpgsqlCommand chekItem = new NpgsqlCommand("Insert into codes_sql_templates_relation (template_id, code_id, sql_code_instruction) values ('"
                            + thisDocument.TemplateId + "', '" + pc.CodeId + "', '" + pc.CodeChange + "' )", DBExchange.Inst.connectDb);
                            chekItem.ExecuteNonQuery();
                            checkedListBox1.SetItemCheckState(ic, CheckState.Unchecked);
                        }
                        else
                        {
                            checkedListBox1.SetItemCheckState(ic, CheckState.Unchecked);
                        }
                    }
                    
                    loadDocumentsParameters();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Выберите коды для добавления");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Выберите документ");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            removeData();
        }


        void removeData()
        {
            if (this.dataGridView1.SelectedCells.Count > 0)
            {


                if (MessageBox.Show("Удаление данных", "Удалить данные?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    int i = this.dataGridView1.CurrentRow.Index;
                    NpgsqlCommand removeData = new NpgsqlCommand("Delete from codes_sql_templates_relation where relation_primary = '" + templateCodeList[i].RelationId + "'", DBExchange.Inst.connectDb);

                    try
                    {

                        removeData.ExecuteNonQuery();


                        //this.dataGridView1.Rows.RemoveAt(i);
                        templateCodeList.RemoveAt(i);
                        loadDocumentsParameters();

                    }
                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    }

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateData();
        }

        private void updateData()
        {
            if (this.dataGridView1.SelectedCells.Count > 0)
            {
               

                int i = this.dataGridView1.CurrentRow.Index;
                newTextData = templateCodeList[i].CodeSqlInstruction;
                fsif.ShowDialog();
                if (newTextData.Length > 0)
                {
                templateCodeList[i].CodeSqlInstruction = newTextData;

                    NpgsqlCommand updateData = new NpgsqlCommand("Update codes_sql_templates_relation set sql_code_instruction = '"
                        + newTextData + "' where relation_primary = '" + templateCodeList[i].RelationId + "'", DBExchange.Inst.connectDb);

                    try
                    {
                        updateData.ExecuteNonQuery();
                        //this.dataGridView1.Rows.RemoveAt(i);
                        templateCodeList.RemoveAt(i);
                        loadDocumentsParameters();
                    }
                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    }
                }
            }

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            updateData();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            updateData();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form prFC = new printFormGist(showDocument(), false, false);
            prFC.Show(); 
        }

        private string showDocument()
        {
            string thisBody = templList[comboBox1.SelectedIndex].TemplateText;
            string documentsBody0 = "";
            string documentsBody1 = "";
            string documentsBody2 = "";
            string documentsBody3 = "";

            foreach (TemplateCodes.TempCode i in templateCodeList)
            {
                thisBody = thisBody.Replace(i.CodeName, i.CodeSqlInstruction);
            }

            thisBody = thisBody.Replace ("{ПациентФИО}", searchPatientBox1.patName);
            thisBody = thisBody.Replace ("{Пациент}", searchPatientBox1.patFullName);
            thisBody = thisBody.Replace ("{ПациентДР}", searchPatientBox1.patRealBirthDate.ToString());
            thisBody = thisBody.Replace("{ПациентВсегоЛет}", searchPatientBox1.patYears.ToString());
            thisBody = thisBody.Replace("{ПациентПасспорт}", searchPatientBox1.patPassport.ToString());
            thisBody = thisBody.Replace("{ПациентПасспортВыдан}", searchPatientBox1.patPasspOrg.ToString());
            thisBody = thisBody.Replace("{ПациентАдрес}", searchPatientBox1.patAddr.ToString());
            thisBody = thisBody.Replace("{ПациентТелефон}", searchPatientBox1.patPhone.ToString());
            thisBody = thisBody.Replace ("{Пользователь}", DBExchange.Inst.UsrSign);
            thisBody = thisBody.Replace ("{ТекущаяДата}", DateTime.Now.ToShortDateString());
            if (comboBoxDiagnos.Items.Count > 0)
            {
                thisBody = thisBody.Replace("{ПациентДиагноз}", comboBoxDiagnos.SelectedItem.ToString());
            }
            else
            { thisBody = thisBody.Replace("{ПациентДиагноз}", "Диагноз не установлен"); }
            //thisBody = thisBody.Replace("ДатаДокумента",);
            //.searchPatientBox1.thisBody = thisBody.Replace("ДиагнозПациента",searchPatientBox1.);
            thisBody = thisBody.Replace ("{ПациентКарта}", searchPatientBox1.PatientKart );
            int[] funct = new int[] {11, 12, 13, 15, 16, 17, 20, 19, 21, 22, 23, 27};
            int[] analys = new int[] { 24, 25 };
            if (DocumentList == null)
            {
                getDocumentsList();
                workWithData();
            }

            if (DocumentList != null)
            {
                foreach (Documentation.Document d in DocumentList)
                {
                    //foreach (int i in funct)
                    //{
                    //    if (d.TypeId == i)
                    //    {
                    documentsBody0 += "<p> " + d.Date.ToShortDateString() + " " + d.DocumentDoctor + " " + d.DocumentShortName + " " + d.DocumentHeader + " " + d.DocumentPatient + " : " + d.DocumentBody + " </p>";
                    documentsBody1 += "<p> " + d.Date.ToShortDateString() + " " + d.DocumentShortName + ": <br>" + d.DocumentBody + " </p>";
                    documentsBody2 += "<br> " + d.Date.ToShortDateString() + " " + d.DocumentShortName + ": " + d.DocumentHeader + " ";
                    documentsBody3 += "<br> " + d.Date.ToShortDateString() + " " + d.DocumentHeader + " ";


                    //    }
                    //}
                }

            }
            
                        thisBody += "<br>";
                thisBody = thisBody.Replace("{ДокументОсновной}", documentsBody0);
                thisBody = thisBody.Replace("{ДокументМини}", documentsBody1);
                thisBody = thisBody.Replace("{ДокументКратко}", documentsBody2);
                thisBody = thisBody.Replace("{ДокументСписок}", documentsBody3);
            return thisBody;
        }

        private void button6_Click(object sender, EventArgs e)
        {
           // updateTemplateBody();
            editTemplate();
        }

        private void editTemplate()
        {
            Forms.FormAddNewSQLTemplateDocument fanstd = new Forms.FormAddNewSQLTemplateDocument(templList[comboBox1.SelectedIndex].TemplateId);
            fanstd.ShowDialog();
            refreshDocumentsList();
        }


        private void updateTemplateBody()
        {
            newTextData1 = templList[comboBox1.SelectedIndex].TemplateText;
            fsif1.ShowDialog();
            if (newTextData1 != null)
            {
                if (newTextData1.Length > 0)
                {

                    templList[comboBox1.SelectedIndex].TemplateText = newTextData1;

                    NpgsqlCommand updateData = new NpgsqlCommand("Update codes_sql_templates set template_text = '"
                        + newTextData1 + "' where template_id = '" + templList[comboBox1.SelectedIndex].TemplateId + "' ", DBExchange.Inst.connectDb);

                    try
                    {
                        updateData.ExecuteNonQuery();
                        //this.dataGridView1.Rows.RemoveAt(i);
                        refreshDocumentsList();
                    }
                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    }
                }
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            getDocumentsList();
            workWithData();
        }

        private void getDocumentsList()
        {
            DocumentList = new Documentation.DocumentsList();

            if (this.searchPatientBox1.patientSet == true)
            {
                fdl = new Forms.FormDocumentsList(true, searchPatientBox1.pIdN, dateTimePicker2.Value, dateTimePicker1.Value);
                fdl.ParentDocumentsList += new EventHandler<Forms.FormParentListEventArgs>(getDocuments);
                fdl.ShowDialog();
            }
            else
            {
                fdl = new Forms.FormDocumentsList(false, 0, dateTimePicker2.Value, dateTimePicker1.Value);
                fdl.ParentDocumentsList += new EventHandler<Forms.FormParentListEventArgs>(getDocuments);
                fdl.ShowDialog();
            }

            

        }

        private void workWithData()
        {
            Processing.DocumentsBodyList dbl = new Processing.DocumentsBodyList();
            DocumentList = dbl.ProcessData(DocumentList);
            //string dl = DocumentList[1].DocumentBody;
        }


        public void getDocuments(object sender, Forms.FormParentListEventArgs e)
        {
            DocumentList = e.DocList;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        public void loadDiags()
        {
            diags = new Processing.Diagnosis.DiagList();
            if (this.searchPatientBox1.pIdN > 0)
            {
                diags.GetDiagListForPatient(this.searchPatientBox1.pIdN);
                this.comboBoxDiagnos.Items.Clear();
                this.comboBoxDiagnos.Text = "";
                if (diags != null)
                {
                    foreach (Processing.Diagnosis.Diag di in diags)
                    {
                        this.comboBoxDiagnos.Items.Add(di.DiagMkbName + " " + di.DiagName);
                    }
                    if (comboBoxDiagnos.Items.Count > 0)
                    {
                        comboBoxDiagnos.SelectedIndex = 0;
                    }
                }
            }

        }

        private void searchPatientBox1_gotPatientId(int _newPatientId)
        {
            loadDiags();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Form prFC = new printFormGist(showDocument(), true, false);
            prFC.Show(); 
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            writeFile(showDocument());
        }

        private void writeFile(string _txt)
        {
            DirectoryInfo di = new DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            if (!(System.IO.Directory.Exists(di.FullName + "\\Medex\\Documents\\")))
            {
                System.IO.Directory.CreateDirectory(di.FullName + "\\Medex\\Documents\\");
            }
            saveFileDialog1.InitialDirectory = di.FullName + "\\Medex\\Documents\\";
            saveFileDialog1.FileName = di.FullName + "\\Medex\\Documents\\Document_" + DateTime.Now.ToShortDateString() + ".html";

            WebBrowser wbC = new WebBrowser();
            
            wbC.DocumentText = _txt;
            

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter s = new StreamWriter(fs);
                    s.Write(wbC.DocumentText);

                    s.Close();
                    fs.Close();


                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }

            
    }

    }



    class TemplateCodes
    {
        public class TempCode
        {
            public int CodeId { get; set; }
            public string CodeName { get; set; }
            public string CodeSqlInstruction { get; set; }
            public int RelationId { get; set; }
        }

        public partial class TempCodeList : List<TempCode>
        {
            public void getTempCodes(int _thisTemplateId)
            {
                DataTable gc = new DataTable();
                NpgsqlDataAdapter getCodes = new NpgsqlDataAdapter("Select code_id, sql_code_instruction, relation_primary, cs.code_text as codetext from codes_sql_templates_relation cstr, codes_sql cs where template_id = '"            
                    +_thisTemplateId+"' and cs.sql_code_id = cstr.code_id ", DBExchange.Inst.connectDb);
                try
                {
                    getCodes.Fill(gc);

                    foreach (DataRow ro in gc.Rows)
                    {
                        TempCode tc = new TempCode();
                        tc.CodeId = (int)ro[0];
                        tc.RelationId = (int)ro[2];
                        tc.CodeSqlInstruction = (string)ro[1];
                        tc.CodeName = (string)ro[3];
                        this.Add(tc);
                    }

                }

                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }

            
        }
    }



    class PrintingCodes
    {

        public class PrintCode
        {
            public string CodeText{get; set;}
            public string CodeDescr { get; set; }
            public String CodeChange { get; set; }
            public int CodeId { get; set; }
        }

        public partial class PrintCodesList : List<PrintCode>
        {
            public void getCodesList()
            {
                DataTable gc = new DataTable();
                NpgsqlDataAdapter getCodes = new NpgsqlDataAdapter("Select * from codes_sql",DBExchange.Inst.connectDb);

                try
                {
                    getCodes.Fill(gc);

                    foreach (DataRow ro in gc.Rows)
                    {
                        PrintCode pc = new PrintCode();
                        pc.CodeId = (int)ro["sql_code_id"];
                        if (ro["code_text"] is DBNull)
                        { }
                        else
                        {
                            pc.CodeText = (string)ro["code_text"];
                        }
                        if (ro["code_description"] is DBNull)
                        { }
                        else
                        {
                            pc.CodeDescr = (string)ro["code_description"];
                        }
                        if (ro["code_change"] is DBNull)
                        { }
                        else
                        {
                            pc.CodeChange = (string)ro["code_change"];
                        }

                        this.Add(pc);
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }
        }



    }



    class PrintingTemplates
    {
        public class Template
        {
            public string TemplateText {get; set;}
            public string TemplateFile { get; set; }
            public String TemplateDescription { get; set; }
            public Int16 TemplateType { get; set; }
            public int TemplateId { get; set; }
        }


        public partial class TemplateList : List<Template>
        {
            public void getTemplates(bool _forPatient)
            {
                NpgsqlDataAdapter div = new NpgsqlDataAdapter("Select template_text, template_file, template_description, template_type, template_id from codes_sql_templates", 
                    DBExchange.Inst.connectDb);
                DataTable tblSqlTemplates = new DataTable();
               
                try
                {

                    div.Fill(tblSqlTemplates);

                    foreach (DataRow ro in tblSqlTemplates.Rows)
                    {
                        Template templateItem = new Template();
                        templateItem.TemplateDescription = (string)ro[2];
                        templateItem.TemplateText = (string)ro[0];
                        templateItem.TemplateFile = (string)ro[1];
                        if (ro[3] is DBNull) { }
                        else
                        {
                            templateItem.TemplateType = (Int16)ro[3];
                        }
                        templateItem.TemplateId = (Int32)ro[4];
                        this.Add(templateItem);

                    }
                }

                catch (Exception exception)

                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }
        }
    }
}
