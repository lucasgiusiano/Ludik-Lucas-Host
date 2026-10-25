using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.MedallaDTOs;

namespace LogicaAplicacion.DTOsMappers.MedallaMappers
{
    public class MedallaAltaMapper
    {

        public static MedallaAltaDto toDto(Medalla medalla)
        {
            return new MedallaAltaDto
            {
                NombreIcono = medalla.NombreIcono,
                Nombre = medalla.Nombre,
                Descripcion = medalla.Descripcion,
                CantidadMonedasBrinda = medalla.MonedasOtorgadas,
            };
        }

        public static Medalla fromDto(MedallaAltaDto dto)
        {

            return new Medalla{
                NombreIcono = dto.NombreIcono,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                MonedasOtorgadas = dto.CantidadMonedasBrinda,
            };

        }
    }













}
