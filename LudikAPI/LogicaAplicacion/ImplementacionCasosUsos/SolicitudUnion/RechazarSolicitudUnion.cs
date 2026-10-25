using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudUnion;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;
using System.Linq;
using System.Threading.Tasks;

namespace LogicaAplicacion.ImplementacionCasosUsos.SolicitudUnion
{
    public class RechazarSolicitudUnion : IRechazarSolicitudUnion
    {
        private readonly IRepositorioSolicitudesUnion _repoSolicitudes;
        private readonly IRepositorioProfesores _repoProfesores;

        public RechazarSolicitudUnion(IRepositorioSolicitudesUnion repoSolicitudes, IRepositorioProfesores repoProfesores)
        {
            _repoSolicitudes = repoSolicitudes;
            _repoProfesores = repoProfesores;
        }

        public async Task<Resultado> EjecutarAsync(int idSolicitud,string profesorId)
        {
            var solicitud = await _repoSolicitudes.GetSolicitudConEstudianteYGrupoPorIdAsync(idSolicitud);
            if (solicitud == null)
                return Resultado.Falla(new Error("Error.Validation", "La solicitud no existe."));

            if (solicitud.Estado != EstadoSolicitud.Pendiente)
                return Resultado.Falla(new Error("Error.Validation", "La solicitud ya fue procesada."));
            var grupo = solicitud.Grupo;
            if (grupo == null)
                return Resultado.Falla(new Error("Error.Validation", "La solicitud no tiene grupo asociado."));

            // Validación: el grupo pertenece al profesor (repo debe implementar PerteneceGrupoAsync)
            var pertenece = await _repoProfesores.PerteneceGrupoAsync(profesorId, grupo.Id);
            if (pertenece.EsFallo)
                return Resultado.Falla(new Error("Error.Autorizacion", "El grupo de la solicitud no pertenece al profesor logueado."));

            solicitud.Estado = EstadoSolicitud.Rechazada;


            var resultado = await _repoSolicitudes.UpdateAsync(solicitud);
            if (resultado.EsFallo) return resultado;

            return Resultado.Exitoso();
        }
    }
}
