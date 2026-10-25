using InterfacesRepositorio;
using LogicaAplicacion.Servicios;
using LogicaNegocio.InterfacesRepositorios;
using Microsoft.Extensions.Logging;

namespace LogicaAplicacion.ImplementacionServicios;

public class ServicioDeReinicioSemanal : IServicioDeReinicioSemanal
{
    private readonly IRepositorioEstudiantes _repositorioEstudiantes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ServicioDeReinicioSemanal> _logger;
    private const int KudosSemanales = 3;

    public ServicioDeReinicioSemanal(
        IRepositorioEstudiantes repositorioEstudiantes,
        IUnitOfWork unitOfWork,
        ILogger<ServicioDeReinicioSemanal> logger)
    {
        _repositorioEstudiantes = repositorioEstudiantes;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task ReiniciarKudosDeEstudiantesAsync()
    {
        _logger.LogInformation("Iniciando tarea de reinicio semanal de kudos...");

        var resultado = await _repositorioEstudiantes.GetAllAsync();
        if (resultado.EsFallo)
        {
            _logger.LogError("No se pudo obtener la lista de estudiantes para el reinicio de kudos.");
            return;
        }

        var estudiantes = resultado.Valor;
        int estudiantesActualizados = 0;

        foreach (var estudiante in estudiantes)
        {
            foreach (var perfil in estudiante.Perfiles)
            {
                perfil.KudosDisponiblesParaOtorgar = KudosSemanales;
                estudiantesActualizados++;
            }
        }

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation($"Reinicio de kudos completado. {estudiantesActualizados} perfiles de estudiantes actualizados a {KudosSemanales} kudos.");
    }
}