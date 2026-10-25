using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class BarraProgresoConfiguracion : IEntityTypeConfiguration<BarraProgreso>
{
    public void Configure(EntityTypeBuilder<BarraProgreso> builder)
    {
        // Se define que la relación con TablaEquivalencia NO debe ser en cascada.
        // Esto impedirá que se borre una TablaEquivalencia si una BarraProgreso la está utilizando.
        builder.HasOne(b => b.TablaEquivalencia)
            .WithMany()
            .HasForeignKey("TablaEquivalenciaId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}