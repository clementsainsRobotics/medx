using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;
using System.Collections;


namespace baza.Document
{
    class SampleType
    {
        public class SampleItem
        {
            public string SampleName { get; set; }
            public int SampleType { get; set; }
        }


        public partial class SList : List<SampleItem>
        {
            public void SampleListGet()
            {
                DataTable dtSampleTypes = new DataTable();


                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '3' ;", DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);                    
                    foreach (DataRow ro in dtSampleTypes.Rows)
                    {
                        SampleItem samTy = new SampleItem();
                        samTy.SampleName = (string)ro["txt"];
                        samTy.SampleType = (int)ro["code_id"];
                        this.Add(samTy);
                    }

                    
                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }
               

            }
        }


        public class LabItem
        {
            public string SampleName { get; set; }
            public int SampleType { get; set; }
        }

        public partial class LabList : List<LabItem>
        {
            public void LabListGet()
            {
                DataTable dtSampleTypes = new DataTable();
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '2' ;", DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    foreach (DataRow ro in dtSampleTypes.Rows)
                    {
                        LabItem samTy = new LabItem();
                        samTy.SampleName = (string)ro["txt"];
                        samTy.SampleType = (int)ro["code_id"];
                        this.Add(samTy);
                    }


                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();   
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }


            }
        }

        public class TemplateItem
        {
            public string SampleName { get; set; }
            public int SampleType { get; set; }
        }

        public partial class TemplateList : List<TemplateItem>
        {
            public void TemplateLabAnalysisListGet(int _labId)
            {
                DataTable dtSampleTypes = new DataTable();
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '5' and code = '"+_labId+"' ;", DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    foreach (DataRow ro in dtSampleTypes.Rows)
                    {
                        TemplateItem samTy = new TemplateItem();
                        samTy.SampleName = (string)ro["txt"];
                        samTy.SampleType = (int)ro["code_id"];
                        this.Add(samTy);
                    }


                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }


            }
        }

        public class MeasureItem
        {
            public string SampleName { get; set; }
            public int SampleType { get; set; }
        }

        public partial class MeasureList : List<MeasureItem>
        {
            public void MeasureListGet()
            {
                DataTable dtSampleTypes = new DataTable();
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '4' ;", DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    foreach (DataRow ro in dtSampleTypes.Rows)
                    {
                        MeasureItem samTy = new MeasureItem();
                        samTy.SampleName = (string)ro["txt"];
                        samTy.SampleType = (int)ro["code_id"];
                        this.Add(samTy);
                    }


                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();   
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }


            }
        }


        public class AnalysisTypeItem
        {
            public string SampleName { get; set; }
            public int SampleType { get; set; }
            
        }

        public partial class AnalysisTypeList : List<AnalysisTypeItem>
        {
            public void MeasureListGet()
            {
                
                DataTable dtSampleTypes = new DataTable();
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter("Select txt, code_id from codes where grp = '8' order by code asc;", DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    foreach (DataRow ro in dtSampleTypes.Rows)
                    {
                        AnalysisTypeItem ati = new AnalysisTypeItem();
                        ati.SampleName = (string)ro["txt"];
                        ati.SampleType = (int)ro["code_id"];
                      
                        this.Add(ati);
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
