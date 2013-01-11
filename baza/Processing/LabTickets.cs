using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;
using System.Collections;

namespace baza.Processing
{
    class LabTickets
    {
        public class Ticket
        {
            public Int64 TicketId { get; set; }
            public int PatientId { get; set; }
            public int TemplateId { get; set; }
            public long LabTicketNumber { get; set; }
            public string PatientFullName { get; set; }
            public string PatientCart { get; set; }
            public string PatientBirth { get; set; }
            public string TemplateName { get; set; }
        }

        public partial class TicketList : List<Ticket>
        {
            public void TicketListGet(int _labId, DateTime datet)
            {
                
                DataTable dtTickets = new DataTable();
                string _command = "Select ticket.ticket_id, ticket.pat_id, ticket.service_id, ticket.lab_sample_number, patient_list.family_name, patient_list.first_name, patient_list.last_name, "
                    + " patient_list.birth_date, patient_list.pin, patient_list.ambcard, codes.txt from ticket JOIN patient_list ON ticket.pat_id = patient_list.pat_id "
                    +" JOIN codes ON ticket.service_id = codes.code_id where ticket.lab_id ='" + _labId + "' and ticket.date_in = '" + datet.ToShortDateString() +
                    "' and ticket.template = 'true' order by pat_id asc ;";
                NpgsqlDataAdapter getTickets = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getTickets.Fill(dtTickets);
                    foreach (DataRow ro in dtTickets.Rows)
                    {
                        Ticket samTy = new Ticket();
                       samTy.TicketId = (Int64)ro["ticket_id"];
                       samTy.PatientId = (int)ro["pat_id"];
                       samTy.TemplateId = (int)ro["service_id"];
                       samTy.LabTicketNumber = (long)ro["lab_sample_number"];
                       samTy.PatientFullName = ((string)ro["family_name"]).Trim() + " " + ((string)ro["first_name"]).Trim() + " " + ((string)ro["last_name"]).Trim();
                       samTy.PatientCart = ((string)ro["ambcard"]).Trim() + " (" + ((int)ro["pin"]).ToString() + ") ";
                       samTy.PatientBirth = ((DateTime)ro["birth_date"]).ToShortDateString();
                       samTy.TemplateName = ((string)ro["txt"]).Trim();
                        this.Add(samTy);
                    }


                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                }


            }



        }


        public class RadioTicket
        {
            public Int64 TicketId { get; set; }
            public int PatientId { get; set; }
            
            public string PatientFullName { get; set; }
            public string PatientCart { get; set; }
            public string PatientBirth { get; set; }
           
        }

