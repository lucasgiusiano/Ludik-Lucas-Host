using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class RendimientoPeriodoConfiguracion : IEntityTypeConfiguration<RendimientoPeriodo>
{
    public void Configure(EntityTypeBuilder<RendimientoPeriodo> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).UseIdentityColumn().ValueGeneratedOnAdd();

        // Configuración del Owned Type RangoFechas
        builder.OwnsOne(r => r.Rangofecha, rf =>
        {
            rf.Property(x => x.fechaInicio).HasColumnName("FechaInicio").IsRequired();
            rf.Property(x => x.fechaFin).HasColumnName("FechaFin").IsRequired();
        });

        // Relación con la tabla de unión explícita
        builder.HasMany(r => r.RendimientoMedallas)
            .WithOne(rpm => rpm.RendimientoPeriodo)
            .HasForeignKey(rpm => rpm.RendimientoPeriodoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}