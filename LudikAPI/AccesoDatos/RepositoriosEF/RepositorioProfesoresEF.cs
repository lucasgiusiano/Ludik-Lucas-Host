using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioProfesoresEF : IRepositorioProfesores
    {
        private readonly ContextoDb _db;
        public RepositorioProfesoresEF(ContextoDb db)
        {
            _db = db;
        }
        public async Task<Resultado> AddAsync(Profesor profesorNuevo)
        {
            try
            {

                await Task.CompletedTask;
                return Resultado.Falla(new Error("NoImplementado", "Sin implementar"));
            }
            catch (Exception e)
            {
                return Resultado.Falla(Error.Unexpected);
            }
        }


        public async Task<Resultado<IEnumerable<Profesor>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<Profesor>> GetByIdAsync(int id)
        {
            try
            {
                Profesor profesor = await _db.Profesores
                    .Include(p => p.Medallas)
                    .Include(p => p.TablasEquivalencia)
                    .Include(p => p.Grupos)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(p => p.Id == id.ToString());

                if (profesor == null)
                {
                    return Resultado<Profesor>.Falla(new Error("Repositorio.Profesor.NoEncontrado", $"No se encontró un profesor con ID {id}."));
                }

                return Resultado<Profesor>.Exitoso(profesor);
            }
            catch (Exception ex)
            {
                return Resultado<Profesor>.Falla(new Error("Repositorio.Profesor.Inesperado", $"Error inesperado al obtener el profesor: {ex.Message}"));
            }
        }
        public async Task<Resultado<Profesor>> GetByStringIdAsync(string id)
        {
            try
            {
                Profesor profesor = await _db.Profesores
                    .Include(p => p.Medallas)
                    .Include(p => p.TablasEquivalencia)
                    .Include(p => p.Grupos)
                    .Include(p => p.RecompensasCreadas )
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (profesor == null)
                {
                    return Resultado<Profesor>.Falla(new Error("Error.NotFound", $"No se encontró un profesor con ID {id}."));
                }

                return Resultado<Profesor>.Exitoso(profesor);
            }
            catch (Exception ex)
            {
                return Resultado<Profesor>.Falla(new Error("Error.Unexpected", $"Error inesperado al obtener el profesor: {ex.Message}"));
            }
        }

        public async Task<Resultado<Profesor>> ObtenerRecompensasPorProfesorIdAsync(string profesorId)
        {
            try
            {
                Profesor profesor = await _db.Profesores
                    .Include(p => p.RecompensasCreadas)
                    .ThenInclude(rp => rp.Recompensa)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(p => p.Id == profesorId);

                return Resultado<Profesor>.Exitoso(profesor);
            }
            catch (Exception ex)
            {
                return Resultado<Profesor>.Falla(new Error("Error.Unexpected", $"Error inesperado al obtener el profesor: {ex.Message}"));
            }
        }



        public async Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado> RemoveAsync(Profesor unObjeto)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado> UpdateAsync(Profesor unObjeto)
        {
            if (unObjeto == null)
                return Resultado.Falla(new Error("Error.Validation", "El objeto a actualizar no puede ser nulo."));

            try
            {
                _db.Profesores.Update(unObjeto);
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
        public async Task<Resultado<bool>> PerteneceGrupoAsync(string profesorId, int perfilId)
        {
            try
            {
                bool pertenece = await _db.Grupos
                    .AnyAsync(g => g.ProfesorId == profesorId && g.Alumnos.Any(a => a.Id == perfilId));
                return Resultado<bool>.Exitoso(pertenece);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<bool>> PoseeMedallaAsync(string profesorId, int medallaId)
        {
            try
            {
                bool posee = await _db.Profesores
                    .Where(p => p.Id == profesorId)
                    .SelectMany(p => p.Medallas)
                    .AnyAsync(m => m.Id == medallaId);
                return Resultado<bool>.Exitoso(posee);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }
        public async Task<bool> EsRecompensaDeAsync(string profesorId, int recompensaId)
        {
            try
            {
                return await _db.Profesores
                                .Where(p => p.Id == profesorId)
                                .SelectMany(p => p.RecompensasCreadas)
                                .AnyAsync(pr => pr.RecompensaId == recompensaId);
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

    }
}
