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
    public class RepositorioPerfilEstudianteGrupoEF : IRepositorioPerfilEstudianteGrupo
    {
        private readonly ContextoDb _db;
        public RepositorioPerfilEstudianteGrupoEF(ContextoDb db)
        {
            _db = db;
        }
        public async Task<Resultado<List<PerfilEstudiante>>> ObtenerPorGrupoIdAsync(int grupoId)
        {
            try
            {
                var perfiles = await _db.PerfilesEstudiantes
                    .Include(p => p.BarraProgreso)
                    .Include(p => p.Estudiante)
                    .Include(p => p.MedallasObtenidas)
                        .ThenInclude(pm => pm.Medalla)
                    .Include(p => p.Grupo)
                        .ThenInclude(g => g.TablaEquivalencia)
                            .ThenInclude(te => te.Equivalencias)
                                .ThenInclude(eq => eq.MedallasNecesarias)
                    .Include(p => p.HistorialRendimientoPeriodos)
                        .ThenInclude(rp => rp.RendimientoMedallas)
                            .ThenInclude(rpm => rpm.Medalla)
                    .Where(p => p.GrupoId == grupoId)
                    .AsSplitQuery()
                    .ToListAsync();

				if (perfiles == null || !perfiles.Any())
					return Resultado<List<PerfilEstudiante>>.Falla(
						new Error("Error.Validation",$"No se encontraron perfiles para el grupo con Id {grupoId}."));

				return Resultado<List<PerfilEstudiante>>.Exitoso(perfiles);
			}
			catch (DbUpdateException dbEx)
			{
				var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
				return Resultado<List<PerfilEstudiante>>.Falla(
					new Error("Error.Unexpected",$"Error al consultar la BD: {detalle}"));
			}
			catch (Exception ex)
			{
				return Resultado<List<PerfilEstudiante>>.Falla(
					new Error("Error.Unexpected", $"Error inesperado: {ex.Message}"));
			}
		}

		public async Task<Resultado> AddAsync(PerfilEstudiante unObjeto)
		{
			if (unObjeto == null)
				return Resultado.Falla(new Error("Error.Validation", "El perfil de estudiante no puede ser nulo."));

			try
			{
				await _db.PerfilesEstudiantes.AddAsync(unObjeto);
				await _db.SaveChangesAsync();
				return Resultado.Exitoso();
			}
			catch (DbUpdateException dbEx)
			{
				var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
				return Resultado.Falla(new Error("Error.Unexpected", $"Error al agregar el perfil: {detalle}"));
			}
			catch (Exception ex)
			{
				return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
			}
		}

		public Task<Resultado<IEnumerable<PerfilEstudiante>>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

        public async Task<Resultado<PerfilEstudiante>> GetByIdAsync(int id)
        {
            try
            {
                var perfil = await _db.PerfilesEstudiantes
                    .Include(p => p.InventarioRecompensas)
                        .ThenInclude(pe => pe.Recompensa)
                    .Include(p => p.Grupo)
                    .Include(p => p.MedallasObtenidas)
                        .ThenInclude(pm => pm.Medalla)
					.Include(p => p.Grupo) //No eliminar estos includes, solucionan el problema que se daba en la seleccion de la meta del estudiante
						.ThenInclude(g => g.TablaEquivalencia) //No eliminar estos includes, solucionan el problema que se daba en la seleccion de la meta del estudiante
							.ThenInclude(te => te.Equivalencias) //No eliminar estos includes, solucionan el problema que se daba en la seleccion de la meta del estudiante
					.Include(p => p.Estudiante)
                        .ThenInclude(e => e.EstPotenciador)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(p => p.Id == id);

				if (perfil == null)
				{
					return Resultado<PerfilEstudiante>.Falla(
						new Error("Error.NotFound", $"No se encontró el perfil de estudiante con Id: {id}."));
				}

				return Resultado<PerfilEstudiante>.Exitoso(perfil);
			}
			catch (Exception ex)
			{
				return Resultado<PerfilEstudiante>.Falla(
					new Error("Error.Unexpected", $"Error inesperado: {ex.Message}"));
			}
		}

		public Task<Resultado> RemoveAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Resultado> RemoveAsync(PerfilEstudiante unObjeto)
		{
			throw new NotImplementedException();
		}

		public async Task<Resultado> UpdateAsync(PerfilEstudiante unObjeto)
		{
			if (unObjeto == null)
				return Resultado.Falla(new Error("Error.Validation", "El objeto a actualizar no puede ser nulo."));


			try
			{
				_db.PerfilesEstudiantes.Update(unObjeto);
				await _db.SaveChangesAsync();
				return Resultado.Exitoso();
			}
			catch (DbUpdateConcurrencyException)
			{
				return Resultado.Falla(Error.Conflict);
			}
			catch (DbUpdateException dbEx)
			{
				var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
				return Resultado.Falla(new Error("Error.Unexpected", $"Error al actualizar en la BD: {detalle}"));
			}
			catch (Exception ex)
			{
				return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
			}
		}

		public List<Medalla> verMedallasAlumno(int idAlumno, int idGrupo)
		{
			throw new NotImplementedException();
		}

        public async Task<Resultado<PerfilEstudiante>> GetPerfilEstudianteAsync(string estudianteId, int grupoId)
        {
            try
            {
                var perfil = await _db.PerfilesEstudiantes
                    .Include(p => p.Estudiante)
                        .ThenInclude(e => e.NombreCompleto)
                    .Include(p => p.MedallasObtenidas)
                        .ThenInclude(pm => pm.Medalla)
                    .Include(p => p.BarraProgreso)
                    .Include(p => p.Estudiante)
                        .ThenInclude(e => e.EstPotenciador)
                            .ThenInclude(pe => pe.Potenciador)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(p => p.EstudianteId == estudianteId && p.GrupoId == grupoId);

                if (perfil == null)
                    return Resultado<PerfilEstudiante>.Falla(
                        new Error("Error.NotFound", $"No se encontró el perfil del estudiante '{estudianteId}' en el grupo {grupoId}."));

                return Resultado<PerfilEstudiante>.Exitoso(perfil);
            }
            catch (Exception ex)
            {
                return Resultado<PerfilEstudiante>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<RecompensaPersonalizacionAvatar>>> ObtenerItemsAvatarAdquiridosAsync(int idPerfilEstudiante)
		{
            try
            {
                var perfilExiste = await _db.PerfilesEstudiantes.AnyAsync(p => p.Id == idPerfilEstudiante);
                if (!perfilExiste)
                    return Resultado<IEnumerable<RecompensaPersonalizacionAvatar>>.Falla(Error.NotFound);
                

                var itemsDeAvatar = await _db.PerfilesEstudiantes
                    .Where(p => p.Id == idPerfilEstudiante)
                    .SelectMany(p => p.InventarioRecompensas) 
                    .Select(per => per.Recompensa)         
                    .OfType<RecompensaPersonalizacionAvatar>()         
                    .Include(pa => pa.AtributoDesbloqueable) 
                    .ToListAsync();

                return Resultado<IEnumerable<RecompensaPersonalizacionAvatar>>.Exitoso(itemsDeAvatar);
            }
            catch (Exception e)
            {
 
                return Resultado<IEnumerable<RecompensaPersonalizacionAvatar>>.Falla(new Error("Error.Unexpected", e.Message));
            }
        }
		public async Task<Resultado> SaveCambiosAsync()
		{
			try
			{
				await _db.SaveChangesAsync();
				return Resultado.Exitoso();
			}
			catch (DbUpdateException dbEx)
			{
				var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
				return Resultado.Falla(new Error("Error.Unexpected", $"Error al guardar los cambios: {detalle}"));
			}
			catch (Exception ex)
			{
				return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
			}
		}

        public async Task<Resultado<PerfilEstudiante>> GetParaAsignacionMedallaAsync(int id)
        {
            try
            {
                var perfil = await _db.PerfilesEstudiantes
                            .Include(p => p.Estudiante)
                                .ThenInclude(e => e.EstPotenciador)
                                    .ThenInclude(ep => ep.Potenciador)
                                    .AsSplitQuery()
                            .FirstOrDefaultAsync(p => p.Id == id);


                if (perfil == null)
                {
                    return Resultado<PerfilEstudiante>.Falla(Error.NotFound);
                }
                return Resultado<PerfilEstudiante>.Exitoso(perfil);
            }
            catch (Exception ex)
            {
                return Resultado<PerfilEstudiante>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<int>>> GetIdsPorEstudianteAsync(string estudianteId)
        {
            var ids = await _db.PerfilesEstudiantes
                .Where(p => p.EstudianteId == estudianteId)
                .Select(p => p.Id)
                .ToListAsync();
            return Resultado<IEnumerable<int>>.Exitoso(ids);
        }

        public async Task<Resultado<List<PerfilEstudiante>>> GetPerfilesPorEstudianteAsync(string estudianteId)
        {
            try
            {
                // Cargamos los perfiles y solo el inventario, que es lo que se va a modificar
                var perfiles = await _db.PerfilesEstudiantes
                    .Where(p => p.EstudianteId == estudianteId)
                    .Include(p => p.InventarioRecompensas) 
                    .Include(p => p.Estudiante)
                        .ThenInclude(e => e.EstPotenciador)
                    .AsSplitQuery()
                    .ToListAsync();

                return Resultado<List<PerfilEstudiante>>.Exitoso(perfiles);
            }
            catch (Exception ex)
            {
                return Resultado<List<PerfilEstudiante>>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Recompensa>>> ObtenerRecompensasInventarioAsync(int idPerfilEstudiante)
        {
            try
            {
                var recompensas = await _db.PerfilEstudianteRecompensas
                    .Where(per => per.PerfilEstudianteId == idPerfilEstudiante)
                    .Select(per => per.Recompensa) 
                    .ToListAsync();                

                return Resultado<IEnumerable<Recompensa>>.Exitoso(recompensas);
            }
            catch (Exception e)
            {
                return Resultado<IEnumerable<Recompensa>>.Falla(new Error("Error.Unexpected", e.Message));
            }
        }
        
    }
}
