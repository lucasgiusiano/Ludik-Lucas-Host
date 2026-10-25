using InterfacesRepositorio;
using System;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace InterfacesRepositorio
{
	public interface IRepositorioUsuarios : IRepositorio<Usuario>
	{
		Task<Resultado<Usuario>> GetUsuarioPorNombreAsync(string nombreUsuario);

        Task<IList<string>> GetRolesAsync(Usuario usuario);

		Task<Resultado<Usuario>> GetByStringIdAsync(string id);
    }

}

