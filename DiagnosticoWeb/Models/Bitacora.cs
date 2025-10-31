using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("Bitacora")]
    public class Bitacora
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string Accion { get; set; }
        public string Mensaje { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
    }

    public class BitacoraList
    {
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Mensaje { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UsuariosBitacora
    {
        public List<ApplicationUser> Usuarios { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<string> Acciones { get; set; }
    }

    public class BitacoraRequest
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string UsuarioId { get; set; }
        public string Accion { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class BitacoraResponse
    {
        public List<BitacoraList> Bitacora { get; set; }
        public int Total { get; set; }
    }
}