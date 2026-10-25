using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.Eventos;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;
using MediatR;
using Entidades = LogicaNegocio.Entidades;

namespace LogicaAplicacion.ImplementacionCasosUsos.SolicitudPerfilMedalla
{
    public class AceptarSolicitudPerfilMedalla : IAceptarSolicitudPerfilMedalla
    {
        private readonly IRepositorioSolicitudPerfilMedalla _repositorio;
        private readonly IRepositorioPerfilEstudianteMedalla _repositorioPerfilEstudianteMedalla;
        private readonly IRepositorioProfesores _repositorioProfesores;
        private readonly IMediator _mediator;

        public AceptarSolicitudPerfilMedalla(IRepositorioSolicitudPerfilMedalla repositorio,IRepositorioPerfilEstudianteMedalla repositorioPerfilEstudianteMedalla,IMediator mediator,IRepositorioProfesores repositorioProfesores)
        {
            _repositorio = repositorio;
            _repositorioPerfilEstudianteMedalla = repositorioPerfilEstudianteMedalla;
            _mediator = mediator;
            _repositorioProfesores = repositorioProfesores;

        }

        public async Task<Resultado> EjecutarAsync(int idSolicitudPerfil,string profesorId)
        {
            var solRes = await _repositorio.GetByIdAsync(idSolicitudPerfil);
            if (solRes.EsFallo || solRes.Valor == null)
                return Resultado.Falla(new Error("Error.NotFound", "La solicitud no existe."));

            var solicitud = solRes.Valor;
            if (solicitud.Estado != EstadoSolicitud.Pendiente)
                return Resultado.Falla(new Error("Error.Validation", "La solicitud ya fue procesada."));
            var resultadoPosee = await _repositorioProfesores.PoseeMedallaAsync(profesorId,solicitud.MedallaId);
            if(resultadoPosee.Valor == false)
            {
                return Resultado.Falla(new Error("Error.Validation", "Esa medalla pertenece a otro docente"));
            }

            solicitud.Estado = EstadoSolicitud.Aceptada;
            var updSol = await _repositorio.UpdateAsync(solicitud);
            if (updSol.EsFallo)
                return updSol;

            var asignMin = new PerfilEstudianteMedalla
            {
                PerfilEstudianteId = solicitud.PerfilEstudianteId,
                MedallaId = solicitud.MedallaId
            };
            var addRes = await _repositorioPerfilEstudianteMedalla.AddAsync(asignMin);
            if (addRes.EsFallo)
                return Resultado.Falla(new Error("Error.Validation", "No se pudo asignar la medalla."));

            var recRes = await _repositorioPerfilEstudianteMedalla
                .GetByIdConPerfilYMedallaAsync(asignMin.Id);
            if (recRes.EsFallo || recRes.Valor == null)
                return Resultado.Falla(new Error("Error.Unexpected", "No se pudo recargar la asignación de medalla."));

            var asignacionCompleta = recRes.Valor;

            var evento = new AsignacionMedallaCompletadaEvento(
                asignacionCompleta.PerfilEstudianteId,
                asignacionCompleta.MedallaId,
                asignacionCompleta.PerfilEstudiante.Estudiante);
            await _mediator.Publish(evento);

            return Resultado.Exitoso();
        }
    }
}
