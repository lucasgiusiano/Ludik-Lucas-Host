using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.TablaClasificacionDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers.TablaClasificacionMappers
{
    public static class TablaClasificacionInfoMapper
    {
        public static TablaClasificacionInfoDto Map(TablaClasificacion tabla, List<PerfilEstudiante> participantes)
        {
            

            return new TablaClasificacionInfoDto
            {
                Id = tabla.Id,
                Nombre = tabla.Nombre,
                MedallaAsociadaId = tabla.MedallaAsociadaId,
                MedallaAsociadaNombre = tabla.MedallaAsociada.Nombre,
                Participantes = participantes
                    .Select(p => new ParticipanteTablaDto
                    {
                        PerfilEstudianteId = p.Id,
                        NombreEstudiante = $"{p.Estudiante.NombreCompleto.Nombre} {p.Estudiante.NombreCompleto.Apellido}",
                        CantidadMedallas = p.MedallasObtenidas
                                               .Count(pm => pm.MedallaId == tabla.MedallaAsociadaId)
                    })
                    .ToList()
            };
        }
    }
}
