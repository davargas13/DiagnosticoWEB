using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;

namespace DiagnosticoWeb.Controllers
{
    public class ReporteAPP
    {
        private static readonly string connectionString = "Server=139.70.80.11;Database=diagnostico_necesidades;User Id=DiagnosticoOSC;Password=34Ml8athZ!hw;";
        private static readonly string carpetaImagenes = "fotos_guardadas";
        private static readonly string urlBase = "https://peb.guanajuato.gob.mx/DiagnosticoNecesidades/Solicitudes/GetImagen/";

        public static async Task Main()
        {
            DateTime fechaInicio = new DateTime(2025, 4, 1);
            DateTime fechaFin = new DateTime(2025, 4, 30);
            string excelNombre = $"{DateTime.Now:yyyyMMdd}_resultado_reporte.xlsx";
            string zipNombre = "reporte_completo.zip";
            
            var datos = EjecutarStoredProcedure("sp_Reporte_Encuestas_Minutas", fechaInicio, fechaFin);

            try
            {                
                foreach (DataRow row in datos.Rows)
                {
                    string domicilioId = row["DomicilioId"]?.ToString();
                    string curp = row["NombreAsociacion"]?.ToString();

                    if (string.IsNullOrEmpty(domicilioId) || string.IsNullOrEmpty(curp))
                        continue;

                    var fotos = ObtenerImagenesPorDomicilio(domicilioId);
                    int index = 1;

                    foreach (var foto in fotos)
                    {
                        var imgBytes = await DescargarImagenBase64Async(foto.Id);
                        if (imgBytes != null)
                        {
                            GuardarImagen(imgBytes, curp, foto.Nombre, domicilioId, index++);
                        }
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ExportarExcel(datos, excelNombre);
            CrearZip(excelNombre, carpetaImagenes, zipNombre);

            Console.WriteLine($"✅ ZIP generado: {zipNombre}");
        }

        static DataTable EjecutarStoredProcedure(string spName, DateTime fechaInicio, DateTime fechaFin)
        {
            var table = new DataTable();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    table.Load(reader);
                }
            }
            return table;
        }

        static List<(int Id, string Nombre)> ObtenerImagenesPorDomicilio(string domicilioId)
        {
            var resultados = new List<(int, string)>();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT id AS archivo_id, Nombre AS archivo_nombre FROM Archivos WHERE Nombre LIKE @nombre", conn))
            {
                cmd.Parameters.AddWithValue("@nombre", $"{domicilioId}_%");
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultados.Add((reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            return resultados;
        }

        static async Task<byte[]> DescargarImagenBase64Async(int archivoId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{urlBase}{archivoId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                return null;
            }
        }

        static void GuardarImagen(byte[] imagenBase64, string curp, string archivoNombre, string domicilioId, int indice)
        {
            if (!Directory.Exists(carpetaImagenes))
                Directory.CreateDirectory(carpetaImagenes);

            string nombreLimpio = archivoNombre.Replace(domicilioId, "").Replace(" ", "_");
            string nombreArchivo = $"{curp}_{nombreLimpio}_{indice}.jpg";
            string ruta = Path.Combine(carpetaImagenes, nombreArchivo);

            var base64Str = Encoding.UTF8.GetString(imagenBase64);
            var bytes = Convert.FromBase64String(base64Str);
            File.WriteAllBytes(ruta, bytes);
        }

        static void ExportarExcel(DataTable datos, string nombreArchivo)
        {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var hoja = package.Workbook.Worksheets.Add("Reporte");
                hoja.Cells["A1"].LoadFromDataTable(datos, true);

                using (var rango = hoja.Cells[1, 1, 1, datos.Columns.Count])
                {
                    rango.Style.Font.Bold = true;
                    rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rango.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 75, 135));
                    rango.Style.Font.Color.SetColor(Color.White);
                }

                hoja.Cells[hoja.Dimension.Address].AutoFitColumns();
                File.WriteAllBytes(nombreArchivo, package.GetAsByteArray());
            }
        }

        static void CrearZip(string excelPath, string carpetaImagenes, string zipPath)
        {
            if (File.Exists(zipPath))
                File.Delete(zipPath);

            using (var archivoZip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                archivoZip.CreateEntryFromFile(excelPath, Path.GetFileName(excelPath));

                foreach (var file in Directory.GetFiles(carpetaImagenes))
                {
                    archivoZip.CreateEntryFromFile(file, Path.Combine("fotos_guardadas", Path.GetFileName(file)));
                }
            }

            File.Delete(excelPath);
            Directory.Delete(carpetaImagenes, true);
        }
    }
}
