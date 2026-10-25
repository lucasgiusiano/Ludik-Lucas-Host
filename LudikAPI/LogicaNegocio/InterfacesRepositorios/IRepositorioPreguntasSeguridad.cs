using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace InterfacesRepositorio
{
	public interface IRepositorioPreguntasSeguridad : IRepositorio<PreguntaRespuestaSeguridad>
	{
		Task<Resultado<List<PreguntaRespuestaSeguridad>>> GetByNombreUsuarioAsync(string nombreUsuario);
    }

}

