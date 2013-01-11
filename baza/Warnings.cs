using System;
using System.Collections.Generic;
//using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace baza
{
    public class Warnings
    {
        public class WarnMessages
        {
            public void SuccessRegister()
            {

            }
            public void warnChoosePatient()
            {
                MessageBox.Show("Выберите пациента", "Не выбран пациент",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            public void warnRegisterWrongName()
            {
                MessageBox.Show("Такое имя пользователя уже существует", "Выберите другое имя пользователя",
                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            public void warnSetNumIss()
            {
                MessageBox.Show("Нет номера исследования", "Укажите номер исследования",
                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            public void warnSetStepKT()
            {
                MessageBox.Show("Не указан шаг КТ", "Укажите шаг",
                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            public void warnGotThisScheme()
            {
                MessageBox.Show("Такая схема уже существует", "Измените название схемы",
                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }
        
        public class WarnLog
        {
             DirectoryInfo di = new DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal));


            public void writeLog(string _thisFunc, string _thisException, string _thisLog)
            {

                //if (!(System.IO.Directory.Exists(Application.StartupPath + "\\Errors\\")))
                //{
                //    System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Errors\\");
                //    System.IO.File.Create(Application.StartupPath + "\\Errors\\errlog.txt");
                //}

                if (!(System.IO.Directory.Exists(di.FullName + "\\Medex\\Errors\\")))
                {
                    System.IO.Directory.CreateDirectory(di.FullName + "\\Medex\\Errors\\");
                    
                    if (!(System.IO.File.Exists(di.FullName + "\\Medex\\Errors\\errlog_"+DateTime.Now.ToShortDateString()+".txt")))
                        {
                            System.IO.File.Create(di.FullName + "\\Medex\\Errors\\errlog_"+DateTime.Now.ToShortDateString()+".txt");
                        }
                
                }

                if (_thisException == "There is already an open DataReader associated with this Command which must be closed first.")
                {
                    DBExchange.Inst.connectDb.Close();
                    DBExchange.Inst.connectDb.Dispose();
                    DBExchange.Inst.connectDb.Open();
                }
                else
                {
                    try
                    {
                        string s3 = "";
                       
                        s3+= "Program version: " + DBExchange.Inst.versionNumber.ToString();
                        s3+="\nFunction: " + _thisFunc;
                        s3+="\nMessage: " + _thisException;
                        s3+="\nStackTrace: " + _thisLog;
                        s3+= "\n=========================================================================================== \n\n";

                        Npgsql.NpgsqlCommand nci = new Npgsql.NpgsqlCommand("insert into error_log (descr, user_id) values ('"+s3+"','"+DBExchange.Inst.dbUsrId+"') returning err_id ", DBExchange.Inst.connectDb);

                        int error_id = (int)nci.ExecuteScalar();

                        System.Windows.Forms.MessageBox.Show("ВНИМАНИЕ ВОЗНИКЛА НЕПРЕДВИДЕННАЯ ОШИБКА \nОшибка зарегистрирована в базе данных \nВы можете сообщить о своих действиях на сайте http://medx.spb.ru/issues \nили описать свои деействия в следующей форме"  );
                        Templates.FormRegisterError fre = new Templates.FormRegisterError(true,error_id);
                        fre.ShowDialog();
                    }

                    catch
                    {
                        FileStream fs = new FileStream(di.FullName + "\\Medex\\Errors\\errlog_" + DateTime.Now.ToShortDateString() + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        StreamWriter s = new StreamWriter(fs);

                        s.Close();
                        fs.Close();

                        FileStream fs1 = new FileStream(di.FullName + "\\Medex\\Errors\\errlog_" + DateTime.Now.ToShortDateString() + ".txt", FileMode.Append, FileAccess.Write);
                        StreamWriter s1 = new StreamWriter(fs1);

                        s1.Write("Date/Time: " + DateTime.Now.ToString());
                        s1.Write("\nProgram version: " + DBExchange.Inst.versionNumber.ToString());
                        s1.Write("\nFunction: " + _thisFunc);
                        s1.Write("\nMessage: " + _thisException);
                        s1.Write("\nStackTrace: " + _thisLog);
                        s1.Write("\n=========================================================================================== \n\n");
                        s1.Close();
                        fs1.Close();


                        Templates.FormRegisterError fre = new Templates.FormRegisterError(false,0);
                        System.Windows.Forms.MessageBox.Show(_thisFunc + " \n" + _thisException + " \n" + _thisLog);
                        fre.ShowDialog();
                    }

                }



            }

        }


        
        }
}
