namespace LogicaAplicacion.DTOs.ImagenDto;

public class SubirImagenDto
{
    public Stream ImagenStream { get; set; }
    public string IdUsuarioAutenticado { get; set; }
    public string Proposito { get; set; }
    public int? EntidadAsociadaId { get; set; }
}