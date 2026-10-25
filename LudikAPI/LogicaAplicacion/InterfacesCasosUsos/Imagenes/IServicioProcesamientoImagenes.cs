using LogicaAplicacion.DTOs.ProcesamientoRecord;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Imagenes;

public interface IServicioProcesamientoImagenes
{
    /// <summary>
    /// Comprime y procesa una imagen para su uso en perfiles.
    /// </summary>
    /// <param name="streamOriginal">El stream de la imagen original.</param>
    /// <returns>Un stream con la imagen procesada y optimizada.</returns>
    Task<Resultado<IEnumerable<StreamProcesado>>> ProcesarImagenPerfilAsync(Stream streamOriginal);
}