using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.DTOsMappers.RecompensaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Estudiante;
using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using LogicaAplicacion.Servicios;
using Entidades = LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.ImplementacionCasosUsos.Estudiantes
{
    public class ObtenerRecompensasInventarioPerfil : IObtenerRecompensasInventarioPerfil
    {
        private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfilEstudiante;
        private readonly IRecompensaEnricher _recompensaEnricher;

        public ObtenerRecompensasInventarioPerfil(
            IRepositorioPerfilEstudianteGrupo repositorioPerfilEstudiante,
            IRecompensaEnricher recompensaEnricher)
        {
            _repositorioPerfilEstudiante = repositorioPerfilEstudiante;
            _recompensaEnricher = recompensaEnricher;
        }
        public async Task<Resultado<List<RecompensaClienteDto>>> EjecutarAsync(int idPerfil, string idEstudiante)
        {
            var resultadoPerfil = await _repositorioPerfilEstudiante.GetByIdAsync(idPerfil);
            var resultadoRecompensas = await _repositorioPerfilEstudiante.ObtenerRecompensasInventarioAsync(idPerfil);

            if (resultadoPerfil.EsFallo)
                return Resultado<List<RecompensaClienteDto>>.Falla(resultadoPerfil.Errores);
            if(resultadoRecompensas.EsFallo)
                return Resultado<List<RecompensaClienteDto>>.Falla(resultadoRecompensas.Errores);


            Entidades.PerfilEstudiante perfil = resultadoPerfil.Valor;
            IEnumerable<Entidades.Recompensa> recompensas = resultadoRecompensas.Valor;

            if (perfil.EstudianteId != idEstudiante)
                return Resultado<List<RecompensaClienteDto>>.Falla(
                    new Error("Error.Forbidden", "No tienes permiso para acceder a este perfil."));

            var dtos = await _recompensaEnricher.EnrichAsync(recompensas);

            return Resultado<List<RecompensaClienteDto>>.Exitoso(dtos);
        }

    }
}
