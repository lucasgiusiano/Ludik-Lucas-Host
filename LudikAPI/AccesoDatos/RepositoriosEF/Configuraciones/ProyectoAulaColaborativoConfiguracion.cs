using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF.Configuraciones
{
    public class ProyectoAulaColaborativoConfiguracion
        : IEntityTypeConfiguration<ProyectoAulaColaborativo>
    {
        public void Configure(EntityTypeBuilder<ProyectoAulaColaborativo> builder)
        {
            builder.ToTable("ProyectosAulaColaborativo");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.CantidadMedallasNecesarias)
                .IsRequired();


            builder.Property(p => p.Estado)
                .IsRequired();

            builder.HasIndex(p => p.GrupoId)
                   .HasDatabaseName("IX_PAC_GrupoId");

            builder.HasOne(p => p.Grupo)
                .WithOne(g => g.Pac)
                .HasForeignKey<ProyectoAulaColaborativo>(p => p.GrupoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.RecompensaClase)
                .WithMany()  
                .HasForeignKey("RecompensaClaseId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
