using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
namespace baza
{


    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]


        static void Main()
        {
           // Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
          //  Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CultureInfo appCIRu = new CultureInfo(0x0419);
            CultureInfo appCIEn = new CultureInfo(0x0409);
            Thread.CurrentThread.CurrentCulture = appCIRu;
            Thread.CurrentThread.CurrentUICulture = appCIRu;
           // InputLanguage RuLng = InputLanguage.FromCulture(appCI);
           // AppDomain.CurrentDomain.AppendPrivatePath("Dlls");

            Application.Run(new StartupForm()) ;
           // Application.CurrentCulture = appCI ;
           
            
           

        }
    }
}
