using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using moment.net;
using moment.net.Enums;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Syncfusion.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DiagnosticoWeb.Controllers
{
    public class DiagnosticosController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;        
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Objeto que permite saber elrol del usuario que esta usando el sistema</param>
        public DiagnosticosController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Funcion que muestra la vista con el listado de beneficiarios registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de beneficiarios registrados en la base de datos</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "diagnosticos.ver")]
        public IActionResult Index()
        {
            var response = new ReponseBeneficiarioIndex();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1).StartOf(DateTimeAnchor.Day);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).EndOf(DateTimeAnchor.Day);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd");
            return View("Index", response);
        }

        [HttpPost]
        [Authorize]
        public string ObtenerDiagnosticos([FromBody] BeneficiariosRequest request)
        {
            var response = new Dictionary<string , Beneficiario>();
            var beneficiarioId = "";
            var aplicacionId = "";
            var municipio = "";
            var aplicacion = "";
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year,now.Month,1).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year,now.Month,DateTime.DaysInMonth(now.Year,now.Month)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            }
            var Filtro_Estatus = "";


            var util = new BeneficiarioCode(_context);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var query = util.BuildBeneficiariosQuery(request, TipoConsulta.Resumen, 
                    false, inicio, fin, false);
                var municipios = _context.Municipio.Where(m=>m.DeletedAt==null).ToList();
                foreach (var mun in municipios)
                {
                    response.Add(mun.Id, new Beneficiario(){Nombre = mun.Nombre});
                }
                command.CommandText = query.Key;

                
                // Agregar el filtro de estatus al diccionario
                if (request.Estatus == "Completo" || request.Estatus == "Incompleto")
                {
                    Filtro_Estatus = " AND A.Estatus = '" + request.Estatus + "'";
                    command.CommandText = command.CommandText.Replace("A.DeletedAt is null", "A.DeletedAt is null " + Filtro_Estatus);
                }

                var iParametro = 0;
                foreach (var parametro in query.Value)
                {
                    command.Parameters.Add(new SqlParameter("@"+(iParametro++),parametro));
                }
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetString(0);
                            aplicacion = reader.GetString(1);
                            municipio = reader.GetString(2);
                            if (!beneficiarioId.Equals(id))
                            {
                                response[municipio].NumSolicitudes++;
                                beneficiarioId = id;
                            }
                            if (aplicacionId != aplicacion)
                            {
                                aplicacionId = aplicacion;
                                response[municipio].Id = aplicacion;
                            }
                        }
                    }
                }
            }
            if (aplicacionId != aplicacion)
            {
                response[municipio].Id = aplicacion;
            }
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var aplicaciones = new List<string>();
                foreach (var mun in response)
                {
                    if (mun.Value.Id != null)
                    {
                        aplicaciones.Add("'"+mun.Value.Id+"'");
                    }
                }

                if (aplicaciones.Any())
                {
                    var query = "SELECT A.CreatedAt, M.Id from AplicacionPreguntas AP inner join Aplicaciones A on AP.AplicacionId = A.Id "+
                        "inner join Beneficiarios B on A.BeneficiarioId = B.Id inner join Domicilio D on B.DomicilioId = D.Id "+
                        "inner join Municipios M on M.id = D.MunicipioId where A.Prueba = 0 and A.Id in(" + string.Join(",", aplicaciones) + ") " + Filtro_Estatus + " GROUP BY a.CreatedAt, M.Id";
                    //var query = "EXEC sp_Reporte_Encuestas_Viviendas @FechaInicio = N'1/04/2025', @FechaFin = N'9/04/2025';";
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                response[reader.GetString(1)].Folio = reader.GetDateTime(0).ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
            }
            _context.Database.CloseConnection();
            return JsonSedeshu.SerializeObject(response);
        }

        private byte[] GenerarKML(DataTable data)
        {
            XNamespace ns = "http://www.opengis.net/kml/2.2";
            XElement kml = new XElement(ns + "kml",
                new XElement(ns + "Document",
                    new XElement(ns + "name", "Puntos GPS")
                )
            );

            XElement document = kml.Element(ns + "Document");

            foreach (DataRow row in data.Rows)
            {
                string latitud = row["Latitud"]?.ToString();
                string longitud = row["Longitud"]?.ToString();

                if (string.IsNullOrWhiteSpace(latitud))
                    latitud = row["LatitudAtm"]?.ToString();
                if (string.IsNullOrWhiteSpace(longitud))
                    longitud = row["LongitudAtm"]?.ToString();

                if (string.IsNullOrWhiteSpace(latitud) || string.IsNullOrWhiteSpace(longitud))
                    continue;

                string etiqueta = "";// row["Encuestador"]?.ToString() ?? "Sin nombre";
                string estatus = row["Estatus"]?.ToString()?.Trim().ToLower();

                string iconoUrl;

                if (estatus == "completa")
                    iconoUrl = "http://maps.google.com/mapfiles/kml/paddle/grn-circle.png"; // verde
                else if (estatus == "incompleta")
                    iconoUrl = "http://maps.google.com/mapfiles/kml/paddle/ylw-circle.png"; // amarillo
                else
                    iconoUrl = "http://maps.google.com/mapfiles/kml/paddle/red-circle.png"; // rojo

                string styleId = $"icon_{etiqueta.GetHashCode()}_{estatus}";

                if (document.Elements(ns + "Style").FirstOrDefault(s => (string)s.Attribute("id") == styleId) == null)
                {
                    document.Add(new XElement(ns + "Style",
                        new XAttribute("id", styleId),
                        new XElement(ns + "IconStyle",
                            new XElement(ns + "Icon",
                                new XElement(ns + "href", iconoUrl)
                            ),
                            new XElement(ns + "scale", "1.2")
                        )
                    ));
                }

                var usuarioLogueado = User.Identity.Name;
                // Columnas a mostrar en la tabla
                var columnas = new[] { "" };
                if (usuarioLogueado == "mcortes")
                {
                    columnas = new[]
                    {
                        "Username", /*"Nombre", "ApellidoPaterno", "ApellidoMaterno",*/ "Estatus", "Municipio", "Localidad", "TipoAsentamiento", "Calle", "Colonia", "NumExterior", "NumInterior", "codigopostal", "Latitud", "Longitud", "LatitudAtm", "LongitudAtm", "Telefono", "TelefonoCasa", /*"RFC", "curp",*/ "FechaNacimiento", "estadoNacimiento", "sexo", "EstadoCivil", "Estudios", "GradoEstudios", "Discapacidad", "CausaDiscapacidad", "FechaEncuestaCreada", "FechaDomicilioCreado"
                    };
                }
                else
                {
                    columnas = new[]
                    {
                        "Username", "Nombre", "ApellidoPaterno", "ApellidoMaterno", "Estatus", "Municipio", "Localidad", "TipoAsentamiento", "Calle", "Colonia", "NumExterior", "NumInterior", "codigopostal", "Latitud", "Longitud", "LatitudAtm", "LongitudAtm", "Telefono", "TelefonoCasa", "RFC", "curp", "FechaNacimiento", "estadoNacimiento", "sexo", "EstadoCivil", "Estudios", "GradoEstudios", "Discapacidad", "CausaDiscapacidad", "FechaEncuestaCreada", "FechaDomicilioCreado"
                    };
                }


                    // Generar tabla HTML bonita
                    string htmlDescripcion = "<![CDATA[<table border='1' cellpadding='4' cellspacing='0' style='border-collapse:collapse; font-family:Arial; font-size:12px;'>";
                foreach (var col in columnas)
                {
                    var valor = row[col]?.ToString();
                    if (!string.IsNullOrWhiteSpace(valor))
                    {
                        htmlDescripcion += $"<tr><td style='background-color:#f2f2f2; font-weight:bold;'>{col}</td><td> :&nbsp {valor}</td></tr><br>";
                    }
                }
                htmlDescripcion += "</table>";

                XElement placemark = new XElement(ns + "Placemark",
                    new XElement(ns + "name", etiqueta),
                    new XElement(ns + "description", htmlDescripcion),
                    new XElement(ns + "styleUrl", $"#{styleId}"),
                    new XElement(ns + "Point",
                    new XElement(ns + "coordinates", $"{longitud},{latitud},0")
                    )
                );

                document.Add(placemark);
            }

            return Encoding.UTF8.GetBytes(kml.ToString());
        }


        public IActionResult exportarSabanaExcel(BeneficiariosRequest request, string inicio, string fin)
        {
            request.Estatus = string.IsNullOrEmpty(request.Estatus) ? "" : request.Estatus;
            var usuarioLogueado = User.Identity.Name;

            if (!DateTime.TryParse(inicio, out DateTime fechaInicio) || !DateTime.TryParse(fin, out DateTime fechaFin))
                return BadRequest("Fechas inválidas. Usa el formato YYYY-MM-DD.");

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                // ✅ Datos principales (sabana)
                var data = ObtenerDatosSabana(connectionString, fechaInicio, fechaFin, request.Estatus);

                // ✅ Datos del SP (diccionario)
                var estadisticas = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                //using (var cmd = new SqlCommand("sp_Diccionario_Encuesta_Familias", conn))
                using (var cmd = new SqlCommand("sp_Diccionario_Encuesta", conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    adapter.Fill(estadisticas);
                }

                //// ✅ Filtrado de columnas si el usuario es "mcortes"
                //var columnasAEliminar = new List<string> { "Nombre", "ApellidoPaterno", "ApellidoMaterno", "RFC", "curp" };
                DataTable dataFiltrado = data.Copy();

                //if (usuarioLogueado == "mcortes")
                //{
                //    foreach (string columna in columnasAEliminar)
                //    {
                //        if (dataFiltrado.Columns.Contains(columna))
                //        {
                //            dataFiltrado.Columns.Remove(columna);
                //        }
                //    }
                //}

                using (var zipStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                    {
                        // 1. Generar Excel con dos hojas (Datos + Diccionario)
                        using (var excelStream = new MemoryStream())
                        using (var excelPackage = new ExcelPackage())
                        {
                            void FormatearHoja(ExcelWorksheet sheet, DataTable tabla, System.Drawing.Color headerColor)
                            {
                                using (var range = sheet.Cells[1, 1, 1, tabla.Columns.Count])
                                {
                                    range.Style.Font.Bold = true;
                                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    range.Style.Fill.BackgroundColor.SetColor(headerColor);
                                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                }

                                sheet.View.FreezePanes(2, 1);
                                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                                sheet.Cells[1, 1, 1, tabla.Columns.Count].AutoFilter = true;
                            }

                            // Hoja principal
                            var hojaDatos = excelPackage.Workbook.Worksheets.Add("Datos Base");
                            hojaDatos.Cells["A1"].LoadFromDataTable(dataFiltrado, true);
                            FormatearHoja(hojaDatos, dataFiltrado, System.Drawing.Color.SteelBlue);

                            // Hoja de estadísticas
                            var hojaEst = excelPackage.Workbook.Worksheets.Add("Diccionario");
                            hojaEst.Cells["A1"].LoadFromDataTable(estadisticas, true);
                            FormatearHoja(hojaEst, estadisticas, System.Drawing.Color.DarkGreen);

                            excelPackage.SaveAs(excelStream);
                            excelStream.Position = 0;

                            var excelEntry = archive.CreateEntry($"Reporte Necesidades_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.xlsx");
                            using (var entryStream = excelEntry.Open())
                            {
                                excelStream.CopyTo(entryStream);
                            }
                        }

                        //2.Generar KML con datos filtrados(si corresponde)
                        //var datosParaKML = usuarioLogueado == "mcortes" ? dataFiltrado : data;
                        //var kmlBytes = GenerarKML(datosParaKML);
                        //var kmlEntry = archive.CreateEntry($"Puntos_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.kml");
                        //using (var entryStream = kmlEntry.Open())
                        //{
                        //    entryStream.Write(kmlBytes, 0, kmlBytes.Length);
                        //}
                    }

                    zipStream.Position = 0;
                    return File(zipStream.ToArray(), "application/zip",
                        $"Reporte_Necesidades{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.zip");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }



        /*
        private IActionResult exportarSabanaExcel(BeneficiariosRequest request, string inicio, string fin)
        {
            if (!DateTime.TryParse(inicio, out DateTime fechaInicio) || !DateTime.TryParse(fin, out DateTime fechaFin))
                return BadRequest("Fechas inválidas. Usa el formato YYYY-MM-DD.");

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string tempPath = Path.Combine(rootPath, "temp");
                string imagesPath = Path.Combine(tempPath, "imagenes");

                // Preparar carpetas
                if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
                Directory.CreateDirectory(imagesPath);

                var data = ObtenerDatosReporte(connectionString, fechaInicio, fechaFin);
                //DescargarImagenesRelacionadas(connectionString, data, rootPath, imagesPath, tempPath).GetAwaiter().GetResult(); //comentado para que se descargar rapido el excel, si se quiere descargar las imagenes, descomentar esta linea

                // Crear ZIP en memoria
                using (var zipStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                    {
                        //// Agregar archivo Excel
                        //var excelEntry = archive.CreateEntry("Reporte.xlsx");
                        //using (var entryStream = excelEntry.Open())
                        //using (var excelPackage = new ExcelPackage())
                        //{
                        //    var ws = excelPackage.Workbook.Worksheets.Add("Datos");

                        //    ws.View.FreezePanes(2, 1);
                        //    ws.Row(1).Height = 20;

                        //    for (int col = 0; col < data.Columns.Count; col++)
                        //    {
                        //        var cell = ws.Cells[1, col + 1];
                        //        cell.Value = data.Columns[col].ColumnName;
                        //        cell.Style.Font.Bold = true;
                        //        cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //        cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SteelBlue);
                        //        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //    }

                        //    for (int row = 0; row < data.Rows.Count; row++)
                        //    {
                        //        for (int col = 0; col < data.Columns.Count; col++)
                        //        {
                        //            ws.Cells[row + 2, col + 1].Value = data.Rows[row][col];
                        //        }
                        //    }

                        //    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        //    excelPackage.SaveAs(entryStream);
                        

                        using (var excelStream = new MemoryStream())
                        using (var excelPackage = new ExcelPackage())
                        {
                            var ws = excelPackage.Workbook.Worksheets.Add("Datos");

                            ws.View.FreezePanes(2, 1);
                            ws.Row(1).Height = 20;

                            for (int col = 0; col < data.Columns.Count; col++)
                            {
                                var cell = ws.Cells[1, col + 1];
                                cell.Value = data.Columns[col].ColumnName;
                                cell.Style.Font.Bold = true;
                                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SteelBlue);
                                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }

                            for (int row = 0; row < data.Rows.Count; row++)
                            {
                                for (int col = 0; col < data.Columns.Count; col++)
                                {
                                    ws.Cells[row + 2, col + 1].Value = data.Rows[row][col];
                                }
                            }

                            ws.Cells[ws.Dimension.Address].AutoFitColumns();

                            excelPackage.SaveAs(excelStream);
                            excelStream.Position = 0;

                            string nombreArchivo = $"Reporte_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.xlsx";
                            return File(excelStream.ToArray(),
                                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                        nombreArchivo);
                        }
                    

                        //// Agregar imágenes al ZIP
                        //foreach (var imagenPath in Directory.GetFiles(imagesPath))
                        //{
                        //    var entry = archive.CreateEntry("imagenes/" + Path.GetFileName(imagenPath));
                        //    using (var fileStream = System.IO.File.OpenRead(imagenPath))
                        //    using (var entryStream = entry.Open())
                        //    {
                        //        fileStream.CopyTo(entryStream);
                        //    }
                        //}

                        //// Agregar faltantes.txt si existe
                        //string faltantesPath = Path.Combine(tempPath, "faltantes.txt");
                        //if (System.IO.File.Exists(faltantesPath))
                        //{
                        //    var entry = archive.CreateEntry("faltantes.txt");
                        //    using (var fileStream = System.IO.File.OpenRead(faltantesPath))
                        //    using (var entryStream = entry.Open())
                        //    {
                        //        fileStream.CopyTo(entryStream);
                        //    }
                        //}
                    }

                    //zipStream.Position = 0;
                    //string nombreZip = $"Reporte_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.zip";
                    //return File(zipStream.ToArray(), "application/zip", nombreZip);

                    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
        */

        private DataTable ObtenerDatosReporte(string connectionString, DateTime inicio, DateTime fin)
        {
            var dataTable = new DataTable();

            using (var conn = new SqlConnection(connectionString))
            //using (var cmd = new SqlCommand("sp_Reporte_Encuesta_F", conn))
            using (var cmd = new SqlCommand("sp_Reporte_Encuesta_F", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // aumentar el timeout (ejemplo: 300 segundos = 5 minutos)
                cmd.CommandTimeout = 600;

                cmd.Parameters.AddWithValue("@FechaInicio", inicio);
                cmd.Parameters.AddWithValue("@FechaFin", fin);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }

        private DataTable ObtenerDatosSabana(string connectionString, DateTime inicio, DateTime fin, string Estatus)
        {
            var dataTable = new DataTable();

            using (var conn = new SqlConnection(connectionString))
            //using (var cmd = new SqlCommand("sp_Reporte_Encuestas_Familias", conn))
            using (var cmd = new SqlCommand("sp_Reporte_Sabana_M", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // aumentar el timeout (ejemplo: 300 segundos = 5 minutos)
                cmd.CommandTimeout = 1000;

                cmd.Parameters.AddWithValue("@FechaInicio", inicio);
                cmd.Parameters.AddWithValue("@FechaFin", fin);
                //cmd.Parameters.AddWithValue("@Estatus", Estatus);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }


        private async Task DescargarImagenesRelacionadas(string connectionString, DataTable data, string rootPath, string imagesPath, string tempPath)
        {
            var faltantes = new List<string>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataRow row in data.Rows)
                {
                    string beneficiarioId = row["BeneficiarioId"].ToString();
                    string domicilioId = row["DomicilioId"].ToString();
                    string curp = row["curp"].ToString();
                    if(string.IsNullOrEmpty(curp))
                    {
                        curp = beneficiarioId;// + DateTime.Now;
                    }

                    var query = @"
                                    SELECT id AS archivo_id, Nombre AS archivo_nombre
                                    FROM Archivos
                                    WHERE Nombre LIKE @param1 OR Nombre LIKE @param2 OR Nombre LIKE @param3";

                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@param1", $"{domicilioId}_F%");
                    cmd.Parameters.AddWithValue("@param2", $"firma{beneficiarioId}");
                    cmd.Parameters.AddWithValue("@param3", $"foto_perfil{beneficiarioId}");

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string archivoId = reader["archivo_id"].ToString();
                        string nombreArchivo = reader["archivo_nombre"].ToString();
                        
                        if (nombreArchivo.Contains("foto_perfil") || nombreArchivo.Contains("firma"))
                        {
                            nombreArchivo = nombreArchivo.Replace(beneficiarioId, curp + "_");
                        }
                        else
                        {
                            nombreArchivo = nombreArchivo.Replace(domicilioId, curp + "_");
                        }

                        //string rutaImagen = Path.Combine(rootPath, "archivos", $"{archivoId}.jpg");

                        // Obtener la imagen en Base64 llamando al método GetImagen
                        string base64Imagen = await ObtenerImagenPorId(archivoId);

                        if (!string.IsNullOrEmpty(base64Imagen))
                        {
                            try
                            {
                                // Convertir Base64 a bytes
                                byte[] imageBytes = Convert.FromBase64String(base64Imagen);

                                // Guardar imagen en la carpeta de destino
                                string destino = Path.Combine(imagesPath, nombreArchivo+".jpg");
                                System.IO.File.WriteAllBytes(destino, imageBytes);
                            }
                            catch (FormatException ex)
                            {
                                // Si la conversión de Base64 falla
                                //faltantes.Add($"{archivoId} ({archivoNombre}) - Error al decodificar la imagen: {ex.Message}");
                                string logPath = Path.Combine(tempPath, "faltantes.txt");
                                string mensaje = $"Error al decodificar la imagen: {archivoId}, Nombre: {nombreArchivo}{Environment.NewLine}";
                                System.IO.File.AppendAllText(logPath, mensaje);
                            }
                        }
                        else
                        {
                            string logPath = Path.Combine(tempPath, "faltantes.txt");
                            string mensaje = $"Imagen no encontrada para ID: {archivoId}, Nombre: {nombreArchivo}{Environment.NewLine}";
                            System.IO.File.AppendAllText(logPath, mensaje);
                        }
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }
        }

        private async Task<string> ObtenerImagenPorId(string id)
        {
            using (var client = new HttpClient())
            {
                // Ajusta la URL según la configuración de tu API
                string apiUrl = $"https://peb.guanajuato.gob.mx/diagnosticoNecesidades/Solicitudes/GetImagen/{id}"; // Asegúrate de que esta URL sea correcta
                string apiUrl2 = $"{Request.Scheme}://{Request.Host}/Solicitudes/GetImagen/{id}";

                try
                {
                    // Llamar al API GetImagen
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Verificar si la respuesta es exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer la imagen en Base64 desde el cuerpo de la respuesta
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // Si la llamada a la API falla
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    return null;
                }
            }
        }

        private void GenerarExcel(DataTable data, string rutaExcel)
        {
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Datos");

                ws.View.FreezePanes(2, 1);
                ws.Row(1).Height = 20;

                for (int col = 0; col < data.Columns.Count; col++)
                {
                    var cell = ws.Cells[1, col + 1];
                    cell.Value = data.Columns[col].ColumnName;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SteelBlue);
                    cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                for (int row = 0; row < data.Rows.Count; row++)
                {
                    for (int col = 0; col < data.Columns.Count; col++)
                    {
                        ws.Cells[row + 2, col + 1].Value = data.Rows[row][col];
                    }
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                package.SaveAs(new FileInfo(rutaExcel));
            }
        }


        private IActionResult exportarSabana(BeneficiariosRequest request, string inicio, string fin)
        {
            var util = new BeneficiarioCode(_context);
            var builder = util.BuildSabana(request, inicio, fin);
            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = AccionBitacora.EXPORTACION,
                Mensaje = "Se exportó sabana.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            _context.SaveChanges();
            return File(Encoding.UTF8.GetBytes(builder), "text/csv", "base"
                            +inicio.Replace("-","").Substring(0,9)+" al "
                            +fin.Replace("-","").Substring(0,9)+".csv");
        }
        
        public IActionResult exportarDiccionarios(BeneficiariosRequest request)
        {
            var excel = new ExcelPackage();
            excel.Workbook.Worksheets.Add("Preguntas");
            excel.Workbook.Worksheets.Add("PreguntasGrados");
            excel.Workbook.Worksheets.Add("Respuestas");
            excel.Workbook.Worksheets.Add("RespuestasGrados");
            excel.Workbook.Worksheets.Add("Categorias");

            var iPregunta = 1;
            var iPreguntaGrado = 1;
            var iRespuesta = 1;
            var iRespuestaGrado = 1;
            var iCarencia = 1;
            var preguntas = excel.Workbook.Worksheets.First();
            var preguntasGrados = excel.Workbook.Worksheets[1];
            var respuestas = excel.Workbook.Worksheets[2];
            var respuestasGrados = excel.Workbook.Worksheets[3];
            var carencias = excel.Workbook.Worksheets[4];
            
            var encuestaVersion = _context.EncuestaVersion.First(ev => ev.Activa);
            var preguntasDB = _context.Pregunta.Where(p => p.DeletedAt == null && p.EncuestaVersionId.Equals(encuestaVersion.Id)).
                OrderBy(p => p.Numero).Include(p=>p.Respuestas).ThenInclude(r=>r.RespuestaGrados)
                .Include(p=>p.PreguntaGrados).ToList();
            var carenciasDB = _context.Carencia.ToList();
            
            carencias.Cells[iCarencia, 1].Value = "Id";
            carencias.Cells[iCarencia, 2].Value = "Clave";
            carencias.Cells[iCarencia, 3].Value = "Nombre";
            carencias.Cells[iCarencia++, 4].Value = "Fecha de creación";
            
            preguntas.Cells[iPregunta, 1].Value = "Id";
            preguntas.Cells[iPregunta, 2].Value = "Numero";
            preguntas.Cells[iPregunta, 3].Value = "Variable";
            preguntas.Cells[iPregunta, 4].Value = "Nombre";
            preguntas.Cells[iPregunta, 5].Value = "Tipo";
            preguntas.Cells[iPregunta, 6].Value = "Condicion";
            preguntas.Cells[iPregunta, 7].Value = "Carencia";
            preguntas.Cells[iPregunta, 8].Value = "Iterable";
            preguntas.Cells[iPregunta++, 9].Value = "Fecha de creación";
            
            preguntasGrados.Cells[iPreguntaGrado, 1].Value = "Id";
            preguntasGrados.Cells[iPreguntaGrado, 2].Value = "Pregunta";
            preguntasGrados.Cells[iPreguntaGrado, 3].Value = "Grado";
            preguntasGrados.Cells[iPreguntaGrado++, 4].Value = "Fecha de creación";

            respuestas.Cells[iRespuesta, 1].Value = "Id";
            respuestas.Cells[iRespuesta, 2].Value = "Numero";
            respuestas.Cells[iRespuesta, 3].Value = "Variable";
            respuestas.Cells[iRespuesta, 4].Value = "Nombre";
            respuestas.Cells[iRespuesta, 5].Value = "Pregunta";
            respuestas.Cells[iRespuesta, 6].Value = "Unica";
            respuestas.Cells[iRespuesta, 7].Value = "Complemento";
            respuestas.Cells[iRespuesta++, 8].Value = "Fecha de creación";
            
            respuestasGrados.Cells[iRespuestaGrado, 1].Value = "Id";
            respuestasGrados.Cells[iRespuestaGrado, 2].Value = "Respuesta";
            respuestasGrados.Cells[iRespuestaGrado, 3].Value = "Grado";
            respuestasGrados.Cells[iRespuestaGrado++, 4].Value = "Fecha de creación";

            foreach (var pregunta in preguntasDB)
            {
                preguntas.Cells[iPregunta, 1].Value = pregunta.Id;
                preguntas.Cells[iPregunta, 2].Value = pregunta.Numero;
                preguntas.Cells[iPregunta, 3].Value = "var"+pregunta.Id+"_1";
                preguntas.Cells[iPregunta, 4].Value = pregunta.Nombre;
                preguntas.Cells[iPregunta, 5].Value = pregunta.TipoPregunta;
                preguntas.Cells[iPregunta, 6].Value = pregunta.Condicion;
                preguntas.Cells[iPregunta, 7].Value = pregunta.CarenciaId;
                preguntas.Cells[iPregunta, 8].Value = pregunta.Iterable;
                preguntas.Cells[iPregunta++, 9].Value = pregunta.CreatedAt.ToString("dd/MM/yyyy");

                foreach (var preguntaGrado in pregunta.PreguntaGrados)
                {
                    preguntasGrados.Cells[iPreguntaGrado, 1].Value = preguntaGrado.Id;
                    preguntasGrados.Cells[iPreguntaGrado, 2].Value = preguntaGrado.PreguntaId;
                    preguntasGrados.Cells[iPreguntaGrado, 3].Value = preguntaGrado.Grado;
                    preguntasGrados.Cells[iPreguntaGrado++, 4].Value = preguntaGrado.CreatedAt.ToString("dd/MM/yyyy");
                }

                foreach (var respuesta in pregunta.Respuestas.OrderBy(r=>r.Numero))
                {
                    respuestas.Cells[iRespuesta, 1].Value = respuesta.Id;
                    respuestas.Cells[iRespuesta, 2].Value = respuesta.Numero;
                    respuestas.Cells[iRespuesta, 3].Value = "";
                    respuestas.Cells[iRespuesta, 4].Value = respuesta.Nombre;
                    respuestas.Cells[iRespuesta, 5].Value = respuesta.PreguntaId;
                    respuestas.Cells[iRespuesta, 6].Value = respuesta.Negativa;
                    respuestas.Cells[iRespuesta, 7].Value = respuesta.Complemento;
                    respuestas.Cells[iRespuesta++, 8].Value = respuesta.CreatedAt.ToString("dd/MM/yyyy");

                    foreach (var respuestaGrado in respuesta.RespuestaGrados)
                    {
                        respuestasGrados.Cells[iRespuestaGrado, 1].Value = respuestaGrado.Id;
                        respuestasGrados.Cells[iRespuestaGrado, 2].Value = respuestaGrado.RespuestaId;
                        respuestasGrados.Cells[iRespuestaGrado, 3].Value = respuestaGrado.Grado;
                        respuestasGrados.Cells[iRespuestaGrado++, 4].Value = respuestaGrado.CreatedAt.ToString("dd/MM/yyyy");
                    }
                }

                if (pregunta.Catalogo != null)
                {
                    var iNumero = 1;
                    var catalogo = Utils.BuildCatalogo(_context, pregunta.Catalogo);
                    foreach (var m in catalogo)
                    {
                        respuestas.Cells[iRespuesta, 1].Value = m.Id;
                        respuestas.Cells[iRespuesta, 2].Value = iNumero++;
                        respuestas.Cells[iRespuesta, 3].Value = "";
                        respuestas.Cells[iRespuesta, 4].Value = m.Nombre;
                        respuestas.Cells[iRespuesta, 5].Value = pregunta.Id;
                        respuestas.Cells[iRespuesta, 6].Value = false;
                        respuestas.Cells[iRespuesta, 7].Value = null;
                        respuestas.Cells[iRespuesta++, 8].Value = m.CreatedAt.ToString("dd/MM/yyyy");
                    }
                }
            }

            foreach (var carencia in carenciasDB)
            {
                carencias.Cells[iCarencia, 1].Value = carencia.Id;
                carencias.Cells[iCarencia, 2].Value = carencia.Clave;
                carencias.Cells[iCarencia, 3].Value = carencia.Nombre;
                carencias.Cells[iCarencia++, 4].Value = carencia.CreatedAt.ToString("dd/MM/yyyy");
            }
            
            return File(
                fileContents: excel.GetAsByteArray(),
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "diccionario.xlsx"
            );
        }
        
        /// <summary>
        /// Funcion que exporta a Excel el listado de beneficiarios consultado por el usuario de acuerdo a los filtros seleccionados
        /// </summary>
        /// <param name="request">Filtros seleccionados por el usuario</param>
        /// <returns>Archivo de excel con el listado de beneficiarios encontrados en la base de datos</returns>
        [HttpPost]
        [Authorize]
        public IActionResult exportarExcel(BeneficiariosRequest request)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year,now.Month,1).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year,now.Month,DateTime.DaysInMonth(now.Year,now.Month)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            }
            try
            {
                if (request.Tipo=="Sabana")
                {
                    //return exportarSabana(request, inicio, fin);
                    //exportarSabana(request, inicio, fin);
                    return exportarSabanaExcel(request, inicio, fin);
                }
                return exportarDiccionarios(request);
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult EstatusVisitas(BeneficiariosRequest request)
        {
            var response = new ReponseBeneficiarioIndex();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1).StartOf(DateTimeAnchor.Day);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).EndOf(DateTimeAnchor.Day);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss");
            return View("Visitas", response);
        }
        
        [HttpPost]
        [Authorize]
        public string ObtenerVisitas([FromBody] BeneficiariosRequest request)
        {
            var response = new Dictionary<string , Beneficiario>();
            var beneficiarioId = "";
            var aplicacionId = "";
            var resultado = "";
            var aplicacion = "";
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year,now.Month,1).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year,now.Month,DateTime.DaysInMonth(now.Year,now.Month)).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var resultados = _context.Respuesta.Where(r => r.PreguntaId.Equals("138")).ToList();
            
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            }
            
            var util = new BeneficiarioCode(_context);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var query = util.BuildBeneficiariosQuery(request, TipoConsulta.Resumen, false, inicio, fin, false);
                foreach (var res in resultados)
                {
                    response.Add(res.Nombre, new Beneficiario{Nombre = res.Nombre});
                }
                command.CommandText = query.Key;
                var iParametro = 0;
                foreach (var parametro in query.Value)
                {
                    command.Parameters.Add(new SqlParameter("@"+(iParametro++),parametro));
                }
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetString(0);
                            aplicacion = reader.GetString(1);
                            resultado = reader.IsDBNull(3) ? "Incompleta": reader.GetString(3);
                            if (!beneficiarioId.Equals(id))
                            {
                                response[resultado].NumSolicitudes++;
                                beneficiarioId = id;
                            }
                            if (aplicacionId != aplicacion)
                            {
                                aplicacionId = aplicacion;
                                response[resultado].Id = aplicacion;
                            }
                        }
                    }
                }
            }
            _context.Database.CloseConnection();
            return JsonSedeshu.SerializeObject(response);
        }
        
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "diagnosticos.ver")]
        public IActionResult VerRutas()
        {
            var response = new VerRutasResponse();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1).StartOf(DateTimeAnchor.Day);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).EndOf(DateTimeAnchor.Day);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss");
            var dependencias = _context.Dependencia.Where(d => d.DeletedAt == null).ToList();
            var depList = new List<DependenciaApiModel>();
            if (dependencias.Any())
            {
                depList.AddRange(dependencias.Select(d => new DependenciaApiModel()
                {
                    Id = d.Id, Nombre = d.Nombre
                }));
            }

            var municipios = _context.Municipio.Where(m => m.DeletedAt == null).ToList();
            var munList = new List<MunicipioApiModel>();
            if (municipios.Any())
            {
                munList.AddRange(municipios.Select(m => new MunicipioApiModel()
                {
                    Id = m.Id, Nombre = m.Nombre
                }));
            }

            response.Dependencias = depList;
            response.Municipios = munList;
            
            return View("Rutas", response);
        }

        [HttpGet]
        [Authorize]
        public string GetTrabajadoresDependencia(string id="")
        {
            var response = new GetTrabajadoresResponse();
            var trabajadores = _context.Trabajador.Where(x => x.DeletedAt == null)
                .Join(_context.TrabajadorDependencia, t=>t.Id, td=>td.TrabajadorId, (x,t)=>new
                {
                    Trabajador=x,
                    TrabajadorDependencia=t
                }).Where(t =>  t.TrabajadorDependencia.DependenciaId.Equals(id) 
                               && t.TrabajadorDependencia.DeletedAt==null)
                .Select(t=>t.Trabajador);
            
            var Trabajadores = new List<TrabajadorList>();
            foreach (var trabajador in trabajadores)
            {
                Trabajadores.Add(new TrabajadorList()
                {
                    Id = trabajador.Id,
                    Nombre = trabajador.Nombre,
                    Username = trabajador.Username,
                    Tipo = trabajador.Tipo == 0 ? "Encuestador" : "Articulador",
                });
            }

            response.Trabajadores = Trabajadores;
            
            return JsonSedeshu.SerializeObject(response);
        }

        [HttpGet("/Diagnosticos/GetTrabajadoresDependencias/{dependenciasIds}")]
        [Authorize]
        public string GetTrabajadoresDependencias(string dependenciasIds="")
        {
            var response = new GetTrabajadoresResponse();
            string[] arrDependenciasId = dependenciasIds != null ? dependenciasIds.Split(new char[] { ',' }) : new string[]{};
            var trabajadores = _context.Trabajador.Where(x => x.DeletedAt == null)
                .Join(_context.TrabajadorDependencia, t=>t.Id, td=>td.TrabajadorId, (x,t)=>new
                {
                    Trabajador=x,
                    TrabajadorDependencia=t
                }).Where(t =>  arrDependenciasId.Contains(t.TrabajadorDependencia.DependenciaId) 
                               && t.TrabajadorDependencia.DeletedAt==null)
                .Select(t=>t.Trabajador);
            
            var Trabajadores = new List<TrabajadorList>();
            foreach (var trabajador in trabajadores)
            {
                Trabajadores.Add(new TrabajadorList()
                {
                    Id = trabajador.Id,
                    Nombre = trabajador.Nombre,
                    Username = trabajador.Username,
                    Tipo = trabajador.Tipo == 0 ? "Encuestador" : "Articulador",
                });
            }

            response.Trabajadores = Trabajadores;
            
            return JsonSedeshu.SerializeObject(response);
        }

        [HttpPost]
        [Authorize]
        public string GetRutas([FromBody] GetRutasRequest request)
        {
            var response = new GetRutasResponse();
            DateTime inicio;
            DateTime fin;
            DateTime.TryParseExact(request.Inicio.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out inicio);
            inicio = inicio.StartOf(DateTimeAnchor.Day);
            DateTime.TryParseExact(request.Fin.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out fin);
            fin = fin.EndOf(DateTimeAnchor.Day);
            var arrMunicipiosId = request.MunicipiosId != null ? request.MunicipiosId.Split(new char[] { ',' }) : new string[]{};
            var arrLocalidadesId = request.LocalidadesId != null ? request.LocalidadesId.Split(new char[] { ',' }) : new string[]{};
            var arrAgebsId = request.AgebsId != null ? request.AgebsId.Split(new char[] { ',' }) : new string[]{};
            var aplicacionesQuery = _context.Aplicacion
                .Include(a => a.Beneficiario)
                    .ThenInclude(b => b.Domicilio)
                .Where(a => a.TrabajadorId.Equals(request.TrabajadorId))
                .Where(a => a.FechaInicio >= inicio.Date)
                .Where(a => a.FechaInicio <= fin.Date)
                .Where(a => a.Activa)
                .Where(a => a.DeletedAt == null);

            var aplicaciones = aplicacionesQuery
                .OrderByDescending(a => a.FechaInicio).ToList();

            var totalGeneral = 0;
            var Domicilios = new List<RutasDomicilioModel>();
            foreach (var aplicacion in aplicaciones)
            {
                var domicilio = aplicacion.Beneficiario.Domicilio;
                var ageb = _context.Ageb.Find(domicilio.AgebId);
                var localidad = _context.Localidad
                    .Where(l => l.Id.Equals(domicilio.LocalidadId))
                    .Include(l => l.Municipio).First();
                if (arrMunicipiosId.Length > 0 && !arrMunicipiosId.Contains(domicilio.MunicipioId))
                {
                    continue;
                }
                if (arrLocalidadesId.Length > 0 && !arrLocalidadesId.Contains(domicilio.LocalidadId))
                {
                    continue;
                }
                if (arrAgebsId.Length > 0 && !arrAgebsId.Contains(domicilio.AgebId))
                {
                    continue;
                }
                Domicilios.Add(new RutasDomicilioModel()
                {
                    DomicilioId = domicilio.Id,
                    Domicilio = domicilio.DomicilioN,
                    Ageb = ageb == null ? "" : ageb.Clave,
                    Localidad = localidad.Nombre,
                    Municipio = localidad.Municipio.Nombre,
                    Latitud = domicilio.LatitudAtm ?? domicilio.Latitud,
                    Longitud = domicilio.LongitudAtm ?? domicilio.Longitud,
                    FechaInicio = aplicacion.FechaInicio.ToString(),
                    FechaFin = aplicacion.FechaFin.ToString(),
                    Beneficiario = new BeneficiarioShortResponse()
                    {
                        Id = aplicacion.Beneficiario.Id,
                        // Nombre = aplicacion.Beneficiario.Nombre,
                        Nombre = aplicacion.Beneficiario.Nombre + ' ' + 
                                 aplicacion.Beneficiario.ApellidoPaterno + ' ' + 
                                 aplicacion.Beneficiario.ApellidoMaterno,
                        ApellidoPaterno = aplicacion.Beneficiario.ApellidoPaterno,
                        ApellidoMaterno = aplicacion.Beneficiario.ApellidoMaterno,
                        Folio = aplicacion.Beneficiario.Folio,
                        EstatusInformacion = aplicacion.Beneficiario.EstatusInformacion,
                        CreatedAt = aplicacion.Beneficiario.CreatedAt.ToString(),
                    },
                });
                totalGeneral++;
            }

            response.Status = "ok";
            response.Domicilios = Domicilios;
            response.Total = totalGeneral;
            
            return JsonSedeshu.SerializeObject(response);
        }
        
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "diagnosticos.ver")]
        public IActionResult VerProductividad()
        {
            try
            {
                var now = DateTime.UtcNow;
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var response = new VerRutasResponse
                {
                    Inicio = firstDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss"),
                    Fin = lastDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss"),
                    Dependencias = _context.Dependencia
                        .AsNoTracking()
                        .Where(d => d.DeletedAt == null)
                        .Select(d => new DependenciaApiModel
                        {
                            Id = d.Id,
                            Nombre = d.Nombre,
                            CreatedAt = d.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdatedAt = d.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                            DeletedAt = d.DeletedAt.ToString() // aquí ya filtramos null en el Where
                        })
                        .ToList(),
                    Municipios = _context.Municipio
                        .AsNoTracking()
                        .Where(m => m.DeletedAt == null)
                        .Select(m => new MunicipioApiModel
                        {
                            Id = m.Id,
                            Nombre = m.Nombre
                        })
                        .ToList()
                };

                return View("Productividad", response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }



        [HttpPost]
        [Authorize]
        public string GetProductividad([FromBody] GetProductividadRequest request)
        {
            var now = DateTime.Now;
            var response = new GetProductividadResponse {
                Trabajadores = new List<TrabajadorProductividadList>()
            };
            var inicio = new DateTime(now.Year, now.Month, 1).StartOf(DateTimeAnchor.Day);
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).EndOf(DateTimeAnchor.Day);
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day);
            }
            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day);
            }
            var municipiosHash = request.MunicipiosId != null ? new HashSet<string>(request.MunicipiosId.Split(",")) : new HashSet<string>();
            var localidadesHash = request.LocalidadesId != null ? new HashSet<string>(request.LocalidadesId.Split(",")) : new HashSet<string>();
            var agebsHash = request.AgebsId != null ? new HashSet<string>(request.AgebsId.Split(",")) : new HashSet<string>();            
            var trabajadores = _context.Trabajador.Where(t=>t.TrabajadorDependencias.Any(td=>td.DependenciaId==request.DependenciaId)).ToList();
            var visitas = _context.Aplicacion.Include(a => a.Beneficiario).ThenInclude(b => b.Domicilio).ThenInclude(d => d.Municipio)
                        .Include(a => a.Beneficiario).ThenInclude(b => b.Domicilio).ThenInclude(d => d.Localidad)
                        .Include(a => a.Beneficiario).ThenInclude(b => b.Domicilio).ThenInclude(d => d.Ageb)
                        .Where(a => a.DeletedAt == null && a.FechaInicio >= inicio && a.FechaInicio <= fin).ToList();
            var trabajadoresMap = new Dictionary<string, TrabajadorProductividadList>();
            var tiempos = new List<TimeSpan>();
            foreach (var trabajador in trabajadores) {
                trabajadoresMap.Add(trabajador.Id, new TrabajadorProductividadList {
                    Id = trabajador.Id,
                    Nombre = trabajador.Nombre,
                    Username = trabajador.Username,
                    Email = trabajador.Email,
                    Completas = 0,
                    IsOpened = false,
                    Aplicaciones = new List<AplicacionProductividadList>(),
                    Tiempos = new List<TimeSpan>()
                });
            }
            foreach (var aplicacion in visitas)
            {                
                var domicilio = aplicacion.Beneficiario.Domicilio;
                if (municipiosHash.Count > 0 && !municipiosHash.Contains(domicilio.MunicipioId))
                {
                    continue;
                }
                if (localidadesHash.Count > 0 && !localidadesHash.Contains(domicilio.MunicipioId))
                {
                    continue;
                }
                if (agebsHash.Count > 0 && !agebsHash.Contains(domicilio.AgebId))
                {
                    continue;
                }
                var trabajador = trabajadoresMap[aplicacion.TrabajadorId];
                var diasAtraso = (now - DateTime.Parse(aplicacion.FechaInicio.ToString())).TotalDays;
                trabajador.DiasRetraso += diasAtraso;
                trabajador.Total++;
                if (aplicacion.FechaFin != null)
                {
                    DateTime tiempoInicio = DateTime.Parse(aplicacion.FechaInicio.ToString());
                    DateTime tiempoFin = DateTime.Parse(aplicacion.FechaFin.ToString());
                    trabajador.Tiempos.Add(tiempoFin.Subtract(tiempoInicio));
                }
                if (aplicacion.Estatus != "Completo")
                {
                    trabajador.Aplicaciones.Add(new AplicacionProductividadList()
                    {
                        Id = aplicacion.Id,
                        Estatus = aplicacion.Estatus,
                        Resultado = aplicacion.Resultado,
                        BeneficiarioId = aplicacion.BeneficiarioId,
                        BeneficiarioNombre = aplicacion.Beneficiario.Nombre + ' ' +
                                             aplicacion.Beneficiario.ApellidoPaterno + ' ' +
                                             aplicacion.Beneficiario.ApellidoMaterno,
                        EncuestaVersionId = aplicacion.EncuestaVersionId,
                        TrabajadorId = aplicacion.TrabajadorId,
                        NumeroAplicacion = aplicacion.NumeroAplicacion,
                        Activa = aplicacion.Activa,
                        MunicipioNombre = aplicacion.Beneficiario.Domicilio.Municipio.Nombre,
                        LocalidadNombre = aplicacion.Beneficiario.Domicilio.Localidad == null ? aplicacion.Beneficiario.Domicilio.LocalidadId : aplicacion.Beneficiario.Domicilio.Localidad.Nombre,
                        AgebClave = aplicacion.Beneficiario.Domicilio.Ageb == null ? aplicacion.Beneficiario.Domicilio.AgebId : aplicacion.Beneficiario.Domicilio.Ageb.Clave,
                        FechaInicio = aplicacion.FechaInicio.ToString(),
                        FechaFin = aplicacion.FechaFin.ToString(),
                        DiasRetraso = diasAtraso,
                    });
                    trabajador.Incompletas++;
                }
                else {
                    trabajador.Completas++;
                }               
            }            
            foreach (var t in trabajadoresMap.Values) {
                if (t.Completas + t.Incompletas > 0) {
                    t.Promedio =  t.Tiempos.Count == 0 ? TimeSpan.Zero : new TimeSpan(Convert.ToInt64(t.Tiempos.Average(i => i.Ticks)));
                    response.Trabajadores.Add(t);
                }                
            }            

            response.Total = response.Trabajadores.Count();
            response.Status = "ok";
            return JsonSedeshu.SerializeObject(response);
        }
        
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "diagnosticos.ver")]
        public IActionResult VerAvancesPerfil()
        {
            var date = DateTime.Now;
            var response = new VerRutasResponse();
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1).StartOf(DateTimeAnchor.Day);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).EndOf(DateTimeAnchor.Day);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss");
            var dependencias = _context.Dependencia.Where(d => d.DeletedAt == null).ToList();
            var depList = new List<DependenciaApiModel>();
            if (dependencias.Any())
            {
                depList.AddRange(dependencias.Select(dependencia => new DependenciaApiModel()
                {
                    Id = dependencia.Id,
                    Nombre = dependencia.Nombre,
                    CreatedAt = dependencia.CreatedAt.ToString(),
                    UpdatedAt = dependencia.UpdatedAt.ToString(),
                    DeletedAt = dependencia.DeletedAt.ToString(),
                }));
            }

            var municipios = _context.Municipio.Where(m => m.DeletedAt == null).ToList();
            var munList = new List<MunicipioApiModel>();
            if (municipios.Any())
            {
                munList.AddRange(municipios.Select(m => new MunicipioApiModel()
                {
                    Id = m.Id, Nombre = m.Nombre
                }));
            }

            response.Dependencias = depList;
            response.Municipios = munList;
            
            return View("AvancePerfil", response);
        }
        
        [HttpPost]
        [Authorize]
        public string GetAvancesPerfil([FromBody] GetAvancesPerfilRequest request)
        {
            var response = new GetAvancesPerfilResponse();
            DateTime inicio;
            DateTime fin;
            DateTime.TryParseExact(request.Inicio.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out inicio);
            DateTime.TryParseExact(request.Fin.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out fin);
            inicio = inicio.StartOf(DateTimeAnchor.Day);
            fin = fin.EndOf(DateTimeAnchor.Day);
            var arrDependenciasId = request.DependenciasId != null ? request.DependenciasId.Split(new char[] { ',' }) : new string[]{};
            var arrTrabajadoresId = request.TrabajadoresId != null ? request.TrabajadoresId.Split(new char[] { ',' }) : new string[]{};
            var arrMunicipiosId = request.MunicipiosId != null ? request.MunicipiosId.Split(new char[] { ',' }) : new string[]{};
            var arrLocalidadesId = request.LocalidadesId != null ? request.LocalidadesId.Split(new char[] { ',' }) : new string[]{};
            var arrAgebsId = request.AgebsId != null ? request.AgebsId.Split(new char[] { ',' }) : new string[]{};
            
            var aplicacionesQuery = _context.Aplicacion
                .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) =>
                new {
                    Apl = a,
                    Ben = b,
                })
                .Join(_context.Domicilio, x => x.Ben.DomicilioId, d => d.Id, (x, d) =>
                new {
                    x.Apl,
                    x.Ben,
                    Dom = d,
                })
                .Join(_context.Trabajador, x => x.Apl.TrabajadorId, t => t.Id, (x, t) =>
                new {
                    x.Apl,
                    x.Ben,
                    x.Dom,
                    Tra = t,
                })
                .Join(_context.TrabajadorDependencia, x => x.Tra.Id, td => td.TrabajadorId, (x, td) =>
                new {
                    x.Apl,
                    x.Ben,
                    x.Dom,
                    x.Tra,
                    TD = td,
                })
                .Join(_context.Dependencia, x => x.TD.DependenciaId, d => d.Id, (x, d) =>
                new {
                    x.Apl,
                    x.Ben,
                    x.Dom,
                    x.Tra,
                    x.TD,
                    Dep = d,
                })
                .Where(a => a.Apl.FechaInicio >= inicio)
                .Where(a => a.Apl.FechaInicio <= fin)
                .Where(a => a.Apl.Activa)
                .Where(a => a.Dom.Activa)
                .Where(a => a.Apl.DeletedAt == null);
            
            if (arrDependenciasId.Length > 0) 
            {
                aplicacionesQuery = aplicacionesQuery.Where(x => arrDependenciasId.Contains(x.Dep.Id));
            }
            if (arrTrabajadoresId.Length > 0) 
            {
                aplicacionesQuery = aplicacionesQuery.Where(x => arrTrabajadoresId.Contains(x.Tra.Id));
            }
            if (arrMunicipiosId.Length > 0)
            {
                aplicacionesQuery = aplicacionesQuery.Where(x => arrMunicipiosId.Contains(x.Dom.MunicipioId));
            }
            if (arrLocalidadesId.Length > 0)
            {
                aplicacionesQuery = aplicacionesQuery.Where(x => arrLocalidadesId.Contains(x.Dom.LocalidadId));
            }
            if (arrAgebsId.Length > 0)
            {
                aplicacionesQuery = aplicacionesQuery.Where(x => arrAgebsId.Contains(x.Dom.AgebId));
            }

            var aplicaciones = aplicacionesQuery.ToList();
            var aplList = new List<EstadisticaModel>();
            var totalGeneral = 0;
            var totalCompletas = 0;
            var totalIncompletas = 0;
            
            var dependenciasList = new Dictionary<string, List<int>>();
            var promediosGroup = new Dictionary<string, List<TimeSpan>>();
            var promediosList = new Dictionary<string, Double>();
            var dependenciasDiasGroup = new Dictionary<string, Dictionary<string, int>>();
            var dependenciasDiasList = new Dictionary<string, double>();

            foreach (var aplicacion in aplicaciones)
            {
                aplList.Add(new EstadisticaModel()
                {
                    Aplicacion =  new AplicacionApiModel()
                    {
                        Id = aplicacion.Apl.Id,
                        Estatus = aplicacion.Apl.Estatus,
                        Resultado = aplicacion.Apl.Resultado,
                        BeneficiarioId = aplicacion.Apl.BeneficiarioId,
                        EncuestaVersionId = aplicacion.Apl.EncuestaVersionId,
                        TrabajadorId = aplicacion.Apl.TrabajadorId,
                        NumeroAplicacion = aplicacion.Apl.NumeroAplicacion,
                        Activa = aplicacion.Apl.Activa,
                        FechaInicio = aplicacion.Apl.FechaInicio.ToString(),
                        FechaFin = aplicacion.Apl.FechaFin.ToString(),
                        CreatedAt = aplicacion.Apl.CreatedAt.ToString(),
                        UpdatedAt = aplicacion.Apl.UpdatedAt.ToString(),
                        DeletedAt = aplicacion.Apl.DeletedAt.ToString(),
                    },
                    Beneficiario =  new BeneficiarioShortResponse()
                    {
                        Id = aplicacion.Ben.Id,
                        Nombre = aplicacion.Ben.Nombre,
                        ApellidoPaterno = aplicacion.Ben.ApellidoPaterno,
                        ApellidoMaterno = aplicacion.Ben.ApellidoMaterno,
                        Folio = aplicacion.Ben.Folio,
                        EstatusInformacion = aplicacion.Ben.EstatusInformacion,
                        CreatedAt = aplicacion.Ben.CreatedAt.ToString(),
                    },
                    Domicilio =  new DomicilioResponse()
                    {
                        Id = aplicacion.Dom.Id,
                        Telefono = aplicacion.Dom.Telefono,
                        TelefonoCasa = aplicacion.Dom.TelefonoCasa,
                        Email = aplicacion.Dom.Email,
                        Domicilio = aplicacion.Dom.DomicilioN,
                        EntreCalle1 = aplicacion.Dom.EntreCalle1,
                        EntreCalle2 = aplicacion.Dom.EntreCalle2,
                        Latitud = aplicacion.Dom.Latitud,
                        Longitud = aplicacion.Dom.Longitud,
                        CodigoPostal = aplicacion.Dom.CodigoPostal,
                        CadenaOCR = aplicacion.Dom.CadenaOCR,
                        Porcentaje = aplicacion.Dom.Porcentaje,
                        Activa = aplicacion.Dom.Activa,
                    },
                    Trabajador = new TrabajadorApiModel()
                    {
                        Id = aplicacion.Tra.Id,
                        Nombre = aplicacion.Tra.Nombre,
                        Username = aplicacion.Tra.Username,
                        Password = aplicacion.Tra.Password,
                        Email = aplicacion.Tra.Email,
                        Codigo = aplicacion.Tra.Codigo,
                        CreatedAt = aplicacion.Tra.CreatedAt.ToString(),
                        UpdatedAt = aplicacion.Tra.UpdatedAt.ToString(),
                        DeletedAt = aplicacion.Tra.DeletedAt.ToString(),
                    },
                    // TrabajadorDependencia = aplicacion.TD,
                    Dependencia = new DependenciaApiModel()
                    {
                        Id = aplicacion.Dep.Id,
                        Nombre = aplicacion.Dep.Nombre,
                        CreatedAt = aplicacion.Dep.CreatedAt.ToString(),
                        UpdatedAt = aplicacion.Dep.UpdatedAt.ToString(),
                        DeletedAt = aplicacion.Dep.DeletedAt.ToString(),
                    },
                });

                var isComplete = aplicacion.Apl.Estatus == "Completo";
                totalGeneral++;
                if (isComplete)
                {
                    totalCompletas++;
                }
                else
                {
                    totalIncompletas++;
                }
                
                var dependenciaNombre = aplicacion.Dep.Nombre;
                if (dependenciasList.TryGetValue(dependenciaNombre, out var depCountList))
                {
                    var tclTotal = depCountList[0];
                    var tclCompletas = depCountList[1];
                    var tclIncompletas = depCountList[2];
                    dependenciasList[dependenciaNombre] = new List<int>()
                    {
                        tclTotal + 1, 
                        isComplete ? tclCompletas + 1 : tclCompletas, 
                        isComplete ? tclIncompletas : tclIncompletas + 1
                    };
                    
                }
                else
                {
                    dependenciasList.Add(dependenciaNombre, new List<int>()
                    {
                        1, isComplete ? 1 : 0, isComplete ? 0 : 1
                    });
                }
                
                if (aplicacion.Apl.FechaFin != null)
                {
                    var tiempoInicio = DateTime.Parse(aplicacion.Apl.FechaInicio.ToString());
                    var tiempoFin = DateTime.Parse(aplicacion.Apl.FechaFin.ToString());
                    var span = tiempoFin.Subtract(tiempoInicio);
                    var fechaFin = tiempoFin.Date;
                    var fechaFinString = fechaFin.Date.ToString("dd/MM/yyyy");

                    // Cálculo de promedios de tiempos de aplicación
                    if (promediosGroup.TryGetValue(dependenciaNombre, out var promedioList))
                    {
                        promediosGroup[dependenciaNombre].Add(span);
                    }
                    else
                    {
                        promediosGroup.Add(dependenciaNombre, new List<TimeSpan>() {span});
                    }

                    // Cálculo de promedios de aplicaciones terminadas por día
                    if (dependenciasDiasGroup.TryGetValue(dependenciaNombre, out var dDiasList))
                    {
                        if (dependenciasDiasGroup[dependenciaNombre].TryGetValue(fechaFinString, out var dDiasFecha))
                        {
                            dependenciasDiasGroup[dependenciaNombre][fechaFinString] = dDiasFecha + 1;
                        }
                        else
                        {
                            dependenciasDiasGroup[dependenciaNombre].Add(fechaFinString, 1);
                        }
                    }
                    else
                    {
                        dependenciasDiasGroup.Add(dependenciaNombre, new Dictionary<string, int>()
                        {
                            {fechaFinString, 1}
                        });
                    }
                }
            }
            
            foreach (var (key, value) in promediosGroup)
            {
                var thisPromedio = value.Any() 
                    ? new TimeSpan(Convert.ToInt64(value.Average(t => t.Ticks))) 
                    : TimeSpan.Zero;
                promediosList.Add(key, thisPromedio.TotalMinutes);
            }
            foreach (var (dependenciaDdg, diasDdg) in dependenciasDiasGroup)
            {
                var diasPromedioList = new List<int>();
                foreach (var (key, value) in diasDdg)
                {
                    diasPromedioList.Add(value);
                }
                
                var thisPromedio = diasPromedioList.Any() ? 
                    diasPromedioList.Average() : 0;
                dependenciasDiasList.Add(dependenciaDdg, thisPromedio);
            }

            response.Status = "ok";
            response.TotalGeneral = totalGeneral;
            response.TotalCompletas = totalCompletas;
            response.TotalIncompletas = totalIncompletas;
            response.CompletadosList = dependenciasList;
            response.PromediosList = promediosList;
            response.DiasList = dependenciasDiasList;
            
            return JsonSedeshu.SerializeObject(response);
        }
        
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "diagnosticos.ver")]
        public IActionResult VerEstadisticas()
        {
            var response = new VerEstadisticasResponse();
            var dependencias = _context.Dependencia.Where(d => d.DeletedAt == null).ToList();
            var depList = new List<DependenciaApiModel>();
            if (dependencias.Count() > 0)
            {
                foreach (var dependencia in dependencias)
                {
                    depList.Add(new DependenciaApiModel()
                    {
                        Id = dependencia.Id,
                        Nombre = dependencia.Nombre,
                        CreatedAt = dependencia.CreatedAt.ToString(),
                        UpdatedAt = dependencia.UpdatedAt.ToString(),
                        DeletedAt = dependencia.DeletedAt.ToString(),
                    });
                }
            }

            var municipios = _context.Municipio.Where(m => m.DeletedAt == null).ToList();
            var munList = new List<MunicipioApiModel>();
            if (municipios.Any())
            {
                munList.AddRange(municipios.Select(m => new MunicipioApiModel()
                {
                    Id = m.Id, Nombre = m.Nombre
                }));
            }

            response.Dependencias = depList;
            response.Municipios = munList;
            
            return View("Estadisticas", response);
        }

        // Clases auxiliares para optimización
        public class EstadisticaMinimal
        {
            public string AplicacionId { get; set; }
            public string Estatus { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public int? Tiempo { get; set; }
            public DateTime UpdatedAt { get; set; }
            public int NumeroIntegrantes { get; set; }
            public string TrabajadorId { get; set; }
            public string TrabajadorNombre { get; set; }
            public string BeneficiarioId { get; set; }
            public string BeneficiarioNombre { get; set; }
            public string BeneficiarioApellidoPaterno { get; set; }
            public string BeneficiarioApellidoMaterno { get; set; }
            public string BeneficiarioFolio { get; set; }
            public string DomicilioId { get; set; }
            public string DependenciaId { get; set; }
            public string DependenciaNombre { get; set; }
        }

        public class TrabajadorEstadisticas
        {
            public string Nombre { get; set; }
            public int Total { get; set; }
            public int Completas { get; set; }
        }
        

        [HttpPost]
        [Authorize]
        public string GetEstadisticas([FromBody] GetEstadisticaRequest request)
        {
            var response = new GetEstadisticasResponse();

            // Parseo de fechas optimizado
            if (!DateTime.TryParseExact(request.Inicio.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime inicio) ||
                !DateTime.TryParseExact(request.Fin.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime fin))
            {
                response.Status = "error";
                //response.Message = "Formato de fecha inválido";
                return JsonSedeshu.SerializeObject(response);
            }

            inicio = inicio.StartOf(DateTimeAnchor.Day);
            fin = fin.EndOf(DateTimeAnchor.Day);

            // Arrays de filtros optimizados
            var municipiosIds = request.MunicipiosId?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            var localidadesIds = request.LocalidadesId?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            var agebsIds = request.AgebsId?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            // Query optimizada con selección específica de campos usando nombres correctos
            var queryBase = from a in _context.Aplicacion
                            join b in _context.Beneficiario on a.BeneficiarioId equals b.Id
                            join d in _context.Domicilio on b.DomicilioId equals d.Id
                            join t in _context.Trabajador on a.TrabajadorId equals t.Id
                            join td in _context.TrabajadorDependencia on t.Id equals td.TrabajadorId
                            join dep in _context.Dependencia on td.DependenciaId equals dep.Id
                            where a.FechaInicio >= inicio
                                  && a.FechaInicio <= fin
                                  && a.Activa
                                  && d.Activa
                                  && !a.Prueba
                                  && t.Username != "pruebas"
                                  && a.DeletedAt == null
                            select new
                            {
                                Aplicacion = a,
                                Beneficiario = b,
                                Domicilio = d,
                                Trabajador = t,
                                Dependencia = dep
                            };

            // Aplicar filtros adicionales
            if (!string.IsNullOrEmpty(request.DependenciaId))
            {
                queryBase = queryBase.Where(x => x.Dependencia.Id == request.DependenciaId);
            }

            if (municipiosIds.Length > 0)
            {
                queryBase = queryBase.Where(x => municipiosIds.Contains(x.Domicilio.MunicipioId));
            }

            if (localidadesIds.Length > 0)
            {
                queryBase = queryBase.Where(x => localidadesIds.Contains(x.Domicilio.LocalidadId));
            }

            if (agebsIds.Length > 0)
            {
                queryBase = queryBase.Where(x => agebsIds.Contains(x.Domicilio.AgebId));
            }

            // Seleccionar solo los campos necesarios para estadísticas
            var estadisticasData = queryBase
                .Select(x => new EstadisticaMinimal
                {
                    AplicacionId = x.Aplicacion.Id,
                    Estatus = x.Aplicacion.Estatus,
                    // Reemplaza la línea problemática en el método GetEstadisticas:
                    FechaInicio = x.Aplicacion.FechaInicio.Value,
                    FechaFin = x.Aplicacion.FechaFin,
                    Tiempo = x.Aplicacion.Tiempo,
                    UpdatedAt = x.Aplicacion.UpdatedAt,
                    NumeroIntegrantes = x.Aplicacion.NumeroIntegrantes,
                    TrabajadorId = x.Trabajador.Id,
                    TrabajadorNombre = x.Trabajador.Nombre,
                    BeneficiarioId = x.Beneficiario.Id,
                    BeneficiarioNombre = x.Beneficiario.Nombre,
                    BeneficiarioApellidoPaterno = x.Beneficiario.ApellidoPaterno,
                    BeneficiarioApellidoMaterno = x.Beneficiario.ApellidoMaterno,
                    BeneficiarioFolio = x.Beneficiario.Folio,
                    DomicilioId = x.Domicilio.Id,
                    DependenciaId = x.Dependencia.Id,
                    DependenciaNombre = x.Dependencia.Nombre
                })
                .AsNoTracking() // Mejora rendimiento para solo lectura
                .ToList();

            // Procesamiento en memoria (más rápido para grandes volúmenes)
            var aplicacionesStats = new List<EstadisticaModel>();
            var estadisticasTrabajadores = new Dictionary<string, TrabajadorEstadisticas>();
            var tiemposEncuestas = new List<TimeSpan>();
            var diasConteo = new Dictionary<DateTime, int>();
            var totalGeneral = 0;
            var totalCompletas = 0;
            var totalIncompletas = 0;
            var totalEncuestados = 0;
            var totalFamilias = 0;

            foreach (var data in estadisticasData)
            {
                totalGeneral++;

                var esCompleta = data.Estatus == "Completo";
                if (esCompleta)
                {
                    totalCompletas++;
                }
                else
                {
                    totalIncompletas++;
                }

                // Estadísticas de tiempo
                if (data.FechaFin.HasValue)
                {
                    TimeSpan duracion;
                    if (data.Tiempo.HasValue && data.Tiempo.Value > 0)
                    {
                        duracion = TimeSpan.FromSeconds(data.Tiempo.Value);
                    }
                    else if (!data.FechaFin.HasValue)
                    {
                        duracion = data.UpdatedAt.Subtract(data.FechaInicio);
                    }
                    else
                    {
                        duracion = data.UpdatedAt.Subtract(data.FechaFin.Value);
                    }
                    tiemposEncuestas.Add(duracion);

                    // Conteo por día
                    var fechaDia = data.FechaFin.Value.Date;
                    diasConteo[fechaDia] = diasConteo.GetValueOrDefault(fechaDia) + 1;
                }

                // Estadísticas por trabajador
                if (!estadisticasTrabajadores.TryGetValue(data.TrabajadorId, out var statsTrabajador))
                {
                    statsTrabajador = new TrabajadorEstadisticas
                    {
                        Nombre = data.TrabajadorNombre,
                        Total = 0,
                        Completas = 0
                    };
                    estadisticasTrabajadores[data.TrabajadorId] = statsTrabajador;
                }

                statsTrabajador.Total++;
                if (esCompleta) statsTrabajador.Completas++;

                // Conteo de familias y encuestados
                totalFamilias++;
                totalEncuestados += data.NumeroIntegrantes == 0 ? 1 : data.NumeroIntegrantes;

                // Solo construir modelos completos si es necesario
                aplicacionesStats.Add(new EstadisticaModel
                {
                    Aplicacion = new AplicacionApiModel
                    {
                        Id = data.AplicacionId,
                        Estatus = data.Estatus,
                        FechaInicio = data.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss"),
                        FechaFin = data.FechaFin?.ToString("yyyy-MM-dd HH:mm:ss"),
                        NumeroIntegrantes = data.NumeroIntegrantes
                    },
                    Beneficiario = new BeneficiarioShortResponse
                    {
                        Id = data.BeneficiarioId,
                        Nombre = data.BeneficiarioNombre,
                        ApellidoPaterno = data.BeneficiarioApellidoPaterno,
                        ApellidoMaterno = data.BeneficiarioApellidoMaterno,
                        Folio = data.BeneficiarioFolio
                    },
                    Trabajador = new TrabajadorApiModel
                    {
                        Id = data.TrabajadorId,
                        Nombre = data.TrabajadorNombre
                    },
                    Dependencia = new DependenciaApiModel
                    {
                        Id = data.DependenciaId,
                        Nombre = data.DependenciaNombre
                    }
                });
            }

            // Top 10 trabajadores por encuestas completas
            var topTrabajadores = estadisticasTrabajadores.Values
                .OrderByDescending(t => t.Completas)
                .ThenByDescending(t => t.Total)
                .Take(10)
                .ToDictionary(
                    t => t.Nombre,
                    t => new List<int> { t.Total, t.Completas, t.Total - t.Completas }
                );

            // Cálculo de promedios
            var promedioTiempo = tiemposEncuestas.Count > 0 ?
                new TimeSpan(Convert.ToInt64(tiemposEncuestas.Average(t => t.Ticks))) : TimeSpan.Zero;

            var promedioDias = diasConteo.Any() ?
                diasConteo.Values.Average() : 0;

            // Construcción de respuesta
            response.Status = "ok";
            response.Aplicaciones = aplicacionesStats;
            response.TotalGeneral = totalGeneral;
            response.TotalCompletas = totalCompletas;
            response.TotalIncompletas = totalIncompletas;
            response.EncuestadosCount = totalEncuestados;
            response.FamiliasCount = totalFamilias;
            response.EncuestasPromedio = promedioTiempo;
            response.DiasPromedio = promedioDias;
            response.EncuestadoresList = topTrabajadores;
            response.CarenciasList = new Dictionary<string, int>();

            return JsonSedeshu.SerializeObject(response);
        }

        [HttpPost]
        [Authorize]
        public IActionResult exportarVisitas(BeneficiariosRequest request)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year,now.Month,1).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year,now.Month,DateTime.DaysInMonth(now.Year,now.Month)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var builder = new StringBuilder();
            var resultados = _context.Respuesta.Where(r => r.PreguntaId.Equals("138")).OrderBy(r=>int.Parse(r.Id)).ToDictionary(r=>r.Nombre);
            var row = new List<string>();
            row.Add("IdEncuestador");
            row.Add("Encuestador");
            row.Add("Id");
            row.Add("Latitud");
            row.Add("Longitud");
            row.AddRange(resultados.Values.Select(res => res.Nombre));
            row.Add("FechaInicio");
            row.Add("FechaSincronizacion");
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");;
            }

            builder.AppendLine(string.Join("|", row));
            var util = new BeneficiarioCode(_context);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var query = util.BuildVisitasQuery(request, inicio, fin);
                command.CommandText = query.Key;
                var iParametro = 0;
                foreach (var parametro in query.Value)
                {
                    command.Parameters.Add(new SqlParameter("@" + (iParametro++), parametro));
                }

                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            row = new List<string>{reader.GetString(0),
                                reader.GetString(1),reader.GetString(2),
                                reader.GetString(3),reader.GetString(4).ToString()};
                            for (var i = 0; i < resultados.Values.Count; i++)
                            {
                                row.Add("");
                            }

                            row.Add(reader.GetDateTime(5).ToString("yyyy-MM-dd")); 
                            row.Add(reader.GetDateTime(6).ToString("yyyy-MM-dd"));
                            row[resultados[reader.GetString(7)].Numero + 4] = reader.GetString(7);
                            builder.AppendLine(string.Join("|", row));
                        }
                    }
                }
                _context.Database.CloseConnection();
            }

            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = AccionBitacora.EXPORTACION,
                Mensaje = "Se exportó base.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            _context.SaveChanges();
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "visitas"
                +inicio.Replace("-","").Substring(0,9)+" al "
                +fin.Replace("-","").Substring(0,9)+".csv");
        }
        
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "diagnosticos.ver")]
        public IActionResult BaseMadre()
        {
            var response = new DiagnosticoResponse();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd");
            response.Hoy = date.ToString("yyyy-MM-dd");
            response.Encuestadores = new List<Model>();
            response.Municipios = new List<Model>();
            foreach (var trabajador in _context.Trabajador.Where(t=>t.DeletedAt==null).OrderBy(t=>t.Nombre))
            {
                response.Encuestadores.Add(new Model
                {
                    Id = trabajador.Id,
                    Nombre = trabajador.Nombre
                });
            }
            foreach (var municipio in _context.Municipio.Where(m=>m.DeletedAt==null).OrderBy(m=>m.Nombre))
            {
                response.Municipios.Add(new Model
                {
                    Id = municipio.Id,
                    Nombre = municipio.Nombre
                });
            }
            return View("BaseMadre", response);
        }


        /*public IActionResult ExportarBaseMadre(Diagnostico request)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(request.Inicio))
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(request.Fin))
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).ToString("yyyy-MM-dd HH:mm:ss");

            if (!DateTime.TryParse(inicio, out DateTime fechaInicio) || !DateTime.TryParse(fin, out DateTime fechaFin))
                return BadRequest("Fechas inválidas. Usa el formato YYYY-MM-DD.");

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                var stream = new MemoryStream();

                using (var excelPackage = new ExcelPackage())
                {
                    // Configuración para mejorar rendimiento
                    excelPackage.Workbook.CalcMode = ExcelCalcMode.Manual;

                    // Hoja principal
                    var ws = excelPackage.Workbook.Worksheets.Add("Datos Base");

                    using (var conn = new SqlConnection(connectionString))
                    using (var cmd = new SqlCommand("sp_Reporte_Sabana_M", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            ws.Cells["A1"].LoadFromDataReader(reader, true);
                        }
                    }

                    // Estilo encabezados
                    using (var range = ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SteelBlue);
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    ws.View.FreezePanes(2, 1);
                    ws.Cells[1, 1, 1, ws.Dimension.End.Column].AutoFitColumns();
                    ws.Cells[1, 1, 1, ws.Dimension.End.Column].AutoFilter = true;

                    // Formato fecha/hora para columnas AI a AL si existen
                    for (int col = 35; col <= 39 && col <= ws.Dimension.End.Column; col++)
                        ws.Column(col).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    // Guardar directo al stream
                    excelPackage.SaveAs(stream);
                }

                stream.Position = 0;

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Reporte_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
        */


        public IActionResult ExportarBaseMadre(Diagnostico request)
        {
            var Estatus = "";
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(request.Inicio))
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(request.Fin))
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");

            if (!DateTime.TryParse(inicio, out DateTime fechaInicio) || !DateTime.TryParse(fin, out DateTime fechaFin))
                return BadRequest("Fechas inválidas. Usa el formato YYYY-MM-DD.");

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Datos principales
                var data = ObtenerDatosSabana(connectionString, fechaInicio, fechaFin, Estatus);

                // Datos del SP
                var estadisticas = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_Diccionario_Encuesta", conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    //cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    adapter.Fill(estadisticas);
                }

                using (var excelStream = new MemoryStream())
                using (var excelPackage = new ExcelPackage())
                {
                    // ✅ función auxiliar con nombre distinto (sheet en lugar de ws)
                    void FormatearHoja(ExcelWorksheet sheet, DataTable tabla, System.Drawing.Color headerColor)
                    {
                        using (var range = sheet.Cells[1, 1, 1, tabla.Columns.Count])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(headerColor);
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        sheet.View.FreezePanes(2, 1);
                        sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                        sheet.Cells[1, 1, 1, tabla.Columns.Count].AutoFilter = true;

                        // Formato dinámico por tipo de dato
                        for (int col = 1; col <= tabla.Columns.Count; col++)
                        {
                            var tipo = tabla.Columns[col - 1].DataType;

                            if (tipo == typeof(DateTime))
                                sheet.Column(col).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                            else if (tipo == typeof(int) || tipo == typeof(long))
                                sheet.Column(col).Style.Numberformat.Format = "0";
                            else if (tipo == typeof(decimal) || tipo == typeof(double) || tipo == typeof(float))
                                sheet.Column(col).Style.Numberformat.Format = "#,##0.00";
                        }
                    }

                    // Hoja principal
                    var hojaDatos = excelPackage.Workbook.Worksheets.Add("Datos Base");
                    hojaDatos.Cells["A1"].LoadFromDataTable(data, true);
                    FormatearHoja(hojaDatos, data, System.Drawing.Color.SteelBlue);

                    // Hoja de estadísticas
                    var hojaEst = excelPackage.Workbook.Worksheets.Add("Diccionario");
                    hojaEst.Cells["A1"].LoadFromDataTable(estadisticas, true);
                    FormatearHoja(hojaEst, estadisticas, System.Drawing.Color.DarkGreen);

                    excelPackage.SaveAs(excelStream);
                    excelStream.Position = 0;

                    return File(
                        excelStream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Reporte_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.xlsx"
                    );
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }




        /*public IActionResult ExportarBaseMadre(Diagnostico request)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(request.Inicio))
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(request.Fin))
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");

            if (!DateTime.TryParse(inicio, out DateTime fechaInicio) || !DateTime.TryParse(fin, out DateTime fechaFin))
                return BadRequest("Fechas inválidas. Usa el formato YYYY-MM-DD.");

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Datos principales
                var data = ObtenerDatosSabana(connectionString, fechaInicio, fechaFin);

                // Datos del procedimiento almacenado
                var estadisticas = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_Diccionario_Encuesta", conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    //cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    adapter.Fill(estadisticas);
                }

                using (var excelStream = new MemoryStream())
                using (var excelPackage = new ExcelPackage())
                {
                    // Hoja principal
                    var ws = excelPackage.Workbook.Worksheets.Add("Datos Base");
                    ws.Cells["A1"].LoadFromDataTable(data, true);

                    using (var range = ws.Cells[1, 1, 1, data.Columns.Count])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SteelBlue);
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    ws.View.FreezePanes(2, 1);
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.Cells[1, 1, 1, data.Columns.Count].AutoFilter = true;

                    // Formato fecha/hora para columnas AI a AL si existen
                    for (int col = 35; col <= 39 && col <= ws.Dimension.End.Column; col++)
                        ws.Column(col).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    // Hoja de estadísticas
                    var wsEst = excelPackage.Workbook.Worksheets.Add("Diccionario");
                    wsEst.Cells["A1"].LoadFromDataTable(estadisticas, true);

                    using (var range = wsEst.Cells[1, 1, 1, estadisticas.Columns.Count])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkGreen);
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    wsEst.View.FreezePanes(2, 1);
                    wsEst.Cells[wsEst.Dimension.Address].AutoFitColumns();
                    wsEst.Cells[1, 1, 1, estadisticas.Columns.Count].AutoFilter = true;

                    // Formato fecha/hora automático para columnas DateTime
                    for (int col = 1; col <= estadisticas.Columns.Count; col++)
                    {
                        if (estadisticas.Columns[col - 1].DataType == typeof(DateTime))
                            wsEst.Column(col).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    }

                    excelPackage.SaveAs(excelStream);
                    excelStream.Position = 0;

                    return File(
                        excelStream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Reporte_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.xlsx"
                    );
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }*/

        [Authorize]
        [HttpPost]
        public string IngresarBaseMadre([FromBody] IngresarSabanaRequest request) {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.Any()? x.Value.Errors.First().ErrorMessage : null })
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }

            try
            {
                var home = new HomeController(_context, null, null, null, null, null);
                var numAplicaciones = home.IngresarAplicacionEnSabana(request.Inicio, request.Fin);
                return "Diagnósticos ingresados a base madre : "+numAplicaciones;
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                return e.Message;
            }
        }
    }
}