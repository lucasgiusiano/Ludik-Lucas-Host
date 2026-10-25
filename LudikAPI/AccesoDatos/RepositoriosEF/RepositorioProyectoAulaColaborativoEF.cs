using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioProyectoAulaColaborativoEF : IRepositorioProyectoAulaColaborativo
    {
        private readonly ContextoDb _db;
        public RepositorioProyectoAulaColaborativoEF(ContextoDb db)
            => _db = db;

        public async Task<Resultado> AddAsync(ProyectoAulaColaborativo pac)
        {
            try
            {
                await _db.Set<ProyectoAulaColaborativo>().AddAsync(pac);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<ProyectoAulaColaborativo>>> GetAllAsync()
        {
            try
            {
                var list = await _db.Set<ProyectoAulaColaborativo>()
                                    .Include(p => p.RecompensaClase)
                                    .ToListAsync();
                return Resultado<IEnumerable<ProyectoAulaColaborativo>>.Exitoso(list);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<ProyectoAulaColaborativo>>.Falla(
                    new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<ProyectoAulaColaborativo>> GetByIdAsync(int id)
        {
            try
            {
                var pac = await _db.Set<ProyectoAulaColaborativo>()
                                   .Include(p => p.RecompensaClase)
                                   .FirstOrDefaultAsync(p => p.Id == id);
                if (pac == null)
                    return Resultado<ProyectoAulaColaborativo>.Falla(
                        new Error("Error.NotFound", $"No se encontró PAC con ID {id}."));
                return Resultado<ProyectoAulaColaborativo>.Exitoso(pac);
            }
            catch (Exception ex)
            {
                return Resultado<ProyectoAulaColaborativo>.Falla(
                    new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<List<ProyectoAulaColaborativo>>> GetByGrupoAsync(int grupoId)
        {
            try
            {
                var list = await _db.Set<ProyectoAulaColaborativo>()
                                    .Where(p => p.GrupoId == grupoId)
                                    .Include(p => p.RecompensaClase)
                                    .ToListAsync();
                return Resultado<List<ProyectoAulaColaborativo>>.Exitoso(list);
            }
            catch (Exception ex)
            {
                return Resultado<List<ProyectoAulaColaborativo>>.Falla(
                    new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> UpdateAsync(ProyectoAulaColaborativo pac)
        {
            try
            {
                _db.Set<ProyectoAulaColaborativo>().Update(pac);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> RemoveAsync(int id)
        {
            try
            {
                var pac = await _db.Set<ProyectoAulaColaborativo>().FindAsync(id);
                if (pac == null)
                    return Resultado.Falla(
                        new Error("Error.NotFound", $"No se encontró PAC con ID {id}."));
                _db.Set<ProyectoAulaColaborativo>().Remove(pac);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> RemoveAsync(ProyectoAulaColaborativo pac)
        {
            try
            {
                await RemoveAsync(pac.Id);
                return Resultado.Exitoso();
            }
            catch (Exception e)
            {
                return Resultado.Falla(new Error("Error.Unexpected", e.Message));
            }
            
        }
        

        
    }

}
