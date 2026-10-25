using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class ProfesorConfiguracion : IEntityTypeConfiguration<Profesor>
{
    public void Configure(EntityTypeBuilder<Profesor> builder)
    {
        builder.ToTable("Profesores");

        // Relación con la tabla base Usuario para la herencia TPT
        builder.HasOne<Usuario>()
            .WithOne()
            .HasForeignKey<Profesor>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Un Profesor es dueño de sus Medallas, Grupos, Recompensas y Tablas de Equivalencia.
        // Si el Profesor se elimina, todo esto se debe eliminar también.

        // Profesor -> Medalla (Uno a Muchos, Cascada)
        builder.HasMany(prof => prof.Medallas)
            .WithOne(m => m.Creador)
            .HasForeignKey(m => m.ProfesorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Profesor -> TablaEquivalencia (Uno a Muchos, Cascada)
        builder.HasMany(prof => prof.TablasEquivalencia)
            .WithOne() // Asumiendo que TablaEquivalencia no necesita navegar de vuelta
            .HasForeignKey(te => te.ProfesorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Profesor -> Grupo (Uno a Muchos, Cascada)
        builder.HasMany(prof => prof.Grupos)
            .WithOne(g => g.Profesor)
            .HasForeignKey(g => g.ProfesorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Profesor -> Recompensas (Uno a Muchos, Cascada)
        builder.HasMany(prof => prof.RecompensasCreadas)
            .WithOne(g => g.Profesor)
            .HasForeignKey(g => g.ProfesorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}