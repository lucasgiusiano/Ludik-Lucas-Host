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
    public class RepositorioTablasEquivalenciaEF : IRepositorioTablasEquivalencia
    {
        private readonly ContextoDb _db;
        public RepositorioTablasEquivalenciaEF(ContextoDb db)
        {
            _db = db;
        }

        public async Task<Resultado> AddAsync(TablaEquivalencia tabla)
        {
            try
            {
                await _db.TablasEquivalencia.AddAsync(tabla);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al guardar la tabla: {detalle}"));
            }
        }

        public Task<Resultado<IEnumerable<TablaEquivalencia>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<IEnumerable<TablaEquivalencia>>> GetByProfesorIdAsync(string profesorId)
        {
            try
            {
                var tablas = await _db.TablasEquivalencia
                    .Where(t => t.ProfesorId == profesorId)
                    .Include(t => t.Equivalencias)
                    .ThenInclude(e => e.MedallasNecesarias)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .ToListAsync();

                return Resultado<IEnumerable<TablaEquivalencia>>.Exitoso(tablas);
            }
            catch (Exception e)
            {
                return Resultado<IEnumerable<TablaEquivalencia>>.Falla(
                    new Error("Error.Unexpected", $"Ocurrió un error al consultar las tablas de equivalencia: {e.Message}")
                );
            }
        }


        public async Task<Resultado<TablaEquivalencia>> GetByIdAsync(int id)
        {
            
            try
            {
                var tablaEquivalencia = await _db.TablasEquivalencia
                    .Include(t => t.Equivalencias)
                    .ThenInclude(e => e.MedallasNecesarias)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tablaEquivalencia == null)
                    return Resultado<TablaEquivalencia>.Falla(Error.NotFound);

                return Resultado<TablaEquivalencia>.Exitoso(tablaEquivalencia);
            }
            catch (Exception e)
            {
                return Resultado<TablaEquivalencia>.Falla(new Error("Erro.Unexpected", e.Message));
            }
        }

        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(TablaEquivalencia unObjeto)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado> UpdateAsync(TablaEquivalencia unObjeto)
        {
            try
            {
                // EF Core rastreará automáticamente los cambios en la entidad "tabla equivalencia"
                // y su colección de "Equivalencias" porque la obtuvimos del mismo DbContext.
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al actualizar la tabla: {detalle}"));
            }
        }
        public async Task<bool> ExisteTablaEquivalenciaConMedallaAsync(int medallaId)
        {
            try
            {
                return await _db.TablasEquivalencia
                                .AsNoTracking()
                                .AnyAsync(t => t.Equivalencias
                                                 .Any(e => e.MedallasNecesarias
                                                               .Any(m => m.Id == medallaId)));
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
    }
}
