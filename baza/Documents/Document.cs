using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataProvider.Documents
{
    public interface IDocument
    {
        Int16 TypeId;
        Int64 Serial;
        Int32 SignedUser;
        DateTime Date;
        Boolean Printed;
        String DocumentShortName;
        String DocumentFullName;
        String MedexDataTable;
        String DocumentBody;
        String DocumentHeader;
        Boolean SetDeleted;
    }


    public sealed class Document : IDocument
    {
       public Int16 TypeId{ get; private set; }
       public Int64 Serial{ get; private set; }
       public Int32 SignedUser{ get; private set; }
       public DateTime Date{ get; private set; }
       public Boolean Printed{ get; private set; }
       public String DocumentShortName{ get; private set; }
       public String DocumentFullName{ get; private set; }
       public String MedexDataTable{ get; private set; }
       public String DocumentBody{ get; private set; }
       public String DocumentHeader{ get; private set; }
       public Boolean SetDeleted{ get; private set; }


       public Document
           (
                   Int16 _TypeId,
        Int64 _Serial,
        Int32 _SignedUser,
        DateTime _Date,
        Boolean _Printed,
        String _DocumentShortName,
        String _DocumentFullName,
        String _MedexDataTable,
        String _DocumentBody,
        String _DocumentHeader,
        Boolean _SetDeleted

           )
       {
           TypeId = _TypeId;
           Serial = _Serial;
           SignedUser = _SignedUser;
           Date = _Date;
           Printed = _Printed;
           DocumentShortName = _DocumentShortName;
           DocumentFullName = _DocumentFullName;
           MedexDataTable = _MedexDataTable;
           DocumentBody = _DocumentBody;
           DocumentHeader = _DocumentHeader;
           SetDeleted = _SetDeleted;

       }

    }
}
