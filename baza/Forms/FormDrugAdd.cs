using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;

namespace baza
{
    public partial class FormDrugAdd : Form
    {
        public DataTable tblDrugGroup;
        public DataTable tblDrugSubgroup;
        public DataTable tblDrugSubgroupCat;
        public DataTable tblMeasure;
        public DataTable tblMeasurePack;


        public FormDrugAdd()
        {
            InitializeComponent();
            loadGroupFormLists();
            fillMeasureData();
                      
        }

        //создает список групп
        private List<string> loadGroups()
        {
            
            List<string> lstDrugGrp = new List<string>();
            
            NpgsqlDataAdapter loadDrugGroups = new NpgsqlDataAdapter("Select * from drug_groups where type=0 order by descr asc", DBExchange.Inst.connectDb);
            
            tblDrugGroup = new DataTable();
            loadDrugGroups.Fill(tblDrugGroup);
            foreach (DataRow roww in tblDrugGroup.Rows)
            {
                lstDrugGrp.Add((string)roww["descr"]);
            }

            return lstDrugGrp;


        }

        //Загрузка типов доз
        private List<string> loadMeasureData()
        {
  
            List<string> lstDose = new List<string>();
            NpgsqlDataAdapter loadMeasure = new NpgsqlDataAdapter("Select * from drug_dose_type where type =0 order by id asc", DBExchange.Inst.connectDb);

            tblMeasure = new DataTable();
            loadMeasure.Fill(tblMeasure);
            foreach (DataRow roww in tblMeasure.Rows)
            {
                    lstDose.Add((string)roww["name"]);
            }
            return lstDose;
        }

        /// <summary>
        /// загрузка типов упаковки
        /// </summary>
        /// <returns></returns>
        private List<string> loadMeasureDataPack()
        {
            List<string> lstPack = new List<string>();

            NpgsqlDataAdapter loadMeasurePack = new NpgsqlDataAdapter("Select * from drug_dose_type where type =1 order by id asc", DBExchange.Inst.connectDb);

            tblMeasurePack = new DataTable();
            loadMeasurePack.Fill(tblMeasurePack);
            foreach (DataRow roww in tblMeasurePack.Rows)
            {
                    lstPack.Add((string)roww["name"]);
            }
            return lstPack;
        }


        //Заполнение списков упаковки и доз
        private void fillMeasureData()
        {
            this.comboBox3.DataSource = loadMeasureDataPack();
            this.comboBox4.DataSource = loadMeasureData();
        }

        //заполняет списки групп в форме
        private void loadGroupFormLists()
        {
            List<string> tempLst = new List<string>();
            tempLst = loadGroups();
            this.comboBox1.DataSource = tempLst;
            this.listBox1.DataSource = tempLst;
            this.comboBox5.DataSource = tempLst;

        }


        //создает список подгрупп
        private List<string> loadSubGroups(int group)
        {
            List<string> lstSubGrp = new List<string>();

            NpgsqlDataAdapter loadDrugGroups = new NpgsqlDataAdapter("Select * from drug_groups where type=1 and lease='"+group+"' order by descr asc", DBExchange.Inst.connectDb);

            tblDrugSubgroup = new DataTable();
            loadDrugGroups.Fill(tblDrugSubgroup);
            foreach (DataRow roww in tblDrugSubgroup.Rows)
            {
                lstSubGrp.Add((string)roww["descr"]);
            }

            
            return lstSubGrp;

        }

        //создает список подгрупп категорий
        private List<string> loadSubGroup1(int sub)
        {
            List<string> lstSubGrpCat = new List<string>();

            NpgsqlDataAdapter loadDrugGroups = new NpgsqlDataAdapter("Select * from drug_groups where type=2 and lease='" + sub + "' order by descr asc", DBExchange.Inst.connectDb);

            tblDrugSubgroupCat = new DataTable();
            loadDrugGroups.Fill(tblDrugSubgroupCat);
            foreach (DataRow roww in tblDrugSubgroupCat.Rows)
            {
                lstSubGrpCat.Add((string)roww["descr"]);
            }


            return lstSubGrpCat;

        }
        //Работа кнопки добавить, в зависимости от выбранного таба будет воспроизведено нужное действие
        private void doInsertButton()
        {
            if (this.tabControl1.SelectedTab == tabPage3)
            {//группы
                string descr = this.textBox5.Text;
                int type = 0;
                int lease = 0;
                if (checkGroupName(descr,type) == false)
                {
                    insertGroup(descr, type, lease);
                }
                
            }
            else if (this.tabControl1.SelectedTab == tabPage4)
            {//подгруппы
                string descr = this.textBox6.Text;
                int type = 1;
                int lease = (int)tblDrugSubgroup.Rows[comboBox5.SelectedIndex]["id"];
                if (checkGroupName(descr, type) == false)
                {
                    insertGroup(descr, type, lease);
                }
            }
            else if (this.tabControl1.SelectedTab == tabPage6)
            {//подклассы
                string descr = this.textBox8.Text;
                int type = 2;
                int lease = (int)tblDrugSubgroupCat.Rows[comboBox7.SelectedIndex]["id"];
                if (checkGroupName(descr, type) == false)
                {
                    insertGroup(descr, type, lease);
                }
            }
            else if (this.tabControl1.SelectedTab == tabPage1 || this.tabControl1.SelectedTab == tabPage2)
            {//препараты
                insertDrug();
            }
            else
            {

            }

        }
        //проверка имени группы
        private bool checkGroupName(string grName, int type)
        {
            bool gotGroup = false;

            NpgsqlCommand selGrpName = new NpgsqlCommand("SELECT id from drug_groups where type = '"+type+"' and lower(descr) like '"+grName.ToLower()+"%' ;", DBExchange.Inst.connectDb);

            try
            {
                Int32 grpName = (Int32)selGrpName.ExecuteScalar();
                if (grpName > 0)
                {
                    gotGroup = true;
                }
            }
            catch (Exception exception)
            {
                gotGroup = false;

            }


            return gotGroup;
        }

