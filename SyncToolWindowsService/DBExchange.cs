using System;
using System.Data;
using Npgsql;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace SyncToolWindowsService
{
   public class DBExchange
    {
       public DataSet LoginSettings;
       public List<string> listLogin;
       private String m_dbUserName = String.Empty;
       public int dbUsrId;
       public String uName;
       public String uFName;
       public String UsrSign;
       public List<string> lstPatients; 
       public NpgsqlConnection connectDb;
       public SqlConnection msSqlConnect;
       public String dBUserName
       {
           get { return m_dbUserName; }
       }
       public string versionNumber = "1.0.31";
       public DataSet dsOnk;
       public DataTable tblDbObjType;
       public DataTable tblJournal;
       public DataTable tblWorkJournal;
       public DataTable tblMessageAll;
       public DataTable tblUsers;
       public DataTable tblMessageAnn;
       public DataTable tblDocumentTypes;
       public DataTable tblKartList;
       private DataTable tblCytNumb;
       public Int32 gistNumb;
       public String selectedPatientId;
       public DataTable tblPatientDocs;
       private DataTable lstPatId;
       private DataTable tblCytNumbChk;
       public DataTable tblUsrData;
       public DataTable writeNewPatient;
       public DataTable tblState;
       public DataTable tblCountry;
       public DataTable tblRegion;
       public DataTable tblCities;
       public DataTable tblArea;
       public DataTable tblStreet;
       public bool chkPassport;
       


       
#region Singleton
       private DBExchange() { }
       private static DBExchange mInstance = null;
       public static DBExchange Inst
       {
           get
           {
               if (mInstance == null)
                   mInstance = new DBExchange();
               return mInstance;               
           }
       }
#endregion

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



           
           string strConnect = "Server=" + dBServer + ";Port=5432;User Id=" + dBLogin + "; Password=" + dBPassHash+ ";Database=medex; ";
           m_dbUserName = dBLogin;
           connectDb = new NpgsqlConnection(strConnect);
           try
           {
               connectDb.Open();
               //  NpgsqlCommand command = new NpgsqlCommand(strConnect, mDBConnection);

           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
           finally
           {
               
           }
       }

       public void cDbClose()
       {
           connectDb.Close();
       }

       public void cDbOpen()
       {
           connectDb.Open();
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
                   System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
               }
           }

       }


           
       //Получает данные о пользователе
       ///
       public void getUsrData()
       {
           DataSet dsOnk = new DataSet();
           try
           {
                           
                 NpgsqlDataAdapter dbUsrDat = new NpgsqlDataAdapter("SELECT * FROM doctors WHERE user_name = '"+dBUserName+"';", connectDb);
                 
                 tblUsrData = dsOnk.Tables.Add("db_usrdata");
 
                    dbUsrDat.Fill(tblUsrData);
           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }

                    
           

           dbUsrId = Convert.ToInt32(tblUsrData.Rows[0]["doc_id"]);
            uName = (string)tblUsrData.Rows[0]["first_name"];
            string uSName = (string)tblUsrData.Rows[0]["last_name"];
            uFName = ((string)tblUsrData.Rows[0]["family_name"]).Trim();
           UsrSign = (uFName + " " + uName[0] + ". " + uSName[0] +".");

       }


       /// <summary>
       // Очищает строку подключения и имя пользователя.
       /// </summary>
       public void StartupLogoff()
       
       {

           cDbClose();
           String dBUserName = String.Empty;
           NpgsqlCommand connectDb = new NpgsqlCommand();

            
       }

       /// <summary>
       // Получение первоначальных данных и типов полей схемы. Сообщения и журнал.
       /// </summary>
       ///  /// GetObjType получает тип поля из хмл документа, который формируется при входе в базу данных 
       /// и сохраняется на диске, затем проверяется только дата последнего изменения таблицы из которой 
       /// получаются данные для списка. Функция затем соотносит полученные данные и отдает тип поля.  
       /// 
       ///   Определение типа данных 
       ///  SELECT table_name, Column_name, data_type, character_maximum_length, column_default 
       ///  FROM information_schema.columns WHERE table_schema = 'public'
       ///  
       public void GetDbObjType()
       {
           try
           {

             //  NpgsqlDataAdapter dbObjType = new NpgsqlDataAdapter("SELECT table_name, Column_name, data_type, character_maximum_length, column_default FROM information_schema.columns WHERE table_schema = 'medex';", connectDb);
               DataSet dsOnk = new DataSet();
               //tblDbObjType = dsOnk.Tables.Add("dbobjtype");
              // dbObjType.Fill(tblDbObjType);
          // dsOnk.WriteXml("data/dbTypes.xml");

           NpgsqlDataAdapter dbMessAll = new NpgsqlDataAdapter("SELECT m_from, m_date, m_body FROM message_center WHERE m_to= 'ALL' ORDER BY m_date ASC LIMIT 60;", connectDb);
           tblMessageAll = dsOnk.Tables.Add("db_message_all");
           dbMessAll.Fill(tblMessageAll);

           NpgsqlDataAdapter dbMessAnn = new NpgsqlDataAdapter("SELECT m_from, m_date, m_body FROM message_center WHERE m_to= 'ANN' ORDER BY m_date DESC LIMIT 60;", connectDb);
           tblMessageAnn = dsOnk.Tables.Add("db_m_announce");
           dbMessAnn.Fill(tblMessageAnn);

           NpgsqlDataAdapter dbDocumTypes = new NpgsqlDataAdapter("SELECT * FROM docum_type;", connectDb);
           tblDocumentTypes = dsOnk.Tables.Add("db_docum_types");
           dbDocumTypes.Fill(tblDocumentTypes);


           //    (Select descr from docum_type where docum_type.did = documents.document_type)
           
          // dsOnk.WriteXml("data/journal.xml");

           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }

           getUsrJournal();
           getUsrData();
        
       }

       /// <summary>
       /// Заполнение журнала пользователя последними, созданными им, документами.
       /// </summary>
       //public void FillJournalList()
       //{
       //    try
       //    {

       //        NpgsqlDataAdapter dbJour = new NpgsqlDataAdapter("SELECT document_date, (Select descr from docum_type where did = documents.document_type) as document,"
       //          + "(Select family_name From patient_list Where patient_list.pat_id = documents.pat_id) as pat_name, "
       //        + "document_number FROM documents WHERE doc_id= '" + dbUsrId + "'ORDER BY document_date DESC LIMIT 50;", connectDb);
       //        tblJournal = dsOnk.Tables.Add("db_journal");
       //        dbJour.Fill(tblJournal);
       //        //   dsOnk.WriteXml("data/journal.xml");

       //    }
       //    catch (Exception exception)
       //    {
       //        System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
       //    }

       //}


       //Заполнение журнала пользователя
       public void getUsrJournal()
       {
           DataSet dsOnk = new DataSet();
           try
           {
               NpgsqlDataAdapter dbJour = new NpgsqlDataAdapter("SELECT document_date,(Select trim(descr) from docum_type where docum_type.did = documents.document_type) as document,"
                 + "(Select trim(family_name) From patient_list Where patient_list.pat_id = documents.pat_id) as pat_name, "
                 + "document_number , document_type FROM documents WHERE doc_id= '" + dbUsrId + "' AND delete = false ORDER BY document_date DESC LIMIT 50;", connectDb);
              
               
               tblJournal = dsOnk.Tables.Add("db_journal");
               tblWorkJournal = dsOnk.Tables.Add("db_work_journal");
               dbJour.Fill(tblWorkJournal);
               tblJournal = tblWorkJournal.Copy();
               tblJournal.Columns.Remove("document_number");
               tblJournal.Columns.Remove("document_type");
           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
       }




       /// <summary>
       /// Обновляет сообщения
       /// </summary>
       public void updateMessagetbl()
       {
           try
           {
           
           DataSet dsOnk = new DataSet();
           NpgsqlDataAdapter dbMessAll = new NpgsqlDataAdapter("SELECT m_from, m_date, m_body FROM message_center WHERE m_to= 'ALL' ORDER BY m_date DESC LIMIT 60;", connectDb);
           tblMessageAll = dsOnk.Tables.Add("db_message_all");
           dbMessAll.Fill(tblMessageAll);
                      }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }

           
      


       }

       public void updateAnnounceTbl()
       {
           try
           {
               
               DataSet dsOnk = new DataSet();
               NpgsqlDataAdapter dbMessAnn = new NpgsqlDataAdapter("SELECT m_from, m_date, m_body FROM message_center WHERE m_to= 'ANN' ORDER BY m_date DESC LIMIT 60;", connectDb);
               tblMessageAnn = dsOnk.Tables.Add("db_message_ann");
               dbMessAnn.Fill(tblMessageAnn);
           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }

           



       }


       ///Создание нового пользователя через меню регистрации
       public void CreateNewDbUser(String new_username, String new_pass, String new_name, String new_familyname, 
           String new_secondname, DateTime new_birthdate, Int16 new_otdel, Int16 new_status, String new_mail)

          {
              try
              {

                  MD5 md5h = MD5.Create();
                  byte[] PassHash = md5h.ComputeHash(Encoding.Default.GetBytes(new_pass));
                  StringBuilder crHash = new StringBuilder();
                  foreach (byte hashByte in PassHash)
                  {
                      crHash.Append(String.Format("{0:x2}", hashByte));
                  }
                  string dBPassHash = crHash.ToString();

                  NpgsqlCommand WriteNewDbUser = new NpgsqlCommand("CREATE USER " + new_username + " WITH PASSWORD '" + dBPassHash + "' IN GROUP postgres", connectDb);
                  WriteNewDbUser.ExecuteNonQuery();
                  
                  NpgsqlCommand WriteNewDbUserInfo = new NpgsqlCommand("INSERT INTO doctors (family_name, first_name, last_name, status, user_name, otdel, u_mail, birth) VALUES ('"
                      + new_familyname + "','" + new_name + "','" + new_secondname + "','" + new_status + "','" + new_username + "','" + new_otdel + "','" + new_mail + "','" + new_birthdate + "');", connectDb);
                  WriteNewDbUserInfo.ExecuteNonQuery();


                  MessageBox.Show("Пользователь создан", "Спасибо за регистрацию",
                       MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                 
                  RegisterForm.ActiveForm.Close();
                  RegisterForm FormRegisterDbUser = new RegisterForm();
                  FormRegisterDbUser.ShowDialog();

              }
              catch (Exception exception)
              {
                  System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
              }
              finally
              {
                  
              }
           
           }

       ///проверка имени пользователя в уже имеющихся именах
       ///
       public void ChkNameDbUser(String u_name)
       {
           try
           {
           DataSet dsOnk = new DataSet();
           
           NpgsqlDataAdapter dbU = new NpgsqlDataAdapter("SELECT family_name,user_name FROM doctors WHERE lower(user_name) = '"+ u_name.ToLower() +"';", connectDb);
           tblUsers = dsOnk.Tables.Add("db_users");
           tblUsers.Clear();
           dbU.Fill(tblUsers);
                      }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
           

       }


       /// <summary>
       /// Загружает список пациентов пользователя, который хранится в строке через запятую или ;
       /// и передает его в листбокс пациентлист
       /// </summary>
       public DataTable loadUsrPatientList()
       {
           DataSet dsOnk = new DataSet();
           DataTable user_data = new DataTable();

           try
           {

               NpgsqlDataAdapter dbU = new NpgsqlDataAdapter("SELECT (select trim(family_name) from patient_list where pat_id = user_data.pat_id),"+
                 "  (select substr(first_name,1,1) from patient_list where pat_id = user_data.pat_id),(select substr(last_name,1,1) from patient_list where pat_id = user_data.pat_id), pat_id "+
                 "FROM user_data WHERE usr_id = '" + dbUsrId + "' and approve = true;", connectDb);
           dbU.Fill(user_data);         
           
           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }

           return user_data;
       }

       /// <summary>
       ///Проверяет есть ли шифрованый** файл списка пациентов, если нет загружает из базы весь лист 
       ///и экспортирует в хмл., при слежующем запуске грузит из хмл в датасэт
       ///Проверка на внесение последней записи в таблицу пациентов и если измененения были то загружает
       ///изменения в датасет.
       ///Потом доделать чтобы грузился не весь лист а только изменения
       /// </summary>
       
       
       public void chkPatListXml()
       {
           tblKartList.Clear();
           DataSet dsOnk = new DataSet();
           try
           {
               dsOnk.ReadXml("data/p_dat.xml");
           }
           catch
           {
               loadKartListFromBase();
               
           }

           tblKartList = dsOnk.Tables["db_kartlist"];
          
           DataRow patKLRow = tblKartList.Rows[tblKartList.Rows.Count - 1];
           Int32 patKLId = Convert.ToInt32(patKLRow["pat_id"]);

           
           NpgsqlCommand selPatId = new NpgsqlCommand("SELECT MAX(pat_id) FROM patient_list;", connectDb);
           
           Int32 lastPatId = (Int32)selPatId.ExecuteScalar();
           
           
            if (lastPatId != patKLId)
            {
                loadKartListFromBase();
            }
            crtPatList();

            
       }

       private void crtPatList()
       {
           
           lstPatients = new List<string>();
           lstPatients.Clear();
           if (tblKartList != null)
           {
               foreach (DataRow row in tblKartList.Rows)
               {
                   DateTime birth = Convert.ToDateTime(row["birth_date"]);
                   lstPatients.Add(((string)row["family_name"]) + " " + ((string)row["first_name"])
                       + " " + ((string)row["last_name"]) + " " + (birth.Year)
                       + " г., карта №" + (Convert.ToString(row["nib"])));

               }
           }
       }


       public void loadPatListXml()
       {
           DataSet dsOnk = new DataSet();
           try
           {
               dsOnk.ReadXml("data/p_dat.xml");
           }
           catch
           {
               loadKartListFromBase();

           }
           finally
           {
               tblKartList = dsOnk.Tables["db_kartlist"];
               crtPatList();
           }
       }

       ///Загружает список пациентотв из базы и сохраняет в хмл
       public void loadKartListFromBase()
       {
           
           try
           {
               NpgsqlDataAdapter dbKL = new NpgsqlDataAdapter("SELECT nib, family_name, first_name, last_name, birth_date, pat_id FROM patient_list ;", connectDb);



               DataSet dsOnk = new DataSet();
               tblKartList = dsOnk.Tables.Add("db_kartlist");
               dbKL.Fill(tblKartList);
               
           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
          // tblKartList.WriteXml("data/p_dat.xml");

       }



       /// <summary>
       /// Создаёт нового пациента
       /// нужно сделать проверку на повторы
       /// </summary>
       /// 
       public void crtNewPatient(bool isComment, bool isTrustMan, bool m1, bool m2, bool m3, bool nnib)
       {

          DataRow wnpRow = writeNewPatient.Rows[0];
          String trustMan;
          String comment;
          String rowTrust;
          String rowComment;
          String rPhone1;
           String iPhone1;
          String rPhone2;
           String iPhone2;
          String rPhone3;
           String iPhone3;
           String rNib;
           String iNib;



          if (chkPassport==true)
          {
              Int64 cPass = (Int64)wnpRow["pass"];
              chkPatInBaseByPass(cPass);
          }
          else
          {
              chkPatByYearName((DateTime)wnpRow["birth_date"],(string)wnpRow["family_name"],(string)wnpRow["first_name"],(string)wnpRow["last_name"],(Int16)wnpRow["town"]);
          }
          if (chkPassport == false)
          {
              if (isComment == true)
                          {
                              comment = ",comment";
                              rowComment = ", '"+wnpRow["comment"]  ; 
                          }
                          else { comment = ""; rowComment = "";}
              if (isTrustMan == true)
                          {
                              trustMan = " ,trust_man, trust_phone";
                              rowTrust =  ", '"+wnpRow["trust_man"] + "', '" + wnpRow["trust_phone"] +"'";
                          }
                          else{trustMan = ""; rowTrust = "";}
              if (m1 == true)
              {
                 rPhone1 = ", " +wnpRow["phone1"];
                 iPhone1 = ", phone1";
              }
              else { rPhone1 = ""; iPhone1 = ""; }
              if (m2 == true)
              {
                 rPhone2 = ", " +wnpRow["phone2"];
                 iPhone2 = ", phone2";
              }
              else { rPhone2 = ""; iPhone2 = ""; }
              if (m3 == true)
              {
                 rPhone3 = ", " +wnpRow["phone3"];
                 iPhone3 = ", phone3";
              }
              else { rPhone3 = ""; iPhone3 = ""; }

              if (nnib == true)
              {
                  rNib = wnpRow["nib"] + ", '" + wnpRow["nib_date"] + "', ";
                  iNib = " nib, cardprt,";
              }
              else { rNib = ""; iNib= ""; }
              
              try
              {
                  

                  //   NpgsqlCommand CreateNewPatient = new NpgsqlCommand("INSERT INTO patient_list "
                  //      + " (family_name, first_name, last_name, birth_date, nib, is_man, doc_id) "
                  //   + " VALUES ('" + wd[0] + "," + wd[1] + "," + wd[2] + "," + wd[3] + "," + wd[4] + "," + wd[5] + "," + wd[6] + "');", connectDb);


                  NpgsqlCommand CreateNewPatient = new NpgsqlCommand("INSERT INTO patient_list "
                                   + " ( family_name, first_name, last_name, birth_date, pass,"+ iNib +" is_man, creator_id, region1,state, "
                                  + "country, town_id, area, street_n, house, building, flat,address "+ iPhone1 + iPhone2 + iPhone3 + comment + trustMan + ")" 
                                    
                                       
                                   + " VALUES ('" +
                                   wnpRow["family_name"] + "', '" +
                                   wnpRow["first_name"] + "', '" +
                                   wnpRow["last_name"] + "', '" +
                                   wnpRow["birth_date"] + "', " +
                                   wnpRow["pass"] + ", " + rNib +
                                   
                                   wnpRow["is_man"] + ", " +
                                   wnpRow["creator_id"] + ", " +
                                   wnpRow["region1"] + ", " +
                                   wnpRow["state"] + ", " +
                                   wnpRow["country"] + ", " +
                                   wnpRow["town_id"] + ", " +
                                   wnpRow["area"] + ", " +
                                   wnpRow["street_n"] + ", " +
                                   wnpRow["house"] + ", " +
                                   wnpRow["building"] + ", " +
                                   wnpRow["flat"] + ", '" +
                                   wnpRow["address"] + "'"+
                                   rPhone1 +
                                   rPhone2 +
                                   rPhone3 +
                                   rowComment + 
                                   rowTrust  + ");", connectDb);
                  CreateNewPatient.ExecuteNonQuery();

                  writeNewPatient.Clear();
              }
              catch (Exception exception)
              {
                  System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
              }
              finally
              {
                  
              }
             
          }
          else
          {
              MessageBox.Show("Проверьте заполнение данных или сделайте поиск в базе пациентов", "Пользователь "+wnpRow["family_name"]+" - существует",
              MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
       }

       //проверка пациента в базе по паспорту
       public void chkPatInBaseByPass(Int64 Pass)
       {
           chkPassport = false;
           
           NpgsqlCommand chkPassPat = new NpgsqlCommand("Select pat_id FROM patient_list WHERE pass ='"+Pass+"';",connectDb);
           
           try
           {
               
               Int32 passportString = (Int32)chkPassPat.ExecuteScalar();
               if (passportString > 0)
                   {
                       chkPassport = true;
                   }
               
         
           }
                catch 
                       {
                           chkPassport = false;
                       }
           finally
           {
               
           }
           
       }

       //проверка пациента по имени и году
       public void chkPatByYearName(DateTime birthdate, String family, String name, String surname, Int16 city)
           {

               chkPassport = false;
               
               NpgsqlCommand chkNamePat = new NpgsqlCommand("Select pat_id FROM patient_list WHERE family_name = '" + family + "' AND birth_date = '" + birthdate + "' AND gorod_n = '" + city + "' AND first_name = '" + name + "' AND last_name = '"+surname+"';",connectDb);
               Int32 passport;
               try
               {
                   passport = (Int32)chkNamePat.ExecuteScalar();
                   if (passport > 0)
                   {
                       chkPassport = true;
                   }
               }
               catch (Exception exception)
               {
                   System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
               }
               finally
               {
                   
               }
           }

       ///Получает структуру таблицы пациентов
       public void getPatientStruct()
       {
           DataSet dsOnk = new DataSet();
           try
           
           {
               
               NpgsqlDataAdapter selPatStru = new NpgsqlDataAdapter("SELECT * FROM patient_list LIMIT 1;", connectDb);
               writeNewPatient = dsOnk.Tables.Add("new_patient");
               selPatStru.Fill(writeNewPatient);
               writeNewPatient.Clear();
           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
           
       }


       /// <summary>
       /// Получает регион страну город и район для листов в newpatientform из хмл и сверяет их количество с базой
       /// </summary>
       /// 
       public void getRegion()
       {
           DataSet dsOnk = new DataSet();
           try
           {
               dsOnk.ReadXml("data/livin.xml");
               Int32 dsOnkRegionCount = dsOnk.Tables["region"].Rows.Count + dsOnk.Tables["city"].Rows.Count + dsOnk.Tables["country"].Rows.Count + dsOnk.Tables["rayon"].Rows.Count;
               DataTable countAddr = dsOnk.Tables.Add("count_addr");
               try
               {
                   
                   NpgsqlDataAdapter selCountAddr = new NpgsqlDataAdapter("SELECT count(*) FROM addr;", connectDb);

                   selCountAddr.Fill(countAddr);
               }
               catch (Exception exception)
               {
                   System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
               }
               
               Int32 addrCount = Convert.ToInt32(countAddr.Rows[0]["count"]);
               if (addrCount != dsOnkRegionCount)
               {
                   chkRegion();
               }

           }
           catch
           {
               chkRegion();

           }
           finally
           {
               tblArea = dsOnk.Tables["rayon"];
               tblCities = dsOnk.Tables["city"];
               tblCountry = dsOnk.Tables["country"];
               tblRegion = dsOnk.Tables["region"];
           }
       }
       /// <summary>
       /// Загрузка регионов
       /// </summary>
       public void chkRegion()
       {
           DataSet dsOnk = new DataSet();
           try
           {
               

               NpgsqlDataAdapter gRe = new NpgsqlDataAdapter("SELECT * FROM region order by int_value asc;", connectDb);
               tblRegion = dsOnk.Tables.Add("region");
               gRe.Fill(tblRegion);

               NpgsqlDataAdapter gOk = new NpgsqlDataAdapter("SELECT * FROM state;", connectDb);
               tblState = dsOnk.Tables.Add("state");
               gOk.Fill(tblState);

               NpgsqlDataAdapter gCo = new NpgsqlDataAdapter("SELECT * FROM country order by int_value asc ;", connectDb);
               tblCountry = dsOnk.Tables.Add("country");
               gCo.Fill(tblCountry);

               NpgsqlDataAdapter gCi = new NpgsqlDataAdapter("SELECT text_value,c_id,int_value FROM towns order by text_value ASC;", connectDb);
               tblCities = dsOnk.Tables.Add("city");
               gCi.Fill(tblCities);

               NpgsqlDataAdapter gRa = new NpgsqlDataAdapter("SELECT text_value,int_value,c_id FROM area WHERE c_id = 7801;", connectDb);
               tblArea = dsOnk.Tables.Add("area");
               gRa.Fill(tblArea);
           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
           


           ///dsOnk.WriteXml("data/livin.xml");

       }





       /// <summary>
       /// получает список улиц для выбранного города, выгружает в хмл, нужно сделать проверку на последнюю запись
       /// </summary>
       /// <param name="city"></param>
       /// 
       public void getStreetAddr(Int16 City)
       {

           DataSet dsOnk = new DataSet();
           tblStreet = dsOnk.Tables.Add("street");
           tblArea = dsOnk.Tables.Add("area");
           try
           {
               
               NpgsqlDataAdapter gSt = new NpgsqlDataAdapter("SELECT addr,area,serial FROM streets WHERE city ='" + City + "' order by addr ASC;", connectDb);
               
               gSt.Fill(tblStreet);

               NpgsqlDataAdapter gAr = new NpgsqlDataAdapter("SELECT int_value,text_value,c_id FROM area WHERE c_id ='" + City + "' order by text_value ASC;", connectDb);

               gAr.Fill(tblArea);

           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
           

         

       }
       public void chkStreetAddr()
       {
           DataSet dsOnk = new DataSet();
           try
           {

               dsOnk.ReadXml("data/street.xml");
               Int32 dsOnkStreetCount = dsOnk.Tables["street"].Rows.Count;
               DataTable countStreet = dsOnk.Tables.Add("count_street");
               try
               {
                   
                   NpgsqlDataAdapter selCountStreet = new NpgsqlDataAdapter("SELECT count(*) FROM streets;", connectDb);

                   selCountStreet.Fill(countStreet);
               }
               catch (Exception exception)
               {
                   System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
               }
               
               Int32 streetCount = Convert.ToInt32(countStreet.Rows[0]["count"]);
               if (streetCount != dsOnkStreetCount)
               {
                   chkStreetAddr();
               }


           }
           catch
           {
               chkStreetAddr();
           }
           finally
           {
               tblStreet = dsOnk.Tables["street"];
           }




       }


       /// <summary>
       /// Получает список всех документов по выбранному пациенту для передачи их в карту пациента - все записи.
       /// из полученной таблицы затем выбираются данные для вкладок обследования, исследования и назначения
       /// </summary>
       
       public void loadPatientDocuments()
       {
           try{
           
           NpgsqlDataAdapter dbKL = new NpgsqlDataAdapter("SELECT * FROM documents WHERE patient_id = '" + selectedPatientId + "';", connectDb);
         
           DataSet dsOnk = new DataSet();
           tblPatientDocs = dsOnk.Tables.Add("db_patient_documents");
           dbKL.Fill(tblPatientDocs);
           
           }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
           
       }


       /// <summary>
       /// проверка номера исследования
       /// Добавление данных из формы цитологии
       /// добавление записи в таблицу документов (лучше сделать в базе)
       /// </summary>
       /// <param name="pat_id"></param>
       /// <param name="date"></param>
       /// <param name="shifr"></param>
       /// <param name="nomer"></param>
       /// <param name="zakluch"></param>
       public void cytoInsertData(String pat_id, DateTime date, Int16 shifr, Int32 nomer, String zakluch)
       {
           try
           {
           
          ///Проверка номера исследования
           NpgsqlDataAdapter chkCytNumb = new NpgsqlDataAdapter("SELECT nomer FROM cytology WHERE nomer = '" + nomer + "';", connectDb);
           DataSet dsOnk = new DataSet();
           tblCytNumb = dsOnk.Tables.Add("db_cytology_number");
           chkCytNumb.Fill(tblCytNumb);
           if (tblCytNumb.Rows.Count >= 1)
           {

               System.Windows.Forms.MessageBox.Show("Исследование № " + nomer + " уже существует", "Выберите другой номер или проверьте последние записии",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
           }
           else
           {
               NpgsqlCommand insCytData = new NpgsqlCommand("INSERT INTO cytology (zakluch, pat_id, date, shifr, nomer) VALUES ('" + zakluch + "','" + pat_id + "','" + date + "','" + shifr + "','" + nomer + "');", connectDb);

               insCytData.ExecuteNonQuery();
           }
                      }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }
           

       }

       




       /// <summary>
       /// Отправка сообщения
       /// </summary>
       /// <param name="m_from"></param>
       /// <param name="m_to"></param>
       /// <param name="m_body"></param>

       public void sendMessage(String m_from, String m_to, String m_body)
       {
           try
           {
           
           NpgsqlCommand sendM = new NpgsqlCommand("INSERT INTO message_center (m_from, m_to, m_body) VALUES ('" + m_from + "','" + m_to + "','" + m_body + "');", connectDb);
           sendM.ExecuteNonQuery();
                      }
           catch (Exception exception)
           {
               System.Windows.Forms.MessageBox.Show(exception.Message.ToString());
           }

           

       }

      }
}
