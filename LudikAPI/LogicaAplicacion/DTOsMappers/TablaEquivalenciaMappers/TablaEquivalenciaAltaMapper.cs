using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.EquivalenciaDTOs;
using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaAplicacion.DTOsMappers.EquivalenciaMappers;

namespace LogicaAplicacion.DTOsMappers.TablaEquivalenciaMappers
{
    public class TablaEquivalenciaAltaMapper
    {
        public static TablaEquivalenciaAltaDto toDto(TablaEquivalencia tablaEquivalencia)
        {
            return new TablaEquivalenciaAltaDto
            {
                Nombre = tablaEquivalencia.Nombre,
                Equivalencias = tablaEquivalencia.Equivalencias
                    .Select(equivalencia => EquivalenciaAltaMapper.toDto(equivalencia)).ToList(),
            };
        }

        public static TablaEquivalencia fromDto(TablaEquivalenciaAltaDto tablaEquivalenciaDto)
        {
            return new TablaEquivalencia
            {
                Nombre = tablaEquivalenciaDto.Nombre,
                Equivalencias = tablaEquivalenciaDto.Equivalencias
                    .Select(equivalenciaDto => EquivalenciaAltaMapper.fromDto(equivalenciaDto)).ToList()

            };
        }


    }
}
