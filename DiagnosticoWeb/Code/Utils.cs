using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Code
{
    public class Utils
    {
        public static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);
            var random = new Random();

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var caracter = (char) random.Next(offset, offset + lettersOffset);
                builder.Append(caracter);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(8, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(6));
            return passwordBuilder.ToString();
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }

            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static string HashDescryptableString(string key, string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string DecryptString(string key, string toDecrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Función que quita de un string saltos de linea, retornos de auto y tabuladores
        /// </summary>
        /// <param name="data">Cadena origen</param>
        /// <returns>Cadena limpia</returns>
        public static string LimpiarCadena(string data)
        {
            string result = data;
            result = result.Replace("\r", "");
            result = result.Replace("\n", "");
            result = result.Replace("\t", "");

            return result;
        }

        public static string DecimalToGrados(string dec)
        {
            try
            {
                var decimal_degrees = double.Parse(dec);
                decimal_degrees = Math.Abs(decimal_degrees);
                var grade = (int) decimal_degrees;
                var minutes = (decimal_degrees - Math.Floor(decimal_degrees)) * 60.0;
                var seconds = (minutes - Math.Floor(minutes)) * 60.0;
                var tenths = (seconds - Math.Floor(seconds)) * 10.0;

                minutes = Math.Floor(minutes);
                seconds = Math.Floor(seconds);
                tenths = Math.Floor(tenths);
                return grade + "°°" + minutes + "'" + seconds + "." + tenths + "''";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static Curp.Estado EstadoToEnum(Estado estado)
        {
            var claveInt = int.Parse(estado.Clave);
            switch (claveInt)
            {
                case 1:
                    return Curp.Estado.Aguascalientes;
                case 2:
                    return Curp.Estado.Baja_California;
                case 3:
                    return Curp.Estado.Baja_California_Sur;
                case 4:
                    return Curp.Estado.Campeche;
                case 5:
                    return Curp.Estado.Coahuila;
                case 6:
                    return Curp.Estado.Colima;
                case 7:
                    return Curp.Estado.Chiapas;
                case 8:
                    return Curp.Estado.Chihuahua;
                case 9:
                    return Curp.Estado.Distrito_Federal;
                case 10:
                    return Curp.Estado.Durango;
                case 11:
                    return Curp.Estado.Guanajuato;
                case 12:
                    return Curp.Estado.Guerrero;
                case 13:
                    return Curp.Estado.Hidalgo;
                case 14:
                    return Curp.Estado.Jalisco;
                case 15:
                    return Curp.Estado.Mexico;
                case 16:
                    return Curp.Estado.Michoacan;
                case 17:
                    return Curp.Estado.Morelos;
                case 18:
                    return Curp.Estado.Nayarit;
                case 19:
                    return Curp.Estado.Nuevo_Leon;
                case 20:
                    return Curp.Estado.Oaxaca;
                case 21:
                    return Curp.Estado.Puebla;
                case 22:
                    return Curp.Estado.Queretaro;
                case 23:
                    return Curp.Estado.Quintana_Roo;
                case 24:
                    return Curp.Estado.San_Luis_Potosi;
                case 25:
                    return Curp.Estado.Sinaloa;
                case 26:
                    return Curp.Estado.Sonora;
                case 27:
                    return Curp.Estado.Tabasco;
                case 28:
                    return Curp.Estado.Tamaulipas;
                case 29:
                    return Curp.Estado.Tlaxcala;
                case 30:
                    return Curp.Estado.Veracruz;
                case 31:
                    return Curp.Estado.Yucatan;
                case 32:
                    return Curp.Estado.Zacatecas;
                default:
                    return Curp.Estado.Extranjero;
            }
        }

        public static List<Model> BuildCatalogo(ApplicationDbContext _context, string catalogo)
        {
            var elementos = new List<Model>();
            switch (catalogo)
            {
                case "Estados":
                    _context.Estado.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Municipios":
                    _context.Municipio.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Localidades":
                    _context.Localidad.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Agebs":
                    _context.Ageb.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "ZonasImpulso":
                    _context.ZonaImpulso.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Caminos":
                    _context.Camino.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Carreteras":
                    _context.Carretera.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "TipoAsentamientos":
                    _context.TipoAsentamiento.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Ocupaciones":
                    _context.Ocupacion.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "EstadosCiviles":
                    _context.EstadoCivil.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break; 
                case "Estudios":
                    _context.Estudio.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "GradosEstudio":
                    _context.GradosEstudio.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Parentescos":
                    _context.Parentesco.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Sexos":
                    _context.Sexo.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Discapacidades":
                    _context.Discapacidad.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "Grados":
                    _context.Grado.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
                case "CausaDiscapacidad":
                    _context.CausaDiscapacidad.Where(m => m.DeletedAt == null).ToList().ForEach(m => elementos.Add(new Model
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        CreatedAt = m.CreatedAt
                    }));
                    break;
            }
            return elementos;
        }
        
        public static List<Dependencia> GetDepedenciasByUsuario(bool isAdmin, ApplicationUser user, 
            ApplicationDbContext _context){
            var dependencias = new List<Dependencia>();
            var depedenciasQuery = _context.Dependencia.Where(d=>d.DeletedAt == null);
            if (!isAdmin)
            {
                depedenciasQuery = depedenciasQuery.Where(x => x.Id == user.DependenciaId);
            }
            depedenciasQuery.OrderByDescending(x => x.Nombre).ToList().ForEach(d =>
                dependencias.Add(new Dependencia
                {
                    Id = d.Id,
                    Nombre = d.Nombre
                }));
            return dependencias;
        }
    }
}