using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;
using Entidades = LogicaNegocio.Entidades;


namespace LogicaAplicacion.ImplementacionCasosUsos.SolicitudPerfilMedalla
{
    public class RechazarSolicitudPerfilMedalla : IRechazarSolicitudPerfilMedalla
    {
        private readonly IRepositorioSolicitudPerfilMedalla _repositorio;
        private readonly IRepositorioProfesores _repositorioProfesores;

        private readonly List<IObserver<Entidades.SolicitudPerfilMedalla>> _observers;


        public RechazarSolicitudPerfilMedalla(IRepositorioSolicitudPerfilMedalla repositorio, IEnumerable<IObserver<Entidades.SolicitudPerfilMedalla>> observers, IRepositorioProfesores repositorioProfesores)
        {
            _repositorio = repositorio;
            _observers = observers.ToList();
            _repositorioProfesores = repositorioProfesores;
        }
        public async Task<Resultado> EjecutarAsync(int idSolicitudPerfil,string profesorId)
        {
            var solicitud = await _repositorio.GetByIdAsync(idSolicitudPerfil);
            if (solicitud.EsFallo || solicitud.Valor == null)
            {
                return Resultado.Falla(new Error("Error.NotFound", "La solicitud de perfil medalla no existe."));
            }
            var solicitudValor = solicitud.Valor;
            if (solicitudValor.Estado != EstadoSolicitud.Pendiente)
            {
                return Resultado.Falla(new Error("Error.Validation", "La solicitud ya fue procesada."));
            }

            var resultadoPosee = await _repositorioProfesores.PoseeMedallaAsync(profesorId, solicitudValor.MedallaId);
            if (resultadoPosee.Valor == false)
            {
                return Resultado.Falla(new Error("Error.Validation", "Esa medalla pertenece a otro docente"));
            }


            solicitudValor.Estado = EstadoSolicitud.Rechazada;
            var resultadoActualizacion = await _repositorio.UpdateAsync(solicitudValor);
            if (resultadoActualizacion.EsFallo)
            {
                return Resultado.Falla(new Error("Error.Validation", "No se pudo actualizar el estado de la solicitud."));
            }
            foreach (var obs in _observers)
                obs.OnNext(solicitudValor);
            return Resultado.Exitoso();
        }
    }
}
