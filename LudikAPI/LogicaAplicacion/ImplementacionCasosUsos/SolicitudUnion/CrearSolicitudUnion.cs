using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudUnion;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;
using Entidad = LogicaNegocio.Entidades;

namespace LogicaAplicacion.ImplementacionCasosUsos.SolicitudUnion
{
    public class CrearSolicitudUnion : ICrearSolicitudUnion
    {
        private readonly IRepositorioEnlacesUnionGrupo _repoEnlaces;
        private readonly IRepositorioEstudiantes _repoEstudiantes;
        private readonly IRepositorioGrupos _repoGrupos;
        private readonly IRepositorioSolicitudesUnion _repoSolicitudes;

        public CrearSolicitudUnion(
            IRepositorioEnlacesUnionGrupo repoEnlaces,
            IRepositorioEstudiantes repoEstudiantes,
            IRepositorioGrupos repoGrupos,
            IRepositorioSolicitudesUnion repoSolicitudes)
        {
            _repoEnlaces = repoEnlaces;
            _repoEstudiantes = repoEstudiantes;
            _repoGrupos = repoGrupos;
            _repoSolicitudes = repoSolicitudes;
        }

        public async Task<Resultado> EjecutarAsync(SolicitudUnionDto dto)
        {
            if (dto == null)
                return Resultado.Falla(new Error("SolicitudUnion.Crear.Validacion", "Los datos de la solicitud no pueden ser nulos."));

            var enlace = await _repoEnlaces.ObtenerPorCodigoAsync(dto.CodigoEnlace);
            if (enlace == null || enlace.Expiracion < DateTime.UtcNow)
                return Resultado.Falla(new Error("Error.Validation", "El enlace es inválido o ha expirado."));

            var resultadoEstudiante = await _repoEstudiantes.GetByStringIdAsync(dto.IdEstudiante);
            if (resultadoEstudiante == null)
                return Resultado.Falla(new Error("Error.Validation", "El estudiante no existe."));
            Entidad.Estudiante estudiante = resultadoEstudiante.Valor;

            var grupo = await _repoGrupos.ObtenerPorEnlaceAsync(enlace.CodigoUnico);
            if (grupo == null)
                return Resultado.Falla(new Error("Error.Validation", "No se encontró el grupo asociado al enlace."));

            bool yaExiste = await _repoSolicitudes.ExisteSolicitudPendiente(dto.IdEstudiante, grupo.Id);
            if (yaExiste)
                return Resultado.Falla(new Error("Error.Validation",
                    "Ya existe una solicitud pendiente para este estudiante y grupo."));

            var nuevaSolicitud = new Entidad.SolicitudUnion
            {
                EstudianteId = dto.IdEstudiante,
                Estudiante = estudiante,
                GrupoId = grupo.Id,
                Grupo = grupo,
                Fecha = DateOnly.FromDateTime(DateTime.UtcNow),
                Estado = EstadoSolicitud.Pendiente
            };

            var resultadoAdd = await _repoSolicitudes.AddAsync(nuevaSolicitud);
            if (resultadoAdd.EsFallo)
                return resultadoAdd;

            return Resultado.Exitoso();
        }
    }
}
