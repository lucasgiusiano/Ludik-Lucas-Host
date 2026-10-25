using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers.SolicitudPerfilMedallaMappers
{
    public static class SolicitudPerfilMedallaMapper
    {
        public static SolicitudPerfilMedallaDto Map(SolicitudPerfilMedalla entidad)
            => new SolicitudPerfilMedallaDto
            {
                Id = entidad.Id,
                PerfilEstudianteId = entidad.PerfilEstudianteId,
                MedallaId = entidad.MedallaId,
                Descripcion = entidad.Descripcion,
                Fecha = entidad.Fecha,
                Estado = entidad.Estado.ToString()
            };
    }
}
