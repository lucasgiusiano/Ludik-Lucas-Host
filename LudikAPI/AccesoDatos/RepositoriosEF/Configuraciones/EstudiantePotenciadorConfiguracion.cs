using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones
{
    public class EstudiantePotenciadorConfiguracion : IEntityTypeConfiguration<EstudiantePotenciador>
    {
        public void Configure(EntityTypeBuilder<EstudiantePotenciador> builder)
        {
            builder.ToTable("EstudiantePotenciadores");

            builder.HasKey(ep => ep.Id);

            builder.Property(ep => ep.FechaActivacion)
                   .IsRequired();

            builder.Property(ep => ep.EstudianteId)
                   .IsRequired();

            // 1:1 con Estudiante usando FK separada
            builder.HasOne(ep => ep.Est)
                   .WithOne(e => e.EstPotenciador)
                   .HasForeignKey<EstudiantePotenciador>(ep => ep.EstudianteId);

            // 1:N a Potenciador
            builder.HasOne(ep => ep.Potenciador)
                   .WithMany()
                   .HasForeignKey(ep => ep.PotenciadorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
