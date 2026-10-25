using LogicaAplicacion.DTOs.KudoDTOs;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers.KudoMappers;

public class KudoMapper
{

    public static TipoKudoDto toDto(TipoKudo tipoKudo)
    {
        return new TipoKudoDto
        {
            Id = tipoKudo.Id,
            Nombre = tipoKudo.Nombre,
            NombreIcono = tipoKudo.NombreIcono

        };
    }

    public static TipoKudo fromDto(TipoKudoDto dto)
    {

        return new TipoKudo
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            NombreIcono = dto.NombreIcono
        };

    }
    
}