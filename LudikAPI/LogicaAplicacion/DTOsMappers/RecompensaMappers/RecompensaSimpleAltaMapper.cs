using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Entidades;
using LogicaNegocio.EntidadesAuxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOsMappers.RecompensaMappers
{
    public class RecompensaSimpleAltaMapper
    {
        public static RecompensaSimpleAltaDto toDto(RecompensaSimple recompensa)
        {
            var dto = new RecompensaSimpleAltaDto
            {
                Nombre = recompensa.Nombre,
                Precio = recompensa.Precio,
            };

            // Es seguro hacer el cast porque sabemos que RecompensaSimple siempre tiene un RepresentacionIcono
            if (recompensa.Representacion is RepresentacionIcono repIcono)
            {
                dto.NombreIcono = repIcono.NombreIcono;
            }

            return dto;
        }

        public static RecompensaSimple FromDto(RecompensaSimpleAltaDto dto)
        {
            var recompensa = new RecompensaSimple
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio
            };


            // Es seguro hacer el cast porque sabemos que RecompensaSimple siempre tiene un RepresentacionIcono
            if (recompensa.Representacion is RepresentacionIcono repIcono)
            {
                repIcono.NombreIcono = dto.NombreIcono;
            }

            return recompensa;
        }
    }
}
