using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioPerfilEstudianteMedalla : IRepositorio<PerfilEstudianteMedalla>
    {
        Task<Resultado> RemoveByIdAsync(int id);
        Task<Resultado<PerfilEstudianteMedalla>> GetByPerfilYMedallaAsync(int perfilId, int medallaId);
        Task<Resultado<PerfilEstudianteMedalla>> GetByIdConPerfilYMedallaAsync(int id);
        Task<Resultado<int>> ContarMedallasPorEstudianteAsync(string estudianteId);
        Task<bool> ExistePerfilEstudianteMedallaAsync(int medallaId);
    }
}
