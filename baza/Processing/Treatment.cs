using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Data;
using System.Collections;

namespace baza.Processing
{
    class Treatment
    {
        //TreatmentStep
#region 
        public class TreatmentStep
        {

            public int LineId;
            public DateTime LineStart;
            public DateTime LineStop;
            public DateTime EffectStart;
            public DateTime EffectStop;
            public DateTime CurrentDate;
            public bool EffLost;
            public string ShortComment;
            public string LongComment;
            public string LineName;
            public int EffectId;
            public int Patient;
            public int Doctor;
            public bool IsCurrent;

        }

               
        public partial class TreatmentStepsList : List<TreatmentStep>
        {
            public void getTreatmentStepByPatientId(int _pId)
            {
                if (_pId > 0)
                {

                    DataTable dtStepList = new DataTable();

                    NpgsqlDataAdapter getSteps = new NpgsqlDataAdapter("Select * from treatment_step where pat_id = '" + _pId + "';", DBExchange.Inst.connectDb);
                    try
                    {
                        getSteps.Fill(dtStepList);
                        processData(dtStepList);
                    }


                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    }
                }
            }

            public void getTreatmentStepByLineId(int _thisLine)
            {
                DataTable dtStepList = new DataTable();

                NpgsqlDataAdapter getSteps = new NpgsqlDataAdapter("Select * from treatment_step where line_id = '" + _thisLine + "';", DBExchange.Inst.connectDb);
                try
                {
                    getSteps.Fill(dtStepList);
                    processData(dtStepList);
                }


                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }

            }


