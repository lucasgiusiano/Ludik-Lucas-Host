using LogicaNegocio.Entidades;
using LogicaNegocio.EntidadesAuxiliares;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class RecompensaConfiguracion : IEntityTypeConfiguration<Recompensa>
{
    public void Configure(EntityTypeBuilder<Recompensa> builder)
    {
        builder.HasIndex(x => x.Nombre);

        // --- Configurar la jerarquía TPH principal para Recompensa ---
        builder.HasDiscriminator<string>("TipoRecompensa")
            .HasValue<RecompensaSimple>("Recompensa_Simple")
            .HasValue<RecompensaPersonalizacionAvatar>("Recompensa_Avatar")
            .HasValue<Potenciador>("Recompensa_Potenciador");

        // Le decimos a EF de forma explícita que la propiedad 'Representacion'
        // debe ser convertida usando el conversor personalizado.
        // EF también inferirá que esta columna debe ser de tipo string.
        builder.Property(r => r.Representacion)
            .HasConversion<RepresentacionVisualConvertidor>();
    }
}