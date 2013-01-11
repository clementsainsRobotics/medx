using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;
using System.Collections;

namespace baza.Editor
{
    class ClassRadiologyItem
    {

        public class MagneticResearches
        {

            public int ResearchId { get; set; }
            public Int16 ResearchGroupCode { get; set; }
            public string ResearchName { get; set; }

        }

        public partial class ResearchList : List<MagneticResearches>
        {

            public void GetResearchList()
            {
                DataTable dtSampleTypes = new DataTable();
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter("Select txt, code_id, code from codes where grp = '7' ;", DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    foreach (DataRow ro in dtSampleTypes.Rows)
                    {
                        MagneticResearches magRes = new MagneticResearches();
                        magRes.ResearchName = (string)ro["txt"];
                        magRes.ResearchId = (int)ro["code_id"];
                        magRes.ResearchGroupCode = (Int16)ro["code"];
                        this.Add(magRes);
                    }


                }
                catch (Exception exception)
                {
                     Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    
                }



            }


        }


        public class ResearchZones
        {

            public int ZoneId { get; set; }
            public string ZoneName { get; set; }
            public int UserId { get; set; }
            public string UserName { get; set; }
            public Int16 ZoneType { get; set; }
           

        }

        public partial class ZoneList : List<ResearchZones>
        {
            public void GetZoneList(int _zoneType, bool _own)
            {

                DataTable dtSampleTypes = new DataTable();
                string _command = "Select descr_zone_id, descr_text, type, doc_in, doctors.family_name as fname"
                + " from zones JOIN doctors on zones.doc_in = doctors.doc_id where zones.type = '" + _zoneType + "';";
                if (_own == true)
                {
                    _command = "Select descr_zone_id, descr_text, type, doc_in, doctors.family_name as fname"
               + " from zones JOIN doctors on zones.doc_in = doctors.doc_id where zones.type = '" + _zoneType + "' and doc_in =  '"+DBExchange.Inst.dbUsrId+"';";
                }
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    if (dtSampleTypes.Rows.Count > 0)
                    {
                        foreach (DataRow ro in dtSampleTypes.Rows)
                        {
                            ResearchZones ResZone = new ResearchZones();
                            ResZone.UserId = (int)ro["doc_in"];
                            ResZone.UserName = (string)ro["fname"];
                            ResZone.ZoneId = (int)ro["descr_zone_id"];
                            ResZone.ZoneName = (string)ro["descr_text"];
                            ResZone.ZoneType = (Int16)ro["type"];

                            this.Add(ResZone);
                        }
                    }

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    
                }



            }

            public void GetPatientZoneList(Int64 _ticket)
            {

                DataTable dtSampleTypes = new DataTable();
                string _command = "Select ticket_id, template_id, zones.descr_zone_id, zones.descr_text, zones.type, zones.doc_in, doctors.family_name as fname from lab_results_radio lrr JOIN " 
                    +" zones ON lrr.template_id = zones.descr_zone_id JOIN doctors on zones.doc_in = doctors.doc_id where lrr.ticket_id = '"+_ticket+"';";
               
                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    if (dtSampleTypes.Rows.Count > 0)
                    {
                        foreach (DataRow ro in dtSampleTypes.Rows)
                        {
                            ResearchZones ResZone = new ResearchZones();
                            ResZone.UserId = (int)ro["doc_in"];
                            ResZone.UserName = (string)ro["fname"];
                            ResZone.ZoneId = (int)ro["descr_zone_id"];
                            ResZone.ZoneName = (string)ro["descr_text"];
                            ResZone.ZoneType = (Int16)ro["type"];

                            this.Add(ResZone);
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


        public class ZoneTemplate
        {

            public int TemplateId { get; set; }
            public string TemplateName { get; set; }
            public string TemplateBody { get; set; }

        }


        public partial class ZoneTemplateList : List<ZoneTemplate>
        {

            public void GetZoneTemplate(int _zone, bool _own)
            {

                DataTable dtSampleTypes = new DataTable();

             //   string _zId = "";
             //   foreach (int i in _template)
             //   {
             //       _zId += ", " + i;
             //   }
             //  _zId.Substring(2, _zId.Length);
                string _command = "Select template, templ_id, template_name from zone_template where zone_id = '" + _zone + "';";
                if (_own == true)
                {
                    _command = "Select template, templ_id, template_name from zone_template where zone_id = '" + _zone + "' and doc_in = '"+DBExchange.Inst.dbUsrId+"';";
                }
                

                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    if (dtSampleTypes.Rows.Count > 0)
                    {
                        foreach (DataRow ro in dtSampleTypes.Rows)
                        {
                            ZoneTemplate ResZone = new ZoneTemplate();
                            ResZone.TemplateName = (string)ro["template_name"];
                            ResZone.TemplateBody = (string)ro["template"];
                            ResZone.TemplateId = (int)ro["templ_id"];


                            this.Add(ResZone);
                        }
                    }

                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();    
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
                    
                }


            }

            public void GetTicketTemplate(long _ticket)
            {

                DataTable dtSampleTypes = new DataTable();

                //   string _zId = "";
                //   foreach (int i in _template)
                //   {
                //       _zId += ", " + i;
                //   }
                //  _zId.Substring(2, _zId.Length);
                string _command = "Select zt.template, zt.templ_id, zt.template_name from lab_results_radio lrr "
                    +" JOIN zone_template zt ON lrr.template_id = zt.zone_id where lrr.ticket_id = '" + _ticket + "';";


                NpgsqlDataAdapter getTypes = new NpgsqlDataAdapter(_command, DBExchange.Inst.connectDb);
                try
                {
                    getTypes.Fill(dtSampleTypes);
                    if (dtSampleTypes.Rows.Count > 0)
                    {
                        foreach (DataRow ro in dtSampleTypes.Rows)
                        {
                            ZoneTemplate ResZone = new ZoneTemplate();
                            ResZone.TemplateName = (string)ro["template_name"];
                            ResZone.TemplateBody = (string)ro["template"];
                            ResZone.TemplateId = (int)ro["templ_id"];


                            this.Add(ResZone);
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
