using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using System.Reflection;

namespace baza.Editor
{
    public partial class FormNewRadiologyItem : Form
    {

        private ClassRadiologyItem.ResearchList rl;
        private ClassRadiologyItem.ZoneTemplateList ztl;
        private ClassRadiologyItem.ZoneList zl;
        private short _thisCase;


        public FormNewRadiologyItem(Int16 _type, short _case, int _zone)
        {
            InitializeComponent();
            _thisCase = _case;
            switch (_case)
            {
                case 1:
                    fillList();
                    if (_type != -1)
                    {
                        this.comboBox1.SelectedIndex = _type;
                    }
                    break;
                case 2:

                    fillTemplateList(_zone);
                    if (_type != -1)
                    {
                        this.comboBox1.SelectedIndex = _type;
                    }
                    break;

            }
        }

        private void fillTemplateList(int _researchId)
        {
            zl = new ClassRadiologyItem.ZoneList();
            this.comboBox1.Items.Clear();
            

            zl.GetZoneList(_researchId, false);


            foreach (ClassRadiologyItem.ResearchZones i in zl)
            {

                this.comboBox1.Items.Add(i.ZoneName);

            }




        }


        private void fillList()
        {
            rl = new ClassRadiologyItem.ResearchList();
            rl.GetResearchList();

            foreach (ClassRadiologyItem.MagneticResearches i in rl)
            {
                this.comboBox1.Items.Add(i.ResearchName);
            }
            if (this.comboBox1.Items.Count > 0)
            {
                this.comboBox1.SelectedIndex = 0;
            }
        }

        private void fillZoneList()
        {



        }
        

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormNewRadiologyItem.ActiveForm.Close();
        }

        private void writeThisZone()
        {
            if (this.textBox1.Text.Trim().Length > 2)
            {
                string _zone = this.textBox1.Text.Trim();
                
                string _commandLine = "";
                
                switch (_thisCase)
                {
                    case 1:
                        Int16 _type = Convert.ToInt16(rl[this.comboBox1.SelectedIndex].ResearchId);

                        NpgsqlCommand _command = new NpgsqlCommand("Insert into zones (descr_text, type, doc_in) values ( :descr , '" + _type + "', '" + DBExchange.Inst.dbUsrId + "')", 
                            DBExchange.Inst.connectDb);
                        using (_command)
                        {
                            _command.Parameters.Add(new NpgsqlParameter("descr", NpgsqlDbType.Text));
                            _command.Parameters[0].Value = _zone;
                        }
                        try
                        {
                            _command.ExecuteNonQuery();
                        }
                        catch (Exception exception)
                        {
                            Warnings.WarnLog log = new Warnings.WarnLog();    
                            log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                        }

                        break;
                    case 2:

                        int _thisZone = zl[this.comboBox1.SelectedIndex].ZoneId;
                        NpgsqlCommand _command1 = new NpgsqlCommand("Insert into zone_template (template_name, zone_id, doc_in, template) values ( :descr , '"
                            +_thisZone+"', '"+DBExchange.Inst.dbUsrId+"', 'Добавьте описание')", DBExchange.Inst.connectDb) ;
                        using (_command1)
                        {
                            _command1.Parameters.Add(new NpgsqlParameter("descr", NpgsqlDbType.Varchar));
                            _command1.Parameters[0].Value = _zone;
                        }
                        try
                        {
                            _command1.ExecuteNonQuery();
                        }
                        catch (Exception exception)
                        {
                            Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(),
                                exception.StackTrace.ToString());                
                        }
                        break;

                }

               
                

                

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeThisZone();
            this.textBox1.Text = "";
        }
    }
}
