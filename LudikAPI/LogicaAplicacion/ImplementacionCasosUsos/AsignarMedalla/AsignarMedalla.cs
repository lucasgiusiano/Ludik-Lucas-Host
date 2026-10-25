using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.Eventos;
using LogicaAplicacion.InterfacesCasosUsos.AsignacionMedalla;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades = LogicaNegocio.Entidades;

namespace LogicaAplicacion.ImplementacionCasosUsos.AsignarMedalla
{
    public class AsignarMedalla : IAsignarMedalla
    {
        private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfilEstudiantes;
        private readonly IRepositorioMedallas _repositorioMedallas;
        private readonly IRepositorioProfesores _repositorioProfesores;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public AsignarMedalla(
            IRepositorioPerfilEstudianteGrupo repositorioPerfilEstudiante,
            IRepositorioMedallas repositorioMedalla,
            IRepositorioProfesores repositorioProfesor,
            IMediator mediator,
            IUnitOfWork unitOfWork)
        {
            _repositorioPerfilEstudiantes = repositorioPerfilEstudiante;
            _repositorioMedallas = repositorioMedalla;
            _repositorioProfesores = repositorioProfesor;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<Resultado> EjecutarAsync(string profesorId, int idPerfilEstudiante, int idMedalla,
            int cantidadAOtorgar)
        {
            if(cantidadAOtorgar < 1)
                return Resultado.Falla(new Error("Error.Validation", "La cantidad de medallas a otorgar debe ser mayor a 0."));

            var perteneceResultado = await _repositorioProfesores.PerteneceGrupoAsync(profesorId, idPerfilEstudiante);
            if (perteneceResultado.EsFallo || !perteneceResultado.Valor)
                return Resultado.Falla(Error.Forbidden);
            

            var poseeResultado = await _repositorioProfesores.PoseeMedallaAsync(profesorId, idMedalla);
            if (poseeResultado.EsFallo || !poseeResultado.Valor)
                return Resultado.Falla(Error.Forbidden);
            

            var medallaResultado = await _repositorioMedallas.GetByIdAsync(idMedalla);
            if (medallaResultado.EsFallo)
                return Resultado.Falla(Error.NotFound);
            

            var perfilResultado = await _repositorioPerfilEstudiantes.GetParaAsignacionMedallaAsync(idPerfilEstudiante);
            if (perfilResultado.EsFallo)
                return Resultado.Falla(Error.NotFound);
            

            var medalla = medallaResultado.Valor;
            var perfilEstudiante = perfilResultado.Valor;


            perfilEstudiante.RecibirMedallas(medalla, cantidadAOtorgar);

            await _unitOfWork.SaveChangesAsync();

            var evento = new AsignacionMedallaCompletadaEvento(
                perfilEstudiante.Id,
                medalla.Id, 
                perfilEstudiante.Estudiante
            );

            await _mediator.Publish(evento);

            return Resultado.Exitoso();
        }
        
    }
}
