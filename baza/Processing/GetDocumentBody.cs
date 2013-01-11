using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;

namespace baza.Processing
{
    public class GetDocumentBody
    {
        public Int64 documentSerial { get; set; }
        public string DocumentBody { get; set; }

    }

    public partial class DocumentsBodyList : List<GetDocumentBody>
    {
        public Documentation.DocumentsList ProcessData(Documentation.DocumentsList _documentSerials)
        {
            
            List<Int64> iidList = new List<long>();
            
            foreach (Documentation.Document idd in _documentSerials)
            {
                int[] funct = new int[] { 11, 12, 13, 15, 16, 17, 20, 19, 21, 22, 23, 27 };
                int[] analys = new int[] { 23, 24, 29 };
                string _command ="";

                try
                {

                    foreach (int i in funct)
                    {

                        if (idd.TypeId == i)
                        {
                            _command = "Select descr from " + idd.MedexDataTable + " where num = '" + idd.DocumentTableId + "' ";
                            NpgsqlCommand getDocument = new NpgsqlCommand(_command, DBExchange.Inst.connectDb);
                            idd.DocumentBody = (string)getDocument.ExecuteScalar();

                        }
                    }

                    foreach (int i in analys)
                    {
                        string getDescr = "<br><table width='80%' border='1'>";
                        if (idd.TypeId == i)
                        {
                            _command = "Select lr.txt_data, lr.value, lr.test_positive, nl.test_name from lab_results lr, norm_limits nl where lr.ticket_id = "
                                + idd.DocumentTableId + " and nl.test_name_id = lr.test_name_id ;";
                            DataTable labRes = new DataTable();
                            NpgsqlCommand getDocDescr = new NpgsqlCommand(_command, DBExchange.Inst.connectDb);
                            NpgsqlDataReader getIt = getDocDescr.ExecuteReader();

                            
                            int colNum = getIt.FieldCount;
                            int ii = 1;
                            while (getIt.Read())
                            {
                                
                                   if(ii % 2 == 0)
                                   {
                                       getDescr += "<td>";
                                   }      
                                   else
                                   {
                                       getDescr += "<tr><td>";
                                   }  

  

                                getDescr += "<span>" + (string)getIt[3];
                                if ((decimal)getIt[1] == -9999)
                                { }
                                else
                                { getDescr += ": " + getIt[1].ToString(); }
                                if (getIt[0].ToString().Length == 0 || getIt[0] is DBNull )
                                { }
                                else
                                { getDescr += ": " + (string)getIt[0]; }

                                if (getIt[2] is DBNull)
                                { }
                                else
                                {
                                    if ((Boolean)getIt[2] == true)
                                    {
                                        getDescr += ": Положительно";
                                    }
                                    else if ((bool)getIt[2] == false)
                                    {
                                        getDescr += ": Отрицательно";
                                    }
                                }

                                getDescr += "</span>";
                                if (ii % 2 == 0)
                                {
                                    getDescr += "</td></tr>";
                                }
                                else
                                {
                                    getDescr += "</td>";
                                }
                                ii++;
                            }
                            
                            getIt.Close();
                            getDescr += "</table>";
                            idd.DocumentBody = getDescr;

                        }

                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }

                
            }
            return _documentSerials;
        }




    }


}
