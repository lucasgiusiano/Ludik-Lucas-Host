using System.ComponentModel;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.KudoDTOs;
using LogicaAplicacion.DTOsMappers.KudoMappers;
using LogicaAplicacion.InterfacesCasosUsos.Kudo;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Entidades = LogicaNegocio.Entidades;

namespace LogicaAplicacion.ImplementacionCasosUsos.Kudo;

public class AsignarKudo: IAsignarKudo
{
    private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfil;
    private readonly IRepositorioEstudiantes _repositorioEstudiantes;
    private readonly IRepositorioTiposKudo _repositorioTiposKudo;
    private readonly IRepositorioUmbralesParaMedallasPorKudos _repositorioUmbralesParaMedallas;
    private readonly IUnitOfWork _unitOfWork;

    public AsignarKudo(IRepositorioPerfilEstudianteGrupo repositorioPerfil, 
        IRepositorioTiposKudo repositorioTipoKudo, 
        IRepositorioEstudiantes repositorioEstudiantes,
        IRepositorioUmbralesParaMedallasPorKudos repositorioUmbrales,
        IUnitOfWork unitOfWork)
    {
        _repositorioPerfil = repositorioPerfil;
        _repositorioEstudiantes = repositorioEstudiantes;
        _repositorioTiposKudo = repositorioTipoKudo;
        _repositorioUmbralesParaMedallas = repositorioUmbrales;
        _unitOfWork = unitOfWork;
    }


    /// <summary>
    /// Asynchronously assigns a kudo from one student to another and evaluates if the recipient qualifies for a medal.
    /// </summary>
    /// <remarks>This method involves multiple asynchronous operations, including retrieving student profiles
    /// and kudo types, and saving changes to the database. It ensures that the kudo assignment adheres to business
    /// rules and evaluates medal thresholds for the recipient.</remarks>
    /// <param name="idEstudianteEmisor">The identifier of the student who is assigning the kudo.</param>
    /// <param name="dto">The data transfer object containing details of the kudo assignment, including the recipient's profile ID and the
    /// kudo type.</param>
    /// <returns>A <see cref="Resultado"/> indicating the success or failure of the operation. If successful, the kudo is
    /// assigned and any applicable medals are evaluated.</returns>
    public async Task<Resultado> EjecutarAsync(string idEstudianteEmisor, AsignarKudoDto dto)
    {
        var resultadoEstudianteEmisor = await _repositorioEstudiantes.GetByStringIdAsync(idEstudianteEmisor);
        if (resultadoEstudianteEmisor.EsFallo)
            return Resultado.Falla(resultadoEstudianteEmisor.Errores);

        Estudiante estudianteEmisor = resultadoEstudianteEmisor.Valor;

        Resultado<Entidades.PerfilEstudiante> resultadoPerfilEstudianteEmisor= estudianteEmisor.ObtenerPerfilEstudiantePor(dto.IdPerfilEstudianteEmisor);

        if (resultadoPerfilEstudianteEmisor.EsFallo)
            return Resultado.Falla(resultadoPerfilEstudianteEmisor.Errores);
        
        Entidades.PerfilEstudiante perfilEstudianteEmisor = resultadoPerfilEstudianteEmisor.Valor;

        if (perfilEstudianteEmisor.Id == dto.IdPerfilEstudianteRecibe)
            return Resultado.Falla(Error.Forbidden);
        

        var resultadoPerfilReceptor = await _repositorioPerfil.GetByIdAsync(dto.IdPerfilEstudianteRecibe);
        if (resultadoPerfilReceptor.EsFallo)
            return Resultado.Falla(resultadoPerfilReceptor.Errores);
        

        Entidades.PerfilEstudiante perfilEstudianteReceptor = resultadoPerfilReceptor.Valor;

        var resultadoTipoKudo = await _repositorioTiposKudo.GetByIdAsync(dto.Kudo.Id);

        if (resultadoTipoKudo.EsFallo)
            return Resultado.Falla(resultadoTipoKudo.Errores);
        

        var resultadoOtorgar = perfilEstudianteEmisor.OtorgarKudo(resultadoTipoKudo.Valor, perfilEstudianteReceptor);
        if (resultadoOtorgar.EsFallo)
            return resultadoOtorgar;

        //En caso de que se haya otorgado un kudo, se evalúa si el perfil receptor cumple con algún umbral para obtener una medalla.
        var resultadoUmbrales = await _repositorioUmbralesParaMedallas.GetAllByGrupoIdAsync(perfilEstudianteReceptor.GrupoId);
        if (resultadoUmbrales.EsFallo)
            return Resultado.Falla(resultadoUmbrales.Errores);
        var umbrales = resultadoUmbrales.Valor;

        var resultadoRecepcion = perfilEstudianteReceptor.RecibirKudoYEvaluarMedalla(resultadoOtorgar.Valor, umbrales.FirstOrDefault(u => u.TipoKudoId == dto.Kudo.Id));
        if (resultadoRecepcion.EsFallo)
            return resultadoRecepcion;

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Resultado.Falla(new Error("Error.Unexpected", "Hubo un error: "+ ex.Message)); 
        }
        
        return Resultado.Exitoso();

    }

}