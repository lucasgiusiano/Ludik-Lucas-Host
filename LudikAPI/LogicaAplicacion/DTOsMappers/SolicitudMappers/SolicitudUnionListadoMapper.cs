using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;

namespace LogicaAplicacion.DTOsMappers.SolicitudMappers
{
    public static class SolicitudUnionListadoMapper
    {
        public static SolicitudUnionListadoDto Mapear(SolicitudUnion solicitud)
        {
            return new SolicitudUnionListadoDto
            {
                IdSolicitud = solicitud.Id,
                IdEstudiante = solicitud.EstudianteId,
                NombreEstudiante = solicitud.Estudiante?.NombreCompleto != null
                    ? $"{solicitud.Estudiante.NombreCompleto.Nombre} {solicitud.Estudiante.NombreCompleto.Apellido}"
                    : "Desconocido",
                fecha = solicitud.Fecha
            };
        }

        public static List<SolicitudUnionListadoDto> MapearLista(IEnumerable<SolicitudUnion> solicitudes)
        {
            return solicitudes.Select(Mapear).ToList();
        }
    }
}
