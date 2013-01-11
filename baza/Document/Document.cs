using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;
using System.Collections;

namespace baza.Documentation
{
    //public interface IDocument
    //{
    //    int TypeId { get; }
    //    Int64 Serial { get; }
    //    Int32 SignedUser { get; }
    //    DateTime Date { get; }
        
    //    String DocumentShortName { get; }
    //     String DocumentDoctor { get; }
    //     String DocumentPatient { get; }
    //    String MedexDataTable { get; }
    //    String DocumentBody { get; }
    //    String DocumentHeader { get; }
    //    Boolean SetDeleted { get; }
    //    Int64 DocumentAe { get;  }

    //}


   // public sealed class Document : IDocument 
    public class Document
    {
        public Int32 TypeId;
        public Int64 Serial;
        public Int32 SignedUser;
        public DateTime Date;
        public String DocumentShortName;
        public String DocumentDoctor;
        public String DocumentPatient;
        public String MedexDataTable;
        public String DocumentBody;
        public String DocumentHeader;
        public Boolean SetDeleted;
        public Int64 DocumentAe;
        public Int64 DocumentTableId;

       //public Int32 TypeId{ get; private set; }
       //public Int64 Serial{ get; private set; }
       //public Int32 SignedUser{ get; private set; }
       //public DateTime Date{ get; private set; }
       //public String DocumentShortName{ get; private set; }
       //public String DocumentDoctor { get; private set; }
       //public String DocumentPatient { get; private set; }
       //public String MedexDataTable{ get; private set; }
       //public String DocumentBody{ get; private set; }
       //public String DocumentHeader{ get; private set; }
       //public Boolean SetDeleted{ get; private set; }
       //public Int64 DocumentAe { get; private set; }
    }

       //public Document
       //    (
       //         Int32 _TypeId,
       //         Int64 _Serial,
       //         Int32 _SignedUser,
       //         DateTime _Date,
       //         String _DocumentShortName,
       //         String _DocumentDoctor, 
       //         String _DocumentPatient,
       //         String _MedexDataTable,
       //         String _DocumentBody,
       //         String _DocumentHeader,
       //         Boolean _SetDeleted,
       //         Int64 _DocumentAe

       //    )
       //    {
       //        TypeId = _TypeId;
       //        Serial = _Serial;
       //        SignedUser = _SignedUser;
       //        Date = _Date;          
       //        DocumentShortName = _DocumentShortName;
       //        DocumentDoctor = _DocumentDoctor;
       //        DocumentPatient = _DocumentPatient;
       //        MedexDataTable = _MedexDataTable;
       //        DocumentBody = _DocumentBody;
       //        DocumentHeader = _DocumentHeader;
       //        SetDeleted = _SetDeleted;
       //        DocumentAe = _DocumentAe;

       //    }

 

  //  }

        public partial class DocumentsList : List<Document>
        {
            public void Get50UsrDocuments(Int32 _usrId)
            {

                DataTable usrDocumentsTable = new DataTable();

                try
                {
                    NpgsqlDataAdapter dbJour = new NpgsqlDataAdapter("SELECT d.*, doc.*, pl.family_name as pfaname, pl.first_name as pfname, pl.last_name as plname, dt.descr, dt.table "
                        +" from documents d, doctors doc, patient_list pl, docum_type dt WHERE d.doc_id= '"
                      + _usrId + "' and pl.pat_id = d.pat_id  AND delete = false and dt.did = d.document_type ORDER BY document_date DESC LIMIT 50;", DBExchange.Inst.connectDb);
                    dbJour.Fill(usrDocumentsTable);
                    if (usrDocumentsTable.Rows.Count > 0)
                    {
                        ProcessDataTable(usrDocumentsTable);
                    }

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }


            }


