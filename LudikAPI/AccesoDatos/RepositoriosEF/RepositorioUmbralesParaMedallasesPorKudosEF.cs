using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccesoDatos.RepositoriosEF;

public class RepositorioUmbralesParaMedallasesPorKudosEF: IRepositorioUmbralesParaMedallasPorKudos
{
    private readonly ContextoDb _db;
    public RepositorioUmbralesParaMedallasesPorKudosEF(ContextoDb db)
    {
        _db = db;
    }

    public async Task<Resultado> AddAsync(UmbralParaMedallaPorKudos unObjeto)
    {
        try
        {
            await _db.UmbralesParaMedallasPorKudos.AddAsync(unObjeto);
            return Resultado.Exitoso();
        }
        catch (Exception e)
        {
            return Resultado.Falla(new Error("Error.Unexpected", e.Message));
        }
    }

    public Task<Resultado> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(UmbralParaMedallaPorKudos unObjeto)
    {
        try
        {
            if (unObjeto == null)
            {
                return Task.FromResult(Resultado.Falla(new Error("Error.Validation", "El umbral a eliminar no puede ser nulo.")));
            }
            _db.UmbralesParaMedallasPorKudos.Remove(unObjeto);
            return Task.FromResult(Resultado.Exitoso());
        }
        catch (Exception e)
        {
            return Task.FromResult(Resultado.Falla(new Error("Error.Unexpected", e.Message)));
        }
    }

    public Task<Resultado> UpdateAsync(UmbralParaMedallaPorKudos unObjeto)
    {
        throw new NotImplementedException();
    }

    public async Task<Resultado<UmbralParaMedallaPorKudos>> GetByIdAsync(int id)
    {
        try
        {
            var umbral = await _db.UmbralesParaMedallasPorKudos
                .Include(u => u.Grupo) 
                .FirstOrDefaultAsync(u => u.Id == id);

            return umbral != null
                ? Resultado<UmbralParaMedallaPorKudos>.Exitoso(umbral)
                : Resultado<UmbralParaMedallaPorKudos>.Falla(Error.NotFound);
        }
        catch (Exception e)
        {
            return Resultado<UmbralParaMedallaPorKudos>.Falla(new Error("Error.Unexpected", e.Message));
        }
    }

    public Task<Resultado<IEnumerable<UmbralParaMedallaPorKudos>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Resultado<IEnumerable<UmbralParaMedallaPorKudos>>> GetAllByGrupoIdAsync(int grupoId)
    {
        try
        {
            var umbrales = await _db.UmbralesParaMedallasPorKudos
                .AsNoTracking()
                .Include(u => u.Grupo)
                .Include(u => u.Medalla)     
                .Include(u => u.TipoKudo)    
                .Where(u => u.GrupoId == grupoId)
                .AsSplitQuery()
                .ToListAsync();


            return Resultado<IEnumerable<UmbralParaMedallaPorKudos>>.Exitoso(umbrales);
        }
        catch (Exception e)
        {
            return Resultado<IEnumerable<UmbralParaMedallaPorKudos>>.Falla(new Error("Error.Unexpected", e.Message));
        }
    }
    public async Task<bool> ExisteConfiguracionAsync(int grupoId, int tipoKudoId)
    {
        return await _db.UmbralesParaMedallasPorKudos
            .AnyAsync(u => u.GrupoId == grupoId && u.TipoKudoId == tipoKudoId);
    }
}