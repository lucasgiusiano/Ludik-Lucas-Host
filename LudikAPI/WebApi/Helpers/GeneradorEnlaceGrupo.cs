using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Routing;

namespace WebApi.Helpers
{
    public class GeneradorEnlaceGrupo : IGeneradorEnlaceGrupo
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GeneradorEnlaceGrupo(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public Resultado<string> GenerarEnlace(string codigo)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return Resultado<string>.Falla(new Error("Error.Context", "No se puede generar un enlace fuera del contexto de una petición HTTP."));


            var urlInvitacion = _linkGenerator.GetUriByAction(
                httpContext,
                action: "UnirseAGrupo",
                controller: "Estudiante",
                values: new { codigo });

            if (string.IsNullOrEmpty(urlInvitacion))
                return Resultado<string>.Falla(new Error("Error.Routing", "No se pudo generar el enlace. La ruta 'UnirseAGrupo' no fue encontrada."));
            

            return Resultado<string>.Exitoso(urlInvitacion);
        }
    }
}
