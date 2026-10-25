using LogicaAplicacion.DTOs.KudoDTOs;
using LogicaAplicacion.DTOsMappers.KudoMappers;
using LogicaAplicacion.InterfacesCasosUsos.Kudo;
using LogicaAplicacion.Servicios;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Kudo;

public class ObtenerKudos: IObtenerKudos
{
    private readonly IRepositorioTiposKudo _repositorioTiposKudo;

    public ObtenerKudos(
        IRepositorioTiposKudo repositorioTiposKudo)
    {
        _repositorioTiposKudo = repositorioTiposKudo;
    }
    public async Task<Resultado<IEnumerable<TipoKudoDto>>> EjecutarAsync()
    {
        var resultadoRepo = await _repositorioTiposKudo.GetAllAsync();

        if (resultadoRepo.EsFallo)
            return Resultado<IEnumerable<TipoKudoDto>>.Falla(resultadoRepo.Errores);
        
        var kudosDto = resultadoRepo.Valor.Select(kudo => KudoMapper.toDto(kudo)).ToList();

        return Resultado<IEnumerable<TipoKudoDto>>.Exitoso(kudosDto);
    }
}