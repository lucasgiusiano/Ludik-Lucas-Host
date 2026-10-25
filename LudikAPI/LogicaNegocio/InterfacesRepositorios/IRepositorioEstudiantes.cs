using InterfacesRepositorio;
using System.Collections.Generic;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace InterfacesRepositorio
{
	public interface IRepositorioEstudiantes : IRepositorio<Estudiante>
	{


		List<Medalla> getMedallasAlumno(int idAlumno, int idGrupo);
		Task<Estudiante> GetByIdAsyncString(string id);
		Task<Resultado<Estudiante>> GetByPerfilIdAsync(int perfilId);

		Task<Resultado<Estudiante>> GetByStringIdAsync(string id);

		Task<Resultado<Estudiante>> GetByIdConHitosAsync(string id);





    }

}

