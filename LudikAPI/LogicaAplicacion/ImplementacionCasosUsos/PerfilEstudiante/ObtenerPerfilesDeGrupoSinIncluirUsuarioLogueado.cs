using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaAplicacion.InterfacesCasosUsos.PerfilEstudiante;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.PerfilEstudiante
{
    public class ObtenerPerfilesDeGrupoSinIncluirUsuarioLogueado:IObtenerPerfilesPorGrupoSinLogueado
    {
        private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfil;
        private readonly IGeneradorUrlsParaColeccionesImagenes _generadorUrlsParaColecciones;
        private readonly IRepositorioGrupos _repositorioGrupos;

        public ObtenerPerfilesDeGrupoSinIncluirUsuarioLogueado(
            IRepositorioPerfilEstudianteGrupo repositorioPerfil,
            IGeneradorUrlsParaColeccionesImagenes generadorUrlsParaColecciones,IRepositorioGrupos repositorioGrupos)
        {
            _repositorioPerfil = repositorioPerfil;
            _generadorUrlsParaColecciones = generadorUrlsParaColecciones;
            _repositorioGrupos = repositorioGrupos;
        }

       
        public async Task<Resultado<List<PerfilEstudianteInformacionDto>>> EjecutarAsync(int grupoId,string estudianteIdLogueado)
        {
            var pertenece = await _repositorioGrupos
            .EstudiantePerteneceAlGrupoAsync(grupoId, estudianteIdLogueado);

            if (!pertenece)
            {
                return Resultado<List<PerfilEstudianteInformacionDto>>.Falla(
                    new Error(
                        Error.Validation.Codigo,
                        "El estudiante no pertenece al grupo especificado."));
            }

            var resultadoPerfiles = await _repositorioPerfil.ObtenerPorGrupoIdAsync(grupoId);
            if (resultadoPerfiles.EsFallo)
                return Resultado<List<PerfilEstudianteInformacionDto>>.Falla(resultadoPerfiles.Errores);

            var perfiles = resultadoPerfiles.Valor!;

            var perfilesCompañeros = perfiles
                .Where(p => !string.Equals(p.EstudianteId, estudianteIdLogueado, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var dtos = new List<PerfilEstudianteInformacionDto>();
            foreach (var perfil in perfilesCompañeros)
            {
                int nota = perfil.CalcularNotaActual();
                var dto = PerfilEstudianteMapper.ToDto(perfil, nota);
                dtos.Add(dto);
            }

            await _generadorUrlsParaColecciones.EjecutarProcesarUrlsAsync(dtos,
                (dto => dto.EnlaceAvatarMiniatura, (dto, url) => dto.EnlaceAvatarMiniatura = url)
            );

            return Resultado<List<PerfilEstudianteInformacionDto>>.Exitoso(dtos);
        }
    }
}
