using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;
using System.Collections;

namespace baza.Processing
{
    class DrugsClass
    {
       
        public class DrugGroup
        {
            public int groupId;
            public string groupDescription;
            public short groupType;
            public int groupLease;


        }

        public partial class DrugGroupList : List<DrugGroup>
        {
            public void getDrugGroupList()
            {
                getData(0);
            }

            public void getDrugSubGroupList()
            {

                getData(1);
            }

            public void getDrugSubGroupCatList()
            {

                getData(2);
            }

            private void getData(int _thisCase)
            {
                string _command ="";
                switch (_thisCase)

               {
                
                    case 0:
                        _command = "Select * from drug_groups where type = 0 order by lease, id desc ;";
                        break;
               
                    case 1:
                        _command ="Select * from drug_groups where type = 1 order by lease, id desc ;";
                        break;

                    case 2:
                        _command = "Select * from drug_groups where type = 2 order by lease, id desc ;";
                        break;
                }
                DataTable dtGrpList = new DataTable();
                NpgsqlDataAdapter getGroups = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getGroups.Fill(dtGrpList);

                    processData(dtGrpList);

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }


            private void processData(DataTable _thisTable)
            {
                 this.Clear();
                 if (_thisTable.Rows.Count > 0)
                 {
                     foreach (DataRow ro in _thisTable.Rows)
                     {
                         DrugGroup samTy = new DrugGroup();
                         samTy.groupId = (int)ro["id"];
                         samTy.groupDescription = (string)ro["descr"];
                         samTy.groupLease = (int)ro["lease"];
                         samTy.groupType = (short)ro["type"];
                         this.Add(samTy);
                     }
                 }
            }

        }


        public class Drug
        {
            public string drugName;
            public string drugAbbr;
            public int drugId;
            public int drugGroupId;
            public int drugSubgroupId;
            public int drugSubgroupCatId;
            public int drugInternationalId;


        }

        public partial class DrugList : List<Drug>
        {
            public void getDrugsByGroupId(int _gId)
            {
                DataTable dtDrugList = new DataTable();

                 NpgsqlDataAdapter getDrugs = new NpgsqlDataAdapter("Select * from drug where drug_group = '"+_gId+"';", DBExchange.Inst.connectDb);
                try
                {
                    getDrugs.Fill(dtDrugList );
                    processData(dtDrugList);
                }

                
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog(); 
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }

            private void processData(DataTable _thisTable)
            {
                this.Clear();
                if (_thisTable.Rows.Count > 0)
                {
                    foreach (DataRow ro in _thisTable.Rows)
                    {
                        
                        Drug dr = new Drug();
                        dr.drugId = (int)ro["drug_id"];
                        dr.drugName = (string)ro["drug_name"];
                        if (ro.IsNull("abbr") == false)
                        {
                            dr.drugAbbr = (string)ro["abbr"];
                        }
                        if (ro.IsNull("drug_group") == false)
                        {
                            dr.drugGroupId = (int)ro["drug_group"];
                        }
                        
                        if (ro.IsNull("international_id") == false)
                        {
                            dr.drugInternationalId = (int)ro["international_id"];
                        }
                        
                        if (ro.IsNull("subgroupcat") == false)
                        {
                            dr.drugSubgroupCatId = (int)ro["subgroupcat"];
                        }
                        if (ro.IsNull("subgroup") == false)
                        {
                            dr.drugSubgroupId = (int)ro["subgroup"];
                        }
                        this.Add(dr);
                    }
                }

            }

            public void getDrugsByName(string _thisName)
            {
                DataTable dtDrugList = new DataTable();

                NpgsqlDataAdapter getDrugs = new NpgsqlDataAdapter("Select * from drug where lower(drug_name) like '%" + _thisName.ToLower() + "%';", DBExchange.Inst.connectDb);
                try
                {
                    getDrugs.Fill(dtDrugList);
                    processData(dtDrugList);
                }


                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }


            }

            
        }

