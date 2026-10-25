using LogicaAplicacion.DTOs.BarraProgresoDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.BarraProgreso;

public interface IObtenerContenidoBarraProgreso
{
    Task<Resultado<BarraProgresoDto>> EjecutarAsync(int perfilEstudianteId, string estudianteId);
}