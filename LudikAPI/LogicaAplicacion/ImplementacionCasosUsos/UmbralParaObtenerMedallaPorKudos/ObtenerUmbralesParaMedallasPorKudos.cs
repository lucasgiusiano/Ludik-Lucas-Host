using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaAplicacion.DTOsMappers.UmbralParaMedallaPorKudosMappers;
using LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.UmbralParaObtenerMedallaPorKudos;

public class ObtenerUmbralesParaMedallasPorKudos: IObtenerUmbralesParaMedallasPorKudos
{
    private readonly IRepositorioUmbralesParaMedallasPorKudos _repositorioUmbrales;

    public ObtenerUmbralesParaMedallasPorKudos(IRepositorioUmbralesParaMedallasPorKudos repositorioUmbrales)
    {
        _repositorioUmbrales = repositorioUmbrales;
    }

    public async Task<Resultado<IEnumerable<UmbralParaMedallaDto>>> EjecutarAsync(int grupoId)
    {
        var resultadoUmbrales = await _repositorioUmbrales.GetAllByGrupoIdAsync(grupoId);

        if (resultadoUmbrales.EsFallo)
            return Resultado<IEnumerable<UmbralParaMedallaDto>>.Falla(resultadoUmbrales.Errores);
        
        var umbrales = resultadoUmbrales.Valor;
        var umbralesDto = umbrales.Select(umbral => UmbralParaMedallaMapper.toDto(umbral));

        return Resultado<IEnumerable<UmbralParaMedallaDto>>.Exitoso(umbralesDto);
    }
}