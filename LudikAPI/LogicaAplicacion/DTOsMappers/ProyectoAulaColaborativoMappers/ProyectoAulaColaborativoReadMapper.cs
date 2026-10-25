using LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers.ProyectoAulaColaborativoMappers
{
    public static class ProyectoAulaColaborativoReadMapper
    {
        public static ProyectoAulaColaborativoDto ToDto(this ProyectoAulaColaborativo pac)
            => new ProyectoAulaColaborativoDto
            {
                Id = pac.Id,
                GrupoId = pac.GrupoId,
                Nombre = pac.Nombre,
                CantidadMedallasNecesarias = pac.CantidadMedallasNecesarias,
                RecompensaClaseId = pac.RecompensaClaseId,
                Estado = pac.Estado
            };
    }
}