            private void processData(DataTable _thisTable)
            {
                this.Clear();
                if (_thisTable.Rows.Count > 0)
                {
                    foreach (DataRow ro in _thisTable.Rows)
                    {

                        TreatmentStep tst  = new TreatmentStep();
                        tst.LineName = (string)ro["line_name"];
                        tst.Doctor = (int)ro["doc_id"];
                        tst.Patient = (int)ro["pat_id"];
                        tst.LineStart = (DateTime)ro["line_start"];
                        if (ro.IsNull("line_stop") == false)
                        {
                            tst.LineStop = (DateTime)ro["line_stop"];
                        }
                        if (ro.IsNull("date_e_stop") == false)
                        {
                            tst.EffectStop = (DateTime)ro["date_e_stop"];
                        }
                        if (ro.IsNull("date_effect") == false)
                        {
                            tst.EffectStart = (DateTime)ro["date_effect"];
                        }

                        tst.EffLost = (bool)ro["eff_lost"];
                        tst.IsCurrent = (bool)ro["current"];
                        tst.ShortComment = (string)ro["eff_short_comment"]; 
                        tst.EffectId = (int)ro["effect_id"];
                        tst.LongComment = (string)ro["comment"];
                        tst.CurrentDate = (DateTime)ro["current_date"];
                        
                        this.Add(tst);
                    }
                }

            }



            
        }

        public partial class TreatmentStepDataProcessing
        {
            //line_id serial NOT NULL,
            //line_start date,
            //line_stop date,
            //date_effect date,
            //date_e_stop date,
            //eff_lost boolean,
            //eff_short_comment character varying(255),
            //effect_id integer,
            //pat_id integer,
            //"comment" text,
            //line_name character varying(100),
            //doc_id integer,
            //"current" boolean,
            //"current_date" date DEFAULT now()

            public void writeTreatmentStep(TreatmentStep _tStep )
            {
                             

                NpgsqlCommand wdata = new NpgsqlCommand("Insert into treatment_step (line_id, line_name, line_start, line_stop, date_effect, date_e_stop, eff_lost, eff_short_comment, effect_id, "+
                    "comment, current, pat_id, doc_id) values "
                + " ('" + _tStep.LineId + "','" + _tStep.LineName + "','" + _tStep.LineStart + "','" + _tStep.LineStop + "','" + _tStep.EffectStart + "','" + _tStep.EffectStop + "','" 
                + _tStep.EffLost + "','" + _tStep.ShortComment + "','"+_tStep.EffectId+"','"+_tStep.LongComment+"','"+_tStep.IsCurrent+"','"+_tStep.Patient+"','"+_tStep.Doctor+"' ); ", DBExchange.Inst.connectDb);
                try
                {
                    wdata.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }


        }
#endregion  

        //TreatmentCycle
        #region
        public class TreatmentCycle
        {

            public int tcNumber; //  tc_number smallint DEFAULT 0,
            public DateTime start; //  tc_start timestamp without time zone,
            public DateTime finish; //  tc_finish timestamp without time zone,
            public int cycleId; //  cycle_id serial NOT NULL,
            public int schemeId; //  scheme_id integer,

        }

        public partial class TreatmentCycleList : List<TreatmentCycle>
        {

            private void processData(DataTable _thisTable)
            {
                this.Clear();
                if (_thisTable.Rows.Count > 0)
                {
                    foreach (DataRow ro in _thisTable.Rows)
                    {
                        TreatmentCycle tc = new TreatmentCycle();
                        tc.cycleId = (int)ro["cycle_id"];
                        tc.schemeId = (int)ro["scheme_id"];
                        tc.tcNumber = (int)ro["tc_number"];
                        tc.start = (DateTime)ro["tc_start"];
                        tc.finish = (DateTime)ro["tc_finish"];
                        this.Add(tc);
                    }
                }

            }
            public void getTreatmentStepByPatientId(int _pId)
            {
                if (_pId > 0)
                {

                    DataTable dtTCList = new DataTable();

                    NpgsqlDataAdapter getSteps = new NpgsqlDataAdapter("Select * from drug_treatment_cycles where pat_id = '" + _pId + "';", DBExchange.Inst.connectDb);
                    try
                    {
                        getSteps.Fill(dtTCList);
                        processData(dtTCList);
                    }


                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    }
                }
            }

            public void getTreatmentStepByLineId(int _thisLine)
            {
                DataTable dtTCList = new DataTable();

                NpgsqlDataAdapter getSteps = new NpgsqlDataAdapter("Select * from treatment_step where line_id = '" + _thisLine + "';", DBExchange.Inst.connectDb);
                try
                {
                    getSteps.Fill(dtTCList);
                    processData(dtTCList);
                }


                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }

            }



        }

        




        #endregion

        //Radiology treatment
        #region


        public class RadiologyTreatmentData
        {

            public DateTime RayStart; //  start_ray timestamp without time zone,
            public DateTime RayStop; //  stop_ray timestamp without time zone,
            public Decimal Sod; //  sod numeric DEFAULT 0,
            public Decimal Rod; //  rod numeric DEFAULT 0,
            public int RadId; //  rad_id serial NOT NULL,
            public int PatId; //  pat_id integer,
            public int Zone; //  "zone" integer,
            public int Fraction; //  fraction integer,

        }


            public partial class RadiologyTreatmentDataList : List<RadiologyTreatmentData>
            {
                            
                private void processData(DataTable _thisTable)
            {
                this.Clear();
                if (_thisTable.Rows.Count > 0)
                {
                    foreach (DataRow ro in _thisTable.Rows)
                    {
                        RadiologyTreatmentData rtd = new RadiologyTreatmentData();

                        rtd.RayStart = (DateTime)ro["start_ray"];
                        rtd.RayStop = (DateTime)ro["stop_ray"];
                        rtd.Sod = (Decimal)ro["sod"];
                        rtd.Rod = (Decimal)ro["rod"];
                        rtd.RadId = (int)ro["rad_id"];
                        rtd.PatId = (int)ro["pat_id"];
                        rtd.Zone = (int)ro["zone"];
                        rtd.Fraction = (int)ro["fraction"];

                        this.Add(rtd);
                    }
                }

            }

                public void getRadiologyTreatmentByPatientId(int _pId)
                {
                    if (_pId > 0)
                    {

                        DataTable dtTCList = new DataTable();

                        NpgsqlDataAdapter getSteps = new NpgsqlDataAdapter("Select * from treatment_data_radiology where pat_id = '" + _pId + "';", DBExchange.Inst.connectDb);
                        try
                        {
                            getSteps.Fill(dtTCList);
                            processData(dtTCList);
                        }


                        catch (Exception exception)
                        {
                            Warnings.WarnLog log = new Warnings.WarnLog();
                            log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                        }
                    }
                }


          }



            public partial class RadiologyTreatmentProcessData
            {
                public void writeData(RadiologyTreatmentData rtd)
                {
                    NpgsqlCommand wdata = new NpgsqlCommand("Insert into treatment_data_radiology ( start_ray, stop_ray, sod, rod, rad_id, pat_id, zone, fraction) values "
               + " ('" +rtd.RayStart+"', '"+rtd.RayStop+"', '"+rtd.Sod+"', '"+rtd.Rod+"', '"+rtd.RadId+"', '"+rtd.PatId+"', '"+rtd.Zone+"', '"+rtd.Fraction+"' ); ", DBExchange.Inst.connectDb);
                    try
                    {
                        wdata.ExecuteNonQuery();
                    }
                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    }


                }

            }

        #endregion

        //efferent treatment cycles
        #region

        public class EfferentTreatmentCycles
        {

            public short ecNumber; //  ec_number smallint,
            public DateTime ecStart; //  ec_finish timestamp without time zone,
            public DateTime ecFinish; //  cycle_id serial NOT NULL,
            public int CycleId; //  ec_start timestamp without time zone,
            public int ReductionDose; //  ec_reduc_doze integer

        }

            
            public partial class EfferentTreatmentCyclesList : List<EfferentTreatmentCycles>
            {
                           
               private void processData(DataTable _thisTable)
            {
                this.Clear();
                if (_thisTable.Rows.Count > 0)
                {
                    foreach (DataRow ro in _thisTable.Rows)
                    {
                        EfferentTreatmentCycles etc = new EfferentTreatmentCycles();

                        etc.ecNumber = (short)ro["ec_number"];
                        etc.CycleId = (int)ro["cycle_id"];
                        etc.ReductionDose = (int)ro["ec_reduc_doze"];
                        etc.ecStart = (DateTime)ro["ec_start"];
                        etc.ecFinish = (DateTime)ro["ec_finish"];

                        this.Add(etc);
                    }
                }

            }
              

            }

        #endregion

        //surgery treatment
        #region
        public class SurgeryTreatmentData
        {
            public DateTime SurgeryDate; //  surgery_date timestamp without time zone,
            public Char SurgeryType; //  sur_type character varying(1) DEFAULT NULL::character varying,
            public string SurgeryComment; //  surgery_comment text,
            public int SurgeryId; //  surgery_id integer NOT NULL DEFAULT nextval(('"medex"."treatment_data_surgery_surgery_id_seq"'::text)::regclass),
            public int PatId; //  pat_id integer,
            public int DocId; //  doctor_id integer,
            public bool Verified; //  verified boolean,
            public string SurgeryName; //  surgery_name character varying(255),
        }


            
            public partial class SurgeryTreatmentDataList : List<SurgeryTreatmentData>
            {
                            
                private void processData(DataTable _thisTable)
            {
                this.Clear();
                if (_thisTable.Rows.Count > 0)
                {
                    foreach (DataRow ro in _thisTable.Rows)
                    {
                        SurgeryTreatmentData std = new SurgeryTreatmentData();

                        std.SurgeryDate = (DateTime)ro["surgery_date"];
                        std.SurgeryType = (Char)ro["sur_type"];
                        std.SurgeryComment = (string)ro["surgery_comment"];
                        std.SurgeryId = (int)ro["surgery_id"];
                        std.PatId = (int)ro["pat_id"];
                        std.DocId = (int)ro["doctor_id"];
                        std.Verified = (bool)ro["verified"];
                        std.SurgeryName = (string)ro["surgery_name"];


                        this.Add(std);
                    }
                }

            }

                public void getSurgeryTreatmentByPatientId(int _pId)
                {
                    if (_pId > 0)
                    {

                        DataTable dtTCList = new DataTable();

                        NpgsqlDataAdapter getSteps = new NpgsqlDataAdapter("Select * from treatment_data_surgery where pat_id = '" + _pId + "';", DBExchange.Inst.connectDb);
                        try
                        {
                            getSteps.Fill(dtTCList);
                            processData(dtTCList);
                        }


                        catch (Exception exception)
                        {
                            Warnings.WarnLog log = new Warnings.WarnLog();
                            log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                        }
                    }
                }

            }

            public partial class SurgeryTreatmentProcessData
            {
                public void writeData(SurgeryTreatmentData std)
                {
                    NpgsqlCommand wdata = new NpgsqlCommand("Insert into treatment_data_surgery ( surgery_date, sur_type, surgery_comment, surgery_id, pat_id, doctor_id, verified, surgery_name) values "
               + " ( '"+std.SurgeryDate+"','"+std.SurgeryType+"', '"+std.SurgeryComment+"', '"+std.SurgeryId+"', '"+std.PatId+"', '"+std.DocId+"', '"+std.Verified+"', '"+std.SurgeryName+"'); ", DBExchange.Inst.connectDb);
                    try
                    {
                        wdata.ExecuteNonQuery();
                    }
                    catch (Exception exception)
                    {
                        Warnings.WarnLog log = new Warnings.WarnLog();
                        log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                    }


                }

            }
        #endregion


            //forum comments
        #region
        //        Кроме того, будет поле списка, в котором - диагнозы, по поводу которых проводилось лечение (из таблицы relation_diag_drug)
