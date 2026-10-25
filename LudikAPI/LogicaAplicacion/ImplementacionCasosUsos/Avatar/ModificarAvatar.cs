using InterfacesRepositorio;
using LogicaAplicacion.DTOs.AvatarDTOs;
using LogicaAplicacion.DTOs.ImagenDto;
using LogicaAplicacion.DTOsMappers;
using LogicaAplicacion.InterfacesCasosUsos.Avatar;
using LogicaAplicacion.InterfacesCasosUsos.Imagenes;
using LogicaNegocio.ConstantesAplicacion;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Entidades = LogicaNegocio.Entidades;

namespace LogicaAplicacion.ImplementacionCasosUsos.Avatar;

public class ModificarAvatar: IModificarAvatar
{
    private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfilesEstudiantes;
    private readonly IRepositorioAvatares _repositorioAvatares;
    private readonly IRepositorioAtributosAvatar _repositorioAtributosAvatar;
    private readonly IServicioGestionImagen _servicioGestionImagen;

    public ModificarAvatar(
        IRepositorioPerfilEstudianteGrupo repositorioPerfilesEstudiantes,
        IRepositorioAvatares repositorioAvatares,
        IRepositorioAtributosAvatar repositorioAtributosAvatar,
        IServicioGestionImagen servicioGestionImagen)
    {
        _repositorioPerfilesEstudiantes = repositorioPerfilesEstudiantes;
        _repositorioAvatares = repositorioAvatares;
        _repositorioAtributosAvatar = repositorioAtributosAvatar;
        _servicioGestionImagen = servicioGestionImagen;
    }

    public async Task<Resultado> EjecutarAsync(int idPerfilEstudiante, string idUsuarioAutenticado, ActualizarAvatarDto avatarDto, Stream streamImagen)
    {
        var resultadoPerfil = await _repositorioPerfilesEstudiantes.GetByIdAsync(idPerfilEstudiante);
        if (resultadoPerfil == null || resultadoPerfil.EsFallo)
            return Resultado.Falla(Error.NotFound);

        Entidades.PerfilEstudiante perfilEstudiante = resultadoPerfil.Valor;

        if (perfilEstudiante.EstudianteId != idUsuarioAutenticado)
            return Resultado.Falla(Error.Forbidden);

        Resultado resultadoValidacionItems = ValidarItemsAvatar(perfilEstudiante, avatarDto);
        if (resultadoValidacionItems.EsFallo)
            return resultadoValidacionItems;


        var resultadoAtributosAAsignar = await _repositorioAtributosAvatar.GetByIdsAsync(avatarDto.AtributosIds);

        if (resultadoAtributosAAsignar.EsFallo)
            return resultadoAtributosAAsignar;

        IEnumerable<Entidades.AtributoAvatar> atributosAAsignar = resultadoAtributosAAsignar.Valor;

        if (atributosAAsignar.Count() != avatarDto.AtributosIds.Count)
            return Resultado.Falla(new Error("Error.NotFound", "Uno o más atributos seleccionados no fueron encontrados."));
        

        var resultadoAvatar = await _repositorioAvatares.GetByPerfilIdAsync(idPerfilEstudiante);
        if (resultadoAvatar.EsFallo)
            return Resultado.Falla(Error.NotFound);

        Entidades.Avatar avatarAActualizar = resultadoAvatar.Valor;

        ActualizarAtributos(avatarAActualizar,avatarDto, atributosAAsignar);
      

        Resultado resultadoActualizarAvatar = await _repositorioAvatares.UpdateAsync(avatarAActualizar);
        if (resultadoActualizarAvatar.EsFallo)
            return resultadoActualizarAvatar;
        SubirImagenDto subirImagenDto = new SubirImagenDto
        {
            ImagenStream = streamImagen,
            IdUsuarioAutenticado = idUsuarioAutenticado,
            Proposito = Constantes.PropositoImagen.PerfilEstudiante,
            EntidadAsociadaId = idPerfilEstudiante
        };

        Resultado resultadoSubirImagen = await _servicioGestionImagen.SubirImagenAsync(subirImagenDto);
        if (resultadoSubirImagen.EsFallo)
        {
            //TODO: Considerar una estrategia de compensación aquí si la actualización del avatar fue exitosa pero la subida de imagen falló.
            return resultadoSubirImagen;
        }

        return Resultado.Exitoso();

    }

    private void ActualizarAtributos(Entidades.Avatar avatarAActualizar, ActualizarAvatarDto avatarDto, IEnumerable<Entidades.AtributoAvatar> atributosAAsignar)
    {
        // Actualizar propiedades generales
        avatarAActualizar.ColorFondo = avatarDto.ColorFondo;
        avatarAActualizar.Voltear = avatarDto.Voltear;
        avatarAActualizar.Rotacion = avatarDto.Rotacion;
        avatarAActualizar.Zoom = avatarDto.Zoom;

        avatarAActualizar.AtributosSeleccionados.Clear();
        foreach (var atributo in atributosAAsignar)
        {
            avatarAActualizar.AtributosSeleccionados.Add(atributo);
        }
    }

    private Resultado ValidarItemsAvatar(Entidades.PerfilEstudiante perfilEstudiante, ActualizarAvatarDto avatarDto)
    {
      
        var itemsDesbloqueadosIds = perfilEstudiante.Inventario
            .OfType<Entidades.RecompensaPersonalizacionAvatar>()
            .Select(pa => pa.AtributoAvatarId)
            .ToHashSet(); //HashSet para búsquedas O(1)

        foreach (var idAtributoSeleccionado in avatarDto.AtributosIds)
        {
            if (!itemsDesbloqueadosIds.Contains(idAtributoSeleccionado))
                return Resultado.Falla(new Error("Error.Forbidden", $"No posees este atributo de avatar. Atributo ID: {idAtributoSeleccionado}."));
            
        }
        return Resultado.Exitoso();

    }

}