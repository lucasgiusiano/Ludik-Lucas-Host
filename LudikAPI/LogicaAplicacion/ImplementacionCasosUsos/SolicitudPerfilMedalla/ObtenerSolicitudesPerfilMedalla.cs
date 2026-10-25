using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs;
using LogicaAplicacion.DTOsMappers.SolicitudPerfilMedallaMappers;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.SolicitudPerfilMedalla
{
    public class ObtenerSolicitudesPerfilMedalla : IObtenerSolicitudPerfilMedalla
    {
        private readonly IRepositorioSolicitudPerfilMedalla _repositorio;
        private readonly IRepositorioProfesores _repositorioProfesor;

        public ObtenerSolicitudesPerfilMedalla(IRepositorioSolicitudPerfilMedalla repositorio, IRepositorioProfesores repositorioProfesores)
        {
            _repositorio = repositorio;
            _repositorioProfesor = repositorioProfesores;
        }

        public async Task<Resultado<List<SolicitudPerfilMedallaDto>>> EjecutarAsync(int grupoId, string profesorId)
        {
            var resultado = await _repositorio.GetByGrupoAsync(grupoId);

            if (resultado.EsFallo)
                return Resultado<List<SolicitudPerfilMedallaDto>>.Falla(resultado.Errores);
            var profesor = await _repositorioProfesor.GetByStringIdAsync(profesorId);
            var valorProfesor = profesor.Valor;
            if (valorProfesor == null || valorProfesor.Grupos == null || !valorProfesor.Grupos.Any(g => g.Id == grupoId))
            {
                return Resultado<List<SolicitudPerfilMedallaDto>>.Falla(
            new Error("Error.Autorizacion", "El grupo no pertenece al profesor logueado.")
                );
            }

            var dtos = resultado.Valor!.Select(SolicitudPerfilMedallaMapper.Map).ToList();
            return Resultado<List<SolicitudPerfilMedallaDto>>.Exitoso(dtos);
        }
    }
}
