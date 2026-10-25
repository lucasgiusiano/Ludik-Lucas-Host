using System.ComponentModel.DataAnnotations;

namespace LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;

public class AltaUmbralParaMedallaPorKudosDto
{
    [Required(ErrorMessage = "El Id de la medalla es obligatorio.")]
    public int MedallaId { get; set; }

    [Required(ErrorMessage = "El Id del tipo de kudo es obligatorio.")]
    public int TipoKudoId { get; set; }

    [Required(ErrorMessage = "El Id del grupo es obligatorio.")]
    public int GrupoId { get; set; }

    [Required(ErrorMessage = "La cantidad de kudos es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de kudos debe ser mayor que cero.")]
    public int CantidadKudos { get; set; }
}