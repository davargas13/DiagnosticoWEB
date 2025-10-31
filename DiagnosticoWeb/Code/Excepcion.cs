using System;
using System.IO;

namespace DiagnosticoWeb.Code
{
    public class Excepcion
    {
        public static void Registrar(Exception e) {
            Registrar(e, null);
        }

        public static void Registrar(string mensaje)
        {
            Registrar(null, mensaje);
        }

        public static void Registrar(Exception e, string mensaje)
        {
            var now = DateTime.Now;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var pathToSave = Path.Combine(path, now.ToString("yyyy-M-d") + ".txt");

            if (!File.Exists(pathToSave))
            {
                using (var sw = File.CreateText(pathToSave))
                {
                    sw.WriteLine(now.ToString("dd-MM-yyyy HH:mm:ss") +" "+ (e == null ? "" : (e.Message+" "+e.StackTrace+(e.InnerException==null?"":e.InnerException.Message))) +(mensaje==null ? "" : " "+mensaje));
                }
            }
            else
            {
                using (var sw = File.AppendText(pathToSave))
                {
                    sw.WriteLine(now.ToString("dd-MM-yyyy HH:mm:ss") + " " + (e == null ? "" : (e.Message + " " + e.StackTrace + (e.InnerException == null ? "" : e.InnerException.Message))) + (mensaje == null ? "" : " " + mensaje));
                }
            }           
        }
    }
}