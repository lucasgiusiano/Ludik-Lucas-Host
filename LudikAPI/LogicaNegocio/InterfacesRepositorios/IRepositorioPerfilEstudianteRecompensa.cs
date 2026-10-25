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
    public interface IRepositorioPerfilEstudianteRecompensa:IRepositorio<PerfilEstudianteRecompensa>
    {
        Task<bool> FueCanjeadaAsync(int recompensaId);
        Task<Resultado<PerfilEstudianteRecompensa>> GetByPerfilAndRecompensaIdAsync(int perfilId, int recompensaId);
    }
}
