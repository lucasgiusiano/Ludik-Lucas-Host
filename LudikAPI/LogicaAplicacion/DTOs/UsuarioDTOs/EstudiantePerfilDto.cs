using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOs.UsuarioDTOs
{
    public class EstudiantePerfilDto : UsuarioBaseDto
    {
        public List<RespuestaDto> RespuestasSeguridad { get; set; }
    }
}