        public partial class TicketListRadio : List<RadioTicket>
        {
            public void RadioListGet(int _labId, DateTime datet)
            {

                DataTable dtTickets = new DataTable();
                string _command = "Select ticket_radio.ticket_id, ticket_radio.pat_id, ticket_radio.type_id, patient_list.family_name, patient_list.first_name, patient_list.last_name, "
                    + " patient_list.birth_date, patient_list.pin, patient_list.ambcard, codes.txt from ticket_radio JOIN patient_list ON ticket_radio.pat_id = patient_list.pat_id "
                    + " JOIN codes ON ticket_radio.type_id = codes.code_id where ticket_radio.type_id ='" + _labId + "' and ticket_radio.date_in = '" + datet.ToShortDateString() +
                    "' ; ";
                NpgsqlDataAdapter getTickets = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getTickets.Fill(dtTickets);
                    foreach (DataRow ro in dtTickets.Rows)
                    {
                        RadioTicket samTy = new RadioTicket();
                        samTy.TicketId = (Int64)ro["ticket_id"];
                        samTy.PatientId = (int)ro["pat_id"];
                        

                        samTy.PatientFullName = ((string)ro["family_name"]).Trim() + " " + ((string)ro["first_name"]).Trim() + " " + ((string)ro["last_name"]).Trim();
                        samTy.PatientCart = ((string)ro["ambcard"]).Trim() + " (" + ((int)ro["pin"]).ToString() + ") ";
                        samTy.PatientBirth = ((DateTime)ro["birth_date"]).ToShortDateString();
                        
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


        public class NormClass
        {
            public int NormID { get; set; }
            public string NormName { get; set; }
        }

        public partial class NormClassList : List<NormClass>
        {
            public void getNormsByType(int _sampleType)
            {
                DataTable dtTemplNorms = new DataTable();


                string _command = "Select norm_name, norm_id from norms where type = '"+_sampleType+"' ;";
                NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getNorm.Fill(dtTemplNorms);
                    this.Clear();
                    foreach (DataRow ro in dtTemplNorms.Rows)
                    {
                        NormClass nl = new NormClass();
                        nl.NormID = (int)ro["norm_id"];
                        nl.NormName = (string)ro["norm_name"];
                        this.Add(nl);
                    }

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    
                }

            }
        }


        public class NormClassFull
        {
            public int NormID { get; set; }
            public string NormName { get; set; }
            public string NormShortName { get; set; }
            public string NormIntName { get; set; }
            public short NormUnitId { get; set;  }
            public short NormType {get; set; }
            public int NormReestrId { get; set; }
              
        }

        public partial class NormFullList : List<NormClassFull>
        {
            
            public void getNormByType(int _sampleId)
            {
                DataTable dtTemplNorms = new DataTable();
                NormClassFull ncf = new NormClassFull();
                string _command = "Select * from norms where norm_id = '" + _sampleId + "' ;";
                NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getNorm.Fill(dtTemplNorms);
                    if (dtTemplNorms.Rows.Count > 0)
                    {

                        foreach (DataRow ro in dtTemplNorms.Rows)
                        {
                            ncf.NormID = (int)ro["norm_id"];
                            ncf.NormName = (string)ro["norm_name"];
                            if (Convert.IsDBNull(ro["norm_int_name"]) == false)
                            {
                                ncf.NormIntName = (string)ro["norm_int_name"];
                            }
                            else 
                            { ncf.NormIntName = ""; }
                            if (Convert.IsDBNull(ro["norm_short_name"]) == false)
                            {
                            ncf.NormShortName = (string)ro["norm_short_name"];
                            }
                            else
                            { ncf.NormShortName = ""; }
                            if (Convert.IsDBNull(ro["unit_id"]) == false)
                            {
                                ncf.NormUnitId = (short)ro["unit_id"];
                            }
                            else
                            {
                                ncf.NormUnitId = 0;
                            }
                            ncf.NormType = (short)ro["type"];
                            if (Convert.IsDBNull(ro["reestr_id"]) == false)
                            {
                                ncf.NormReestrId = (int)ro["reestr_id"];

                            }
                            else
                            {

                                ncf.NormReestrId = 0;
                            }
                            
                            this.Add(ncf);
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




        public class NormAges
        {
            public int NormAgeId { get; set; }
            public int NormAgeFrom { get; set; }
            public int NormAgeTo { get; set; }
        }

        public partial class NormAgeList : List<NormAges>
        {
            public void getNormAges()
            {
                DataTable dtTemplNorms = new DataTable();


                string _command = "Select * from norm_ages ;";
                NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getNorm.Fill(dtTemplNorms);
                    this.Clear();
                    foreach (DataRow ro in dtTemplNorms.Rows)
                    {
                        NormAges nl = new NormAges();
                        nl.NormAgeId= (int)ro["age_id"];
                        nl.NormAgeFrom= (int)ro["age_from"];
                        nl.NormAgeTo = (int)ro["age_to"];
                        this.Add(nl);
                    }

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    
                }

            }
        }


        public class NormLimit
        {
            public int NormID { get; set; }
            public string NormName { get; set; }
            public int NormLimitId { get; set; }
        }

        public partial class NormLimitList : List<NormLimit>
        {
            public void getNorms(Int64 _thisTicket)
            {
                DataTable dtTemplNorms = new DataTable();


                string _command = "Select lab_results.test_name_id, norm_limits.test_name, norm_limits.norm_id from lab_results JOIN norm_limits ON "
                    + " lab_results.test_name_id = norm_limits.test_name_id where lab_results.ticket_id = '" + 
                    _thisTicket + "' and lab_results.got_data = false ;";
                NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getNorm.Fill(dtTemplNorms);
                    this.Clear();
                    foreach (DataRow ro in dtTemplNorms.Rows)
                    {
                        NormLimit nl = new NormLimit();
                        nl.NormID = (int)ro["test_name_id"];
                        nl.NormName = (string)ro["test_name"];
                        nl.NormLimitId = (int)ro["norm_id"];
                        this.Add(nl);
                    }
                    
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    
                }

            }


            public void loadLabNormLimits(int _labID, short _sampleType)
            {
                DataTable dtLabNorms = new DataTable();
                string _command = "Select norm_id, test_name, test_name_id from norm_limits where lab_id is null and delete_this is not true ;";

                if (_labID == -2)
                {
                    _command = "Select norm_id, test_name , test_name_id from norm_limits where delete_this is not true ;";
                }

                else
                    if (_labID > 0)
                    {
                        _command = "Select norm_id, test_name, test_name_id from norm_limits where lab_id = '" + _labID + "' and type = '" + _sampleType + "' and delete_this is not true ;";
                    }



                NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getNorm.Fill(dtLabNorms);
                    this.Clear();
                    if (dtLabNorms.Rows.Count > 0)
                    {
                        foreach (DataRow ro in dtLabNorms.Rows)
                        {
                            NormLimit nl = new NormLimit();
                            nl.NormID = (int)ro["test_name_id"];
                            nl.NormName = (string)ro["test_name"];
                            nl.NormLimitId = (int)ro["norm_id"];
                            this.Add(nl);

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

        public class NormLimitClassFull
        {
            public int NormID { get; set; }
            public string NormName { get; set; }
            public int NormLimitId { get; set; }
            public short NormType { get; set; }
            public string NormLimitCode { get; set; }
            public DateTime NormFrom { get; set; }
            public DateTime NormTo { get; set; }
            public short NormUnitId { get; set; }
        }

        public partial class NormLimitClassFullList : List<NormLimitClassFull>
        {
            

            public void loadLabNormLimits(int _thisNorm)
            {
                DataTable dtLabNorms = new DataTable();
                
                  
                string _command = "Select * from norm_limits where test_name_id = '" + _thisNorm + "' ;";
                NpgsqlDataAdapter getNorm = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getNorm.Fill(dtLabNorms);
                    this.Clear();
                    if (dtLabNorms.Rows.Count > 0)
                    {
                        foreach (DataRow ro in dtLabNorms.Rows)
                        {
                            NormLimitClassFull nl = new NormLimitClassFull();
                            nl.NormID = (int)ro["test_name_id"];
                            if (ro["test_name"] is DBNull)
                            {
                                nl.NormName = "";
                            }
                            else
                            {
                                nl.NormName = (string)ro["test_name"];
                            }
                            nl.NormLimitId = (int)ro["norm_id"];
                            nl.NormType = (short)ro["type"];
                            if (ro["test_code"] is DBNull)
                            {
                                nl.NormLimitCode = "";
                            }
                            else
                            {
                                nl.NormLimitCode = (string)ro["test_code"];
                            }
                            nl.NormFrom = (DateTime)ro["from_ot"];
                            nl.NormTo = (DateTime)ro["to_do"];
                            if (ro["Unit_id"] is DBNull)
                            {
                                nl.NormUnitId = -1;
                            }
                            else
                            {
                                nl.NormUnitId = (short)ro["unit_id"];
                            }
                            this.Add(nl);

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
        

        public class PatientTicket
        {
            public string TicketName { get; set; }
            public Int64 TicketId { get; set; }
            public int TemplateId { get; set; }
            public DateTime TicketDateIn { get; set; }
            public DateTime TicketDateApp { get; set; }
            public DateTime TicketDateOut { get; set; }
            public string TicketDoctorFName { get; set; }
            public int TicketDoctorIdIn { get; set; }
            public int TicketDoctorIdApp { get; set; }
            public int TicketDoctorIdOut { get; set; }
            public bool TicketIsTemplate { get; set; }
            public int TicketPatientId { get; set; }
            public long TicketLabSampleId { get; set; }
        }


        public partial class PatientTicketList : List<PatientTicket>
        {
            public void GetPatientTicketsNotFinished(int _pidn)
            {
                try
                {
                    
                    string _command = "Select trim(rr.name) as name, service_id, ticket_id, date_in, "
                    + "date_app, date_out, trim(dt.family_name) as family_name, doc_in, doc_app, doc_out, template, lab_sample_number "
                    + "from ticket tt, reestr rr, doctors dt where pat_id = "
                    + _pidn + " and template = false and rr.reestr = tt.service_id and dt.doc_id = tt.doc_in and date_out is null "
                    + "UNION "
                    + "Select trim(cs.txt) as name, service_id, ticket_id, date_in, "
                    + "date_app, date_out, trim(dt.family_name) as family_name, doc_in, doc_app, doc_out, template, lab_sample_number "
                    + "from ticket tt, codes cs, doctors dt where pat_id = "
                    + _pidn + " and template = true and cs.code_id = tt.service_id and dt.doc_id = tt.doc_in and date_out is null order by date_in ASC ";

                    NpgsqlDataAdapter loadReestrTickets = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                    DataTable tlMyPatientTicket = new DataTable();
                    loadReestrTickets.Fill(tlMyPatientTicket);
                    foreach (DataRow roww in tlMyPatientTicket.Rows)
                    {

                        PatientTicket pt = new PatientTicket();

                        pt.TicketName = (string)roww["name"];
                        pt.TicketId = (Int64)roww["ticket_id"];
                        pt.TemplateId = (int)roww["service_id"];
                        pt.TicketDateIn = (DateTime)roww["date_in"];

                            pt.TicketDateApp = (DateTime)roww["date_app"];
                            if (roww["date_out"] is DBNull)
                            {
                                pt.TicketDateOut = Convert.ToDateTime("1111.11.11"); 
                            }
                            else
                            {
                                pt.TicketDateOut = (DateTime)roww["date_out"];
                            }
                        pt.TicketDoctorFName = (string)roww["family_name"];
                        pt.TicketDoctorIdIn = (int)roww["doc_in"];
                        if (roww["doc_app"] is DBNull) 
                        {
                            pt.TicketDoctorIdApp = -1;
                        }
                        else
                        {
                            pt.TicketDoctorIdApp = (int)roww["doc_app"];
                        }
                        if (roww["doc_out"] is DBNull)
                        {
                            pt.TicketDoctorIdOut = -1;
                        }
                        else
                        {
                            pt.TicketDoctorIdOut = (int)roww["doc_out"];
                        }
                        pt.TicketIsTemplate = (bool)roww["template"];
                        pt.TicketLabSampleId = (long)roww["lab_sample_number"];
                        pt.TicketPatientId = _pidn;
                        this.Add(pt);
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    ;
                }
            }

            public void GetPatientTicketsFinished(int _pidn, DateTime _dateIn, DateTime _dateOut)
            {
                try
                {
                    //  and dt.doc_id = tt.doc_out
                    string _command = "Select trim(cs.txt) as name, service_id, ticket_id, date_in, doc_out, date_out, template, trim(dt.family_name) as family_name, lab_sample_number "
                    + "from ticket tt, codes cs, doctors dt where  pat_id = " + _pidn + " and template = true and cs.code_id = tt.service_id and dt.doc_id = tt.doc_out and date_out between '"
                    +_dateIn+"' and '"+_dateOut+"' order by date_out ASC ";

                    NpgsqlDataAdapter loadReestrTickets = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                    DataTable tlMyPatientTicket = new DataTable();
                    loadReestrTickets.Fill(tlMyPatientTicket);
                    foreach (DataRow roww in tlMyPatientTicket.Rows)
                    {

                        PatientTicket pt = new PatientTicket();

                        pt.TicketName = (string)roww["name"];
                        pt.TicketId = (Int64)roww["ticket_id"];
                        pt.TemplateId = (int)roww["service_id"];
                        pt.TicketDateIn = (DateTime)roww["date_in"];

                        pt.TicketDateOut = (DateTime)roww["date_out"];

                        pt.TicketDoctorFName = (string)roww["family_name"];
                        if (roww["doc_out"] is DBNull)
                        {
                            pt.TicketDoctorIdOut = -1;
                        }
                        else
                        {
                            pt.TicketDoctorIdOut = (int)roww["doc_out"];
                        }
                        pt.TicketLabSampleId = (long)roww["lab_sample_number"];
                        pt.TicketIsTemplate = (bool)roww["template"];
                        pt.TicketPatientId = _pidn;
                        this.Add(pt);
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    ;
                }
            }


        }


        public class PatientAnalysis
        {
            
            public string AName { get; set; }
            public decimal AValue { get; set; }
            public bool? AValueBool { get; set; }
            public string AValueText { get; set; }
            public DateTime ADate { get; set; }
            public Int64 ASample { get; set; }
            public String ASampleName { get; set; }
            public Int64 ATestId { get; set; }

        }

        public partial class PatientAnalysisList : List<PatientAnalysis>
        {
            public void GetTicketAnalysis(Int64 Ticket)
            {

                try
                {
                    // 
                    // 
                    // and lr.sample_id = bs.sample_id

                    string _command = "Select lr.test_id, lr.test_name_id, lr.sample_id, lr.value, lr.test_positive, lr.txt_data, trim(nl.test_name) as test_name, lr.date, bs.sample_name "
                    + "from lab_results lr, norm_limits nl, bio_sample bs where lr.ticket_id = "
                    + Ticket + " and lr.got_data = true and lr.test_name_id = nl.test_name_id and lr.sample_id = bs.sample_id order by test_id ASC ";

                    NpgsqlDataAdapter loadReestrTickets = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                    DataTable tlMyPatientTicket = new DataTable();
                    loadReestrTickets.Fill(tlMyPatientTicket);
                    if (tlMyPatientTicket.Rows.Count == 0)
                    {
                        _command = "Select lr.test_id, lr.test_name_id, lr.sample_id, lr.value, lr.test_positive, lr.txt_data, trim(nl.test_name) as test_name, lr.date "
                        + "from lab_results lr, norm_limits nl where lr.ticket_id = "
                        + Ticket + " and lr.got_data = true and lr.test_name_id = nl.test_name_id order by test_id ASC ";
                        loadReestrTickets = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                        loadReestrTickets.Fill(tlMyPatientTicket);
                    }
                    foreach (DataRow roww in tlMyPatientTicket.Rows)
                    {

                        PatientAnalysis pa = new PatientAnalysis();

                        pa.AName= (string)roww["test_name"];
                        if (Convert.IsDBNull(roww["value"]) == false)
                        {
                            pa.AValue = (decimal)roww["value"];
                        }
                        if (Convert.IsDBNull(roww["test_positive"]) == true)
                        {
                            pa.AValueBool = null;
                            
                        }
                        else
                        {
                            pa.AValueBool = (bool)roww["test_positive"];
                        }
                        if (Convert.IsDBNull(roww["txt_data"]) == false)
                        {
                            pa.AValueText = (string)roww["txt_data"];
                        }
                        pa.ADate = (DateTime)roww["date"];
                        if ( (long)roww["sample_id"] == -1 )
                        {
                            pa.ASample = (Int64)roww["sample_id"];
                            pa.ASampleName = "не указан";
                        }
                            else if (Convert.IsDBNull(roww["sample_id"]) == false)
                        {

                            pa.ASample = (Int64)roww["sample_id"];

                            pa.ASampleName = (string)roww["sample_name"];
                        }


                        else 
                        {
                            pa.ASample = -1;
                            pa.ASampleName = "не указан";
                        }
                        pa.ATestId = (Int64)roww["test_id"];

                        this.Add(pa);
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());               
                    
                }
            }

            public void GetImportdTicketAnalysis(Int64 Ticket)
            {

                try
                {
                    // , bs.sample_name
                    // , bio_sample bs
                    // and lr.sample_id = bs.sample_id

                    string _command = "Select lr.test_id, lr.test_name_id, lr.sample_id, lr.value, lr.test_positive, lr.txt_data, trim(nl.test_name) as test_name, lr.date "
                    + "from lab_results lr, norm_limits nl where lr.ticket_id = "
                    + Ticket + " and lr.got_data = true and lr.test_name_id = nl.test_name_id  order by test_id ASC ";

                    NpgsqlDataAdapter loadReestrTickets = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                    DataTable tlMyPatientTicket = new DataTable();
                    loadReestrTickets.Fill(tlMyPatientTicket);
                    foreach (DataRow roww in tlMyPatientTicket.Rows)
                    {

                        PatientAnalysis pa = new PatientAnalysis();

                        pa.AName = (string)roww["test_name"];
                        if (Convert.IsDBNull(roww["value"]) == false)
                        {
                            pa.AValue = (decimal)roww["value"];
                        }
                        if (Convert.IsDBNull(roww["test_positive"]) == true)
                        {
                            pa.AValueBool = null;

                        }
                        else
                        {
                            pa.AValueBool = (bool)roww["test_positive"];
                        }
                        if (Convert.IsDBNull(roww["txt_data"]) == false)
                        {
                            pa.AValueText = (string)roww["txt_data"];
                        }
                        pa.ADate = (DateTime)roww["date"];
                        if (Convert.IsDBNull(roww["sample_id"]) == false)
                        {
                            pa.ASample = (Int64)roww["sample_id"];
                            pa.ASampleName = pa.ASample.ToString();
                        }
                        else
                        {
                            pa.ASample = -999;
                            pa.ASampleName = "получен из лаборатории";
                        }
                        pa.ATestId = (Int64)roww["test_id"];

                        this.Add(pa);
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());

                }
            }


            public void GetTicketAnalysisForFill(Int64 Ticket)
            {

                try
                {

                    string _command = "Select test_id, lr.test_name_id, sample_id, value , trim(nl.test_name) as test_name, date "
                    + "from lab_results lr, norm_limits nl where lr.ticket_id = '" + Ticket + "'  and lr.sample_id is null and lr.got_data = false and nl.test_name_id = lr.test_name_id order by test_id ASC ";

                    NpgsqlDataAdapter loadReestrTickets = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                    DataTable tlMyPatientTicket = new DataTable();
                    loadReestrTickets.Fill(tlMyPatientTicket);
                    foreach (DataRow roww in tlMyPatientTicket.Rows)
                    {

                        PatientAnalysis pa = new PatientAnalysis();

                        pa.ATestId = (Int64)roww["test_id"];
                        
                        pa.AValue = -9999;

                        pa.AValueText = "";

                       pa.AValueBool = null;

                        pa.AName = (string)roww["test_name"];
                       
                        pa.ADate = (DateTime)roww["date"];
                        

                        this.Add(pa);
                    }
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    ;
                }
            }



        }


    }
}
