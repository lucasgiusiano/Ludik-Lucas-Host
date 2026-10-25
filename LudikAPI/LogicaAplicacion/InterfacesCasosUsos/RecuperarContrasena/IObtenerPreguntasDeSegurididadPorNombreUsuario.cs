using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;

public interface IObtenerPreguntasDeSegurididadPorNombreUsuario
{
    Task<Resultado<PreguntasDto>> EjecutarAsync(string nombreUsuario);
}