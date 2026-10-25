using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;

public class UmbralParaMedallaDto
{
    public int Id { get; set; }
    public int CantidadKudos { get; set; }

    public int MedallaId { get; set; }
    public string MedallaNombre { get; set; }
    public string RutaIconoMedalla { get; set; }

    public int TipoKudoId { get; set; }
    public string TipoKudoNombre { get; set; }
}