using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;
using System.Collections;


namespace baza.UserClass
{
    class Doctors
    {

        public class Identity
        {
            public int DoctorId { get; set; }
            public Int16 DoctorGroup { get; set; }
            public Int16 DoctorStatus { get; set; }
            public Int16 DoctorSpecial { get; set; }

            public string DoctorFamilyName { get; set; }
            public string DoctorFirstName { get; set; }
            public string DoctorLastName { get; set; }
            public string DoctorFullName { get; set; }

            public bool DoctorTrialIn { get; set; }
            public string DoctorUserName { get; set; }

        }

        public class IdentityList : List <Identity>
        {

            public void getIdentityListByGroup(short _group)
            {

                DataTable dtIdent = new DataTable();
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter("Select * from doctors where otdel = '"+_group+"' ;", DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtIdent);
                    foreach (DataRow ro in dtIdent.Rows)
                    {
                        Identity _Ident = new Identity();

                        _Ident.DoctorFamilyName = (string)ro["family_name"];
                        _Ident.DoctorFirstName = (string)ro["first_name"];
                        _Ident.DoctorLastName = (string)ro["last_name"];
                        _Ident.DoctorFullName = (string)ro["family_name"] + " " + ((string)ro["first_name"]).Substring(0, 1) + ". " + ((string)ro["last_name"]).Substring(0,1)+"." ;
                       
                        _Ident.DoctorTrialIn = (bool)ro["trial_participation"];
                        _Ident.DoctorUserName = (string)ro["user_name"];

                        _Ident.DoctorId = (int)ro["doc_id"];
                        _Ident.DoctorGroup = (short)ro["otdel"];
                        _Ident.DoctorSpecial = (short)ro["special"];
                        _Ident.DoctorStatus = (short)ro["status"];

                        this.Add(_Ident);
                    }


                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    
                }


            }


            public void getIdentityListByGroupID(short _groupId)
            {

                DataTable dtIdent = new DataTable();
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter("Select * from doctors where otdel in (Select division from divisions where divisions.group = '" + _groupId + "') ;", DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtIdent);
                    if (dtIdent.Rows.Count > 0)
                    {
                        foreach (DataRow ro in dtIdent.Rows)
                        {
                            Identity _Ident = new Identity();

                            _Ident.DoctorFamilyName = (string)ro["family_name"];
                            _Ident.DoctorFirstName = (string)ro["first_name"];
                            _Ident.DoctorLastName = (string)ro["last_name"];
                            _Ident.DoctorFullName = (string)ro["family_name"] + " " + ((string)ro["first_name"]).Substring(0, 1) + ". " + ((string)ro["last_name"]).Substring(0, 1) + ".";

                            _Ident.DoctorTrialIn = (bool)ro["trial_participation"];
                            _Ident.DoctorUserName = (string)ro["user_name"];

                            _Ident.DoctorId = (int)ro["doc_id"];
                            _Ident.DoctorGroup = (short)ro["otdel"];
                            _Ident.DoctorSpecial = (short)ro["special"];
                            _Ident.DoctorStatus = (short)ro["status"];

                            this.Add(_Ident);
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
