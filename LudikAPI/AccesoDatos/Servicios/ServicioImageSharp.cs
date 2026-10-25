using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.ProcesamientoRecord;
using LogicaAplicacion.InterfacesCasosUsos.Imagenes;
using LogicaNegocio.Resultados;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace AccesoDatos.Servicios
{
    public class ServicioImageSharp: IServicioProcesamientoImagenes
    {
        private const int SizeCompleta = 1024;
        private const int SizeMiniatura = 100;
        private const int CalidadJpeg = 85;

        public async Task<Resultado<IEnumerable<StreamProcesado>>> ProcesarImagenPerfilAsync(Stream streamOriginal)
        {
            try
            {
                var sourceStream = new MemoryStream();
                await streamOriginal.CopyToAsync(sourceStream);
                sourceStream.Position = 0;

                using var image = await Image.LoadAsync(sourceStream);

                // Generar imagen completa
                var fullStream = await ProcesarVersionAsync(image, SizeCompleta);

                // Generar miniatura
                var thumbStream = await ProcesarVersionAsync(image, SizeMiniatura);

                var resultados = new List<StreamProcesado>
                {
                    new("completa", fullStream),
                    new("mini", thumbStream)
                };

                return Resultado<IEnumerable<StreamProcesado>>.Exitoso(resultados);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<StreamProcesado>>.Falla(new Error("Error.Unexpected", "No se pudo procesar la imagen. Error: "+ex.Message));
            }
        }

        private async Task<Stream> ProcesarVersionAsync(Image originalImage, int size)
        {
            // Clonamos la imagen para no afectar el original al procesar múltiples versiones
            using var clone = originalImage.Clone(ctx => ctx.Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max // Mantiene la relación de aspecto
            }));

            var outputStream = new MemoryStream();
            await clone.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = CalidadJpeg });
            outputStream.Position = 0; // Rebobinar para lectura
            return outputStream;
        }
    }
}
