using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs;
using LogicaNegocio.Entidades;
using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.DTOsMappers.ProyectoAulaColaborativoMappers
{
    public static class ProyectoAulaColaborativoMapper
    {
        public static ProyectoAulaColaborativo ToDomain(AltaProyectoAulaColaborativoDto dto,int grupoId)
        {
            return new ProyectoAulaColaborativo
            {
                GrupoId = grupoId,
                Nombre = dto.Nombre,
                CantidadMedallasNecesarias = dto.CantidadMedallasNecesarias,
                RecompensaClaseId = dto.RecompensaClaseId,
                Estado = EstadoPAC.Activo,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin
            };
        }
    }
}
