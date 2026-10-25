using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Login;
using LogicaAplicacion.InterfacesCasosUsos.Jwt;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion
{
    public class PruebasLogin
    {
        private readonly Mock<UserManager<Usuario>> _mockUserManager;
        private readonly Mock<SignInManager<Usuario>> _mockSignInManager;
        private readonly Mock<IManejadorJwt> _mockManejadorJwt;
        private readonly LoginUsuario _service;

        public PruebasLogin()
        {
            _mockUserManager = MockUserManager();
            _mockSignInManager = MockSignInManager(_mockUserManager.Object);
            _mockManejadorJwt = new Mock<IManejadorJwt>();

            _service = new LoginUsuario(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockManejadorJwt.Object);
        }

        [Fact]
        public async Task EjecutarAsync_LoginExitoso_RetornaDtoConToken()
        {
            // Arrange
            var loginDto = new LoginSolicitudDto
            {
                NombreUsuario = "usuario1",
                Contrasenia = "clave123"
            };
            var usuario = new Usuario
            {
                Id = "id1",
                UserName = "usuario1"
            };

            _mockSignInManager
                .Setup(s => s.PasswordSignInAsync(loginDto.NombreUsuario, loginDto.Contrasenia, false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockUserManager
                .Setup(u => u.FindByNameAsync(loginDto.NombreUsuario))
                .ReturnsAsync(usuario);

            _mockUserManager
                .Setup(u => u.GetRolesAsync(usuario))
                .ReturnsAsync(new List<string> { "Profesor" });

            _mockManejadorJwt
                .Setup(j => j.GenerarToken(usuario.Id, usuario.UserName, "Profesor"))
                .Returns("token-fake");

            // Act
            var resultado = await _service.EjecutarAsync(loginDto);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal("usuario1", resultado.Valor.NombreUsuario);
            Assert.Equal("id1", resultado.Valor.Id);
            Assert.Equal("Profesor", resultado.Valor.Rol);
            Assert.Equal("token-fake", resultado.Valor.Token);
        }

        [Fact]
        public async Task EjecutarAsync_LoginFallido_RetornaFalla()
        {
            // Arrange
            var loginDto = new LoginSolicitudDto
            {
                NombreUsuario = "usuario1",
                Contrasenia = "clave123"
            };

            _mockSignInManager
                .Setup(s => s.PasswordSignInAsync(loginDto.NombreUsuario, loginDto.Contrasenia, false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var resultado = await _service.EjecutarAsync(loginDto);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation");
        }

        [Fact]
        public async Task EjecutarAsync_CuentaBloqueada_RetornaFallaLockout()
        {
            // Arrange
            var loginDto = new LoginSolicitudDto
            {
                NombreUsuario = "usuario1",
                Contrasenia = "clave123"
            };

            _mockSignInManager
                .Setup(s => s.PasswordSignInAsync(loginDto.NombreUsuario, loginDto.Contrasenia, false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            // Act
            var resultado = await _service.EjecutarAsync(loginDto);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation" && e.Mensaje.Contains("Cuenta bloqueada"));
        }

        [Fact]
        public async Task EjecutarAsync_UsuarioNoEncontrado_RetornaFalla()
        {
            // Arrange
            var loginDto = new LoginSolicitudDto
            {
                NombreUsuario = "usuario1",
                Contrasenia = "clave123"
            };

            _mockSignInManager
                .Setup(s => s.PasswordSignInAsync(loginDto.NombreUsuario, loginDto.Contrasenia, false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockUserManager
                .Setup(u => u.FindByNameAsync(loginDto.NombreUsuario))
                .ReturnsAsync((Usuario?)null);

            // Act
            var resultado = await _service.EjecutarAsync(loginDto);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.NotFound");
        }

        [Fact]
        public async Task EjecutarAsync_UsuarioSinRol_RetornaDtoConRolNull()
        {
            // Arrange
            var loginDto = new LoginSolicitudDto
            {
                NombreUsuario = "usuario1",
                Contrasenia = "clave123"
            };

            var usuario = new Usuario
            {
                Id = "id1",
                UserName = "usuario1"
            };

            _mockSignInManager
                .Setup(s => s.PasswordSignInAsync(loginDto.NombreUsuario, loginDto.Contrasenia, false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockUserManager
                .Setup(u => u.FindByNameAsync(loginDto.NombreUsuario))
                .ReturnsAsync(usuario);

            _mockUserManager
                .Setup(u => u.GetRolesAsync(usuario))
                .ReturnsAsync(new List<string>()); // sin roles

            _mockManejadorJwt
                .Setup(j => j.GenerarToken(usuario.Id, usuario.UserName, null))
                .Returns("token-fake");

            // Act
            var resultado = await _service.EjecutarAsync(loginDto);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.Null(resultado.Valor!.Rol);
            Assert.Equal("token-fake", resultado.Valor.Token);
        }


        // Helpers para mockear UserManager y SignInManager
        private static Mock<UserManager<Usuario>> MockUserManager()
        {
            var store = new Mock<IUserStore<Usuario>>();
            return new Mock<UserManager<Usuario>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<SignInManager<Usuario>> MockSignInManager(UserManager<Usuario> userManager)
        {
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<Usuario>>();
            return new Mock<SignInManager<Usuario>>(userManager,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null, null);
        }
    }
}