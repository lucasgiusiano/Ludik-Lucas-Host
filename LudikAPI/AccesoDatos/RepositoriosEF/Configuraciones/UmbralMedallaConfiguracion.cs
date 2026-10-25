using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class UmbralMedallaConfiguracion: IEntityTypeConfiguration<UmbralParaMedallaPorKudos>
{
    public void Configure(EntityTypeBuilder<UmbralParaMedallaPorKudos> builder)
    {
        builder
            .HasOne(umbral => umbral.Grupo)
            .WithMany() 
            .HasForeignKey(umbral => umbral.GrupoId)
            .OnDelete(DeleteBehavior.Cascade); 


        builder
            .HasOne(umbral => umbral.Medalla)
            .WithMany()
            .HasForeignKey(umbral => umbral.MedallaId)
            .OnDelete(DeleteBehavior.Restrict);


        builder
            .HasOne(umbral => umbral.TipoKudo)
            .WithMany()
            .HasForeignKey(umbral => umbral.TipoKudoId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