        public partial class DrugDataProcessing
        {
            public void writeDrugTreatment(decimal _dose, decimal _unit, string _krat, DateTime _start, DateTime _stop, int _patient, int _drug, int _doc)
            {
                string _srtrU = _unit.ToString();
                decimal _wunit = Convert.ToDecimal(_unit);
                

                NpgsqlCommand wdata = new NpgsqlCommand("Insert into drug_treatment (real_dose, unit_id, kratnost, drug_start, drug_stop, pat_id, drug_id, doctor_id) values "
                + " ('" + _dose + "','" + _wunit + "','" + _krat + "','" + _start + "','" + _stop + "','" + _patient + "','" + _drug + "','" + _doc + "' ); ", DBExchange.Inst.connectDb);
                try
                {
                    wdata.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }


        }


        public class DrugDose
        {
            public decimal DoseId;
            public string DoseName;
            public short DoseType;
        }

        public partial class DrugDoseList : List<DrugDose>
        {
            public void getDoseTypes()
            {
                try
                {
                    NpgsqlDataAdapter loadMeasure = new NpgsqlDataAdapter("Select * from drug_dose_type order by id asc", DBExchange.Inst.connectDb);

                    DataTable tblMeasure = new DataTable();
                    loadMeasure.Fill(tblMeasure);
                    foreach (DataRow roww in tblMeasure.Rows)
                    {
                        DrugDose newDose = new DrugDose();
                        newDose.DoseId = (decimal)roww["id"];
                        newDose.DoseName = (string)roww["name"];
                        newDose.DoseType = (short)roww["type"];
                        this.Add(newDose);

                    }
                }
                 
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            
          }           

        }

        public class DrugTreatment
        {
            public long drugTreatmentId;
            public int patientId;
            public int doctorId;
            public int drugId;
            public string doctorName;
            public string patientName;
            public string drugName;
            public decimal drugDose;
            public string drugDoseName;
            public string kratnost;
            public DateTime drugStart;
            public DateTime drugStop;
            public DateTime WrittenDate;

        }

        public partial class PatientDrugTreatment : List<DrugTreatment>
        {
            public void getPatientDrugs(int _pid)
            {
                NpgsqlDataAdapter gpd = new NpgsqlDataAdapter("Select * from view_drug_treatment where pat_id = '"+_pid+"' and deleted = false order by date_in asc", DBExchange.Inst.connectDb);
                DataTable dtGPD = new DataTable();
                gpd.Fill(dtGPD);
                ProcessData(dtGPD);

            }

            private void ProcessData(DataTable _thisTable)
            {
                this.Clear();
                if (_thisTable.Rows.Count > 0)
                {
                    foreach (DataRow ro in _thisTable.Rows)
                    {
                        DrugTreatment dt = new DrugTreatment();
                        string doctor = ((string)ro["doc_fname"]).Trim() + " " + ro["doc_finame"].ToString().Substring(0, 1) + ". " + ro["doc_lname"].ToString().Substring(0, 1) + ".";
                        string patient = ((string)ro["pat_fname"]).Trim() + " " + ro["pat_finame"].ToString().Substring(0, 1) + ". " + ro["pat_lname"].ToString().Substring(0, 1) + ".";
                        dt.doctorId = (int)ro["doctor_id"];
                        dt.doctorName = doctor;
                        dt.patientName = patient;
                        dt.drugDose = (decimal)ro["real_dose"];
                        dt.drugId = (int)ro["drug_id"];
                        dt.drugName = (string)ro["drug_name"];
                        dt.drugStart = (DateTime)ro["drug_start"];
                        dt.drugStop = (DateTime)ro["drug_stop"];
                        dt.drugTreatmentId = (long)ro["drug_treatment_id"];
                        dt.kratnost = (string)ro["kratnost"];
                        dt.patientId = (int)ro["pat_id"];
                        dt.WrittenDate = (DateTime)ro["date_in"];
                        dt.drugDoseName = (string)ro["dose_name"];
                        this.Add(dt);
                    }

                }


            }


        }


    }
}
