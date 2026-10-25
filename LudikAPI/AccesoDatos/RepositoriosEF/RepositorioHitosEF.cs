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
    public class RepositorioHitosEF : IRepositorioHitos
    {
        private readonly ContextoDb _db;
        public RepositorioHitosEF(ContextoDb db)
        {
            _db = db;
        }

        public async Task<Resultado> AddAsync(Hito unObjeto)
        {
            try
            {
                await _db.Hitos.AddAsync(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Hito>>> GetAllAsync()
        {
            try
            {
                var hitos = await _db.Hitos
                    .Include(h => h.Recompensa)
                    .ToListAsync();
                return Resultado<IEnumerable<Hito>>.Exitoso(hitos);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Hito>>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<Hito>> GetByIdAsync(int id)
        {
            try
            {
                var hito = await _db.Hitos
                    .Include(h => h.Recompensa)
                    .FirstOrDefaultAsync(h => h.Id == id);
                if (hito == null)
                    return Resultado<Hito>.Falla(new Error("Error.NotFound", "Hito no encontrado."));
                return Resultado<Hito>.Exitoso(hito);
            }
            catch (Exception ex)
            {
                return Resultado<Hito>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> RemoveAsync(int id)
        {
            try
            {
                var hito = await _db.Hitos.FindAsync(id);
                if (hito == null)
                    return Resultado.Falla(new Error("Error.NotFound", "Hito no encontrado."));
                _db.Hitos.Remove(hito);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> RemoveAsync(Hito unObjeto)
        {
            try
            {
                _db.Hitos.Remove(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> UpdateAsync(Hito unObjeto)
        {
            try
            {
                _db.Hitos.Update(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }
    }
}
