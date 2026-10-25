using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
	public class RepositorioGruposEF : IRepositorioGrupos

	{
		private readonly ContextoDb _db;
		public RepositorioGruposEF(ContextoDb db)
		{
			_db = db;
		}

		public async Task<Resultado> AddAsync(Grupo unGrupo)
		{
			if (unGrupo == null)
				return Resultado.Falla(new Error("Error.Validation", "El grupo a guardar debe de contener datos."));

			try
			{
				if (unGrupo.TablaEquivalencia != null)
					_db.Entry(unGrupo.TablaEquivalencia).State = EntityState.Unchanged;

				await _db.Grupos.AddAsync(unGrupo);
				await _db.SaveChangesAsync();
				return Resultado.Exitoso();
			}
			catch (DbUpdateException dbEx)
			{
				var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
				return Resultado.Falla(new Error("Error.Unexpected", $"Error al guardar el grupo en la BD: {detalle}"));
			}

			catch (GrupoNoValidoExeption valEx)
			{
				return Resultado.Falla(new Error("Error.Validation", valEx.Message));
			}
			catch (Exception e)
			{
				return Resultado.Falla(Error.Unexpected);
			}
		}

        public async Task<Resultado<Grupo>> GetByIdAsync(int id)
        {
            try
            {
                var grupo = await _db.Grupos
                    .Include(g => g.Profesor)
                    
                    .Include(g => g.Alumnos)
						.ThenInclude(a => a.HistorialRendimientoPeriodos)
                    
                    .Include(g => g.Alumnos)
                        .ThenInclude(al => al.MedallasObtenidas)
                            .ThenInclude(pm => pm.Medalla)
                    
                    .Include(g => g.TablaEquivalencia)
                        .ThenInclude(t => t.Equivalencias)
							.ThenInclude(tm => tm.MedallasNecesarias)

                    .Include(g => g.Alumnos)
                    .ThenInclude(al => al.Estudiante)
                    .ThenInclude(e => e.NombreCompleto)

                    .Include(g => g.EnlaceUnion)
                    .Include(g => g.Tienda)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (grupo == null)
                    return Resultado<Grupo>.Falla(Error.NotFound);

                return Resultado<Grupo>.Exitoso(grupo);
            }
            catch (Exception e)
            {
                return Resultado<Grupo>.Falla(Error.Unexpected);
            }
        }

        public async Task<Resultado<IEnumerable<Grupo>>> GetAllAsync()
		{
			try
			{
				var grupos = await _db.Grupos.ToListAsync();
				return Resultado<IEnumerable<Grupo>>.Exitoso(grupos);
			}
			catch (Exception e)
			{
				return Resultado<IEnumerable<Grupo>>.Falla(Error.Unexpected);
			}
		}

		public async Task<Resultado> UpdateAsync(Grupo grupoNuevo)
		{
			if (grupoNuevo == null)
				return Resultado.Falla(new Error("Error.Validation", "El grupo para actualizar no puede ser null."));


			try
			{
				var grupoExistente = await _db.Grupos.FindAsync(grupoNuevo.Id);
				if (grupoExistente == null)
					return Resultado.Falla(Error.NotFound);


				_db.Entry(grupoExistente).CurrentValues.SetValues(grupoNuevo);

				await _db.SaveChangesAsync();
				return Resultado.Exitoso();
			}
			catch (DbUpdateConcurrencyException dbEx)
			{

				return Resultado.Falla(new Error(Error.Conflict.Codigo, $"Error de concurrencia al actualizar el grupo: {dbEx.Message}"));
			}
			catch (DbUpdateException dbEx)
			{
				var detalle = dbEx.InnerException?.Message ?? dbEx.Message;

				return Resultado.Falla(new Error("Error.Unexpected", $"Error al actualizar el grupo en la BD: {detalle}"));
			}
			catch (GrupoNoValidoExeption valEx)
			{
				return Resultado.Falla(new Error("Error.Unexpected", valEx.Message));
			}
			catch (Exception e)
			{
				return Resultado.Falla(Error.Unexpected);
			}
		}

		public async Task<Resultado> RemoveAsync(int id)
		{
			try
			{
				var grupo = await _db.Grupos
					.Include(g => g.Alumnos)
					.Include(g => g.Solicitudes)
					.Include(g => g.TablasClasificacion)
					.Include(g => g.EnlaceUnion)
					.Include(g => g.Tienda)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(g => g.Id == id);

				if (grupo == null)
				{
					return Resultado.Falla(Error.NotFound);
				}

				if (grupo.Alumnos != null && grupo.Alumnos.Any())
					_db.PerfilesEstudiantes.RemoveRange(grupo.Alumnos);
				if (grupo.Solicitudes != null && grupo.Solicitudes.Any())
					_db.SolicitudesUnion.RemoveRange(grupo.Solicitudes);
				if (grupo.TablasClasificacion != null && grupo.TablasClasificacion.Any())
					_db.TablasClasificacion.RemoveRange(grupo.TablasClasificacion);

				if (grupo.EnlaceUnion != null)
					_db.EnlacesUnion.Remove(grupo.EnlaceUnion);
				if (grupo.Tienda != null)
					_db.Tiendas.Remove(grupo.Tienda);

				_db.Grupos.Remove(grupo);
				await _db.SaveChangesAsync();
				return Resultado.Exitoso();
			}
			catch (DbUpdateException dbEx)
			{
				var detalle = dbEx.InnerException?.Message ?? dbEx.Message;

				return Resultado.Falla(new Error("Error.Unexpected", $"Error al eliminar el grupo: {detalle}"));
			}
			catch (Exception e)
			{
				return Resultado.Falla(Error.Unexpected);
			}
		}




		public async Task<Resultado<IEnumerable<Grupo>>> obtenerGruposPorProfesorAsync(string idProfesor)
		{
            try
            {
                if (string.IsNullOrWhiteSpace(idProfesor))
                    return Resultado<IEnumerable<Grupo>>.Falla(
                        new Error("Error.Validation", "El ID de profesor no puede estar vacío."));

                var grupos = await _db.Grupos
                    .Include(g => g.Alumnos)
                        .ThenInclude(a => a.MedallasObtenidas)
                            .ThenInclude(pm => pm.Medalla)

                    .Include(g => g.Alumnos)
                        .ThenInclude(a => a.HistorialRendimientoPeriodos)
                            .ThenInclude(rp => rp.RendimientoMedallas)
                                .ThenInclude(rpm => rpm.Medalla)

                    .Include(g => g.TablaEquivalencia)
                        .ThenInclude(te => te.Equivalencias)
                            .ThenInclude(eq => eq.MedallasNecesarias)
                    .Where(g => g.ProfesorId == idProfesor)
                    .AsSplitQuery()
                    .ToListAsync();

                return Resultado<IEnumerable<Grupo>>.Exitoso(grupos);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Grupo>>.Falla(
                    new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<TablaEquivalencia>> GetTablaEquivalenciaPorPerfilEstudianteAsync(int perfilEstudianteId)
        {
            try
            {
                var tablaEquivalencia = await _db.PerfilesEstudiantes
                    .Where(p => p.Id == perfilEstudianteId)
                    .Include(p => p.Grupo.TablaEquivalencia.Equivalencias)
                    .ThenInclude(e => e.MedallasNecesarias)
                    .Select(p => p.Grupo.TablaEquivalencia)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();

                if (tablaEquivalencia == null)
                {
                    return Resultado<TablaEquivalencia>.Falla(Error.NotFound);
                }

                return Resultado<TablaEquivalencia>.Exitoso(tablaEquivalencia);
            }
            catch (Exception e)
            {
                return Resultado<TablaEquivalencia>.Falla(new Error("Error.Unexpected", e.Message));
            }
        }

      

		public Task<Resultado> RemoveAsync(Grupo unObjeto)
		{
			throw new NotImplementedException();
		}

		public async Task<Grupo> ObtenerPorEnlaceAsync(string codigoBase)
		{
			return await _db.Grupos
					.Include(g => g.EnlaceUnion)
					.FirstOrDefaultAsync(g => g.EnlaceUnion.CodigoUnico == codigoBase);
		}

		public async Task<List<Grupo>> ObtenerGruposPorEstudianteId(string idEstudiante)
		{

			if (string.IsNullOrWhiteSpace(idEstudiante))
			{
				// Devuelve una lista vacía si el ID es inválido para evitar errores en la consulta.
				return new List<Grupo>();
			}

			List<Grupo> gruposDelEstudiante = await _db.Grupos
				.Include(g => g.Alumnos)
				.Where(g => g.Alumnos.Any(pe => pe.EstudianteId == idEstudiante))
				.ToListAsync();

			return gruposDelEstudiante;
		}

		public async Task<List<Grupo>> ObtenerGruposPorProfesorId(string idProfesor)
		{
			if (string.IsNullOrWhiteSpace(idProfesor))
			{
				// Devuelve una lista vacía si el ID es inválido para evitar errores en la consulta.
				return new List<Grupo>();
			}

			List<Grupo> gruposDelProfesor = await _db.Grupos
				.Include(g => g.Alumnos)
				.Where(g => g.ProfesorId == idProfesor)
				.ToListAsync();

			return gruposDelProfesor;
		}

        public async Task<Resultado<List<Grupo>>> ObtenerGruposPorIdsYProfesor(List<int> idsGrupos, string profesorId)
        {
            try
            {
         
                var grupos = await _db.Grupos
                    .Include(g => g.Tienda)
                    .ThenInclude(t => t.Recompesas)
                    .Where(g => idsGrupos.Contains(g.Id) && g.ProfesorId == profesorId)
                    .ToListAsync();

                return Resultado<List<Grupo>>.Exitoso(grupos);
            }
            catch (Exception ex)
            {
                return Resultado<List<Grupo>>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }
        public async Task<bool> EstudiantePerteneceAlGrupoAsync(int grupoId, string estudianteId)
        {
            return await _db.PerfilesEstudiantes
                .AnyAsync(p =>
                    p.GrupoId == grupoId
                    && p.EstudianteId == estudianteId 
                );
        }

        public async Task<Resultado<bool>> GrupoPerteneceProfesorAsync(int grupoId, string profesorId)
        {
            try
            {
				var pertenece = await _db.Grupos
					.AnyAsync(g => g.Id == grupoId && g.ProfesorId == profesorId);
                return Resultado<bool>.Exitoso(pertenece);
            }
            catch (Exception e)
            {
                return Resultado<bool>.Falla(new Error("Error.Unexpected", "Ha ocurrido un error. Error: "+ e.Message));
            }
        }
        public async Task<Resultado<IEnumerable<Medalla>>> GetMedallasDelProfesorPorGrupoAsync(int grupoId)
        {
            try
            {
                var grupo = await _db.Grupos
                    .Where(g => g.Id == grupoId)
                    .Include(g => g.Profesor)
                        .ThenInclude(p => p.Medallas)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();

                if (grupo == null || grupo.Profesor == null)
                    return Resultado<IEnumerable<Medalla>>.Falla(Error.NotFound);

                var medallas = grupo.Profesor.Medallas ?? new List<Medalla>();
                return Resultado<IEnumerable<Medalla>>.Exitoso(medallas);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Medalla>>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }
        public async Task<Resultado<Grupo>> GetGrupoConPerfilesYRecompensasAsync(string grupoId)
        {
            try
            {
                var grupo = await _db.Grupos
            .Where(g => g.Id.ToString() == grupoId)
            .Include(g => g.Alumnos)
                .ThenInclude(pe => pe.Estudiante) 
            .Include(g => g.Alumnos)
                .ThenInclude(pe => pe.InventarioRecompensas)
                    .ThenInclude(per => per.Recompensa) 
            .AsSplitQuery()
            .FirstOrDefaultAsync();

                if (grupo == null)
                {
                    return Resultado<Grupo>.Falla(new Error("Error.NotFound", "Grupo no encontrado."));
                }

                return Resultado<Grupo>.Exitoso(grupo);
            }
            catch (Exception ex)
            {
                return Resultado<Grupo>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }
    }

}
