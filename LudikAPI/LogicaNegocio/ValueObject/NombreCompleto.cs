using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.ValueObjects
{
    public record NombreCompleto
    {
        public const int LongitudMinima = 3;
        public const int LongitudMaxima = 20;
        private static readonly Regex FormatoValido = new("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", RegexOptions.Compiled);

        public string Nombre { get; init; }
        public string Apellido { get; init; }

        /// <summary>
        /// Encapsula los posibles errores de validación para NombreCompleto.
        /// </summary>
        private static class ErroresDeValidacion
        {
            public static readonly Error NombreNuloOVacio = new("NombreCompleto.Nombre.NuloOVacio", "El nombre no puede ser nulo o vacío.");
            public static readonly Error NombreLongitudInvalida = new("NombreCompleto.Nombre.Longitud", $"El nombre debe tener entre {LongitudMinima} y {LongitudMaxima} caracteres.");
            public static readonly Error NombreFormatoInvalido = new("NombreCompleto.Nombre.Formato", "El nombre solo puede contener letras y espacios.");

            public static readonly Error ApellidoNuloOVacio = new("NombreCompleto.Apellido.NuloOVacio", "El apellido no puede ser nulo o vacío.");
            public static readonly Error ApellidoLongitudInvalida = new("NombreCompleto.Apellido.Longitud", $"El apellido debe tener entre {LongitudMinima} y {LongitudMaxima} caracteres.");
            public static readonly Error ApellidoFormatoInvalido = new("NombreCompleto.Apellido.Formato", "El apellido solo puede contener letras y espacios.");
        }

        private NombreCompleto(string nombre, string apellido)
        {
            Nombre = nombre;
            Apellido = apellido;
        }


        public static Resultado<NombreCompleto> Crear(string nombre, string apellido)
        {
            var errores = new List<Error>();

            ValidarCampo(errores, nombre, esNombre: true);
            ValidarCampo(errores, apellido, esNombre: false);

            if (errores.Any())
                return Resultado<NombreCompleto>.Falla(errores);
            
            return Resultado<NombreCompleto>.Exitoso(new NombreCompleto(nombre, apellido));
        }

        /// <summary>
        /// Valida un campo (nombre o apellido) y agrega los errores correspondientes a la lista.
        /// </summary>
        private static void ValidarCampo(List<Error> errores, string valor, bool esNombre)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                errores.Add(esNombre ? ErroresDeValidacion.NombreNuloOVacio : ErroresDeValidacion.ApellidoNuloOVacio);
                return;
            }

            if (valor.Length < LongitudMinima || valor.Length > LongitudMaxima)
                errores.Add(esNombre ? ErroresDeValidacion.NombreLongitudInvalida : ErroresDeValidacion.ApellidoLongitudInvalida);
            

            if (!FormatoValido.IsMatch(valor))
                errores.Add(esNombre ? ErroresDeValidacion.NombreFormatoInvalido : ErroresDeValidacion.ApellidoFormatoInvalido);
            
        }
    }


    

}

