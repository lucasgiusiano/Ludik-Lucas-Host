using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;

namespace LogicaAplicacion.DTOs.UsuarioDTOs
{
    public class EstudianteAltaDto
    {

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre usuario debe tener entre 3 y 20 caracteres.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(20, ErrorMessage = "El nombre no puede exceder 20 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(20, ErrorMessage = "El apellido no puede exceder 20 caracteres.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La contrasenia es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "La contrasenia debe tener al menos 8 caracteres.")]
        public string Contrasenia { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Debe proporcionar exactamente 2 preguntas de seguridad.")]
        [MaxLength(2, ErrorMessage = "Debe proporcionar exactamente 2 preguntas de seguridad.")]
        public List<PreguntaSeguridadSeleccionadaDto> PreguntasDeSeguridad { get; set; }
    }
}