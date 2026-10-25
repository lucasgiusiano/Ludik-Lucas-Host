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
    public interface IRepositorioSolicitudPerfilMedalla: IRepositorio<SolicitudPerfilMedalla>
    {
        Task<Resultado<List<SolicitudPerfilMedalla>>> GetByPerfilAsync(int perfilEstudianteId);
        Task<Resultado<List<SolicitudPerfilMedalla>>> GetByGrupoAsync(int grupoId);
    }
}
