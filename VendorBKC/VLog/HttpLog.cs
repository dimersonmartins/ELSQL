using System;
using System.IO;

namespace Vendor.VLog
{
    public static class HttpLog
    {
        #region Aplicação 
        public static void AppGravarLog(string logMessage, string local)
        {
            try
            {
                string dia = DateTime.Now.Day.ToString();
                string mes = DateTime.Now.Month.ToString();
                string ano = DateTime.Now.Year.ToString();

                AppCriarPastas(mes, ano);

                using (StreamWriter w = File.AppendText(@$"Logs\App\{ano}\{mes}\" + dia + ".txt"))
                {
                    AppLog(logMessage, local, w);
                }
            }
            catch { }
        }

        private static void AppCriarPastas(string mes, string ano)
        {
            try
            {
                if (!Directory.Exists(@"Logs\App"))
                    Directory.CreateDirectory(@"Logs\App");

                if (!Directory.Exists(@"Logs\App\" + ano))
                    Directory.CreateDirectory(@"Logs\App\" + ano);

                if (!Directory.Exists(@$"Logs\App\{ano}\" + mes))
                    Directory.CreateDirectory(@$"Logs\App\{ano}\" + mes);
            }
            catch { }
        }
     
        private static void AppLog(string logMessage, string local, TextWriter w)
        {
            try
            {
                w.WriteLine("");
                w.Write("Local => " + local);
                w.WriteLine("");
                w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                w.WriteLine("");
                w.WriteLine($"Messager:{logMessage}");
                w.WriteLine("");
                w.WriteLine("");
                w.WriteLine("");
            }
            catch { }
        }
        #endregion


        #region BancoDeDados
        public static void DatabaseGravarLog(string logMessage, string table)
        {
            try
            {
                string dia = DateTime.Now.Day.ToString();
                string mes = DateTime.Now.Month.ToString();
                string ano = DateTime.Now.Year.ToString();

                DatabaseCriarPastas(mes, ano);

                using (StreamWriter w = File.AppendText(@$"Logs\Database\{ano}\{mes}\" + dia + ".txt"))
                {
                    DatabaseLog(logMessage, table, w);
                }
            }
            catch { }
        }

        private static void DatabaseCriarPastas(string mes, string ano)
        {
            try
            {
                if (!Directory.Exists(@"Logs\Database"))
                    Directory.CreateDirectory(@"Logs\Database");

                if (!Directory.Exists(@"Logs\Database\" + ano))
                    Directory.CreateDirectory(@"Logs\Database\" + ano);

                if (!Directory.Exists(@$"Logs\Database\{ano}\" + mes))
                    Directory.CreateDirectory(@$"Logs\Database\{ano}\" + mes);
            }
            catch { }
        }

        private static void DatabaseLog(string logMessage, string table, TextWriter w)
        {
            try
            {
                w.WriteLine("");
                w.Write("table => " + table);
                w.WriteLine("");
                w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                w.WriteLine("");
                w.WriteLine($"Messager:{logMessage}");
                w.WriteLine("");
                w.WriteLine("");
                w.WriteLine("");
            }
            catch { }
        }
        #endregion
    }
}
