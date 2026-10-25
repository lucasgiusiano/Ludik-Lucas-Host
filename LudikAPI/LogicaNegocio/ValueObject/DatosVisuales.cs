using System.Text.Json.Serialization;

namespace LogicaNegocio.ValueObject
{
	[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
	[JsonDerivedType(typeof(DatosIcono), "Icono")]
	[JsonDerivedType(typeof(DatosImagen), "Imagen")]
	public abstract record DatosVisuales;

	public record DatosImagen(string NombreMiniatura, string NombreCompleta) : DatosVisuales;

	public record DatosIcono(string NombreIcono) : DatosVisuales;
}
