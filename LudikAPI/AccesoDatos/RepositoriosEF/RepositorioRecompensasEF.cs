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
    public class RepositorioRecompensasEF : IRepositorioRecompensas
    {
        private readonly ContextoDb _db;
        public RepositorioRecompensasEF(ContextoDb db)
        {
            _db = db;
        }

        public async Task<Resultado> AddAsync(Recompensa unObjeto)
        {
            try
            {
                await _db.Recompensas.AddAsync(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public Task<Resultado<IEnumerable<Recompensa>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<Recompensa>> GetByIdAsync(int id)
        {
            try
            {
                var recompensa = await _db.Recompensas
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (recompensa == null)
                    return Resultado<Recompensa>.Falla(
                        new Error("Error.NotFound", $"No se encontró la recompensa con Id {id}."));

                return Resultado<Recompensa>.Exitoso(recompensa);
            }
            catch (Exception ex)
            {
                return Resultado<Recompensa>.Falla(
                    new Error("Error.Unexpected", ex.Message));
            }
        }


        public async Task<Resultado> RemoveAsync(int id)
        {
            try
            {
                var entidad = await _db.Recompensas
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (entidad == null)
                    return Resultado.Falla(new Error("Error.NotFound", $"No se encontró la recompensa con Id {id}."));

                _db.Recompensas.Remove(entidad);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> RemoveAsync(Recompensa unObjeto)
        {
            try
            {

                var existe = await _db.Recompensas.AnyAsync(r => r.Id == unObjeto.Id);
                if (!existe)
                    return Resultado.Falla(new Error("Error.NotFound", $"No se encontró la recompensa con Id {unObjeto.Id}."));

                _db.Recompensas.Remove(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> UpdateAsync(Recompensa unObjeto)
        {
            try
            {
                var existe = await _db.Recompensas.AnyAsync(r => r.Id == unObjeto.Id);
                if (!existe)
                    return Resultado.Falla(new Error("Error.NotFound", $"No se encontró la recompensa con Id {unObjeto.Id}."));

                _db.Recompensas.Update(unObjeto);
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
