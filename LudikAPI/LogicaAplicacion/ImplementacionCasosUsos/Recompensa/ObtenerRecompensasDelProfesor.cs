using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.DTOsMappers.RecompensaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Recompensa;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Recompensa;

public class ObtenerRecompensasDelProfesor: IObtenerRecompensasDelProfesor
{
    private readonly IRepositorioRecompensasDeProfesores _repositorioRecompensas;
    public ObtenerRecompensasDelProfesor(IRepositorioRecompensasDeProfesores repositorioRecompensas)
    {
        _repositorioRecompensas = repositorioRecompensas;
    }
    public async Task<Resultado<IEnumerable<RecompensaDto>>> EjecutarAsync(string profesorId)
    {
        var resultado = await _repositorioRecompensas.GetByProfesorIdAsync(profesorId);
        if (resultado.EsFallo)
            return Resultado<IEnumerable<RecompensaDto>>.Falla(resultado.Errores);

        var recompensasDto = resultado.Valor.Select(pr => RecompensaSimpleMapper.ToDto(pr.Recompensa));
        return Resultado<IEnumerable<RecompensaDto>>.Exitoso(recompensasDto);
    }
}