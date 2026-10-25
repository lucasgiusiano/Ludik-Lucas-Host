using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class MedallaConfiguracion:IEntityTypeConfiguration<Medalla>
{
    public void Configure(EntityTypeBuilder<Medalla> builder)
    {
        builder.HasIndex(x => x.Nombre);
    }
}