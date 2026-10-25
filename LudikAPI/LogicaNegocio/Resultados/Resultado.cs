using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Resultados
{
    /// <summary>
    /// Representa el resultado de una operación que no devuelve un valor específico.
    /// Encapsula el estado de éxito o fracaso y los errores asociados.
    /// </summary>
    public class Resultado
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool EsExitoso { get; }

        /// <summary>
        /// Indica si la operación falló. Es la negación de EsExitoso.
        /// </summary>
        public bool EsFallo => !EsExitoso;

        /// <summary>
        /// Colección de errores si la operación falló.
        /// Es una lista de solo lectura para el exterior, pero se inicializa internamente.
        /// </summary>
        public IReadOnlyList<Error> Errores { get; }

        // Constructor protegido para forzar el uso de los métodos fábrica (Exitoso, Falla)
        protected Resultado(bool esExitoso, IEnumerable<Error> errores)
        {
            EsExitoso = esExitoso;
            Errores = (errores ?? Enumerable.Empty<Error>()).ToList().AsReadOnly();
        }

        /// <summary>
        /// Crea un resultado exitoso.
        /// </summary>
        /// <returns>Una instancia de Resultado indicando éxito.</returns>
        public static Resultado Exitoso() => new(true, Enumerable.Empty<Error>());

        /// <summary>
        /// Crea un resultado fallido con un único error.
        /// </summary>
        /// <param name="error">El error que causó el fallo.</param>
        /// <returns>Una instancia de Resultado indicando fallo con el error especificado.</returns>
        public static Resultado Falla(Error error) => new(false, new List<Error> { error });

        /// <summary>
        /// Crea un resultado fallido con una colección de errores.
        /// </summary>
        /// <param name="errores">La colección de errores que causaron el fallo.</param>
        /// <returns>Una instancia de Resultado indicando fallo con los errores especificados.</returns>
        public static Resultado Falla(IEnumerable<Error> errores) => new(false, errores);
    }
}
