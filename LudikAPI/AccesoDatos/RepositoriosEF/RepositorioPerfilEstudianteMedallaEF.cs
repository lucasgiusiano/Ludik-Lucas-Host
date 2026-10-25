using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioPerfilEstudianteMedallaEF : IRepositorioPerfilEstudianteMedalla
    {
        private readonly ContextoDb _db;
        public RepositorioPerfilEstudianteMedallaEF(ContextoDb db)
        {
            _db = db;
        }
        public async Task<Resultado> AddAsync(PerfilEstudianteMedalla unObjeto)
        {
            if (unObjeto == null)
                return Resultado.Falla(new Error("Error.Validation", "La entidad no puede ser nula."));
            try
            {
                _db.PerfilEstudianteMedallas.Add(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al insertar en BD: {detalle}"));
            }
            catch (System.Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> RemoveAsync(PerfilEstudianteMedalla unObjeto)
        {
            if (unObjeto == null)
                return Resultado.Falla(new Error("Error.Validation", "La entidad no puede ser nula."));
            try
            {
                _db.PerfilEstudianteMedallas.Remove(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al eliminar en BD: {detalle}"));
            }
            catch (System.Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> RemoveByIdAsync(int id)
        {
            try
            {
                var entidad = await _db.PerfilEstudianteMedallas.FindAsync(id);
                if (entidad == null)
                    return Resultado.Falla(new Error("Error.NotFound", $"No se encontró PerfilEstudianteMedalla con Id={id}."));
                _db.PerfilEstudianteMedallas.Remove(entidad);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al eliminar en BD: {detalle}"));
            }
            catch (System.Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> UpdateAsync(PerfilEstudianteMedalla unObjeto)
        {
            if (unObjeto == null)
                return Resultado.Falla(new Error("Error.Validation", "La entidad no puede ser nula."));
            try
            {
                _db.PerfilEstudianteMedallas.Update(unObjeto);
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
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al actualizar en BD: {detalle}"));
            }
            catch (System.Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<PerfilEstudianteMedalla>> GetByIdAsync(int id)
        {
            try
            {
                var entidad = await _db.PerfilEstudianteMedallas
                                       .Include(pm => pm.PerfilEstudiante)
                                       .Include(pm => pm.Medalla)
                                       .FirstOrDefaultAsync(pm => pm.Id == id);
                if (entidad == null)
                    return Resultado<PerfilEstudianteMedalla>.Falla(new Error("Error.NotFound", $"No encontrado Id={id}."));
                return Resultado<PerfilEstudianteMedalla>.Exitoso(entidad);
            }
            catch (System.Exception ex)
            {
                return Resultado<PerfilEstudianteMedalla>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<PerfilEstudianteMedalla>> GetByPerfilYMedallaAsync(int perfilId, int medallaId)
        {
            try
            {
                var entidad = await _db.PerfilEstudianteMedallas
                                       .Where(pm => pm.PerfilEstudianteId == perfilId && pm.MedallaId == medallaId)
                                       .FirstOrDefaultAsync();
                if (entidad == null)
                    return Resultado<PerfilEstudianteMedalla>.Falla(new Error("Error.NotFound", "No existe asignación previa."));
                return Resultado<PerfilEstudianteMedalla>.Exitoso(entidad);
            }
            catch (System.Exception ex)
            {
                return Resultado<PerfilEstudianteMedalla>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<PerfilEstudianteMedalla>>> GetAllAsync()
        {
            try
            {
                var lista = await _db.PerfilEstudianteMedallas
                                     .Include(pm => pm.PerfilEstudiante)
                                     .Include(pm => pm.Medalla)
                                     .ToListAsync();
                return Resultado<IEnumerable<PerfilEstudianteMedalla>>.Exitoso(lista);
            }
            catch (System.Exception ex)
            {
                return Resultado<IEnumerable<PerfilEstudianteMedalla>>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<PerfilEstudianteMedalla>> GetByIdConPerfilYMedallaAsync(int id)
        {
            try
            {
                var entidad = await _db.PerfilEstudianteMedallas
                    .Include(pm => pm.PerfilEstudiante)
                        .ThenInclude(pe => pe.MedallasObtenidas)
                    .Include(pm => pm.PerfilEstudiante)
                        .ThenInclude(pe => pe.Estudiante)
                            .ThenInclude(est => est.Perfiles)
                    .Include(pm => pm.Medalla)
                    .FirstOrDefaultAsync(pm => pm.Id == id);

                if (entidad == null)
                    return Resultado<PerfilEstudianteMedalla>
                        .Falla(new Error("Error.NotFound", $"No se encontró la asignación con Id={id}."));

                return Resultado<PerfilEstudianteMedalla>.Exitoso(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<PerfilEstudianteMedalla>
                    .Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<int>> ContarMedallasPorEstudianteAsync(string estudianteId)
        {
            try
            {

                int totalMedallas = await _db.PerfilEstudianteMedallas
                    .CountAsync(pem => pem.PerfilEstudiante.EstudianteId == estudianteId);

                return Resultado<int>.Exitoso(totalMedallas);
            }
            catch (Exception ex)
            {

                return Resultado<int>.Falla(
                    new Error("Error.Unexpected", $"Ocurrió un error inesperado al contar las medallas del estudiante: {ex.Message}"));
            }
        }
        public async Task<bool> ExistePerfilEstudianteMedallaAsync(int medallaId)
        {
            try
            {
                return await _db.PerfilEstudianteMedallas
                                .AsNoTracking()
                                .AnyAsync(pm => pm.MedallaId == medallaId);
            }
            catch (Exception ex)
            {
                // Opcional: loggear ex
                // Log.Error(ex, "Error comprobando medalla en PerfilEstudianteMedalla");
                return false;
            }
        }
    }
}
