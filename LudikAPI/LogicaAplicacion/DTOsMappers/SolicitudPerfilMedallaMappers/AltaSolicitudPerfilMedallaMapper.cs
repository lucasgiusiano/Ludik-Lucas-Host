using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers.SolicitudPerfilMedallaMappers
{
    public static class AltaSolicitudPerfilMedallaMapper
    {
        public static SolicitudPerfilMedalla Map(AltaSolicitudPerfilMedallaDto dto,int grupoId)
            => new SolicitudPerfilMedalla
            {
                PerfilEstudianteId = dto.PerfilEstudianteId,
                MedallaId = dto.MedallaId,
                GrupoId = grupoId,
                Descripcion = dto.Descripcion
            };
    }
}