            public void Get50PatientDocuments(Int32 _patId)
            {

                DataTable usrDocumentsTable = new DataTable();

                try
                {

                    NpgsqlDataAdapter selectPatientDocuments = new NpgsqlDataAdapter("Select d.*, doc.*, pl.family_name as pfaname, pl.first_name as pfname, pl.last_name as plname, dt.descr, dt.table "
                        +" from documents d, doctors doc, patient_list pl, docum_type dt where d.pat_id ='"
                    + _patId + "' and pl.pat_id = d.pat_id  AND delete=false and dt.did = d.document_type ORDER BY document_number DESC LIMIT 50", DBExchange.Inst.connectDb);
                    selectPatientDocuments.Fill(usrDocumentsTable);
                    if (usrDocumentsTable.Rows.Count > 0)
                    {
                        ProcessDataTable(usrDocumentsTable);
                    }

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }


            }

            public void GetPatientDocumentsByDate(Int32 _patId, DateTime dateFrom, DateTime dateTo)
            {

                DataTable usrDocumentsTable = new DataTable();

                try
                {

                    NpgsqlDataAdapter selectPatientDocuments = new NpgsqlDataAdapter("Select d.*, doc.*, pl.family_name as pfaname, pl.first_name as pfname, pl.last_name as plname, dt.descr, dt.table "
                        +" from documents d, doctors doc, patient_list pl, docum_type dt where d.pat_id ='"
                    + _patId + "' and pl.pat_id = d.pat_id  AND delete=false AND document_date between '" + dateFrom + "' AND '" + dateTo + "' and dt.did = d.document_type and doc.doc_id = d.doc_id ORDER BY document_date ASC", DBExchange.Inst.connectDb);
                    selectPatientDocuments.Fill(usrDocumentsTable);
                    if (usrDocumentsTable.Rows.Count > 0)
                    {
                        ProcessDataTable(usrDocumentsTable);
                    }

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }


            }


            public void Get50PatientDocumentsWithoutAE(Int32 _patId)
            {

                DataTable usrDocumentsTable = new DataTable();

                try
                {

                    NpgsqlDataAdapter selectPatientDocuments = new NpgsqlDataAdapter("Select d.*, doc.*, pl.family_name as pfaname, pl.first_name as pfname, pl.last_name as plname, dt.descr, dt.table "
                        +" from documents d, doctors doc, patient_list pl, docum_type dt where d.pat_id ='"
                    + _patId + "' and pl.pat_id = d.pat_id AND delete=false and ae_occured is null and doc.doc_id = d.doc_id and dt.did = d.document_type ORDER BY document_number DESC LIMIT 50", DBExchange.Inst.connectDb);
                    selectPatientDocuments.Fill(usrDocumentsTable);
                    if (usrDocumentsTable.Rows.Count > 0)
                    {
                        ProcessDataTable(usrDocumentsTable);
                    }

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }


            }


            private void ProcessDataTable(DataTable _thisTable)
            {

                this.Clear();

                try
                {
                    foreach (DataRow i in _thisTable.Rows)
                    {
                        string doctor = ((string)i["family_name"]).Trim() + " " + i["first_name"].ToString().Substring(0, 1) + ". " + i["last_name"].ToString().Substring(0, 1) + ".";
                        string patient = ((string)i["pfaname"]).Trim() + " " + i["pfname"].ToString().Substring(0, 1) + ". " + i["plname"].ToString().Substring(0, 1) + ".";
                        Document ido = new Document();
                        ido.Date = (DateTime)i["document_date"];
                        if (Convert.IsDBNull(i["ae_occured"]) == false)
                        {
                            ido.DocumentAe = (long)i["ae_occured"];
                        }
                        else
                        {
                            ido.DocumentAe = 0;
                        }
                        ido.DocumentDoctor = doctor;
                        ido.DocumentHeader = (string)i["document_header"];
                        ido.DocumentPatient = patient;
                        ido.DocumentShortName =(string)i["descr"];
                        ido.MedexDataTable = (string)i["table"];
                        ido.Serial =  (long)i["document_number"];
                        ido.SetDeleted = (bool)i["delete"];
                        ido.SignedUser = (int)i["doc_id"];
                        ido.TypeId = (int)i["document_type"];
                        ido.DocumentTableId = (long)i["document_id"];

                        this.Add(ido);
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
