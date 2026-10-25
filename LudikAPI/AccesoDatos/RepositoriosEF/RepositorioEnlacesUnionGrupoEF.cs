using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
	public class RepositorioEnlacesUnionGrupoEF : IRepositorioEnlacesUnionGrupo
	{
		private readonly ContextoDb _db;
		public RepositorioEnlacesUnionGrupoEF(ContextoDb db)
		{
			_db = db;
		}
		public Task<Resultado> AddAsync(EnlaceUnion unObjeto)
		{
			throw new NotImplementedException();
		}

		public Task<bool> ExisteSolicitudPendiente(int idEstudiante, int idGrupo)
		{
			throw new NotImplementedException();
		}

		public Task<Resultado<IEnumerable<EnlaceUnion>>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Resultado<EnlaceUnion>> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public string obtenerCodigoInvitacion(int idGrupo)
		{
			throw new NotImplementedException();
		}

		public async Task<EnlaceUnion> ObtenerPorCodigoAsync(string codigo)
		{
			var codigoExtraido = codigo?.Trim().Split('/').LastOrDefault();

			if (string.IsNullOrEmpty(codigoExtraido))
				return null;

			return await _db.EnlacesUnion
							.FirstOrDefaultAsync(e => e.CodigoUnico == codigoExtraido);
		}

		public Task<Grupo> ObtenerPorEnlaceAsync(string codigoBase)
		{
			throw new NotImplementedException();
		}

		public Task<Resultado> RemoveAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Resultado> RemoveAsync(EnlaceUnion unObjeto)
		{
			throw new NotImplementedException();
		}

		public Task<Resultado> UpdateAsync(EnlaceUnion unObjeto)
		{
			throw new NotImplementedException();
		}
	}
}
