using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;

public interface IAltaUmbralParaMedallaPorKudos
{
    Task<Resultado> EjecutarAsync(string profesorId,AltaUmbralParaMedallaPorKudosDto dto);
}