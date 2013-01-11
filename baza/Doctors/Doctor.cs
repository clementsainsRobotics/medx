using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataProvider.Doctors
{
    public interface IDoctor
    {
        Int32 Id { get; }
        String FirstName { get; }
        String LastName { get; }
        String PatronymicName { get; }
    }
    
    public sealed class Doctor : IDoctor
    {
        public Int32 Id { get; private set; }
        public String FirstName { get; private set; }
        public String LastName { get; private set; }
        public String PatronymicName { get; private set; }
    }
}
