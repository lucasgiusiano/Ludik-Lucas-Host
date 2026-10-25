using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class PerfilEstudianteMedallaConfiguracion : IEntityTypeConfiguration<PerfilEstudianteMedalla>
{
    public void Configure(EntityTypeBuilder<PerfilEstudianteMedalla> builder)
    {
        builder.HasKey(pm => pm.Id);
        builder.Property(pm => pm.Id).ValueGeneratedOnAdd();

        builder.HasOne(pm => pm.PerfilEstudiante)
            .WithMany(pe => pe.MedallasObtenidas)
            .HasForeignKey(pm => pm.PerfilEstudianteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pm => pm.Medalla)
            .WithMany()
            .HasForeignKey(pm => pm.MedallaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}