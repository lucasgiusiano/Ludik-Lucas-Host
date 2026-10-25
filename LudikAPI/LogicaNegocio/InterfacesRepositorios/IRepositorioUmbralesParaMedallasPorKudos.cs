using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.InterfacesRepositorios;

public interface IRepositorioUmbralesParaMedallasPorKudos: IRepositorio<UmbralParaMedallaPorKudos>
{
    Task<bool> ExisteConfiguracionAsync(int grupoId, int tipoKudoId);

    public Task<Resultado<IEnumerable<UmbralParaMedallaPorKudos>>> GetAllByGrupoIdAsync(int grupoId);
}