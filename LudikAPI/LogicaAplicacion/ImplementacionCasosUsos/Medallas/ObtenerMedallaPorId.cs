using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.InterfacesCasosUsos.Medalla;
using LogicaAplicacion.DTOsMappers.MedallaMappers;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Medallas;

public class ObtenerMedallaPorId : IObtenerMedallaPorId
{
	private readonly IRepositorioMedallas _repositorioMedallas;
	private readonly IRepositorioProfesores _repositorioProfesores;


    public ObtenerMedallaPorId(IRepositorioMedallas repositorioMedallas,IRepositorioProfesores repositorioProfesores)
	{
		_repositorioMedallas = repositorioMedallas;
		_repositorioProfesores = repositorioProfesores;
    }

    public async Task<Resultado<MedallaDto>> EjecutarAsync(int idMedalla, string profesorId)
    {
        var resultadoRepo = await _repositorioMedallas.GetByIdAsync(idMedalla);
        if (resultadoRepo.EsFallo)
            return Resultado<MedallaDto>.Falla(resultadoRepo.Errores);

        var medalla = resultadoRepo.Valor;

        var resultadoPosee = await _repositorioProfesores.PoseeMedallaAsync(profesorId, idMedalla);
        if (resultadoPosee.EsFallo)
            return Resultado<MedallaDto>.Falla(resultadoPosee.Errores);

        // <- aca faltaba la validación
        if (!resultadoPosee.Valor)
        {
            return Resultado<MedallaDto>.Falla(
                new Error("Error.Forbidden", "No tienes permiso para visualizar esta medalla.")
            );
        }

        var medallaDto = MedallaMapper.toDto(medalla);
        return Resultado<MedallaDto>.Exitoso(medallaDto);
    }
}