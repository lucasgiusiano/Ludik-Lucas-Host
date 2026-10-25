using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;

namespace LogicaAplicacion.DTOs.RestablecerContrasenaDTO;

public class InformacionParaRestablecerContrasenaDto
{
    public string NombreUsuario { get; set; }
    public List<RespuestaDto> Respuestas { get; set; }
    public string NuevaContrasena { get; set; }
}