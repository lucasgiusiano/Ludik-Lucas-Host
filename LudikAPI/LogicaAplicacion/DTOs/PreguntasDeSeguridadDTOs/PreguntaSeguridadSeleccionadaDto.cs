using System.ComponentModel.DataAnnotations;

namespace LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;

public class PreguntaSeguridadSeleccionadaDto
{
    [Required(ErrorMessage = "El ID de la pregunta es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de la pregunta no es válido.")]
    public int PreguntaId { get; set; }

    [Required(ErrorMessage = "La respuesta de seguridad es obligatoria.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "La respuesta debe tener entre 3 y 50 caracteres.")]
    public string Respuesta { get; set; }
}