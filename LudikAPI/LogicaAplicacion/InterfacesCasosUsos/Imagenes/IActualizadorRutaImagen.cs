using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Imagenes;

// Define el contrato para las estrategias que actualizan la ruta de la imagen en diferentes entidades
public interface IActualizadorRutaImagen
{
    Task<Resultado> ActualizarRutasAsync(string idUsuarioAutenticado, int? entidadAsociadaId, Dictionary<string, string> rutas);
}