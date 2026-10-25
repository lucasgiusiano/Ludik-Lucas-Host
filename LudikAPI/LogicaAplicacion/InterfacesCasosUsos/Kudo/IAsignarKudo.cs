using LogicaAplicacion.DTOs.KudoDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Kudo;

public interface IAsignarKudo
{
    Task<Resultado> EjecutarAsync(string idEstudianteEmisor,AsignarKudoDto kudoDto);
}