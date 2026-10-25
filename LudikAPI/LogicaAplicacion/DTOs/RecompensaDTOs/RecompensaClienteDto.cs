using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.DTOs.RecompensaDTOs;

public class RecompensaClienteDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Precio { get; set; }
    public TipoVisual Tipo { get; set; } 
    public RespuestaVisual Datos { get; set; } 
}


public abstract record RespuestaVisual;

public record RespuestaImagenDto(string UrlMiniatura, string UrlCompleta) : RespuestaVisual;

public record RespuestaIconoDto(string NombreIcono) : RespuestaVisual;