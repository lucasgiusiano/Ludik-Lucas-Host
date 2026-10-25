using LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.UmbralParaObtenerMedallaPorKudos;

public class EliminarUmbralParaMedallaPorKudos: IEliminarUmbralParaMedallaPorKudos
{
    private readonly IRepositorioUmbralesParaMedallasPorKudos _repositorioUmbrales;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarUmbralParaMedallaPorKudos(
        IRepositorioUmbralesParaMedallasPorKudos repositorioUmbrales,
        IUnitOfWork unitOfWork)
    {
        _repositorioUmbrales = repositorioUmbrales;
        _unitOfWork = unitOfWork;
    }

    public async Task<Resultado> EjecutarAsync(int umbralId, string idProfesor)
    {

        var resultadoObtener = await _repositorioUmbrales.GetByIdAsync(umbralId);
        if (resultadoObtener.EsFallo)
            return resultadoObtener;
        

        var umbral = resultadoObtener.Valor;

        if (umbral.Grupo.ProfesorId != idProfesor)
            return Resultado.Falla(Error.Forbidden);
        
        await _repositorioUmbrales.RemoveAsync(umbral);

        await _unitOfWork.SaveChangesAsync();

        return Resultado.Exitoso();
    }
}