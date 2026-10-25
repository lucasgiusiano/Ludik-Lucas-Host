using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.DTOsMappers.EstudianteMappers
{
    public class EstudianteAltaMapper
    {
        public static EstudianteAltaDto toDto(Estudiante estudiante)
        {
            return new EstudianteAltaDto
            {
                NombreUsuario = estudiante.UserName,
                Nombre = estudiante.NombreCompleto.Nombre,
                Apellido = estudiante.NombreCompleto.Apellido,
                Contrasenia = estudiante.PasswordHash
            };
        }
        public static Estudiante fromDto(EstudianteAltaDto dto, NombreCompleto nombreCompleto)
        {
            return new Estudiante
            {
                UserName = dto.NombreUsuario,
                NombreCompleto = nombreCompleto,
                

            };
        }
    }
}
