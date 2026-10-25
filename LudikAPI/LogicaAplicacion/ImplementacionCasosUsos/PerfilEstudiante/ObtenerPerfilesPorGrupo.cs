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
    public class ObtenerPerfilesPorGrupo : IObtenerPerfilesPorGrupo
    {
        private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfil;
        private readonly IGeneradorUrlsParaColeccionesImagenes _generadorUrlsParaColecciones;
        private readonly IRepositorioGrupos _repoGrupos;

        public ObtenerPerfilesPorGrupo(
            IRepositorioPerfilEstudianteGrupo repositorioPerfil,
            IGeneradorUrlsParaColeccionesImagenes generadorUrlsImagenes,
            IRepositorioGrupos repoGrupos)
        {
            _repositorioPerfil = repositorioPerfil;
            _generadorUrlsParaColecciones = generadorUrlsImagenes;
            _repoGrupos = repoGrupos;
        }

        public async Task<Resultado<List<PerfilEstudianteInformacionDto>>> EjecutarAsync(int grupoId,string profesorId)
        {
            
            var resultadoPerfiles = await _repositorioPerfil.ObtenerPorGrupoIdAsync(grupoId);
            if (resultadoPerfiles.EsFallo)
                return Resultado<List<PerfilEstudianteInformacionDto>>.Falla(resultadoPerfiles.Errores);

            var grupoRes = await _repoGrupos.GetByIdAsync(grupoId);
            if (grupoRes.Valor.ProfesorId != profesorId)
            {
                return Resultado<List<PerfilEstudianteInformacionDto>>.Falla(new Error("Error.Unauthorized", "Este grupo pertenece a otro profesor"));
            }


            var perfiles = resultadoPerfiles.Valor!;
            List<PerfilEstudianteInformacionDto> dtos = new List<PerfilEstudianteInformacionDto>();
            foreach (var perfil in perfiles)
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
