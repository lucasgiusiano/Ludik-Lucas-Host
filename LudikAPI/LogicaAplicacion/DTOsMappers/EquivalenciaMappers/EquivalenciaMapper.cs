using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.EquivalenciaDTOs;
using LogicaAplicacion.DTOsMappers.MedallaMappers;

namespace LogicaAplicacion.DTOsMappers.EquivalenciaMappers;

public class EquivalenciaMapper
{
    public static EquivalenciaDto toDto(Equivalencia equivalencia)
    {
        return new EquivalenciaDto
        {
            Id = equivalencia.Id,
            Nota = equivalencia.Nota,
            MedallasNecesarias = equivalencia.MedallasNecesarias.Select(medalla => MedallaMapper.toDto(medalla))
                .ToList()
        };
    }

    public static Equivalencia fromDto(EquivalenciaDto dto)
    {
        return new Equivalencia
        {
            Id = dto.Id,
            Nota = dto.Nota,
            MedallasNecesarias = dto.MedallasNecesarias.Select(medallaDto => MedallaMapper.fromDto(medallaDto))
                .ToList()
        };
    }
}