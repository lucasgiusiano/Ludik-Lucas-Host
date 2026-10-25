using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.UmbralParaObtenerMedallaPorKudos;

public class ActualizarUmbralParaMedallaPorKudos: IActualizarUmbralParaMedallaPorKudos
{
    private readonly IRepositorioUmbralesParaMedallasPorKudos _repositorioUmbrales;
    private readonly IUnitOfWork _unitOfWork;

    public ActualizarUmbralParaMedallaPorKudos(IRepositorioUmbralesParaMedallasPorKudos repositorioUmbrales, 
        IUnitOfWork unitOfWork)
    {
        _repositorioUmbrales = repositorioUmbrales;
        _unitOfWork = unitOfWork;
    }

    public async Task<Resultado> EjecutarAsync(UmbralParaMedallaDto dto, string profesorId)
    {
        var resultadoObtener = await _repositorioUmbrales.GetByIdAsync(dto.Id);
        if (resultadoObtener.EsFallo)
            return resultadoObtener;
        
        var umbralAActualizar = resultadoObtener.Valor;

        if (umbralAActualizar.Grupo.ProfesorId != profesorId)
            return Resultado.Falla(Error.Forbidden);


        actualizarValoresUmbral(umbralAActualizar, dto);
        umbralAActualizar.CantidadKudos = dto.CantidadKudos;

        var resultadoValidacion = umbralAActualizar.esValido();
        if (resultadoValidacion.EsFallo)
            return resultadoValidacion;
        
        await _unitOfWork.SaveChangesAsync();

        return Resultado.Exitoso();
    }

    private void actualizarValoresUmbral(UmbralParaMedallaPorKudos umbralMedalla ,UmbralParaMedallaDto dto)
    {
        umbralMedalla.MedallaId = dto.MedallaId;
        umbralMedalla.TipoKudoId = dto.TipoKudoId;
        umbralMedalla.CantidadKudos = dto.CantidadKudos;
    }
}