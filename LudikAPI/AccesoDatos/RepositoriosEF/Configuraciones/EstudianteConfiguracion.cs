using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class EstudianteConfiguracion : IEntityTypeConfiguration<Estudiante>
{
    public void Configure(EntityTypeBuilder<Estudiante> builder)
    {
        builder.ToTable("Estudiantes");

        // Relación con la tabla base Usuario para la herencia TPT
        builder.HasOne<Usuario>()
            .WithOne()
            .HasForeignKey<Estudiante>(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación M:N entre Estudiante y Hito
        builder.HasMany(e => e.Hitos)
            .WithMany() // No hay propiedad de navegación de vuelta en la clase Hito
            .UsingEntity<Dictionary<string, object>>(
                "EstudianteHitos", // Nombre de la nueva tabla de unión
                j => j
                    .HasOne<Hito>()
                    .WithMany()
                    .HasForeignKey("HitoId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<Estudiante>()
                    .WithMany()
                    .HasForeignKey("EstudianteId")
                    // Cascade delete aquí es aceptable, si se borra el estudiante, se borra su registro de hitos.
                    .OnDelete(DeleteBehavior.Cascade));

        builder.HasOne(e => e.EstPotenciador)
                   .WithOne(ep => ep.Est)
                   .HasForeignKey<EstudiantePotenciador>(ep => ep.Id)
                   .OnDelete(DeleteBehavior.Cascade);
    }
}