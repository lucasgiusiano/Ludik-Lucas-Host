using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaNegocio.Entidades;
using Entidad = LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudUnion;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.ImplementacionCasosUsos.SolicitudUnion
{
    public class AceptarSolicitudUnion : IAceptarSolicitudUnion
    {
        private readonly IRepositorioSolicitudesUnion _repoSolicitudes;
        private readonly IRepositorioGrupos _repoGrupos; 
        private readonly IRepositorioPerfilEstudianteGrupo _repoPerfilEstudianteGrupo;
        private readonly IRepositorioProfesores _repositorioProfesores;
        public AceptarSolicitudUnion(IRepositorioSolicitudesUnion repoSolicitudes,IRepositorioGrupos repositorioGrupos, IRepositorioPerfilEstudianteGrupo repoPerfilEstudianteGrupo,IRepositorioProfesores repositorioProfesores)
        {
            _repoSolicitudes = repoSolicitudes;
            _repoGrupos = repositorioGrupos;
            _repoPerfilEstudianteGrupo = repoPerfilEstudianteGrupo;
            _repositorioProfesores = repositorioProfesores;
        }
        public async Task<Resultado> EjecutarAsync(int idSolicitud,string profesorId)
        {
            var solicitud = await _repoSolicitudes.GetSolicitudConEstudianteYGrupoPorIdAsync(idSolicitud);
            if (solicitud == null)
                return Resultado.Falla(new Error("Error.Validation", "La solicitud no existe."));

            if (solicitud.Estado != EstadoSolicitud.Pendiente)
                return Resultado.Falla(new Error("Error.Validation", "La solicitud ya fue procesada."));

            var profesorResultado = await _repositorioProfesores.GetByStringIdAsync(profesorId);
            if (profesorResultado.EsFallo || profesorResultado.Valor == null)
                return Resultado.Falla(new Error("Error.Autorizacion", "No se encontró el profesor logueado."));

            var profesor = profesorResultado.Valor;

            var grupo = solicitud.Grupo;
            if (grupo == null)
                return Resultado.Falla(new Error("Error.Validation", "La solicitud no tiene grupo asociado."));

            if (profesor.Grupos == null || !profesor.Grupos.Any(g => g.Id == grupo.Id))
            {
                return Resultado.Falla(new Error("Error.Autorizacion", "El grupo de la solicitud no pertenece al profesor logueado."));
            }

            solicitud.Estado = EstadoSolicitud.Aceptada;

            var perfil = new Entidad.PerfilEstudiante(solicitud.EstudianteId, grupo.Id);

            var resultadoPerfil = await _repoPerfilEstudianteGrupo.AddAsync(perfil);
            if (resultadoPerfil.EsFallo) return resultadoPerfil;

            grupo.Alumnos ??= new List<Entidad.PerfilEstudiante>();
            grupo.Alumnos.Add(perfil);

            var resultadoSolicitud = await _repoSolicitudes.UpdateAsync(solicitud);
            if (resultadoSolicitud.EsFallo) return resultadoSolicitud;

            var resultadoGrupo = await _repoGrupos.UpdateAsync(grupo);
            if (resultadoGrupo.EsFallo) return resultadoGrupo;

            return Resultado.Exitoso();
        }
    }
}
