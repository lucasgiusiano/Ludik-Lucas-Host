using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Resultados
{
    /// <summary>
    /// Representa un error específico ocurrido durante una operación.
    /// Es inmutable por diseño al ser un record.
    /// </summary>
    public record Error
    {
        /// <summary>
        /// Código único que identifica el tipo de error.
        /// </summary>
        public string Codigo { get; }

        public string Mensaje { get; }

        /// <summary>
        /// Constructor para crear una instancia de Error.
        /// </summary>
        /// <param name="codigo">El código del error.</param>
        /// <param name="mensaje">El mensaje descriptivo del error.</param>
        public Error(string codigo, string mensaje)
        {
            Codigo = codigo;
            Mensaje = mensaje;
        }

        /// <summary>
        /// Representa un error indicando que no se encontró un recurso.
        /// </summary>
        public static readonly Error NotFound = new("Error.NotFound", "El recurso solicitado no fue encontrado.");

        /// <summary>
        /// Representa un error de validación general.
        /// </summary>
        public static readonly Error Validation = new("Error.Validation", "Una o más validaciones fallaron.");

        /// <summary>
        /// Representa un error donde el usuario está autenticado pero no tiene
        /// los permisos necesarios para realizar una acción específica sobre un recurso.
        /// </summary>
        public static readonly Error Forbidden = new("Error.Forbidden", "No tiene los permisos necesarios para realizar esta acción específica.");

        /// <summary>
        /// Representa un error por no estar autorizado.
        /// </summary>
        public static readonly Error Unauthorized = new("Error.Unauthorized", "No esta autorizado para realizar esta operación.");

		/// <summary>
		/// Representa un error genérico o inesperado.
		/// </summary>
		public static readonly Error Unexpected = new("Error.Unexpected", "Ocurrió un error inesperado.");

        /// <summary>
        /// Representa un error debido a un conflicto con el estado actual del recurso.
        /// Ejemplo: intentar crear un recurso que ya existe.
        /// </summary>
        public static readonly Error Conflict = new("Error.Conflict", "La operación entra en conflicto con el estado actual del recurso o el recurso ya existe.");
    }
}
