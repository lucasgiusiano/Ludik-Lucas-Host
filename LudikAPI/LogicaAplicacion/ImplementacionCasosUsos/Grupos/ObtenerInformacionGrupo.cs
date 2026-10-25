using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOsMappers.GrupoMappers;
using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Grupos
{
    public class ObtenerInformacionGrupo : IObtenerInformacionGrupo
    {
        private readonly IRepositorioGrupos _repoGrupos;

        public ObtenerInformacionGrupo(IRepositorioGrupos repoGrupos)
        {
            _repoGrupos = repoGrupos;
        }

        public async Task<Resultado<GrupoInformacionDto>> EjecutarAsync(int grupoId,string profesorId)
        {
            var resultadoGrupo = await _repoGrupos.GetByIdAsync(grupoId);

            if (resultadoGrupo.EsFallo)
                return Resultado<GrupoInformacionDto>.Falla(resultadoGrupo.Errores);
           
            var grupo = resultadoGrupo.Valor!;

            
            var dto = GrupoInformacionMapper.ToDto(grupo);

            return Resultado<GrupoInformacionDto>.Exitoso(dto);
        }
    }

}
