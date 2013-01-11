using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;

namespace baza
{
    public class tabSurgeryEnv
    {
        // Заполнение существующих этапов
        public DataTable tlPatientEtap;
        public DataTable tlFullDrugList;
        public List<string> fillListSurgeryEtap(int pidn)
        {

            tlPatientEtap = new DataTable();
            NpgsqlDataAdapter selPatEtaps = new NpgsqlDataAdapter("Select line_id, line_start, line_stop, line_name from treatment_step where pat_id = " + pidn + " order by current_date Asc ", DBExchange.Inst.connectDb);
            List<string> listSurgeryEtap = new List<string>();

            try
            {
                selPatEtaps.Fill(tlPatientEtap);


                foreach (DataRow row in tlPatientEtap.Rows)
                {
                    listSurgeryEtap.Add(((DateTime)row[1]).ToShortDateString() + " " + ((DateTime)row[2]).ToShortDateString() + " " + ((string)row[3]));
                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                

            }
            return listSurgeryEtap;

        }

        public List<string> fillSchemeAllDrugs()
        {

            tlFullDrugList = new DataTable();
            NpgsqlDataAdapter selDrugs = new NpgsqlDataAdapter("Select drug_name, drug_id from drug order by drug_name Asc ", DBExchange.Inst.connectDb);
            List<string> listAllDrugs = new List<string>();

            try
            {
                selDrugs.Fill(tlFullDrugList);


                foreach (DataRow row in tlFullDrugList.Rows)
                {
                    listAllDrugs.Add((string)row[0]);
                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                

            }
            return listAllDrugs;

           
        }



    }
}