//Поле 2
//отражается только название линии, начало, окончание, эффект лечения, дата последних изменений (current_date). Сортировка по дате начала линии.
//Зависимые формы:
//В этом разделе доп кнопка "Перейти к выбранной записи" - открывает доп форму как основную
//циклы лекарственного лечения (таблица drug_treatment_cycles)

//        CREATE TABLE medex.drug_treatment_cycles
//(
//  tc_number smallint DEFAULT 0,
//  tc_start timestamp without time zone,
//  tc_finish timestamp without time zone,
//  cycle_id serial NOT NULL,
//  scheme_id integer,
//  CONSTRAINT drug_treatment_cycles_pkey PRIMARY KEY (cycle_id)
//)



//циклы эфферентной терапии (efferent_treatment_sycles)
//        CREATE TABLE medex.efferent_treatment_cycles
//(
//  ec_number smallint,
//  ec_finish timestamp without time zone,
//  cycle_id serial NOT NULL,
//  ec_start timestamp without time zone,
//  ec_reduc_doze integer
//)

//хирургическое лечение (treatment_data_surgery)



//CREATE TABLE medex.treatment_data_surgery
//(
//  surgery_date timestamp without time zone,
//  sur_type character varying(1) DEFAULT NULL::character varying,
//  surgery_comment text,
//  surgery_id integer NOT NULL DEFAULT nextval(('"medex"."treatment_data_surgery_surgery_id_seq"'::text)::regclass),
//  pat_id integer,
//  doctor_id integer,
//  verified boolean,
//  surgery_name character varying(255),
//  CONSTRAINT treatment_data_surgery_pkey PRIMARY KEY (surgery_id)
//)




