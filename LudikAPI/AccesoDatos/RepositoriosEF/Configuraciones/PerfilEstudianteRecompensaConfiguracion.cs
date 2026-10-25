using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class PerfilEstudianteRecompensaConfiguracion : IEntityTypeConfiguration<PerfilEstudianteRecompensa>
{
    public void Configure(EntityTypeBuilder<PerfilEstudianteRecompensa> builder)
    {
        builder.ToTable("PerfilEstudianteRecompensas");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.HasOne(x => x.PerfilEstudiante)
               .WithMany(pe => pe.InventarioRecompensas)
               .HasForeignKey(x => x.PerfilEstudianteId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Recompensa)
               .WithMany()
               .HasForeignKey(x => x.RecompensaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.PerfilEstudianteId, x.RecompensaId });
    }
}