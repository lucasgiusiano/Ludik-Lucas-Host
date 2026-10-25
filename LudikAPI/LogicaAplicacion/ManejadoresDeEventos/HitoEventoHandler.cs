using InterfacesRepositorio;
using LogicaAplicacion.Eventos;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LogicaAplicacion.ManejadoresDeEventos;

public class HitoEventoHandler : INotificationHandler<AsignacionMedallaCompletadaEvento>
{
    private readonly IRepositorioHitos _repoHitos;
    private readonly IRepositorioPerfilEstudianteGrupo _repoPerfiles;
    private readonly ILogger<HitoEventoHandler> _logger;
    private readonly IRepositorioPerfilEstudianteRecompensa _repoRecompensas;
    private readonly IRepositorioPerfilEstudianteMedalla _repoPerfilEstudianteMedalla;
    private readonly IRepositorioEstudiantes _repositorioEstudiantes;
    private readonly IUnitOfWork _unitOfWork;

    public HitoEventoHandler(
        IRepositorioHitos repoHitos,
        IRepositorioPerfilEstudianteGrupo repoPerfiles,
        ILogger<HitoEventoHandler> logger,
        IRepositorioPerfilEstudianteRecompensa repoRecompensas,
        IUnitOfWork unitOfWork, IRepositorioPerfilEstudianteMedalla repositorioPerfilEstudianteMedalla, IRepositorioEstudiantes repositorioEstudiantes)
    {
        _repoHitos = repoHitos;
        _repoPerfiles = repoPerfiles;
        _logger = logger;
        _repoRecompensas = repoRecompensas;
        _unitOfWork = unitOfWork;
        _repoPerfilEstudianteMedalla = repositorioPerfilEstudianteMedalla;
        _repositorioEstudiantes = repositorioEstudiantes;

    }



    public async Task Handle(AsignacionMedallaCompletadaEvento notification, CancellationToken cancellationToken)
    {
        try
        {
            var estudiante = notification.Estudiante;
            var totalMedallas = await _repoPerfilEstudianteMedalla.ContarMedallasPorEstudianteAsync(estudiante.Id);
            int totalMedallasValor = totalMedallas.Valor;
            var estudianteConHitos = await _repositorioEstudiantes.GetByIdConHitosAsync(estudiante.Id);
            var estudianteConHitosValor = estudianteConHitos.Valor;
            var resultadoTodosHitos = await _repoHitos.GetAllAsync();

            if (resultadoTodosHitos.EsFallo)
            {
                _logger.LogError($"[HitoObserver] Error al leer hitos: {resultadoTodosHitos.EsFallo}");
                return;
            }

            var hitosPendientes = resultadoTodosHitos.Valor!
                .Where(h => h.Cumple(totalMedallasValor))
                .ToList();

            foreach (var hito in hitosPendientes)
            {
                // Obtener todos los perfiles de este estudiante
                var perfilesResultado = await _repoPerfiles
                    .GetPerfilesPorEstudianteAsync(notification.Estudiante.Id);
                if (perfilesResultado.EsFallo)
                {
                    _logger.LogError("No se pudieron obtener perfiles para EstudianteId {Id}",
                                     notification.Estudiante.Id);
                    continue;
                }

                // Activar y marcar para persistencia

                if (hito.Recompensa is Potenciador pot)
                {
                    estudianteConHitosValor.ActivarPotenciador(pot);
                    await _repositorioEstudiantes.UpdateAsync(estudianteConHitosValor);



                    // Marcar el hito como obtenido y guardar en el estudiante
                    estudianteConHitosValor.Hitos.Add(hito);
                    await _repositorioEstudiantes.UpdateAsync(estudianteConHitosValor);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "[HitoEventHandler] Excepción interna al procesar hitos.");
        }
    }

}

