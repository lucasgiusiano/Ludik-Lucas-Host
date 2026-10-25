using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.InterfacesRepositorio;
using System;

namespace InterfacesRepositorio
{
	public interface IRepositorioEnlacesUnionGrupo : IRepositorio<EnlaceUnion>
	{
        public String obtenerCodigoInvitacion(int idGrupo);
        Task<EnlaceUnion> ObtenerPorCodigoAsync(string codigo);
        Task<Grupo> ObtenerPorEnlaceAsync(string codigoBase);
        Task<bool> ExisteSolicitudPendiente(int idEstudiante, int idGrupo);

    }

}

