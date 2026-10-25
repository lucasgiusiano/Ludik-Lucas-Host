using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;

public interface IActualizarUmbralParaMedallaPorKudos
{
    Task<Resultado> EjecutarAsync(UmbralParaMedallaDto dto, string profesorId);
}