using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaNegocio.Resultados;

public interface IObtenerPerfilConMedallas
{
    Task<Resultado<PerfilConMedallasDto>> EjecutarAsync(string estudianteId, int grupoId);
}
