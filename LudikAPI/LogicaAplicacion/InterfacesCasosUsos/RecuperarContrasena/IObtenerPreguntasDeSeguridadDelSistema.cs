using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;

public interface IObtenerPreguntasDeSeguridadDelSistema
{
    Task<Resultado<PreguntasDto>> EjecutarAsync();
}