//лучевая терапия (treatment_data_radiology)

//        CREATE TABLE medex.treatment_data_radiology
//(
//  start_ray timestamp without time zone,
//  stop_ray timestamp without time zone,
//  sod numeric DEFAULT 0,
//  rod numeric DEFAULT 0,
//  rad_id serial NOT NULL,
//  pat_id integer,
//  "zone" integer,
//  fraction integer,
//  CONSTRAINT treatment_data_radiology_pkey PRIMARY KEY (rad_id)
//)


        

//+ вопросы комментарии история изменений файлы какдля остальных форм
//Поле 4
//по умолчанию - на основании таблицы relation_treatment_step собирается список методов лечения: название метода (цикла химиотерапии, цикла лучевой терапии, операции, зоны лучевой терапии), дата начала (по ней - сортировка), метод лечения (поле treatment_type в таблице relation) При смене записи выбирается соответствующая запись в соответствующей дополнительной форме и открывается в поле 3. Для добавления записи нужен вопрос с просьюой выбрать добавляемый метод лечения.
//        CREATE TABLE medex.relation_treatment_step
//(
//  treatment_id integer,
//  step_id integer,
//  treatment_type bytea
//)
        
        //Поле 3
//циклы лекарственного лечения (таблица drug_treatment_cycles):
//название цикла (схема лечения, через scheme_id и соответствующей таблице)
//- номер цикла
//- начало
//- окончание
//- редукция доз
//циклы эфферентной терапии (efferent_treatment_sycles)
//такие же поля, как и для циклов лекарственной терапии, кроме редукции доз
//хирургическое лечение (treatment_data_surgery)
//дата и название операции (полное), возможно, ссылка на файл протокола (не знаю, надо ли это). нужно оставить поле comment, куда пока будем записывать типы операции (в дальнейшем, возможно, надо будет добавить поле типа операции, но в связи с различиями в классификациях нужно либо пользоваться одной, либо вместо поля добавлять таблицу relation с кодами классификации, а в форму - список с этими характеристиками. также нужен список показаний из таблицы relation diag_drug
//лучевая терапия (treatment_data_radiology)

//все поля таблицы последовательно + также нужен список показаний из таблицы relation diag_drug

        #endregion


    }
}
