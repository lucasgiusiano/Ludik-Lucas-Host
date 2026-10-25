using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOsMappers.UsuarioMappers
{
    public class LoginRespuestaMapper
    {
        public static LoginRespuestaDto toDto(string id, string nombreUsuario)
        {

            return new LoginRespuestaDto
            {
                Token = null, // El token se genera en el controlador
                Rol = null, // El rol se asigna en el caso de uso
                NombreUsuario = nombreUsuario,
                Id = id
            };
        }

       
    }
}
