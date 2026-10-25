using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioTablasClasificacionEF : IRepositorioTablasClasificacion
    {
        private readonly ContextoDb _db;
        public RepositorioTablasClasificacionEF(ContextoDb db)
        {
            _db = db;
        }

        public async Task<Resultado> AddAsync(TablaClasificacion tabla)
        {
            try
            {
                await _db.TablasClasificacion.AddAsync(tabla);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al guardar la tabla: {detalle}"));
            }
            
        }

        public async Task<Resultado<IEnumerable<TablaClasificacion>>> GetAllAsync()
        {
            try
            {
                var tablas = await _db.TablasClasificacion
                    .AsNoTracking()
                    .Include(tc => tc.MedallaAsociada)
                    .AsSplitQuery()
                    .ToListAsync();

                return Resultado<IEnumerable<TablaClasificacion>>.Exitoso(tablas);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<TablaClasificacion>>.Falla(
                    new Error("Error.Unexpected",
                        "Error al intentar obtener las tablas de clasificación del gurpo. Error: " + ex.Message)
                    );
            }
        }

        public async Task<Resultado<IEnumerable<TablaClasificacion>>> GetAllByAsync(int grupoId)
        {
            try
            {
                var tablas = await _db.TablasClasificacion
                    .Where(p => p.GrupoId == grupoId)
                    .AsNoTracking()
                    .Include(tc => tc.MedallaAsociada)
                    .AsSplitQuery()
                    .ToListAsync();

                return Resultado<IEnumerable<TablaClasificacion>>.Exitoso(tablas);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<TablaClasificacion>>.Falla(
                    new Error("Error.Unexpected", "Error al intentar obtener las tablas de clasificación del gurpo. Error: "+ex.Message)
                );
            }
        }

        public async Task<Resultado<TablaClasificacion>> GetByIdAsync(int id)
        {
            try
            {
                var tabla = await _db.TablasClasificacion
                    .AsNoTracking()
                    .Where(tc => tc.Id == id)

                    .Include(tc => tc.MedallaAsociada)

                    .Include(tc => tc.Grupo)
                        .ThenInclude(g => g.Alumnos)
                        .ThenInclude(pe => pe.MedallasObtenidas)

                    .Include(tc => tc.Grupo)
                        .ThenInclude(g => g.Alumnos)
                            .ThenInclude(pe => pe.Estudiante)
                                .ThenInclude(e => e.NombreCompleto)

                    .AsSplitQuery()
                    .FirstOrDefaultAsync();

                if (tabla == null)
                    return Resultado<TablaClasificacion>.Falla(
                        new Error("Error.NotFound", "Tabla de clasificación no encontrada.")
                    );

                return Resultado<TablaClasificacion>.Exitoso(tabla);
            }
            catch (Exception ex)
            {
                return Resultado<TablaClasificacion>.Falla(
                    new Error("Error.Unexpected", "Error al obtener la tablas de clasificación. Error :"+ ex.Message)
                );
            }
        }

        public async Task<Resultado> RemoveAsync(int id)
        {
            try
            {
                var tabla = await _db.TablasClasificacion.FindAsync(id);
                if (tabla == null)
                    return Resultado.Falla(new Error("NotFound", "Tabla de clasificación no encontrada."));

                _db.TablasClasificacion.Remove(tabla);
                await _db.SaveChangesAsync();

                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al eliminar la tabla: {detalle}"));
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public Task<Resultado> RemoveAsync(TablaClasificacion unObjeto)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> UpdateAsync(TablaClasificacion unObjeto)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> ExisteTablaClasificacionConMedallaAsync(int medallaId)
        {
            try
            {
                return await _db.TablasClasificacion
                                .AsNoTracking()
                                .AnyAsync(tc => tc.MedallaAsociadaId == medallaId);
            }
            catch (Exception ex)
            {
                // Opcional: loggear el error
                // Log.Error(ex, "Error comprobando existencia de medalla en tablas de clasificación");
                return false;
            }
        }
    }
}
