using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Entidades;
using LogicaNegocio.EntidadesAuxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.DTOsMappers.RecompensaMappers
{
    public static class RecompensaSimpleMapper
    {
        public static RecompensaDto ToDto(Recompensa recompensa)
        { 
            if (recompensa == null) return null!;

            var infoVisual = recompensa.Representacion.GetInformacionVisual();

            return new RecompensaDto
            {
                Id = recompensa.Id,
                Nombre = recompensa.Nombre,
                Precio = recompensa.Precio,
                Representacion = infoVisual.Datos, // Correcto: Asigna el objeto de datos polimórfico
                Tipo = infoVisual.Tipo
            };
        }
        public static RecompensaSimple FromDto(RecompensaSimpleAltaDto dto)
        {
            var recompensa = new RecompensaSimple
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,

            };

            if (recompensa.Representacion is RepresentacionIcono repIcono)
            {
                repIcono.NombreIcono = dto.NombreIcono;
            }

            return recompensa;
        }
    }
}
