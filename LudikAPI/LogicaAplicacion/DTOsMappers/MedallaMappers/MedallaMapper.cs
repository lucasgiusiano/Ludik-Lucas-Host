using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.MedallaDTOs;

namespace LogicaAplicacion.DTOsMappers.MedallaMappers;

public class MedallaMapper
{
    public static MedallaDto toDto(Medalla medalla)
    {
        return new MedallaDto
        {
            Id = medalla.Id,
            NombreIcono = medalla.NombreIcono,
            Nombre = medalla.Nombre,
            Descripcion = medalla.Descripcion,
            CantidadMedallasBrinda = medalla.MonedasOtorgadas,
        };
    }

    public static Medalla fromDto(MedallaDto dto)
    {

        return new Medalla
        {
            Id = dto.Id,
            NombreIcono = dto.NombreIcono,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            MonedasOtorgadas = dto.CantidadMedallasBrinda,
        };

    }
}
