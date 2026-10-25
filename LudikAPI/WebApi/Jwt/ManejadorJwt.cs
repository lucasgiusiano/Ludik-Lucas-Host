using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogicaAplicacion.InterfacesCasosUsos.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace WebApi.Jwt
{
    public class ManejadorJwt :IManejadorJwt{

        private readonly IConfiguration _configuracion;

        public ManejadorJwt(IConfiguration config)
        {
            _configuracion = config;
        }

        public string GenerarToken(string usuarioId, string nombreUsuario, string rol)
        {
            var jwtConfig = _configuracion.GetSection("JwtSettings");
            string claveSecreta = jwtConfig.GetValue<string>("Key");
            string issuer = jwtConfig.GetValue<string>("Issuer");
            string audience = jwtConfig.GetValue<string>("Audience");
            int validezMinutos = jwtConfig.GetValue<int>("TokenValidityInMinutes");

            var keyBytes = Encoding.UTF8.GetBytes(claveSecreta);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioId),
                new Claim(ClaimTypes.Name, nombreUsuario),
                new Claim(ClaimTypes.Role, rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(validezMinutos),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
