using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers
{
    public static class ExtensionesControlador
    {
        public static IActionResult ManejarFallo(this ControllerBase controller, Resultado resultado)
        {
            if (resultado.EsExitoso)
            {
                throw new InvalidOperationException("No se puede manejar un resultado exitoso con ManejarFallo.");
            }

            var primerError = resultado.Errores.First();

            // Mapear código de error a un StatusCode HTTP.
            int statusCode = primerError.Codigo switch
            {
                "Error.Unauthorized" => StatusCodes.Status401Unauthorized,
                "Error.Validation" => StatusCodes.Status400BadRequest,
                "Error.Forbidden" => StatusCodes.Status403Forbidden,
                "Error.NotFound" => StatusCodes.Status404NotFound,
                "Error.Conflict" => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            // Para errores de validación, es útil devolver todos los errores
            object responsePayload = primerError.Codigo == "Error.Validation"
                ? resultado.Errores.Select(e => new { e.Codigo, e.Mensaje })
                : new { primerError.Codigo, primerError.Mensaje };

            return controller.StatusCode(statusCode, responsePayload);
        }
    }
}
