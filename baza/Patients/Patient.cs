using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataProvider.Patients
{
    public enum Gender
    {
        Male,
        Female
    }
    
    public interface IPatient
    {
        Int32 Id { get; }
        String FirstName { get; }
        String LastName { get; }
        String PatronymicName { get; }
        DateTime BirthDate { get; }
        Gender Gender { get; }
    }
    
    public sealed class Patient : IPatient
    {
        public Int32 Id { get; private set; }
        public String FirstName { get; private set; }
        public String LastName { get; private set; }
        public String PatronymicName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; private set; }

        public Patient
        (
            Int32 _id,
            String _firstName,
            String _patronymicName,
            String _lastName,
            DateTime _birthDate,
            Gender _gender
        )
        {
            Id = _id;
            FirstName = _firstName;
            LastName = _lastName;
            PatronymicName = _patronymicName;
            BirthDate = _birthDate;
            Gender = _gender;
        }
    }
}
