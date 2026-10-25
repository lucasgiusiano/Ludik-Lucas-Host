using InterfacesRepositorio;
using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOsMappers.UmbralParaMedallaPorKudosMappers;

namespace LogicaAplicacion.ImplementacionCasosUsos.UmbralParaObtenerMedallaPorKudos;

public class AltaUmbralParaMedallaPorKudos: IAltaUmbralParaMedallaPorKudos
{
    private readonly IRepositorioUmbralesParaMedallasPorKudos _repositorioUmbralesParaMedallasPorKudos;
    private readonly IRepositorioTiposKudo _repositorioTiposKudo;
    private readonly IRepositorioMedallas _repositorioMedallas;
    private readonly IRepositorioGrupos _repositorioGrupos;
    private readonly IUnitOfWork _unitOfWork;


    public AltaUmbralParaMedallaPorKudos(
        IRepositorioUmbralesParaMedallasPorKudos repositorioUmbralesParaMedallasPorKudos,
        IRepositorioTiposKudo repositorioTiposKudo,
        IRepositorioMedallas repositorioMedallas,
        IRepositorioGrupos repositorioGrupos,
        IUnitOfWork unitOfWork)
    {
        _repositorioUmbralesParaMedallasPorKudos = repositorioUmbralesParaMedallasPorKudos;
        _repositorioTiposKudo = repositorioTiposKudo;
        _repositorioMedallas = repositorioMedallas;
        _repositorioGrupos = repositorioGrupos;
        _unitOfWork = unitOfWork;
    }

    public async Task<Resultado> EjecutarAsync(string idProfesor,AltaUmbralParaMedallaPorKudosDto dto)
    {
        var resultadoGrupo = await _repositorioGrupos.GetByIdAsync(dto.GrupoId);
        if (resultadoGrupo.EsFallo|| resultadoGrupo.Valor.ProfesorId != idProfesor)
            return Resultado<UmbralParaMedallaPorKudos>.Falla(Error.Forbidden);
        
        var existe = await _repositorioUmbralesParaMedallasPorKudos.ExisteConfiguracionAsync(dto.GrupoId, dto.TipoKudoId);
        if (existe)
            return Resultado<UmbralParaMedallaPorKudos>.Falla(Error.Conflict);

        var resultadoMedalla = await _repositorioMedallas.GetByIdAsync(dto.MedallaId);

        if ( resultadoMedalla.EsFallo)
            return Resultado<UmbralParaMedallaPorKudos>.Falla(resultadoMedalla.Errores);
        
        var resultadoTipoKudo = await _repositorioTiposKudo.GetByIdAsync(dto.TipoKudoId);
       
        if (resultadoTipoKudo.EsFallo)
            return Resultado<UmbralParaMedallaPorKudos>.Falla(resultadoTipoKudo.Errores);


        var nuevoUmbral = UmbralParaMedallaPorKudosAltaMapper.fromDto(dto);


        var resultadoValidacion = nuevoUmbral.esValido();
        if (resultadoValidacion.EsFallo)
            return Resultado<UmbralParaMedallaPorKudos>.Falla(resultadoValidacion.Errores);
        

        await _repositorioUmbralesParaMedallasPorKudos.AddAsync(nuevoUmbral);
        await _unitOfWork.SaveChangesAsync();


        return Resultado<UmbralParaMedallaPorKudos>.Exitoso();
    }

    
}


