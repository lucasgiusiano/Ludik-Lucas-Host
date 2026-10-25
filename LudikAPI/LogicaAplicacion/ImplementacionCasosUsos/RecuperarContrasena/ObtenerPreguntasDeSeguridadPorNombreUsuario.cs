using InterfacesRepositorio;
using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaAplicacion.DTOsMappers.PreguntaDeSeguridadMappers;
using LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.RecuperarContrasena;

public class ObtenerPreguntasDeSeguridadPorNombreUsuario: IObtenerPreguntasDeSegurididadPorNombreUsuario
{

    private readonly IRepositorioPreguntasSeguridad _repositorioPreguntasSeguridad;
    public ObtenerPreguntasDeSeguridadPorNombreUsuario(IRepositorioPreguntasSeguridad repositorioPreguntasSeguridad)
    {
        _repositorioPreguntasSeguridad = repositorioPreguntasSeguridad;
    }
    public async Task<Resultado<PreguntasDto>> EjecutarAsync(string nombreUsuario)
    {
        Resultado<List<PreguntaRespuestaSeguridad>> resultado = await _repositorioPreguntasSeguridad.GetByNombreUsuarioAsync(nombreUsuario);
  
        if (resultado.EsFallo)
            return Resultado<PreguntasDto>.Falla(resultado.Errores);
        

        List<PreguntaRespuestaSeguridad> preguntasYRespuestas = resultado.Valor;

        PreguntasDto dto = PreguntasDeSeguridadMapper.toDto(preguntasYRespuestas);

        return Resultado<PreguntasDto>.Exitoso(dto);
    }
}