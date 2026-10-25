using System.ComponentModel.DataAnnotations;

namespace LogicaAplicacion.DTOs.RecompensaDTOs;

public class RecompensaYGruposDto
{
    [Required]
    public int RecompensaId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Debe proporcionar al menos un Id de grupo.")]
    public List<int> GruposIds { get; set; }
}