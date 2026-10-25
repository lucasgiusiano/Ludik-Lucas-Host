using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaAplicacion.DTOsMappers;
using LogicaAplicacion.DTOsMappers.PreguntaDeSeguridadMappers;
using LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.RecuperarContrasena;

public class ObtenerPreguntasDeSeguridadDelSistema: IObtenerPreguntasDeSeguridadDelSistema
{
    private readonly IRepositorioPreguntasDeSeguridadDelSistema _repositorioPreguntasDeSeguridadDelSistema;

    public ObtenerPreguntasDeSeguridadDelSistema(IRepositorioPreguntasDeSeguridadDelSistema repositorioPreguntasDeSeguridadDelSistema)
    {
        _repositorioPreguntasDeSeguridadDelSistema = repositorioPreguntasDeSeguridadDelSistema;
    }
    public async Task<Resultado<PreguntasDto>> EjecutarAsync()
    {
        var resultado = await _repositorioPreguntasDeSeguridadDelSistema.GetAllAsync();
        if (resultado == null)
            return Resultado<PreguntasDto>.Falla(resultado.Errores);

        PreguntasDto preguntasDto = PreguntasDeSeguridadDelSistemaMapper.toDto(resultado.Valor);
        return Resultado<PreguntasDto>.Exitoso(preguntasDto);
    }
}