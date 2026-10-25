using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF.Configuraciones;

public static class TablasIdentityConfiguracion
{
    public static void ConfigurarTablasIdentity(this ModelBuilder builder)
    {
        builder.Entity<IdentityRole>(b =>
        {
            b.ToTable("Roles");
            b.Property(r => r.Id).HasColumnName("RolId");
            b.Property(r => r.Name).HasColumnName("NombreRol");
            b.Property(r => r.NormalizedName).HasColumnName("NombreRolNormalizado");
            b.Property(r => r.ConcurrencyStamp).HasColumnName("EstampaConcurrencia");
        });

        builder.Entity<IdentityUserRole<string>>(b =>
        {
            b.ToTable("UsuariosRoles");
            b.Property(ur => ur.UserId).HasColumnName("UsuarioId");
            b.Property(ur => ur.RoleId).HasColumnName("RolId");
        });

        builder.Entity<IdentityUserClaim<string>>(b =>
        {
            b.ToTable("ReclamacionesUsuario");
            b.Property(uc => uc.Id).HasColumnName("ReclamacionUsuarioId");
            b.Property(uc => uc.UserId).HasColumnName("UsuarioId");
            b.Property(uc => uc.ClaimType).HasColumnName("TipoReclamacion");
            b.Property(uc => uc.ClaimValue).HasColumnName("ValorReclamacion");
        });

        builder.Entity<IdentityUserLogin<string>>(b =>
        {
            b.ToTable("IniciosSesionUsuario");
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            b.Property(l => l.LoginProvider).HasColumnName("Proveedor");
            b.Property(l => l.ProviderKey).HasColumnName("ClaveProveedor");
            b.Property(l => l.ProviderDisplayName).HasColumnName("NombreProveedor");
            b.Property(l => l.UserId).HasColumnName("UsuarioId");
        });

        builder.Entity<IdentityRoleClaim<string>>(b =>
        {
            b.ToTable("ReclamacionesRoles");
            b.Property(rc => rc.Id).HasColumnName("ReclamacionRolId");
            b.Property(rc => rc.RoleId).HasColumnName("RolId");
            b.Property(rc => rc.ClaimType).HasColumnName("TipoReclamacion");
            b.Property(rc => rc.ClaimValue).HasColumnName("ValorReclamacion");
        });

        builder.Entity<IdentityUserToken<string>>(b =>
        {
            b.ToTable("TokensUsuario");
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            b.Property(t => t.UserId).HasColumnName("UsuarioId");
            b.Property(t => t.LoginProvider).HasColumnName("Proveedor");
            b.Property(t => t.Name).HasColumnName("NombreToken");
            b.Property(t => t.Value).HasColumnName("ValorToken");
        });
    }
}
