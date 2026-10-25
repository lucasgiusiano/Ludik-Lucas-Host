using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.TablaClasificacionDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers.TablaClasificacionMappers
{
    public static class TablaClasificacionMapper
    {
        public static TablaClasificacion MapAlta(
            TablaClasificacionAltaDto dto,
            Grupo grupo)
        {
            return new TablaClasificacion
            {
                Nombre = dto.Nombre,
                MedallaAsociadaId = dto.MedallaAsociadaId,
                GrupoId = grupo.Id,
                Grupo = grupo,
            };
        }
    }
}
