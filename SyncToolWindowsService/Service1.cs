using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data.SqlClient;
using System.Threading;
using System.Security.Cryptography;

namespace SyncToolWindowsService
{
    public partial class Service1 : System.ServiceProcess.ServiceBase
    {
        private String m_dbUserName = String.Empty;
        private DataTable gotData;
        
        private System.Timers.Timer startSync;
        private NpgsqlConnection connectDb;
        private SqlConnection msSqlConnect;
        private String dBUserName
        {
            get { return m_dbUserName; }
        }
    


        public Service1()
        {
            InitializeComponent();
            createDbLog();
            syncDbMode();

        }

        public void writeLog(string writeThis)
        {
            string sSource = "Medex syncDB service";
            string sLog = "Application";
            string sEvent = writeThis;
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Information, 4);

 

        }



        public void writeLogDiag(string writeThis, Int16 Num)
        {
            string sSource = "Medex запись диагнозов";
            string sLog = "Application";
            string sEvent = writeThis;
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Information, Num);

            //ELog.WriteEntry(sEvent, EventLogEntryType.Warning, 234, ctype(3,short))

        }

        public void writeLogPat(string writeThis, Int16 Num)
        {
            string sSource = "Medex запись пациентов";
            string sLog = "Application";
            string sEvent = writeThis;
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Information , Num);

            //ELog.WriteEntry(sEvent, EventLogEntryType.Warning, 234, ctype(3,short))

        }

        public void writeLogWarn(string writeThis)
        {
            string sSource = "Medex syncDB service";
            string sLog = "Application";
            string sEvent = writeThis;
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error, 6);

            //ELog.WriteEntry(sEvent, EventLogEntryType.Warning, 234, ctype(3,short))

        }


        public void DbInitConnection(String dBServer, String dBLogin, String dBPassword)
        {


            MD5 md5h = MD5.Create();
            byte[] PassHash = md5h.ComputeHash(Encoding.Default.GetBytes(dBPassword));
            StringBuilder crHash = new StringBuilder();
            foreach (byte hashByte in PassHash)
            {
                crHash.Append(String.Format("{0:x2}", hashByte));
            }
            string dBPassHash = crHash.ToString();

            byte[] UNameHash = md5h.ComputeHash(Encoding.Default.GetBytes(dBLogin));
            StringBuilder crHashN = new StringBuilder();
            foreach (byte hashByte in UNameHash)
            {
                crHashN.Append(String.Format("{0:x2}", hashByte));
            }
            string dBLoginHash = crHashN.ToString();




            string strConnect = "Server=" + dBServer + ";Port=5432;User Id=" + dBLogin + "; Password=" + dBPassHash + ";Database=medex; ; searchpath=medex;";
            m_dbUserName = dBLogin;
            connectDb = new NpgsqlConnection(strConnect);
            try
            {
                connectDb.Open();
                //  NpgsqlCommand command = new NpgsqlCommand(strConnect, mDBConnection);

            }
            catch (Exception exception)
            {

                writeLogWarn(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + " " + exception.StackTrace.ToString());
            }
            finally
            {

            }
        }


        public void DbInitSourceConnection(String dBServer, String dBLogin, String dBPassword, String dbDatabase)
        {

            string strConnect = "Server=" + dBServer + ";user id=" + dBLogin + "; Password=" + dBPassword + "; Database=" + dbDatabase + "; ";

            msSqlConnect = new SqlConnection(strConnect);
            if (msSqlConnect.State != ConnectionState.Open)
            {
                try
                {
                    msSqlConnect.Open();
                    //  NpgsqlCommand command = new NpgsqlCommand(strConnect, mDBConnection);

                }
                catch (Exception exception)
                {
                    writeLogWarn(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + " " + exception.StackTrace.ToString());
                }
            }

        }


        private void createDbLog()
        {
            writeLog("Start sync");
        }

        public void checkDbConnection()
        {
            checkSourceDbConnection();
            checkTargetDbConnection();
        }

        

        private void initPostgeConnection()
        {
            DbInitConnection("host", "name", "pass");
        }

        private void initMsDConnection()
        {
            DbInitSourceConnection("host", "name", "pass", "empty");
        }

        private bool checkTargetDbConnection()
        {
            
            bool TargetDbState = false;
            if (connectDb.State == ConnectionState.Open)
            {
                TargetDbState = true;
            }
            else
            {
                try
                {
                    initPostgeConnection();
                    TargetDbState = true;
                }
                catch (Exception exception)
                {

                    writeLogWarn(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + " " + exception.StackTrace.ToString());
                    TargetDbState = false;
                    
                }

            }
            return TargetDbState;

        }


        private bool checkSourceDbConnection()
        {
            
            bool SourceDbState = false;
            if (msSqlConnect.State == ConnectionState.Open)
            {
                SourceDbState = true;
            }
            else
            {
                try
                {
                    initMsDConnection();
                    SourceDbState = true;
                }
                catch (Exception exception)
                {
                    writeLogWarn( exception.Message.ToString()+ " checkSourcedbConnection");
                    
                    SourceDbState = false;
                }
            }



            return SourceDbState;
        }
        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        private void syncDbMode()
        {
            DateTime cd = DateTime.Now;
           if (cd.Hour >= 8 && cd.Hour <= 16 && cd.DayOfWeek != DayOfWeek.Saturday && cd.DayOfWeek != DayOfWeek.Sunday)
            {
                initPostgeConnection();
                initMsDConnection();
                if (checkSourceDbConnection() == checkTargetDbConnection() == true)
                {
                    Thread awd = new Thread(new ThreadStart(autoWriteData));
                    awd.Start();
                    awd.Join();

                    Thread awdd = new Thread(new ThreadStart(autoWriteDiagData));
                    awdd.Start();
                    awdd.Join();
                    
                    msSqlConnect.Close();
                    connectDb.Close();
                }
           }
            startS();

        }

        private void startS()
        {
            startSync = new System.Timers.Timer();
            startSync.Interval = 1000000;
            startSync.Start();
            startSync.Elapsed += new System.Timers.ElapsedEventHandler(syncMode);
        }

        void syncMode(object sender, EventArgs e)
        {
            startSync.Stop();
            syncDbMode();

        }

        private int getMaxPinFromSource()
        {

            int maxPin = 0;


            SqlCommand chkMaxPin = new SqlCommand("Select MAX(pin) FROM patient;", msSqlConnect);

            try
            {

                maxPin = (int)chkMaxPin.ExecuteScalar();

            }
            catch (Exception exception)
            {
                writeLogWarn(exception.Message.ToString()+" getMaxPinFromSource");
            }
            finally
            {

            }

            return maxPin;
        }

        private int getMaxPinFromTarget()
        {

            int maxPin = 21500;


            NpgsqlCommand chkMaxPin = new NpgsqlCommand("Select MAX(pin) FROM patient_list ;", connectDb);

            try
            {

                maxPin = (int)chkMaxPin.ExecuteScalar();

            }
            catch (Exception exception)
            {
                writeLogWarn(exception.Message.ToString() +" getMaxPinFromTarget");
            }
            finally
            {

            }

            return maxPin;
        }


        private int getMaxDiagFromTarget()
        {

            int maxPin = 0;


            NpgsqlCommand chkMaxPin = new NpgsqlCommand("Select MAX(hospnumber) FROM diag_data ;", connectDb);

            try
            {

                maxPin = (int)chkMaxPin.ExecuteScalar();

            }
            catch (Exception exception)
            {
                writeLogWarn(exception.Message.ToString()+ " getMaxDiagFromTarget");
            }
            finally
            {

            }

            return maxPin;
        }
        private int getMaxDiagFromSource()
        {

            int maxPin = 0;


            SqlCommand chkMaxPin = new SqlCommand("Select MAX(hospnumber) FROM HStorys;", msSqlConnect);

            try
            {

                maxPin = (int)chkMaxPin.ExecuteScalar();

            }
            catch (Exception exception)
            {
                writeLogWarn(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + " " + exception.StackTrace.ToString());
            }
            finally
            {

            }

            return maxPin;
        }

        private void autoWriteDiagData()
        {
            checkDbConnection();
            NpgsqlConnection postgreConn = connectDb;
            
            int maxSourceDiag = getMaxDiagFromSource();
            int maxTargetDiag = getMaxDiagFromTarget();
            if (maxSourceDiag == maxTargetDiag)
            {
               //writeLog( "Базы диагнозов синхронизированы");
            
            }

            else
            {


                int maxSourcePin = getMaxPinFromSource();
                int maxTargetPin = getMaxPinFromTarget();
                if (maxSourcePin != maxTargetPin)
                {
                    writeLog("Необходимо синхронизировать базы пациентов");
                }
                else
                {

                    autoGetSourceDiagData();
                    try
                    {


                        NpgsqlTransaction trans = postgreConn.BeginTransaction();

                        foreach (DataRow row in gotData.Rows)
                        {

                            string quote_n_in = "";
                            string quote_n_in_value = "";
                            if (String.IsNullOrEmpty(row["talonnum"].ToString()) == false)
                            {
                                quote_n_in_value = ", '" + row["talonnum"] + "', '" + row["talondate"]+"' ";
                                quote_n_in = ", quote_num, quote_date";
                            }

                            string cap_in = "";
                            string cap_in_val = "";

                            if (String.IsNullOrEmpty(row["caption"].ToString()) == false)
                            {
                                cap_in = ", caption";
                                cap_in_val = ", '"+row["caption"]+"'";
                            }

                            if (String.IsNullOrEmpty(row["diagout"].ToString()) && String.IsNullOrEmpty(row["diagin"].ToString()) == false)
                            {
                                NpgsqlCommand CreateNewPatient = new NpgsqlCommand("INSERT INTO diag_data "
                                                 + " (hospnumber, division, diag_date, pat_id, pin, diag, type, main_mark "+quote_n_in+""+cap_in+")"
                                                 + " VALUES ('" + row["hospnumber"] + "', '" + row["division"] + "', '" + row["reporttime"] + "',(select MAX(pat_id) from patient_list where pin = " + row["pin"] + " ), '"
                                                 + row["pin"] + "', '" + row["diagin"] + "','1', 'true' " + quote_n_in_value + ""+cap_in_val+");", connectDb);
                                CreateNewPatient.ExecuteNonQuery();
                            }
                            else if (row["diagout"] == row["diagin"] && String.IsNullOrEmpty(row["diagout"].ToString()) == false)
                            {
                                NpgsqlCommand CreateNewPatient = new NpgsqlCommand("INSERT INTO diag_data "
                                                 + " (hospnumber, division, diag_date, pat_id, pin, diag, type, main_mark " + quote_n_in + "" + cap_in + ")"
                                                 + " VALUES ('" + row["hospnumber"] + "', '" + row["division"] + "', '" + row["reporttime"] + "',(select MAX(pat_id) from patient_list where pin = " + row["pin"] + " ), '"
                                                 + row["pin"] + "', '" + row["diagin"] + "','3','true' " + quote_n_in_value + "" + cap_in_val + ");", connectDb);
                                CreateNewPatient.ExecuteNonQuery();
                            }
                            else if (row["diagout"] != row["diagin"] && String.IsNullOrEmpty(row["diagout"].ToString()) == false && String.IsNullOrEmpty(row["diagin"].ToString()) == false)
                            {
                                NpgsqlCommand CreateNewPatient = new NpgsqlCommand("INSERT INTO diag_data "
                                                 + " (hospnumber, division, diag_date, pat_id, pin, diag, type, main_mark " + quote_n_in + "" + cap_in + ")"
                                                 + " VALUES ('" + row["hospnumber"] + "', '" + row["division"] + "', '" + row["reporttime"] + "',(select MAX(pat_id) from patient_list where pin = " + row["pin"] + " ), '" + row["pin"] + "', '"
                                                 + row["diagin"] + "','1','false' " + quote_n_in_value + "" + cap_in_val + ");", connectDb);
                                CreateNewPatient.ExecuteNonQuery();
                                NpgsqlCommand CreateNewPatient1 = new NpgsqlCommand("INSERT INTO diag_data "
                                                 + " (hospnumber, division, diag_date, pat_id, pin, diag, type, main_mark " + quote_n_in + "" + cap_in + ")"
                                                 + " VALUES ('" + row["hospnumber"] + "', '" + row["division"] + "', '" + row["reporttime"] + "',(select MAX(pat_id) from patient_list where pin = " + row["pin"] + " ) , '" + row["pin"] + "', '"
                                                 + row["diagout"] + "','3','true' " + quote_n_in_value + "" + cap_in_val + ");", connectDb);
                                CreateNewPatient1.ExecuteNonQuery();
                            }
                            else if (row["diagout"] != row["diagin"] && String.IsNullOrEmpty(row["diagout"].ToString()) == false && String.IsNullOrEmpty(row["diagin"].ToString()) == true)
                            {

                                NpgsqlCommand CreateNewPatient1 = new NpgsqlCommand("INSERT INTO diag_data "
                                                 + " (hospnumber, division, diag_date, pat_id, pin, diag, type, main_mark " + quote_n_in + "" + cap_in + ")"
                                                 + " VALUES ('" + row["hospnumber"] + "', '" + row["division"] + "', '" + row["reporttime"] + "',(select MAX(pat_id) from patient_list where pin = " + row["pin"] + " ), '"
                                                 + row["pin"] + "', '" + row["diagout"] + "','3', 'true' " + quote_n_in_value + "" + cap_in_val + ");", connectDb);
                                CreateNewPatient1.ExecuteNonQuery();
                            }
                            else if (String.IsNullOrEmpty(row["diagsec"].ToString()) == false)
                            {
                                NpgsqlCommand CreateNewPatient = new NpgsqlCommand("INSERT INTO diag_data "
                                      + " (hospnumber, division, diag_date, pat_id, pin, diag, type " + quote_n_in + "" + cap_in + ")"
                                      + " VALUES ('" + row["hospnumber"] + "', '" + row["division"] + "', '" + row["reporttime"] + "',(select MAX(pat_id) from patient_list where pin = " + row["pin"] + " ), '"
                                      + row["pin"] + "', '" + row["diagec"] + "','6' " + quote_n_in_value + "" + cap_in_val + ");", connectDb);
                                CreateNewPatient.ExecuteNonQuery();
                            }
                            else if (String.IsNullOrEmpty(row["diagsec1"].ToString()) == false)
                            {
                                NpgsqlCommand CreateNewPatient = new NpgsqlCommand("INSERT INTO diag_data "
                                      + " (hospnumber, division, diag_date, pat_id, pin, diag, type " + quote_n_in + "" + cap_in + ")"
                                      + " VALUES ('" + row["hospnumber"] + "', '" + row["division"] + "', '" + row["reporttime"] + "',(select MAX(pat_id) from patient_list where pin = "
                                      + row["pin"] + " ), '" + row["pin"] + "', '" + row["diagec1"] + "','6' " + quote_n_in_value + "" + cap_in_val + ");", connectDb);
                                CreateNewPatient.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                        maxSourceDiag = getMaxDiagFromSource();
                        maxTargetDiag = getMaxDiagFromTarget();
                        if (maxSourceDiag == maxTargetDiag)
                        {
                           
                            writeLogDiag("Записано " + gotData.Rows.Count + " диагнозов", Convert.ToInt16(gotData.Rows.Count));
                           

                        }

                        else
                        {
                            autoWriteDiagData();
                        }
                    }
                    catch (Exception exception)
                    {

                        writeLogWarn(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + " " + exception.StackTrace.ToString());
                        

                    }
                    finally
                    {
                       
                        Thread.Sleep(0);
                    }
                }
            }
        }


        private void autoGetSourceDiagData()
        {
            checkDbConnection();
            int maxSourceDiag = getMaxDiagFromSource();
            int maxTargetDiag = getMaxDiagFromTarget();
           
            if (maxSourceDiag == maxTargetDiag)
            {
                //writeLog("Базы синхронизированы");              
            }
            else
            {
                SqlDataAdapter getData = new SqlDataAdapter("select top 100 * from HStorys where hospnumber > " + maxTargetDiag + " order by hospnumber asc ;", msSqlConnect);
                gotData = new DataTable();
                try
                {
                    getData.Fill(gotData);
                  
                    //writeLog("gotDataDiag " + gotData.Rows.Count + " диагнозов");
                    
                }
                catch (Exception exception)
                {
                    writeLogWarn(exception.Message.ToString() + " autoGetSourceDiagData");
                }
                finally
                {

                }
            }

            // addressf village cmiflag act
        }

        private void autoWriteData()
        {
            
            checkDbConnection();
            int maxSourcePin = getMaxPinFromSource();
            int maxTargetPin = getMaxPinFromTarget();
            if (maxSourcePin == maxTargetPin)
            {
              
               // writeLog( "Базы пациентов синхронизированы");
                
            }
            else
            {
                autoGetSourceData();

                try
                {

                    NpgsqlConnection postgreConn = connectDb;
                    NpgsqlTransaction trans = postgreConn.BeginTransaction();

                    foreach (DataRow row in gotData.Rows)
                    {

                        bool man = false;

                        if (String.IsNullOrEmpty(row["sex"].ToString()) == false)
                        {
                            if (Convert.ToInt32(row["sex"]) == 1)
                            {
                                man = true;
                            }
                        }

                        if (row["cardprt"].ToString() == "")
                        {
                            row["cardprt"] = DateTime.Today;
                        }
                        if (row["doc_date"].ToString() == "")
                        {
                            row["doc_date"] = "01.01.1001";
                        }
                        if (row["Pol_Date"].ToString() == "")
                        {
                            row["Pol_Date"] = "01.01.1001";
                        }
                        if (row["Pol_DateTo"].ToString() == "")
                        {
                            row["Pol_DateTo"] = "01.01.1001";
                        }
                        if (row["birth"].ToString() == "")
                        {
                            row["birth"] = "01.01.1001";
                        }
                        if (row["sex"].ToString().Trim() == "")
                        {
                            row["sex"] = -1;
                        }

                        if (row["patgroup"].ToString().Trim() == "")
                        {
                            row["patgroup"] = -1;
                        }

                        if (row["Region1"].ToString().Trim() == "")
                        {
                            row["Region1"] = -1;
                        }
                        if (row["Region2"].ToString().Trim() == "")
                        {
                            row["Region2"] = -1;
                        }
                        if (row["Region3"].ToString().Trim() == "")
                        {
                            row["Region3"] = -1;
                        }

                        if (row["Town"].ToString().Trim() == "")
                        {
                            row["Town"] = -1;
                        }
                        if (row["Street"].ToString().Trim() == "")
                        {
                            row["Street"] = -1;
                        }
                        if (row["AddressF"].ToString().Trim() == "")
                        {
                            row["AddressF"] = -1;
                        }
                        if (row["Village"].ToString().Trim() == "")
                        {
                            row["Village"] = -1;
                        }
                        if (row["CMIFlag"].ToString().Trim() == "")
                        {
                            row["CMIFlag"] = -1;
                        }
                        if (row["CMICategory"].ToString().Trim() == "")
                        {
                            row["CMICategory"] = -1;
                        }
                        if (row["pol_type"].ToString().Trim() == "")
                        {
                            row["pol_type"] = -1;
                        }
                        if (row["pol_comp"].ToString().Trim() == "")
                        {
                            row["pol_comp"] = -1;
                        }
                        if (row["asession"].ToString().Trim() == "")
                        {
                            row["asession"] = -1;
                        }
                        if (row["birthweight"].ToString().Trim() == "")
                        {
                            row["birthweight"] = -1;
                        }
                        if (row["birthheight"].ToString().Trim() == "")
                        {
                            row["birthheight"] = -1;
                        }
                        if (row["AdvantageCategory"].ToString().Trim() == "")
                        {
                            row["AdvantageCategory"] = -1;
                        }
                        if (row["SocialStatus"].ToString().Trim() == "")
                        {
                            row["SocialStatus"] = -1;
                        }
                        if (row["doc_type"].ToString().Trim() == "")
                        {
                            row["doc_type"] = -1;
                        }
                        string addr = row["Address"].ToString();
                        addr = addr.Replace("\\", "\\\\");
                        addr = addr.Replace("\'", "\"");
                        string secAddr = row["SecAddress"].ToString();
                        secAddr = secAddr.Replace("\\", "\\\\");
                        string job = row["job"].ToString();
                        job = job.Replace("\\", "\\\\");
                        job = job.Replace("\'", "\"");
                        string building = row["Building"].ToString();
                        building = building.Replace("\\", "\\\\");
                        string flat = row["Flat"].ToString();
                        flat = flat.Replace("\\", "\\\\");
                        string house = row["House"].ToString();
                        house = house.Replace("\\", "\\\\");
                        string doc_org = row["doc_org"].ToString();
                        doc_org = doc_org.Replace("\\", "\\\\");
                        int int_nib = -1;
                        string nib = row["ambcard"].ToString();
                        //if (nib.Trim() != "")
                        if (String.IsNullOrEmpty(nib.ToString()) == false)
                        {
                            try
                            {
                                if (nib.Length > 3)
                                {
                                    nib = nib.Remove(nib.Length - 3, 1);
                                }
                            }
                            catch (Exception exception)
                            {
                                writeLogWarn(exception.Message.ToString() + " autowriteData1 ");
                             

                            }
                            finally
                            {
                                
                            }

                            int_nib = Convert.ToInt32(nib);
                        }
                        Int64 pass = -1;
                        if ((int)row["doc_type"] == 2)
                        {
                            if (String.IsNullOrEmpty(row["doc_ser"].ToString()) == false)
                            {
                                try
                                {
                                    int ser1 = Convert.ToInt32(row["doc_ser"].ToString().Trim());
                                    if (String.IsNullOrEmpty(row["doc_num"].ToString()) == false)
                                    {
                                        try
                                        {
                                            int ser2 = Convert.ToInt32(row["doc_num"].ToString().Trim());
                                            string docum = ser1.ToString() + "" + ser2.ToString();
                                            pass = Convert.ToInt64(docum);
                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }
                        }

                        NpgsqlCommand CreateNewPatient = new NpgsqlCommand("INSERT INTO patient_list "
                                         + " (pin,  family_name,  first_name,  last_name,  sex,  birth_date,  ambcard,  cardprt,  patgroup," +
                                         " advantagecategory,  socialstatus,  doc_type,  doc_ser,  doc_num,  doc_date,  doc_org,  region1,  region2,  region3, " +
                                         " town,  street,  house,  building,  construction,  flat,  addrindex,  address,  addressf,  secaddress,  village,  phone, " +
                                         " cmiflag,  cmicategory,  pol_type,  pol_ser,  pol_num,  pol_comp,  pol_date,  pol_dateto,  job,  birthweight,  birthheight, " +
                                         " registred,  snils,  asession,  adatetime,  act,  is_man, creator_id, street_n, nib, country, curr_doctor, town_id, pass, nib_date, state, area)"


                                         + " VALUES ('"
                                         + row["pin"] + "','" + row["lname"] + "','" + row["fname"] + "','" + row["sname"] + "','" + Convert.ToInt32(row["sex"]) + "','" + Convert.ToDateTime(row["birth"]).Date + "','" + row["ambcard"] + "','" + row["cardprt"] + "','" + row["patgroup"] + "','" +
                                          row["AdvantageCategory"] + "','" + row["SocialStatus"] + "','" + row["doc_type"] + "','" + row["doc_ser"] + "','" + row["doc_num"] + "','" + row["doc_date"] + "','" + doc_org + "','" + row["Region1"] + "','" + row["Region2"] + "','" + row["Region3"]
                                         + "','" + row["Town"] + "','" + row["Street"] + "','" + house + "','" + building + "','" + row["Construction"] + "','" + flat + "','" + row["Addrindex"] + "','" + addr + "','" + Convert.ToInt32(row["Addressf"]) +
                                         "','" + secAddr + "','" + Convert.ToInt32(row["Village"]) + "','" + row["phone"] + "', '" +
                                           Convert.ToInt32(row["CMIFlag"]) + "','" + row["CMICategory"] + "','" + row["pol_type"] + "','" + row["pol_ser"] + "','" + row["pol_num"] + "','" + row["pol_comp"] + "','" + row["Pol_Date"] + "','" + row["Pol_DateTo"] + "','" + job + "','" + row["birthweight"] + "','" + row["birthheight"] + "','" +
                                          row["registred"] + "','" + row["SNILS"] + "','" + row["asession"] + "','" + row["adatetime"] + "','" + Convert.ToInt32(row["act"]) + "','" + man + "','1',(select MIN(serial) from streets where street = " + row["street"] + "),'" + int_nib +
                                          "','1','2',(select int_value from towns t where town = " + row["Town"] + " ),'" + pass + "','" + row["cardprt"] + "', (select state from region a where a.int_value = " + row["Region1"] + "), (select area from streets b where b.serial = " + row["Street"] + ") );", connectDb);
                        CreateNewPatient.ExecuteNonQuery();


                    }
                    trans.Commit();
                    maxSourcePin = getMaxPinFromSource();
                    maxTargetPin = getMaxPinFromTarget();
                    if (maxSourcePin == maxTargetPin)
                    {

                        writeLogPat("Записано " + gotData.Rows.Count + " пациентов", Convert.ToInt16(gotData.Rows.Count));
                        
                    }
                    else
                    {
                        autoWriteData();
                    }
                }
                catch (Exception exception)
                {
                    writeLogWarn(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + " " + exception.StackTrace.ToString());
                 }
                finally
                {
                    Thread.Sleep(0);
                }
            }
        }

        private void autoGetSourceData()
        {
            
            checkDbConnection();
            int maxSourcePin = getMaxPinFromSource();
            int maxTargetPin = getMaxPinFromTarget();
            if (maxSourcePin == maxTargetPin)
            {
               // writeLog( "Базы пациентов синхронизированы");
                
            }
            else
            {
                SqlDataAdapter getData = new SqlDataAdapter("select top 100 * from Patient where pin > " + maxTargetPin + " order by pin asc ;", msSqlConnect);
                gotData = new DataTable();
                try
                {
                    getData.Fill(gotData);
                  //  writeLog( "gotDataPatient " + gotData.Rows.Count + " rows");
               
                }
                catch (Exception exception)
                {
                    writeLogWarn(exception.Message.ToString() + " " + MethodBase.GetCurrentMethod().Name + " " + exception.StackTrace.ToString());
                }
                finally
                {

                }
            }

            // addressf village cmiflag act
        }









    }
}
