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
    public interface IRepositorioProyectoAulaColaborativo : IRepositorio<ProyectoAulaColaborativo>
    {
        Task<Resultado<List<ProyectoAulaColaborativo>>> GetByGrupoAsync(int grupoId);
    }
}
