using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs;
using LogicaAplicacion.DTOsMappers.SolicitudPerfilMedallaMappers;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Entidades=LogicaNegocio.Entidades;

namespace LogicaAplicacion.ImplementacionCasosUsos.SolicitudPerfilMedalla
{
    public class AltaSolicitudPerfilMedalla : IAltaSolicitudPerfilMedalla
    {
        private readonly IRepositorioSolicitudPerfilMedalla _repoSolicitudesPerfilMedalla;
        private readonly IRepositorioPerfilEstudianteGrupo _repoPerfilEstudianteGrupo;
        private readonly IRepositorioMedallas _repoMedallas;
        private readonly List<IObserver<Entidades.SolicitudPerfilMedalla>> _observers;

        public AltaSolicitudPerfilMedalla(IRepositorioSolicitudPerfilMedalla repoSolicitudesPerfilMedalla,IRepositorioPerfilEstudianteGrupo repositorioPerfilEstudianteGrupo,IRepositorioMedallas repositorioMedallas, IEnumerable<IObserver<Entidades.SolicitudPerfilMedalla>> observers)
        {
            _repoSolicitudesPerfilMedalla = repoSolicitudesPerfilMedalla;
            _repoPerfilEstudianteGrupo = repositorioPerfilEstudianteGrupo;
            _repoMedallas = repositorioMedallas;
            _observers = observers.ToList();

        }
        public async Task<Resultado> EjecutarAsync(AltaSolicitudPerfilMedallaDto dto, string estudianteLogueado)
        {
            var perfilResultado = await _repoPerfilEstudianteGrupo.GetByIdAsync(dto.PerfilEstudianteId);
            if (perfilResultado.EsFallo)
                return Resultado.Falla(new Error("Error.NotFound",
                    $"No existe perfil con Id {dto.PerfilEstudianteId}."));

            var perfil = perfilResultado.Valor!;
            var grupoId = perfil.GrupoId;


            if (perfil.EstudianteId != estudianteLogueado)
                return Resultado.Falla(new Error("Error.Forbidden",
                    "No puedes hacer una solicitud para un perfil que no es tuyo."));

            var medallaResultado = await _repoMedallas.GetByIdAsync(dto.MedallaId);
            if (medallaResultado.EsFallo)
                return Resultado.Falla(new Error("Error.NotFound",
                    $"No existe medalla con Id {dto.MedallaId}."));

            if (perfil.GrupoId != grupoId)
                return Resultado.Falla(new Error("Error.Forbidden",
                    "El perfil de estudiante no pertenece a ese grupo."));

            var solicitud = AltaSolicitudPerfilMedallaMapper.Map(dto,grupoId);
            var validacion = solicitud.esValido();
            if (validacion.EsFallo)
                return validacion;

            var historialResultado = await _repoSolicitudesPerfilMedalla
                .GetByPerfilAsync(dto.PerfilEstudianteId);
            if (historialResultado.EsFallo)
                return historialResultado;  

            var ultima = historialResultado.Valor!
                .OrderByDescending(s => s.Fecha)
                .FirstOrDefault();

            if (ultima != null)
            {
                var diasPasados = (DateTime.UtcNow - ultima.Fecha).TotalDays;
                if (diasPasados < 14)
                {
                    return Resultado.Falla(new Error(
                        "Error.Validation",
                        $"Ya hiciste una solicitud hace {Math.Floor(diasPasados)} días; debes esperar {14 - Math.Floor(diasPasados)} días más."));
                }
            }

            var addResultado = await _repoSolicitudesPerfilMedalla.AddAsync(solicitud);
            if (addResultado.EsFallo)
                return addResultado;

            var grupo = perfil.Grupo; 


            grupo.AgregarSolicitudPerfilMedalla(solicitud);


            return Resultado.Exitoso();
        }
    }
}
