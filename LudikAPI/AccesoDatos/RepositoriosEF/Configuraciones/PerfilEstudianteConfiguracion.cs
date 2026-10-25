using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class PerfilEstudianteConfiguracion : IEntityTypeConfiguration<PerfilEstudiante>
{
    public void Configure(EntityTypeBuilder<PerfilEstudiante> builder)
    {
        // Relación PerfilEstudiante -> Estudiante (M-1): cuando se borre Estudiante, se eliminan sus perfiles.
        builder.HasOne(p => p.Estudiante)
            .WithMany(e => e.Perfiles)
            .HasForeignKey(p => p.EstudianteId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación 1:1 PerfilEstudiante -> BarraProgreso: al borrar el perfil también borre la barra.
        builder.HasOne(p => p.BarraProgreso)
            .WithOne()
            .HasForeignKey<BarraProgreso>(b => b.PerfilEstudianteId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación PerfilEstudiante -> Grupo (M-1): SIN CASCADA para evitar ciclos.
        builder.HasOne(p => p.Grupo)
            .WithMany(g => g.Alumnos)
            .HasForeignKey(p => p.GrupoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación 1:1 PerfilEstudiante -> Avatar (Composición)
        builder.HasOne(p => p.Avatar)
            .WithOne()
            .HasForeignKey<Avatar>("PerfilEstudianteId") // FK explícita
            .OnDelete(DeleteBehavior.Cascade);
        // Índice compuesto para evitar duplicados de (GrupoId, EstudianteId)
        builder.HasIndex(pe => new { pe.GrupoId, pe.EstudianteId })
            .IsUnique()
            .HasDatabaseName("UX_PerfilEstudiante_GrupoId_EstudianteId");
    }
}