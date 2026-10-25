using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.DTOsMappers.MedallaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Medalla;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Medallas;

public class ObtenerTodasLasMedallas : IObtenerTodasLasMedallas
{
    private readonly IRepositorioMedallas _repositorioMedallas;

    public ObtenerTodasLasMedallas(IRepositorioMedallas repositorioMedallas)
    {
        _repositorioMedallas = repositorioMedallas;
    }
    /// <summary>
    /// Obtiene todas las medallas.
    /// </summary>
    /// <returns>
    /// Un Resultado exitoso con la lista de MedallaDto, o un Resultado de fallo 
    /// si ocurrió un error en la capa de acceso a datos.
    /// </returns>
    public async Task<Resultado<IEnumerable<MedallaDto>>> EjecutarAsync(string profesorId)
    {
        var resultadoRepo = await _repositorioMedallas.GetByProfesorAsync(profesorId);

        if (resultadoRepo.EsFallo)
            return Resultado<IEnumerable<MedallaDto>>.Falla(resultadoRepo.Errores);
        
        IEnumerable<Medalla> medallas = resultadoRepo.Valor;

        IEnumerable<MedallaDto> medallasDtos = medallas.Select( medalla =>  MedallaMapper.toDto(medalla));

        return Resultado<IEnumerable<MedallaDto>>.Exitoso(medallasDtos);
    }

}