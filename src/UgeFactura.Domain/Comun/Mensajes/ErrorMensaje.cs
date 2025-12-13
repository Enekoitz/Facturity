using System;
using System.Collections.Generic;
using System.Text;

namespace UgeFactura.Domain.Comun.Mensajes
{
    public enum CodigoError
    {
        AliasVacio,
        AliasDemasiadoLargo,
        NombreVacio,
        PrimerApellidoVacio,
        NombreDemasiadoLargo,
        PrimerApellidoDemasiadoLargo,
        SegundoApellidoDemasiadoLargo,
        RazonSocialVacio,
        RazonSocialDemasiadoLargo
    }

    public static class ErrorMensaje
    {
        private static readonly Dictionary<CodigoError, string> _es = new()
        {
            { CodigoError.NombreVacio, "El nombre no puede estar vacío." },
            { CodigoError.PrimerApellidoVacio, "El primer apellido no puede estar vacío." },
            { CodigoError.NombreDemasiadoLargo, "El nombre debe tener una longitud máxima de {0} caracteres." },
            { CodigoError.PrimerApellidoDemasiadoLargo, "El primer apellido debe tener una longitud máxima de {0} caracteres." },
            { CodigoError.SegundoApellidoDemasiadoLargo, "El segundo apellido debe tener una longitud máxima de {0} caracteres." },
            { CodigoError.RazonSocialVacio, "La razón social no puede estar vacía." },
            { CodigoError.RazonSocialDemasiadoLargo, "La razón social debe tener una longitud máxima de {0} caracteres." },
            { CodigoError.AliasVacio, "El alias no puede estar vacía." },
            { CodigoError.AliasDemasiadoLargo, "El alias debe tener una longitud máxima de {0} caracteres." }
        };

        private static readonly Dictionary<CodigoError, string> _en = new()
        {
            { CodigoError.NombreVacio, "Name cannot be empty." },
            { CodigoError.PrimerApellidoVacio, "First surname cannot be empty." },
            { CodigoError.NombreDemasiadoLargo, "Name must have a maximum length of {0} characters." },
            { CodigoError.PrimerApellidoDemasiadoLargo, "First surname must have a maximum length of {0} characters." },
            { CodigoError.SegundoApellidoDemasiadoLargo, "Second surname must have a maximum length of {0} characters." },
            { CodigoError.RazonSocialVacio, "Company name cannot be empty." },
            { CodigoError.RazonSocialDemasiadoLargo, "Company name must have a maximum length of {0} characters." },
            { CodigoError.AliasVacio, "La razón social no puede estar vacía." },
            { CodigoError.AliasDemasiadoLargo, "La razón social debe tener una longitud máxima de {0} caracteres." }
        };

        public static string Get(CodigoError codigo, string idioma = "es")
        {
            return idioma switch
            {
                "es" => _es[codigo],
                "en" => _en[codigo],
                _ => _es[codigo]
            };
        }
    }
}
