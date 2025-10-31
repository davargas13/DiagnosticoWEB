using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de Trabajadores a objetos de persistencia, tabla que guarda los usuarios que pueden usar la aplicacion movil
    /// </summary>
    [Table("Trabajadores")]
    public class Trabajador
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Codigo { get; set; }
        public byte Tipo { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual List<TrabajadorDependencia> TrabajadorDependencias { get; set; }
    }

    public class TrabajadorApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Codigo { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class TrabajadorCreateEditModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre debe tener como máximo 100 caracteres.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [MinLength(6, ErrorMessage = "El nombre de usuario debe tener al menos 6 caracteres.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "El nombre de usuario no debe contener espacios, acentos o caracteres especiales")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario debe tener como máximo 50 caracteres.")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [MaxLength(100, ErrorMessage = "El correo electrónico debe tener como máximo 100 caracteres.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "El código es obligatorio.")]
        [MaxLength(4, ErrorMessage = "El código debe tener como máximo 4 caracteres.")]
        public string Codigo { get; set; }

        public bool IsDisabled { get; set; }
        
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(50, ErrorMessage = "La contraseña debe tener como máximo 50 caracteres.")]
        public string Password { get; set; }
        public string IsDependencia { get; set; }
        [RequiredIf("IsDependencia",true)]
        public string Dependencia { get; set; }
        [Required(ErrorMessage = "El rol es requerido.")]
        public string Rol { get; set; }
        public string Perfil { get; set; }
       
        public List<Dependencia> Dependencias { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<Perfil> Perfiles { get; set; }
        public List<string> Regiones { get; set; }
        
    }

    public class TrabajadorList
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Username { get; set; }
        public string Tipo { get; set; }
        public string Dependencias { get; set; }
        public string Email { get; set; }   
    }

    public class TrabajadoresCampoRequest
    {
        public string Nombre { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DependenciaId { get; set; }
        public string Tipo { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class TrabajadoresCampoResponse
    {
        public List<TrabajadorList> Usuarios { get; set; }
        public int Total { get; set; }
    }

    public class TrabajadorCampoCreateEditModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre debe tener como máximo 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [MinLength(6, ErrorMessage = "El nombre de usuario debe tener al menos 6 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario debe tener como máximo 50 caracteres.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [MaxLength(100, ErrorMessage = "El correo electrónico debe tener como máximo 100 caracteres.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "El código es obligatorio.")]
        [MaxLength(4, ErrorMessage = "El código debe tener como máximo 4 caracteres.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(50, ErrorMessage = "La contraseña debe tener como máximo 50 caracteres.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "La dependencia es obligatoria.")]
        public string DependenciaId { get; set; }
        public byte Tipo { get; set; }
        public List<string> Regiones { get; set; }

    }

    public class TrabajadorRequest
    {
        public string Nombre { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DependenciaId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }        
    }
    
    public class TrabajadorResponse
    {
        public List<ApplicationUser> Usuarios { get; set; }
        public int Total { get; set; }
    }

    public class TrabajadorModelList
    {
        public string Id { get; set; }
        public string NombreDependencias { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class LoginMovilModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginMovilResponse
    {
        public Trabajador Trabajador { get; set; }
        public string Result { get; set; }
    } 
    
    public class TokenResponse
    {
        public string Token { get; set; }
        public string Result { get; set; }
    }

    public class ActivarUsuario
    {
        public string Id { get; set; }
    }
}