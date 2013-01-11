using System;
using System.Collections.Generic;
////using System.Linq;
using System.Text;
using System.Reflection;
using Npgsql;

namespace baza
{
    /// <summary>
    /// Данный класс инкапсулирует извлечение объектной модели данных о пациенте из БД
    /// </summary>
    public class DALCPatients
    {
        NpgsqlConnection mDBConnection;
        
        /// <summary>
        /// Хранит информацию о пациентах в виде объектов
        /// </summary>
        List<Patient> mPatients = new List<Patient>();
        public List<Patient> Patients { get { return mPatients; } }

        public DALCPatients()
        {            
        }

        /// <summary>
        /// Этот метод делает запрос к базе данных, обновляя информацию о пациентах
        /// в объектной модели пациентов
        /// </summary>
        /// 

        public void RefreshData()
        {
            string connectionStr = "Server=127.0.0.1;Port=5432;User Id="username";Password="password";Database=medex;";
            string commandStr = "select * from patients;";

            try
            {
                mDBConnection = new NpgsqlConnection(connectionStr);
                mDBConnection.Open();
                NpgsqlCommand command = new NpgsqlCommand(commandStr, mDBConnection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                mPatients.Clear();
                while (dataReader.Read())
                {
                    string idStr = dataReader[0].ToString();
                    string nameStr = dataReader[1].ToString();
                    Patient newPatient = new Patient(
                            Convert.ToInt32(idStr),
                            nameStr);
                    mPatients.Add(newPatient);
                }

                mDBConnection.Close();
            }
            catch (Exception exception)
            {
                 Warnings.WarnLog log = new Warnings.WarnLog();    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());                
            }
        }
    }
}
