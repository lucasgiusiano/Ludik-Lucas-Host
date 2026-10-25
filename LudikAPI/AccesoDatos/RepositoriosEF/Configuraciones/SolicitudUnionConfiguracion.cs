using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class SolicitudUnionConfiguracion : IEntityTypeConfiguration<SolicitudUnion>
{
    public void Configure(EntityTypeBuilder<SolicitudUnion> builder)
    {
        // No se puede borrar un Estudiante si tiene solicitudes pendientes.
        builder.HasOne(s => s.Estudiante)
            .WithMany()
            .HasForeignKey(s => s.EstudianteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}