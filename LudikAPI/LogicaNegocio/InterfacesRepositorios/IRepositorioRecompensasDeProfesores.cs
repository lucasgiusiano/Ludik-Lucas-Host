using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.InterfacesRepositorios;

public interface IRepositorioRecompensasDeProfesores: IRepositorio<RecompensaProfesor>
{
    Task<Resultado<IEnumerable<RecompensaProfesor>>> GetByProfesorIdAsync(string profesorId);
}