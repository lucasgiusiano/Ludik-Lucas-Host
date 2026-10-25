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
    public class RecompensaEditarMapper
    {
        public static void Update(RecompensaSimple entidad, RecompensaSimpleEditarDto dto)
        {
            entidad.Nombre = dto.Nombre;
            entidad.Precio = dto.Precio;
            if (entidad.Representacion is RepresentacionIcono repIcono)
            {
                repIcono.NombreIcono = dto.NombreIcono;
            }
        }
    }
}
