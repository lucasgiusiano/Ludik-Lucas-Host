using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class RendimientoPeriodoMedallaConfiguracion: IEntityTypeConfiguration<RendimientoPeriodoMedalla>
{
    public void Configure(EntityTypeBuilder<RendimientoPeriodoMedalla> builder)
    {
        builder.ToTable("RendimientoPeriodoMedallas");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();

        // No se puede borrar una Medalla si está en un registro de rendimiento.
        builder.HasOne(x => x.Medalla)
            .WithMany()
            .HasForeignKey(x => x.MedallaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.RendimientoPeriodoId, x.MedallaId });
    }
}