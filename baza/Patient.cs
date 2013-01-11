using System;
using System.Collections.Generic;
////using System.Linq;
using System.Text;

namespace baza
{
    /// <summary>
    /// Данный класс содержит объект данных, инкапсулирующий сведения о пациенте
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Идентификатор пациента в базе данных
        /// </summary>
        int mId;
        public int Id { get { return mId; } }

        /// <summary>
        /// Имя пациента
        /// </summary>
        string mName;
        public string Name { get { return mName; } }

        public Patient(int _id, string _name)
        {
            mId = _id;
            mName = _name;
           
        }
    }
}
