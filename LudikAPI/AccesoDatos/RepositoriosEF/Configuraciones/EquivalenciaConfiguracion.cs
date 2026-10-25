using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class EquivalenciaConfiguracion:IEntityTypeConfiguration<Equivalencia>
{
    public void Configure(EntityTypeBuilder<Equivalencia> builder)
    {
        // Relación M:N entre Equivalencia y Medalla (MedallasNecesarias)
        builder.HasMany(e => e.MedallasNecesarias)
            .WithMany() // No hay navegación de vuelta en Medalla
            .UsingEntity<Dictionary<string, object>>(
                "EquivalenciaMedallas", // Nombre de la tabla de unión
                j => j
                    .HasOne<Medalla>()
                    .WithMany()
                    .HasForeignKey("MedallaId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<Equivalencia>()
                    .WithMany()
                    .HasForeignKey("EquivalenciaId")
                    .OnDelete(DeleteBehavior.Cascade)); // Si se borra la equivalencia, se borra el registro de unión
    }
}