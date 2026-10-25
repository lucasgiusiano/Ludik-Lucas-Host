using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioPerfilEstudianteRecompensaEF : IRepositorioPerfilEstudianteRecompensa
    {
        private readonly ContextoDb _db;
        public RepositorioPerfilEstudianteRecompensaEF(ContextoDb db)
        {
            _db = db;
        }

        public async Task<Resultado> AddAsync(PerfilEstudianteRecompensa unObjeto)
        {
            try
            {
                await _db.PerfilEstudianteRecompensas.AddAsync(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<PerfilEstudianteRecompensa>>> GetAllAsync()
        {
            try
            {
                var lista = await _db.PerfilEstudianteRecompensas
                    .Include(x => x.PerfilEstudiante)
                    .Include(x => x.Recompensa)
                    .AsSplitQuery()
                    .ToListAsync();

                return Resultado<IEnumerable<PerfilEstudianteRecompensa>>.Exitoso(lista);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<PerfilEstudianteRecompensa>>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado<PerfilEstudianteRecompensa>> GetByIdAsync(int id)
        {
            try
            {
                var item = await _db.PerfilEstudianteRecompensas
                    .Include(x => x.PerfilEstudiante)
                    .Include(x => x.Recompensa)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (item == null)
                    return Resultado<PerfilEstudianteRecompensa>.Falla(new Error("Error.NotFound", "No se encontró la recompensa del perfil."));

                return Resultado<PerfilEstudianteRecompensa>.Exitoso(item);
            }
            catch (Exception ex)
            {
                return Resultado<PerfilEstudianteRecompensa>.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> UpdateAsync(PerfilEstudianteRecompensa unObjeto)
        {
            try
            {
                _db.PerfilEstudianteRecompensas.Update(unObjeto);
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
                var item = await _db.PerfilEstudianteRecompensas.FindAsync(id);
                if (item == null)
                    return Resultado.Falla(new Error("Error.NotFound", "No se encontró la recompensa del perfil."));

                _db.PerfilEstudianteRecompensas.Remove(item);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public async Task<Resultado> RemoveAsync(PerfilEstudianteRecompensa unObjeto)
        {
            try
            {
                _db.PerfilEstudianteRecompensas.Remove(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Database", ex.Message));
            }
        }
        public async Task<bool> FueCanjeadaAsync(int recompensaId)
        {
            try
            {
                return await _db.PerfilEstudianteRecompensas
                                .AnyAsync(p => p.RecompensaId == recompensaId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<Resultado<PerfilEstudianteRecompensa>> GetByPerfilAndRecompensaIdAsync(int perfilId, int recompensaId)
        {
            try
            {
                var item = await _db.PerfilEstudianteRecompensas
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.PerfilEstudianteId == perfilId && x.RecompensaId == recompensaId);

                if (item == null)
                {
                    return Resultado<PerfilEstudianteRecompensa>.Falla(new Error("Error.NotFound",
                        $"La relación PerfilEstudianteRecompensa para Perfil ID {perfilId} y Recompensa ID {recompensaId} no existe."));
                }

                return Resultado<PerfilEstudianteRecompensa>.Exitoso(item);
            }
            catch (Exception ex)
            {
                return Resultado<PerfilEstudianteRecompensa>.Falla(
                    new Error("Error.Unexpected", $"Error al buscar relación en la BD: {ex.Message}"));
            }
        }
    }
}
