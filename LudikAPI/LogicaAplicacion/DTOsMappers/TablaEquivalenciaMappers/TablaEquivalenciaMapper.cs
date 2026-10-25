using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaAplicacion.DTOsMappers.EquivalenciaMappers;

namespace LogicaAplicacion.DTOsMappers.TablaEquivalenciaMappers;

public class TablaEquivalenciaMapper
{
    public static TablaEquivalenciaDto toDto(TablaEquivalencia tablaEquivalencia)
    {
        return new TablaEquivalenciaDto
        {
            Id = tablaEquivalencia.Id,
            Nombre = tablaEquivalencia.Nombre,
            Equivalencias = tablaEquivalencia.Equivalencias
                .Select(equivalencia => EquivalenciaMapper.toDto(equivalencia)).ToList(),
        };
    }

    public static TablaEquivalencia fromDto(TablaEquivalenciaDto tablaEquivalenciaDto)
    {
        return new TablaEquivalencia
        {
            Id = tablaEquivalenciaDto.Id,
            Nombre = tablaEquivalenciaDto.Nombre,
            Equivalencias = tablaEquivalenciaDto.Equivalencias
                .Select(equivalenciaDto => EquivalenciaMapper.fromDto(equivalenciaDto)).ToList()

        };
    }
}