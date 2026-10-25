using LogicaAplicacion.Servicios;
using System.Reflection.Metadata;

namespace LogicaAplicacion.ImplementacionServicios;

public class GeneradorUrlsParaColeccionesImagenes: IGeneradorUrlsParaColeccionesImagenes
{
    private readonly IGeneradorUrlImagen _generadorUrlImagen;
    public GeneradorUrlsParaColeccionesImagenes(IGeneradorUrlImagen generadorUrlImagen)
    {
        _generadorUrlImagen = generadorUrlImagen;
    }
    public async Task EjecutarProcesarUrlsAsync<T>(IEnumerable<T> dtos, params (Func<T, string> getPath, Action<T, string> setUrl)[] accesoresDePropiedad)
    {
        if (dtos == null || !dtos.Any())
            return; 
        

        var tasks = new List<(Task<string> generationTask, T dto, Action<T, string> setter)>();

        foreach (var dto in dtos)
        {
            foreach (var accessor in accesoresDePropiedad)
            {
                var nombreImagen = accessor.getPath(dto);
                tasks.Add(
                    (_generadorUrlImagen.GenerarUrlLecturaAsync(nombreImagen), dto, accessor.setUrl)
                );
            }
        }

        await Task.WhenAll(tasks.Select(t => t.generationTask));

        // Ahora que todas las tareas han terminado, asignamos las URLs generadas.
        foreach (var task in tasks)
        {
            // El resultado de la tarea (la URL) está disponible en task.generationTask.Result
            var generatedUrl = await task.generationTask;
            task.setter(task.dto, generatedUrl);
        }
    }
}