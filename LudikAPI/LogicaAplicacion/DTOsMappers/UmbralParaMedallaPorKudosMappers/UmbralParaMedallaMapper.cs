using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers.UmbralParaMedallaPorKudosMappers;

public static class UmbralParaMedallaMapper
{
    public static UmbralParaMedallaDto toDto(UmbralParaMedallaPorKudos umbral)
    {
        return new UmbralParaMedallaDto
        {
            Id = umbral.Id,
            CantidadKudos = umbral.CantidadKudos,
            MedallaId = umbral.MedallaId,
            MedallaNombre = umbral.Medalla?.Nombre ?? "N/A",
            RutaIconoMedalla= umbral.Medalla?.NombreIcono ?? "default.png",
            TipoKudoId = umbral.TipoKudoId,
            TipoKudoNombre = umbral.TipoKudo?.Nombre ?? "N/A"
        };
    }
}