using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.ProfesorDTOs;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.DTOsMappers.ProfesorMappers
{
    public class ProfesorAltaMapper
    {
        public static ProfesorAltaDto toDto(string email,string nombreUsuario, string nombre, string apellido, string contrasenia)
        {
            return new ProfesorAltaDto
            {
                Correo = email,
                NombreUsuario = nombreUsuario,
                Nombre = nombre,
                Apellido = apellido,
                Contrasenia = contrasenia
            };
        }
        public static Profesor fromDto(ProfesorAltaDto dto)
        {
            return new Profesor
            {
                UserName = dto.NombreUsuario,
                NombreCompleto =  NombreCompleto.Crear(dto.Nombre, dto.Apellido).Valor,
                Email = dto.Correo,

            };
        }
    }
}
