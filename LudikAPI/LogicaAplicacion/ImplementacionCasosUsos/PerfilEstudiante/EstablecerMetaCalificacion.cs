using InterfacesRepositorio;
using LogicaAplicacion.DTOs.BarraProgresoDTOs;
using LogicaAplicacion.DTOs.EstablecerMetaCalificacionDto;
using LogicaAplicacion.InterfacesCasosUsos.PerfilEstudiante;
using Entidades = LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.PerfilEstudiante;

public class EstablecerMetaCalificacion: IEstablecerMetaCalificacion
{
    private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfilesEstudiantes;

    public EstablecerMetaCalificacion(IRepositorioPerfilEstudianteGrupo repositorioPerfilesEstudiantes)
    {
        _repositorioPerfilesEstudiantes = repositorioPerfilesEstudiantes;
    }

	public async Task<Resultado> EjecutarAsync(EstablecerMetaCalificacionDto dto, string estudianteId)
	{
		var resultadoPerfil = await _repositorioPerfilesEstudiantes.GetByIdAsync(dto.PerfilEstudianteId);
		if (resultadoPerfil == null || resultadoPerfil.EsFallo)
			return Resultado.Falla(Error.NotFound);

		Entidades.PerfilEstudiante perfilEstudiante = resultadoPerfil.Valor;
		if (perfilEstudiante.EstudianteId != estudianteId)
			return Resultado<BarraProgresoDto>.Falla(Error.Forbidden);

		var resultado = perfilEstudiante.EstablecerMetaDeCalificacion(dto.MetaCalificacion);

		if (resultado.EsExitoso)
		{
			// Guardar los cambios
			var resultadoUpdate = await _repositorioPerfilesEstudiantes.UpdateAsync(perfilEstudiante);
			if (resultadoUpdate.EsFallo)
				return resultadoUpdate;
		}

		return resultado;
	}

}