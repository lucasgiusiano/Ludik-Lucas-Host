using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        // Renombrar columnas de Identity
        builder.Property(u => u.Id).HasColumnName("UsuarioId");
        builder.Property(u => u.UserName).HasColumnName("NombreUsuario");
        builder.Property(u => u.NormalizedUserName).HasColumnName("NombreUsuarioNormalizado");
        builder.Property(u => u.Email).HasColumnName("Correo");
        builder.Property(u => u.NormalizedEmail).HasColumnName("CorreoNormalizado");
        builder.Property(u => u.EmailConfirmed).HasColumnName("CorreoConfirmado");
        builder.Property(u => u.PasswordHash).HasColumnName("ContraseniaHash");
        builder.Property(u => u.SecurityStamp).HasColumnName("EstampaSeguridad");
        builder.Property(u => u.ConcurrencyStamp).HasColumnName("EstampaConcurrencia");
        builder.Property(u => u.PhoneNumber).HasColumnName("Telefono");
        builder.Property(u => u.PhoneNumberConfirmed).HasColumnName("TelefonoConfirmado");
        builder.Property(u => u.TwoFactorEnabled).HasColumnName("AutenticacionDosFactores");
        builder.Property(u => u.LockoutEnd).HasColumnName("FinBloqueo");
        builder.Property(u => u.LockoutEnabled).HasColumnName("BloqueoHabilitado");
        builder.Property(u => u.AccessFailedCount).HasColumnName("IntentosFallidos");

        // Configurar Owned Type para NombreCompleto
        builder.OwnsOne(u => u.NombreCompleto, nc =>
        {
            nc.Property(x => x.Nombre)
                .HasColumnName("Nombre")
                .IsRequired()
                .HasMaxLength(20);

            nc.Property(x => x.Apellido)
                .HasColumnName("Apellido")
                .IsRequired()
                .HasMaxLength(20);
        });
    }
}