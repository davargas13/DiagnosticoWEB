using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("Perfil")]
    public class Perfil
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public int Sistema { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class PerfilRequest
    {
        public string Perfil { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class PerfilCreateEdit
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El nombre del perfil es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre del perfil debe tener máximo 100 caracteres")]
        public string Nombre { get; set; }
        [EnsureOneElement(ErrorMessage = "El perfil debe tener al menos un permiso")]
        public List<string> PermisosIds { get; set; }
        public List<Permiso> Permisos { get; set; }
        public int Sistema { get; set; }
    }

    public class PerfilResponse
    {
        public List<PerfilList> Perfiles { get; set; }
        public int Total { get; set; }
    }

    public class PerfilList
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public int Sistema { get; set; }
        public int Permisos { get; set; }
    }
} 