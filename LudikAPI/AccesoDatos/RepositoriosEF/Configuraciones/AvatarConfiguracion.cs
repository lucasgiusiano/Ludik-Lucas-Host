using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class AvatarConfiguracion : IEntityTypeConfiguration<Avatar>
{
    public void Configure(EntityTypeBuilder<Avatar> builder)
    {
        // Relación M:N para los atributos del avatar.
        builder.HasMany(avatar => avatar.AtributosSeleccionados)
            .WithMany() // Sin navegación inversa desde AtributoAvatar
            .UsingEntity<Dictionary<string, object>>(
                "AvatarAtributos", // Nombre de la tabla de unión
                // Configuración de la FK hacia AtributoAvatar
                j => j.HasOne<AtributoAvatar>().WithMany().HasForeignKey("AtributoSeleccionadoId"),
                // Configuración de la FK hacia Avatar
                j => j.HasOne<Avatar>().WithMany().HasForeignKey("AvatarId"),
                // Definición de la clave primaria compuesta para la tabla de unión
                j => j.HasKey("AvatarId", "AtributoSeleccionadoId")
            );
    }
}