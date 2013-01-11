using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;

namespace baza.Editor
{
    public partial class FormNewTemplateEditor : Form
    {
        private ClassRadiologyItem.ResearchList rl;
        private ClassRadiologyItem.ZoneList zl;
        private ClassRadiologyItem.ZoneTemplateList ztl;
        private int _SelectedZoneIndex;

        public FormNewTemplateEditor()
        {
            InitializeComponent();
            fillTypeList();

        }

        private void fillTypeList()
        {
            rl = new ClassRadiologyItem.ResearchList();
            rl.GetResearchList();
            this.comboBox1.Items.Clear();

            foreach (ClassRadiologyItem.MagneticResearches i in rl)
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
            zl = new ClassRadiologyItem.ZoneList();
            this.comboBox2.Items.Clear();
            int _zType = rl[this.comboBox1.SelectedIndex].ResearchId;

            zl.GetZoneList(_zType, false);
            

            foreach (ClassRadiologyItem.ResearchZones i in zl)
            {

                this.comboBox2.Items.Add(i.ZoneName);

            }


                this.comboBox2.Items.Add("Добавить новую зону...");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillTemplateList();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormNewTemplateEditor.ActiveForm.Close();
        }


        private void fillZoneList(int _thisZone)
        {
            ztl = new ClassRadiologyItem.ZoneTemplateList();
            
            ztl.GetZoneTemplate(_thisZone, this.checkBox1.Checked);
            this.comboBox3.Items.Clear();
            foreach (ClassRadiologyItem.ZoneTemplate i in ztl)
            {

                this.comboBox3.Items.Add(i.TemplateName);

            }
            this.comboBox3.Items.Add("Добавить новый шаблон...");
        }


        private void getTemplateBody()
        {
            
            this.richTextBox1.Text = ztl[this.comboBox3.SelectedIndex].TemplateBody;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedIndex != -1)
        {
            _SelectedZoneIndex = this.comboBox2.SelectedIndex;
            if (this.comboBox2.SelectedItem.ToString().Trim() == "Добавить новую зону...")
            {
                short _Type = Convert.ToInt16(this.comboBox1.SelectedIndex);
                Editor.FormNewRadiologyItem fnri = new FormNewRadiologyItem(_Type,1,0);
                fnri.ShowDialog();
                fillTemplateList();
            }
            else
            {
                int _zone = zl[_SelectedZoneIndex].ZoneId;
                fillZoneList(_zone);
                //getTemplateBody(_zone);
            }
        }
        }


        private void writeTemplate()
        {

            if (this.richTextBox1.Text.Trim().Length > 2)
            {
                if (this.comboBox3.SelectedIndex != -1)
                {
                    string _template = this.richTextBox1.Text.Trim();
                    Int16 _type = Convert.ToInt16(zl[_SelectedZoneIndex].ZoneId);
                    string _com = "";
                    if (ztl[this.comboBox3.SelectedIndex].TemplateBody == null)
                    {
                        _com = "Insert into zone_template (template, zone_id, doc_in,, method_type) values ('" + _template + "', '" + _type + "', '"
                            + DBExchange.Inst.dbUsrId + "', '" + rl[this.comboBox1.SelectedIndex].ResearchId + "')";
                    }
                    else
                    {
                        _com = "Update zone_template set template = '" + _template + "' , zone_id = '" + _type + "', doc_in = '" + DBExchange.Inst.dbUsrId
                            + "', method_type = '" + rl[this.comboBox1.SelectedIndex].ResearchId + "' where templ_id = '" + ztl[this.comboBox3.SelectedIndex].TemplateId + "'";
                    }

                    NpgsqlCommand _command = new NpgsqlCommand(_com, DBExchange.Inst.connectDb);

                    try
                    {

                        _command.ExecuteNonQuery();
                        int selCom2 = this.comboBox2.SelectedIndex;

                        fillTemplateList();
                        comboBox2.SelectedIndex = selCom2;
                    }
                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Выберите или создайте новый шаблон");
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeTemplate();
            this.richTextBox1.Text = "";
            fillTemplateList();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            fillTemplateList();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox3.SelectedItem.ToString().Trim() == "Добавить новый шаблон...")
            {
                short _Type = Convert.ToInt16(this.comboBox2.SelectedIndex);
                int _rId = rl[this.comboBox1.SelectedIndex].ResearchId;
                Editor.FormNewRadiologyItem fnri = new FormNewRadiologyItem(_Type, 2, _rId);
                fnri.ShowDialog();
                if (_Type != -1)
                {
                    fillZoneList(zl[_Type].ZoneId);
                }
            }
            else
            {
                getTemplateBody();
            }
        }

    }


}
