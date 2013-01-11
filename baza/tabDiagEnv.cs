using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Data;
using System.Reflection;


namespace baza
{
    class Diag
    {
        public class Diagnosis
        {
            public int DiagSerial;
            public int DiagMkbId;
            public string DiagBody;
            public DateTime DiagDate;
            public bool DiagMain;
            public short DiagType;

            private DataTable tlDiagData;

            public Diagnosis getDiag(int _gotSerial)
            {
                NpgsqlDataAdapter getDiagData = new NpgsqlDataAdapter("Select * from diag_data where serial = '" + _gotSerial + "';", DBExchange.Inst.connectDb);
                tlDiagData = new DataTable();
                Diagnosis _diag = new Diagnosis();
                try
                {
                    _diag.DiagSerial = _gotSerial;
                    getDiagData.Fill(tlDiagData);
                    
                    if (tlDiagData.Rows.Count > 0)
                    {
                        if (Convert.IsDBNull(tlDiagData.Rows[0]["type"]) == false)
                        {
                            _diag.DiagType = Convert.ToInt16(tlDiagData.Rows[0]["type"]);
                        }
                        if (Convert.IsDBNull(tlDiagData.Rows[0]["diag_date"]) == false)
                        {
                            _diag.DiagDate = Convert.ToDateTime(tlDiagData.Rows[0]["diag_date"]);
                        }
                        if (Convert.IsDBNull(tlDiagData.Rows[0]["main_mark"]) == false)
                        {
                            _diag.DiagMain = (bool)tlDiagData.Rows[0]["main_mark"];
                        }
                        if (Convert.IsDBNull(tlDiagData.Rows[0]["comment"]) == false)
                        {
                            _diag.DiagBody = (string)tlDiagData.Rows[0]["comment"];
                        }
                        if (Convert.IsDBNull (tlDiagData.Rows[0]["diag"]) == false )
                        {
                            _diag.DiagMkbId = (int)tlDiagData.Rows[0]["diag"];
                        }

                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());


                }
                return _diag;
            }
        }
    }


    public class tabDiagEnv
    {
        public List<int> diagNumList;
        public List<Int16> diagGroupNumList;
        public List<Int16> diagSubGroupNumList;

        

        public List<string> getDiagGroupFromBase()
        {
            NpgsqlCommand selDiDa = new NpgsqlCommand("select id, trim(name), trim(full_group) from diag_grp where menu_level=true", DBExchange.Inst.connectDb);
            diagGroupNumList = new List<Int16>();
            List<string> listDiagDescr = new List<string>();
            
            try
            {
                NpgsqlDataReader readData = selDiDa.ExecuteReader();
                int colNum = readData.FieldCount;
                while (readData.Read())
                {
                    string document = (string)readData[2] + " " + (string)readData[1];
                    listDiagDescr.Add(document);
                    diagGroupNumList.Add((Int16)readData[0]);

                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                

            }
            return listDiagDescr;
        }


        public List<string> getDiagSubGroupFromBase(Int16 grpId)
        {
            NpgsqlCommand selDiDa = new NpgsqlCommand("select id, trim(name), trim(full_group) from diag_grp where group_lease = '" + grpId + "' and menu_level=false ", DBExchange.Inst.connectDb);
            diagSubGroupNumList = new List<Int16>();
            List<string> listDiagDescr = new List<string>();

            try
            {
                NpgsqlDataReader readData = selDiDa.ExecuteReader();
                int colNum = readData.FieldCount;
                while (readData.Read())
                {
                    string document = (string)readData[2] + " " + (string)readData[1];
                    listDiagDescr.Add(document);
                    diagSubGroupNumList.Add((Int16)readData[0]);

                }

            }
            catch (Exception exception)
            {
               Warnings.WarnLog log = new Warnings.WarnLog(); 
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString()); 
                    

            }
            return listDiagDescr;
        }

        public List<Int16> getDiagSubGroupIntFromBase(Int16 grpId)
        {
            NpgsqlCommand selDiDa = new NpgsqlCommand("select id from diag_grp where group_lease = '" + grpId + "' and menu_level=false ", DBExchange.Inst.connectDb);
            diagSubGroupNumList = new List<Int16>();
           
            try
            {
                NpgsqlDataReader readData = selDiDa.ExecuteReader();
                int colNum = readData.FieldCount;
                while (readData.Read())
                {

                    diagSubGroupNumList.Add((Int16)readData[0]);

                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                

            }
            return diagSubGroupNumList;
        }



        public List<string> getMkbDiagFromBase(Int16 grpId)
        {
            NpgsqlCommand selDiDa = new NpgsqlCommand("select diag, trim(name), trim(ds) from diags where sel_grp = '" + grpId + "' ", DBExchange.Inst.connectDb);
            diagNumList = new List<int>();
            List<string> listDiagDescr = new List<string>();

            try
            {
                NpgsqlDataReader readData = selDiDa.ExecuteReader();
                int colNum = readData.FieldCount;
                while (readData.Read())
                {
                    string document = (string)readData[2] + " " + (string)readData[1];
                    listDiagDescr.Add(document);
                    diagNumList.Add((int)readData[0]);

                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }
            return listDiagDescr;
        }

        public List<string> fillDiagTypeList()
        {
            NpgsqlCommand selDiDa = new NpgsqlCommand("select trim(diag) from diag_type order by id ", DBExchange.Inst.connectDb);           
            List<string> listDiagDescr = new List<string>();

            try
            {
                NpgsqlDataReader readData = selDiDa.ExecuteReader();
                int colNum = readData.FieldCount;
                while (readData.Read())
                {
                    string document = (string)readData[0] ;
                    listDiagDescr.Add(document);
                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                

            }
            return listDiagDescr;
        }

        public List<string> getDiagById(int _thisId)
        {

            NpgsqlCommand selDiDa = new NpgsqlCommand("select diag, trim(name), trim(ds), mid_grp, sub_grp, sel_grp from diags where diag = '" + _thisId + "' ", DBExchange.Inst.connectDb);
            diagNumList = new List<int>();
            List<string> listDiagDescr = new List<string>();

            try
            {
                NpgsqlDataReader readData = selDiDa.ExecuteReader();
                int colNum = readData.FieldCount;
                while (readData.Read())
                {
                    string document = (string)readData[2] + " " + (string)readData[1];
                    listDiagDescr.Add(document);
                    diagNumList.Add((int)readData[0]);

                }

            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }
            return listDiagDescr;

        }

    }
}
