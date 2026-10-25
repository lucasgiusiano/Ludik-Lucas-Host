using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;

public interface IEliminarUmbralParaMedallaPorKudos
{
    Task<Resultado> EjecutarAsync(int umbralMedallaId, string profesorId);
}