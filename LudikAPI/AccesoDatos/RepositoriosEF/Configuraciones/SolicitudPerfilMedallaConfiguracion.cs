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
    public class SolicitudPerfilMedallaConfiguracion
        : IEntityTypeConfiguration<SolicitudPerfilMedalla>
    {
        public void Configure(EntityTypeBuilder<SolicitudPerfilMedalla> builder)
        {
            builder.HasKey(spm => spm.Id);

            builder.HasIndex(spm => spm.PerfilEstudianteId);
            builder.HasIndex(spm => spm.MedallaId);
            builder.HasIndex(spm => spm.GrupoId);

            builder.HasOne<PerfilEstudiante>()
                .WithMany()  
                .HasForeignKey(spm => spm.PerfilEstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Medalla>()
                .WithMany()  
                .HasForeignKey(spm => spm.MedallaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(spm => spm.Grupo)
                .WithMany(gr => gr.SolicitudesPerfilMedalla)
                .HasForeignKey(spm => spm.GrupoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(spm => spm.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(spm => spm.Fecha)
                .IsRequired();

            builder.Property(spm => spm.Estado)
                .IsRequired();
        }
    }
}
