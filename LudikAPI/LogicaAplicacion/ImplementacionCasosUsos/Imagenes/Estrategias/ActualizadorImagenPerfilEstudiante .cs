using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.Imagenes;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Imagenes.Estrategias;

public class ActualizadorImagenPerfilEstudiante: IActualizadorRutaImagen
{
    private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfiles;

    public ActualizadorImagenPerfilEstudiante(IRepositorioPerfilEstudianteGrupo repositorioPerfiles)
    {
        _repositorioPerfiles = repositorioPerfiles;
    }
    public async Task<Resultado> ActualizarRutasAsync(string idUsuarioAutenticado, int? entidadAsociadaId, Dictionary<string, string> rutas)
    {
        if (!entidadAsociadaId.HasValue)
        {
            return Resultado.Falla(new Error("Error.Validation", "Se requiere el ID del perfil de estudiante."));
        }

        var resultadoPerfil = await _repositorioPerfiles.GetByIdAsync(entidadAsociadaId.Value);
        if (resultadoPerfil.EsFallo) return Resultado.Falla(Error.NotFound);

        var perfil = resultadoPerfil.Valor;
        if (perfil.EstudianteId != idUsuarioAutenticado)
        {
            return Resultado.Falla(Error.Forbidden);
        }

        perfil.NombreImagenCompleta = rutas["completa"];
        perfil.NombreImagenMiniatura = rutas["mini"];
        await _repositorioPerfiles.UpdateAsync(perfil);

        return Resultado.Exitoso();
    }

}