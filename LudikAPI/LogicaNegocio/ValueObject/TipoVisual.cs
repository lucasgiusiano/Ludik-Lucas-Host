using System.Text.Json.Serialization;

namespace LogicaNegocio.ValueObject;
// atributo para que la API devuelva "Imagen" en lugar de 0.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipoVisual
{
    Imagen,
    Icono
}
