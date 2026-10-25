namespace LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;

public class RespuestaDto
{
    public int PreguntaRespuestaSeguridadId { get; set; } // Id de PreguntaRespuestaSeguridad, que es la clave primaria de la tabla PreguntasRespuestasSeguridad
    public int PreguntaDeSeguridadId { get; set; } // Id de la pregunta
    public string Respuesta { get; set; }
    
}