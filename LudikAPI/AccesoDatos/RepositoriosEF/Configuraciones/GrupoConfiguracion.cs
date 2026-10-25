using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class GrupoConfiguracion : IEntityTypeConfiguration<Grupo>
{
    public void Configure(EntityTypeBuilder<Grupo> builder)
    {
        // Índices
        builder.HasIndex(x => x.Nombre);
        builder.HasIndex(x => x.ProfesorId).HasDatabaseName("IX_Grupo_ProfesorId");

        // Grupo -> SolicitudUnion (Uno a Muchos, Cascada)
        builder.HasMany(gr => gr.Solicitudes)
            .WithOne(su => su.Grupo)
            .HasForeignKey(su => su.GrupoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación con Tienda (Uno a Uno)
        builder.HasOne(gr => gr.Tienda)
            .WithOne(t => t.Grupo)
            .HasForeignKey<Tienda>(t => t.GrupoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación con TablaEquivalencia (Muchos a Uno, SIN Cascada)
        builder.HasOne(grupo => grupo.TablaEquivalencia)
            .WithMany() // No hay navegación inversa desde TablaEquivalencia
            .HasForeignKey("TablaEquivalenciaId")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(gr => gr.SolicitudesPerfilMedalla)
            .WithOne(spm => spm.Grupo)
            .HasForeignKey(spm => spm.GrupoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(gr => gr.Pac)
                .WithOne(pac => pac.Grupo)
                .HasForeignKey<ProyectoAulaColaborativo>(pac => pac.GrupoId)
                .OnDelete(DeleteBehavior.Cascade);

    }
}