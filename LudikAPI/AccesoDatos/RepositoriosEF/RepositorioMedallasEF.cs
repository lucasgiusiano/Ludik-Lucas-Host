using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioMedallasEF : IRepositorioMedallas
    {
        private readonly ContextoDb _db;
        public RepositorioMedallasEF(ContextoDb db)
        {
            _db = db;
        }
        public async Task<Resultado> AddAsync(Medalla unaMedalla)
        {
            if (unaMedalla == null)
                return Resultado.Falla(new Error("Error.Validation", "La medalla no puede ser nula."));
            

            try
            {

                await _db.Medallas.AddAsync(unaMedalla);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {

                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;

                return Resultado.Falla(new Error("Error.Unexpected", $"Error al guardar la medalla en la BD: {detalle}")); 
            }
            catch (Exception e)
            {

                return Resultado.Falla(new Error("Error.Unexpected", e.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Medalla>>> GetAllAsync()
        {
            try
            {
                var medallas = await _db.Medallas.ToListAsync();
                return Resultado<IEnumerable<Medalla>>.Exitoso(medallas);
            }
            catch (Exception e)
            {

                return Resultado<IEnumerable<Medalla>>.Falla(Error.Unexpected);
            }

        }

        public async Task<Resultado<Medalla>> GetByIdAsync(int id)
        {
            try
            {
                var medalla = await _db.Medallas.FindAsync(id);
                if (medalla == null)
                    return Resultado<Medalla>.Falla(Error.NotFound); 
                
                return Resultado<Medalla>.Exitoso(medalla); 
            }
            catch (Exception e)
            {

                return Resultado<Medalla>.Falla(Error.Unexpected); 
            }

        }


        public async Task<Resultado> RemoveAsync(int id)
        {
            try
            {
                var medalla = await _db.Medallas.FindAsync(id);

                if (medalla == null)
                    return Resultado.Falla(Error.NotFound);
                

                _db.Medallas.Remove(medalla);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;

                return Resultado.Falla(new Error("Error.Unexpected", $"Error al eliminar la medalla: {detalle}"));
            }
            catch (Exception e)
            {

                return Resultado.Falla(Error.Unexpected);
            }

        }

        public async Task<Resultado> RemoveAsync(Medalla unaMedalla)
        {
            if (unaMedalla == null)
                return Resultado.Falla(new Error("Error.Validation", "La medalla a eliminar no puede ser nula."));

            try
            {

                _db.Medallas.Remove(unaMedalla);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso(); 
            }
            catch (DbUpdateConcurrencyException dbEx) 
            {
                return Resultado.Falla(Error.Conflict);
            }
            catch (DbUpdateException dbEx) 
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;

                return Resultado.Falla(new Error("Error.Unexpected", $"Error al eliminar la medalla: {detalle}"));
            }
            catch (Exception e)
            {

                return Resultado.Falla(Error.Unexpected); 
            }

        }

        public async Task<Resultado> UpdateAsync(Medalla unaMedalla)
        {
            if (unaMedalla == null)
                return Resultado.Falla(new Error("Error.Validation", "La medalla a actualizar no puede ser nula."));
            

            try
            {

                _db.Entry(unaMedalla).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {

                return Resultado.Falla(new Error(Error.Conflict.Codigo, $"Error de concurrencia al actualizar la medalla: {dbEx.Message}")); 
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;

                return Resultado.Falla(new Error("Error.Unexpected", $"Error al actualizar la medalla en la BD: {detalle}"));
            }
            catch (Exception e)
            {
                return Resultado.Falla(Error.Unexpected);
            }

        }

        public async Task<List<Medalla>> ObtenerMedallasAsignablesMutuamenteAsync(int idGrupo)
        {
            throw new NotImplementedException("Este método aún no está implementado en el repositorio de Medallas.");
        }

        public async Task<Resultado<IEnumerable<Medalla>>> FindByIdsAsync(List<int> ids)
        {
            try
            {
                var resultado = await _db.Medallas
                    .Where(m => ids.Contains(m.Id))
                    .ToListAsync();
                return Resultado<IEnumerable<Medalla>>.Exitoso(resultado);
            }
            catch (Exception e)
            {
                return Resultado<IEnumerable<Medalla>>.Falla(new Error("Error.Uknown", e.Message));
            }
            
            
        }

        public async Task<Resultado<IEnumerable<Medalla>>> GetByProfesorAsync(string profesorId)
        {
            try
            {
                var list = await _db.Medallas
                    .Where(m => m.ProfesorId == profesorId)
                    .ToListAsync();
                return Resultado<IEnumerable<Medalla>>.Exitoso(list);
            }
            catch (Exception e)
            {
                return Resultado<IEnumerable<Medalla>>.Falla(new Error("Error.Uknown", e.Message));
            }
        }
    }
}
