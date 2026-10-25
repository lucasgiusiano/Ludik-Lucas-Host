using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class TablaEquivalenciaConfiguracion:IEntityTypeConfiguration<TablaEquivalencia>
{
    public void Configure(EntityTypeBuilder<TablaEquivalencia> builder)
    {
        builder.HasIndex(x => x.Nombre);

        // Una TablaEquivalencia tiene muchas Equivalencias.
        // Si se borra una TablaEquivalencia, se borran sus Equivalencias asociadas.
        builder.HasMany(t => t.Equivalencias)
            .WithOne(e => e.TablaEquivalencia)
            .HasForeignKey(e => e.TablaEquivalenciaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}