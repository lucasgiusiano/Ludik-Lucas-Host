using InterfacesRepositorio;
using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaAplicacion.DTOsMappers.TablaEquivalenciaMappers;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.TablaEquivalencia;

public class ObtenerTablasEquivalenciaDelProfesor: IObtenerTablasEquivalenciaDelProfesor
{
    private readonly IRepositorioTablasEquivalencia _repositorioTablasEquivalencia;
    private readonly IRepositorioProfesores _repositorioProfesores;

    public ObtenerTablasEquivalenciaDelProfesor(IRepositorioTablasEquivalencia repositorioTablasEquivalencia, IRepositorioProfesores repositorioProfesores)
    {
        _repositorioTablasEquivalencia = repositorioTablasEquivalencia;
        _repositorioProfesores = repositorioProfesores;
    }

    public async Task<Resultado<IEnumerable<TablaEquivalenciaDto>>> EjecutarAsync(string profesorId)
    {
        var profesorResultado = await _repositorioProfesores.GetByStringIdAsync(profesorId);
        if (profesorResultado.EsFallo)
            return Resultado<IEnumerable<TablaEquivalenciaDto>>.Falla(Error.NotFound);
        

        var resultadoRepo = await _repositorioTablasEquivalencia.GetByProfesorIdAsync(profesorId);
        if (resultadoRepo.EsFallo)
            return Resultado<IEnumerable<TablaEquivalenciaDto>>.Falla(resultadoRepo.Errores);

        var tablasDto = resultadoRepo.Valor.Select( tabla => TablaEquivalenciaMapper.toDto(tabla));

        return Resultado<IEnumerable<TablaEquivalenciaDto>>.Exitoso(tablasDto);
    }
}