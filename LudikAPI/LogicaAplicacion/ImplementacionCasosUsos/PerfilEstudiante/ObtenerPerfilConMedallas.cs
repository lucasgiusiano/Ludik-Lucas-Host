using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaNegocio.Resultados;
using InterfacesRepositorio;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicaAplicacion.DTOsMappers.MedallaMappers;
using LogicaAplicacion.Servicios;


public class ObtenerPerfilConMedallas :IObtenerPerfilConMedallas
{
    private readonly IRepositorioPerfilEstudianteGrupo _repoPerfilGrupo;
    private readonly IGeneradorUrlImagen _generadorUrlImagen;
    public ObtenerPerfilConMedallas(IRepositorioPerfilEstudianteGrupo repoPerfilGrupo, IGeneradorUrlImagen generadorUrlImagen)
    {
        _repoPerfilGrupo = repoPerfilGrupo;
        _generadorUrlImagen = generadorUrlImagen;
    }

    public async Task<Resultado<PerfilConMedallasDto>> EjecutarAsync(string estudianteId, int grupoId)
    {

        var resultadoPerfil = await _repoPerfilGrupo.GetPerfilEstudianteAsync(estudianteId, grupoId);
        if (!resultadoPerfil.EsExitoso)
            return Resultado<PerfilConMedallasDto>.Falla(resultadoPerfil.Errores);

        var perfil = resultadoPerfil.Valor;

        var dto = PerfilMapper.ToDtoConMedallas(perfil);
        dto.EnlaceAvatar = await _generadorUrlImagen.GenerarUrlLecturaAsync(dto.EnlaceAvatar);

        return Resultado<PerfilConMedallasDto>.Exitoso(dto);
    }
}
