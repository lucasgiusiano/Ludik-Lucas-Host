using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.Imagenes;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Imagenes.Estrategias;

public class ActualizadorImagenPerfilProfesor: IActualizadorRutaImagen
{
    private readonly IRepositorioProfesores _repositorioProfesores; // Asume que este repositorio existe

    public ActualizadorImagenPerfilProfesor(IRepositorioProfesores repositorioProfesores)
    {
        _repositorioProfesores = repositorioProfesores;
    }

    public async Task<Resultado> ActualizarRutasAsync(string idUsuarioAutenticado, int? entidadAsociadaId, Dictionary<string, string> rutas)
    {
        /*  SI SE QUISIERA TENER LA POSIBILIDAD DE ASIGNAR UNA IMAGEN DE PERFIL AL PROFESOR
        var profesor = await _repositorioProfesores.GetByStringIdAsync(idUsuarioAutenticado);
        if (profesor == null)
        {
            return Resultado.Falla(Error.NotFound);
        }

        profesor.RutaImagenCompleta = rutas["completa"];
        profesor.RutaImagenMiniatura = rutas["mini"];
        await _repositorioProfesores.UpdateAsync(profesor);

        return Resultado.Exitoso();
        */
        return Resultado.Falla(new Error("Error.Unexpected", "El metodo no esta implementado aun."));
    }
}