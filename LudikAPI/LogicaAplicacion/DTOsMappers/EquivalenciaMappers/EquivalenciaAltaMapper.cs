using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.EquivalenciaDTOs;
using LogicaAplicacion.DTOsMappers.MedallaMappers;

namespace LogicaAplicacion.DTOsMappers.EquivalenciaMappers
{
    public class EquivalenciaAltaMapper
    {
        public static EquivalenciaAltaDto toDto(Equivalencia equivalencia)
        {
            return new EquivalenciaAltaDto
            {
                Nota = equivalencia.Nota,
                MedallasNecesarias = equivalencia.MedallasNecesarias.Select(medalla => MedallaBasicaMapper.toDto(medalla))
                    .ToList()
            };
        }

        public static Equivalencia fromDto(EquivalenciaAltaDto dto)
        {
            return new Equivalencia
            {
                Nota = dto.Nota,
                MedallasNecesarias = dto.MedallasNecesarias.Select(medallaDto => MedallaBasicaMapper.fromDto(medallaDto))
                    .ToList()
            };
        }


    }

}
