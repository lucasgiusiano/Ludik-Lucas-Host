using InterfacesRepositorio;
using LogicaAplicacion.DTOs.AtributoAvatarDTOs;
using LogicaAplicacion.DTOs.AvatarDTOs;
using LogicaAplicacion.DTOsMappers;
using LogicaAplicacion.InterfacesCasosUsos.Avatar;
using LogicaAplicacion.Servicios;
using Entidades= LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Avatar;

public class ObtenerAtributosAvatarDisponiblesParaPerfil : IObtenerAtributosAvatarDisponiblesParaPerfil
{
    private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfilesEstudiantes;
    private readonly IGeneradorUrlsParaColeccionesImagenes _generadorUrlsParaColecciones;

    public ObtenerAtributosAvatarDisponiblesParaPerfil(
        IRepositorioPerfilEstudianteGrupo repositorioPerfilesEstudiantes,
        IGeneradorUrlsParaColeccionesImagenes generadorUrls)
    {
        _repositorioPerfilesEstudiantes = repositorioPerfilesEstudiantes;
        _generadorUrlsParaColecciones = generadorUrls;
    }
    /// <summary>
    /// Retorna los atributos de avatar disponibles para un perfil de estudiante.
    /// <summary>
    public async Task<Resultado<IEnumerable<AtributoAvatarDto>>> EjecutarAsync(int idPerfilEstudiante, string idUsuarioAutenticado)
    {
        var resultadoPerfil = await _repositorioPerfilesEstudiantes.GetByIdAsync(idPerfilEstudiante);
        var resultadoAtributosAvatar = await _repositorioPerfilesEstudiantes.ObtenerItemsAvatarAdquiridosAsync(idPerfilEstudiante);

        if (resultadoPerfil.EsFallo)
            return Resultado<IEnumerable<AtributoAvatarDto>>.Falla(resultadoPerfil.Errores);
        if (resultadoAtributosAvatar.EsFallo)
            return Resultado<IEnumerable<AtributoAvatarDto>>.Falla(resultadoAtributosAvatar.Errores);

        var perfilEstudiante = resultadoPerfil.Valor;
        IEnumerable<Entidades.RecompensaPersonalizacionAvatar> itemsDisponiblesParaPersonazarAvatar = resultadoAtributosAvatar.Valor;

        if (perfilEstudiante.EstudianteId != idUsuarioAutenticado)
            return Resultado<IEnumerable<AtributoAvatarDto>>.Falla(Error.Forbidden);
        

        var atributosDto = itemsDisponiblesParaPersonazarAvatar
            .Select(item => AtributoAvatarMapper.toDto(item.AtributoDesbloqueable)).ToList();

        await _generadorUrlsParaColecciones.EjecutarProcesarUrlsAsync(atributosDto,
            (dto => dto.EnlaceImagen, (dto, url) => dto.EnlaceImagen = url)
            
        );

        return Resultado<IEnumerable<AtributoAvatarDto>>.Exitoso(atributosDto);

    }
}