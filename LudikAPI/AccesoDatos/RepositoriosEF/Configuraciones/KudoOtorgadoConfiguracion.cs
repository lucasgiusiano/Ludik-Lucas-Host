using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class KudoOtorgadoConfiguracion : IEntityTypeConfiguration<KudoOtorgado>
{
    public void Configure(EntityTypeBuilder<KudoOtorgado> builder)
    {
        // 1. Configuración de la relación Receptor -> KudosRecibidos
        builder.HasOne(kudo => kudo.Receptor) // Un KudoOtorgado tiene un PerfilEstudiante Receptor...
            .WithMany(perfil => perfil.KudosRecibidos) // ...y un PerfilEstudiante tiene muchos KudosRecibidos.
            .HasForeignKey(kudo => kudo.PerfilEstudianteReceptorId) // La clave foránea en KudoOtorgado es PerfilEstudianteReceptorId.
            .OnDelete(DeleteBehavior.Restrict); // Evita que se borre un perfil si ha recibido kudos.

        // 2. Configuración de la relación Emisor -> KudosOtorgados
        builder.HasOne(kudo => kudo.Emisor) // Un KudoOtorgado tiene un PerfilEstudiante Emisor...
            .WithMany(perfil => perfil.KudosOtorgados) // ...y un PerfilEstudiante tiene muchos KudosOtorgados.
            .HasForeignKey(kudo => kudo.PerfilEstudianteEmisorId) // La clave foránea es PerfilEstudianteEmisorId.
            .OnDelete(DeleteBehavior.Restrict); // Evita que se borre un perfil si ha otorgado kudos.
    }
}