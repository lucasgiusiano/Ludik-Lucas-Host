using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.InterfacesRepositorios;

public interface IRepositorioAvatares : IRepositorio<Avatar>
{
    
    public Task<Resultado<Avatar>> GetByPerfilIdAsync(int idPerfilEstudiante);
}