using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;
using System.Collections;


namespace baza.Processing
{
    class Diagnosis
    {
        public class Diag
        {
            public int DiagSerial;
            public int DiagPatient;
            public int DiagDoctor;
            public int DiagMkbId;
            public string DiagMkbName;
            public string DiagName;
            public string DiagBody;
            public DateTime DiagDate;
            public bool DiagMain;
            public decimal DiagType;            
        }


        public partial class DiagList : List<Diag>
        {
            public void GetDiagListForPatient(int _pid)
            {
                DataTable dtDiagnosis = new DataTable();

                NpgsqlDataAdapter selPatDiags = new NpgsqlDataAdapter("Select dd.diag_date, trim(ds.name) as name, dd.diag, "
                + "trim(dd.comment) as comment, dd.usr_id, pat_id, main_mark, type, serial, ds.ds from diag_data dd, diags ds where dd.pat_id = "
                + _pid + " and dd.delete=false and ds.diag = dd.diag order by dd.diag_date Asc ", DBExchange.Inst.connectDb);

               
                try
                {
                    selPatDiags.Fill(dtDiagnosis);
                    if (dtDiagnosis.Rows.Count > 0)
                    {
                        foreach (DataRow ro in dtDiagnosis.Rows)
                        {
                            Diag diagnos = new Diag();
                            diagnos.DiagSerial = (int)ro["serial"];
                            diagnos.DiagName = (string)ro["name"];
                            diagnos.DiagDate = (DateTime)ro["diag_date"];
                            diagnos.DiagPatient = (int)ro["pat_id"];
                            diagnos.DiagType = (decimal)ro["type"];
                            diagnos.DiagDoctor = (int)ro["usr_id"];
                            diagnos.DiagMain = (bool)ro["main_mark"];
                            diagnos.DiagMkbId = (int)ro["diag"];
                            diagnos.DiagMkbName = ((string)ro["ds"]).Trim();
                            this.Add(diagnos);
                        }
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
