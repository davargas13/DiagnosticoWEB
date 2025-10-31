using System;
using System.Collections.Generic;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Code
{
    public class Familia
    {
        public List<IntegranteFamilia> Integrantes { get; set; }
        public bool Analfabetismo {get; set;}
        public bool Inasistencia {get; set;}
        public bool PrimariaCompleta {get; set;}
        public bool SecundariaCompleta {get; set;}
        public bool Educativa {get; set;}
        public bool ServiciosSalud {get; set;}
        public bool SeguridadSocial {get; set;}
        public bool Vivienda {get; set;}
        public bool Piso {get; set;}
        public bool Techo {get; set;}
        public bool Muro {get; set;}
        public bool Hacinamiento {get; set;}
        public bool Servicios {get; set;}
        public bool Agua {get; set;}
        public bool Drenaje {get; set;}
        public bool Electricidad {get; set;}
        public bool Combustible {get; set;}
        public bool Alimentaria {get; set;}
        public double IngresoFamiliar {get; set;}
        public bool EconomicamenteActiva {get; set;}
        public int LineaBienestar {get; set;}
        public int NumCarencias {get; set;}
        public string NivelPobreza {get; set;}
        public string GradoAlimnetaria {get; set;}
        public bool TieneActa {get; set;}
        public short SeguridadPublica {get; set;}
        public short SeguridadParque {get; set;}
        public short ConfianzaLiderez {get; set;}
        public short ConfianzaInstituciones {get; set;}
        public short Movilidad {get; set;}
        public short RedesSociales {get; set;}
        public short Tejido {get; set;}
        public short Satisfaccion {get; set;}
        public bool PropiedadVivienda {get; set;}
        public bool Discapacidad {get; set;}
    }
}