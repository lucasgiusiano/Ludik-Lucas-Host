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
    public class RepositorioTiendasEF : IRepositorioTiendas
    {
        private readonly ContextoDb _db;
        public RepositorioTiendasEF(ContextoDb db)
        {
            _db = db;
        }

        public Task<Resultado> AddAsync(Tienda unObjeto)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<IEnumerable<Tienda>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<Tienda>> GetByIdAsync(int id)
        {
            try
            {
                var tienda = await _db.Tiendas
                    .Include(t => t.Recompesas)            
                    .FirstOrDefaultAsync(t => t.Id == id);
                if (tienda == null)
                    return Resultado<Tienda>.Falla(new Error("Error.NotFound", $"No se encontró la tienda con Id {id}."));
                return Resultado<Tienda>.Exitoso(tienda);
            }
            catch (Exception ex)
            {
                return Resultado<Tienda>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }
        

        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(Tienda unObjeto)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado> UpdateAsync(Tienda tienda)
        {
            try
            {
                _db.Tiendas.Update(tienda);
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