        //записывает группу лекарств
        private void insertGroup(string descript, int type, int lease)
        {


            NpgsqlCommand insGroup = new NpgsqlCommand("insert into drug_groups (descr, type, lease) "
            +"values ('" + descript + "','" + type + "','" + lease + "') ;", DBExchange.Inst.connectDb);
            try
            {
                insGroup.ExecuteNonQuery();

            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
            finally { }

        }


        //запись препарата в базу
        private void insertDrug()
        {
            if (this.textBox1.Text.Trim().Length > 2)
            {
                string nameDrug = this.textBox1.Text.Trim();
                string strAbbr = this.textBox3.Text.Trim();
                string strInter = this.textBox2.Text.Trim();
                string strGroup ="";
                string strInsGroup = "";
                string strPack = "";
                string strInsPack = "";
                string strDose = "";
                string strInsDose = "";
                if (this.comboBox1.SelectedIndex != -1)
                {
                    strGroup += ",drug_group ";
                    strInsGroup += ", '" + (int)tblDrugGroup.Rows[this.comboBox1.SelectedIndex]["id"] + "'";

                    if (this.comboBox2.SelectedIndex != -1)
                    {
                        strGroup += ",subgroup ";
                        strInsGroup += ", '" + (int)tblDrugSubgroup.Rows[this.comboBox2.SelectedIndex]["id"] + "'";

                        if (this.comboBox8.SelectedIndex != -1)
                        {
                            strGroup += ",subgroupcat ";
                            strInsGroup += ", '"+(int)tblDrugSubgroupCat.Rows[this.comboBox8.SelectedIndex]["id"]+"'" ;
                        }
                    }
                }
                if (this.textBox4.Text.Trim().Length > 0)
                {
                    strPack = ",dose_pack, dose_pack_value, dose_name";
                    strInsPack += ",'"+tblMeasurePack.Rows[this.comboBox3.SelectedIndex]["id"]+"'";
                    strInsPack += ",'" +Convert.ToInt16(this.textBox4.Text.Trim()) + "'";
                    strInsPack += ",'" + tblMeasure.Rows[this.comboBox4.SelectedIndex]["id"] + "'";
                }
                
                    if (this.textBox9.Text.Trim().Length >0)
                    {
                        strDose += ", dose_value";
                        strInsDose += ",'"+Convert.ToDecimal(this.textBox9.Text.Trim())+"'";
                        if (this.textBox10.Text.Trim().Length > 0)
                        {
                            strDose += ", days";
                            strInsDose += ",'" + Convert.ToDecimal(this.textBox10.Text.Trim()) + "'";
                        
                        if (this.textBox11.Text.Trim().Length > 0)
                        {
                            strDose += ", times";
                            strInsDose += ",'" + Convert.ToDecimal(this.textBox11.Text.Trim()) + "'";
                        }
                        }
                    }
                
                

                NpgsqlCommand insDrug = new NpgsqlCommand("insert into drug (drug_name, abbr, international, doc_in " + strGroup + " " + strPack + " " + strDose + ") "
+ "values ('" + nameDrug + "','" + strAbbr + "','" + strInter + "', '"+DBExchange.Inst.dbUsrId+"' "+strInsGroup+" "+strInsPack+" "+strInsDose+") ;", DBExchange.Inst.connectDb);
                try
                {
                    insDrug.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
            }

        }

        //поиск препарата, отсюда реализовать проверку
        private List<string> searchDescrDrug()
        {
            List<string> lstSearch = new List<string>();

            
            
            return lstSearch;
        }
        //Закрывает форму
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormDrugAdd.ActiveForm.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            doInsertButton();
            loadGroupFormLists();
        }



    }
}
////сверху список диагнозов из мкб фильтрация и при выборе добавлять в базу + чтобы можно было поставить всю категорию
// открывать несколько форм