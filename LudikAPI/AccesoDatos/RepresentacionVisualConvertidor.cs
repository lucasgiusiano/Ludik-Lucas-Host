using LogicaNegocio.EntidadesAuxiliares;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccesoDatos;

public class RepresentacionVisualConvertidor : ValueConverter<RepresentacionVisualBase, string>
{
    public RepresentacionVisualConvertidor() : base(
        // Función para convertir del Objeto -> a la Base de Datos (string JSON)
        v => JsonSerializer.Serialize(v, ToJsonOptions()),

        // Función para convertir de la Base de Datos (string JSON) -> al Objeto
        v => Deserialize(v)
    )
    {
    }

    // Método auxiliar para deserializar al tipo correcto
    private static RepresentacionVisualBase Deserialize(string json)
    {
        if (string.IsNullOrEmpty(json))
            return null;

        // Usamos una clase temporal para leer el "discriminador" que guardaremos en el JSON
        var typeInfo = JsonSerializer.Deserialize<JsonTypeInfo>(json, FromJsonOptions());

        // Basado en el tipo, deserializamos al objeto concreto
        return typeInfo.Type switch
        {
            nameof(RepresentacionImagen) => JsonSerializer.Deserialize<RepresentacionImagen>(json, FromJsonOptions()),
            nameof(RepresentacionIcono) => JsonSerializer.Deserialize<RepresentacionIcono>(json, FromJsonOptions()),
            _ => throw new NotSupportedException($"El tipo '{typeInfo.Type}' no es soportado.")
        };
    }

    // Opciones para la serialización
    private static JsonSerializerOptions ToJsonOptions()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new RepresentacionVisualJsonConverter());
        return options;
    }

    // Opciones para la deserialización
    private static JsonSerializerOptions FromJsonOptions()
    {
        return new JsonSerializerOptions();
    }

    // Clase auxiliar para leer el tipo desde el JSON
    private class JsonTypeInfo
    {
        public string Type { get; set; }
    }
}

// Convertidor de JSON personalizado para añadir el discriminador de tipo
public class RepresentacionVisualJsonConverter : JsonConverter<RepresentacionVisualBase>
{
    public override RepresentacionVisualBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // La deserialización principal la maneja el método Deserialize del ValueConverter
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, RepresentacionVisualBase value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Escribir el discriminador
        writer.WriteString("Type", value.GetType().Name);

        // Escribir las propiedades del objeto concreto
        if (value is RepresentacionImagen img)
        {
            writer.WriteString(nameof(img.NombreImagenCompleta), img.NombreImagenCompleta);
            writer.WriteString(nameof(img.NombreImagenMiniatura), img.NombreImagenMiniatura);
        }
        else if (value is RepresentacionIcono icon)
        {
            writer.WriteString(nameof(icon.NombreIcono), icon.NombreIcono);
        }

        writer.WriteEndObject();
    }
}