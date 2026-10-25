using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaAplicacion.DTOs.ProfesorDTOs;
using LogicaNegocio.Entidades;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.DTOsMappers.PreguntaDeSeguridadMappers;

public class PreguntaDeSeguridadMapper
{
    public static PreguntaDeSeguridadDto toDto(PreguntaRespuestaSeguridad pR)
    {
        return new PreguntaDeSeguridadDto
        {
            Id = pR.Id,
            Pregunta = pR.PreguntaDeSeguridad.Texto
        };
    }
}