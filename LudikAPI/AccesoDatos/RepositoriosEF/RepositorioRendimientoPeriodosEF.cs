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
    public class RepositorioRendimientoPeriodosEF : IRepositorioRendimientoPeriodos
    {
        private readonly ContextoDb _db;
        public RepositorioRendimientoPeriodosEF(ContextoDb db)
        {
            _db = db;
        }

        public async Task<Resultado> AddAsync(RendimientoPeriodo unObjeto)
        {
            if (unObjeto == null)
                return Resultado.Falla(new Error("Error.Validation", "El rendimiento periodo no puede ser nulo."));

            try
            {
                await _db.RendimientosPeriodos.AddAsync(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al guardar el rendimiento: {detalle}"));
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public void almacenarLogrosPrevios(int idGrupo)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<IEnumerable<RendimientoPeriodo>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<RendimientoPeriodo>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(RendimientoPeriodo unObjeto)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> UpdateAsync(RendimientoPeriodo unObjeto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExisteEnRendimientoPeriodoAsync(int medallaId)
        {
            try
            {
                return await _db.RendimientosPeriodos
                                .AsNoTracking()
                                .SelectMany(rp => rp.RendimientoMedallas)
                                .AnyAsync(rpm => rpm.MedallaId == medallaId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
