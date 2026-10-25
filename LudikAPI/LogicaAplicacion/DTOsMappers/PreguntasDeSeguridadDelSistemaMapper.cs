using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers;

public class PreguntasDeSeguridadDelSistemaMapper
{
    public static PreguntasDto toDto(IEnumerable<PreguntaDeSeguridad> preguntas)
    {
        return new PreguntasDto
        {
            Preguntas = preguntas.Select(p => new PreguntaDeSeguridadDto
            {
                Id = p.Id,
                Pregunta = p.Texto
            }).ToList()
        };
    }
}