using InterfacesRepositorio;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.InterfacesCasosUsos.Recompensa;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Recompensa;

public class AsignarRecompensaTiendas: IAsignarRecompensaTiendas
{
    private readonly IRepositorioRecompensas _repositorioRecompensas;
    private readonly IRepositorioGrupos _repositorioGrupos;
    private readonly IRepositorioProfesores _repositorioProfesores;
    private readonly IUnitOfWork _unitOfWork;

    public AsignarRecompensaTiendas(
        IRepositorioRecompensas repositorioRecompensas,
        IRepositorioGrupos repositorioGrupos,
        IRepositorioProfesores repositorioProfesores,
        IUnitOfWork unitOfWork)
    {
        _repositorioRecompensas = repositorioRecompensas;
        _repositorioGrupos = repositorioGrupos;
        _repositorioProfesores = repositorioProfesores;
        _unitOfWork = unitOfWork;
    }

    public async Task<Resultado> EjecutarAsync(RecompensaYGruposDto recompensaYGrupos, string profesorId)
    {

        var resultadoProfesor = await _repositorioProfesores.ObtenerRecompensasPorProfesorIdAsync(profesorId);
        if (resultadoProfesor.EsFallo)
            return Resultado.Falla(resultadoProfesor.Errores);

        Profesor profesor = resultadoProfesor.Valor;
        if(!profesor.TieneRecompensa(recompensaYGrupos.RecompensaId))
            return Resultado.Falla(new Error("Error.Validation", "La recompensa que desea asignar no te pertenece."));

        var resultadoRecompensa = await _repositorioRecompensas.GetByIdAsync(recompensaYGrupos.RecompensaId);
        if (resultadoRecompensa.EsFallo)
            return Resultado.Falla(resultadoRecompensa.Errores);
        
        var recompensa = resultadoRecompensa.Valor;

        var resultadoGrupos = await _repositorioGrupos.ObtenerGruposPorIdsYProfesor(recompensaYGrupos.GruposIds, profesorId);
        if (resultadoGrupos.EsFallo)
            return Resultado.Falla(resultadoGrupos.Errores); 
        
        var grupos = resultadoGrupos.Valor;

        if (grupos.Count != recompensaYGrupos.GruposIds.Distinct().Count())
            return Resultado.Falla(new Error("Error.Validation", "Uno o más grupos no existen o no pertenecen al profesor."));
        

        var erroresDeAsignacion = new List<Error>();
        foreach (var grupo in grupos)
        {
            if (grupo.Tienda == null)
            {
                erroresDeAsignacion.Add(new Error("Error.Conflict", $"El grupo '{grupo.Nombre}' no tiene una tienda asociada."));
                continue; // Saltar al siguiente grupo
            }

            var resultadoAsignacion = grupo.Tienda.AgregarRecompensa(recompensa);

            if (resultadoAsignacion.EsFallo)
                erroresDeAsignacion.AddRange(resultadoAsignacion.Errores);
            
        }


        if (erroresDeAsignacion.Any())
            return Resultado.Falla(erroresDeAsignacion);
        

        await _unitOfWork.SaveChangesAsync();

        return Resultado.Exitoso();
    }
}