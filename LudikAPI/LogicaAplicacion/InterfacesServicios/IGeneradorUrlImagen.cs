namespace LogicaAplicacion.Servicios;

public interface IGeneradorUrlImagen
{
    Task<string?> GenerarUrlLecturaAsync(string? nombreBlob);
}