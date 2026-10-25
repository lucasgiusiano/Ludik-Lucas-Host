using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers.UmbralParaMedallaPorKudosMappers;

public class UmbralParaMedallaPorKudosAltaMapper
{
    public static UmbralParaMedallaPorKudos fromDto(AltaUmbralParaMedallaPorKudosDto dto)
    {
        return new UmbralParaMedallaPorKudos
        {
            MedallaId = dto.MedallaId,
            TipoKudoId = dto.TipoKudoId,
            GrupoId = dto.GrupoId,
            CantidadKudos = dto.CantidadKudos
        };
    }
}