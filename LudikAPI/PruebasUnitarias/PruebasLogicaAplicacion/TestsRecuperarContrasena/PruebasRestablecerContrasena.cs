using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;
using LogicaAplicacion.DTOs.RestablecerContrasenaDTO;
using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.RecuperarContrasena;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using InterfacesRepositorio;

namespace PruebasUnitarias.PruebasLogicaAplicacion.RecuperarContrasena
{
    public class PruebasRestablecerContrasena
    {
        private readonly Mock<IRepositorioUsuarios> _repoUsuariosMock;
        private readonly Mock<UserManager<Usuario>> _userManagerMock;
        private readonly IPasswordHasher<Usuario> _hasher;
        private readonly Mock<IRepositorioPreguntasSeguridad> _repoPregMock;

        public PruebasRestablecerContrasena()
        {
            _repoUsuariosMock = new Mock<IRepositorioUsuarios>();
            _repoPregMock = new Mock<IRepositorioPreguntasSeguridad>();
            _hasher = new PasswordHasher<Usuario>();

            var store = new Mock<IUserStore<Usuario>>();
            _userManagerMock = new Mock<UserManager<Usuario>>(
                store.Object, null, null, null, null, null, null, null, null
            );
        }

        private RestablecerContrasena CreateService() =>
            new RestablecerContrasena(
                _repoUsuariosMock.Object,
                _userManagerMock.Object,
                _hasher,
                _repoPregMock.Object
            );

        [Fact]
        public async Task EjecutarAsync_UsuarioNoExiste_RetornaNotFound()
        {
            // Arrange
            var dto = new InformacionParaRestablecerContrasenaDto
            {
                NombreUsuario = "inexistente",
                Respuestas = new List<RespuestaDto>(),
                NuevaContrasena = "ABC123!!"
            };
            _repoUsuariosMock
                .Setup(r => r.GetUsuarioPorNombreAsync("inexistente"))
                .ReturnsAsync(Resultado<Usuario>.Falla(new[] { Error.NotFound }));

            var svc = CreateService();

            // Act
            var resultado = await svc.EjecutarAsync(dto);

            // Assert
            Assert.False(resultado.EsExitoso);
            Assert.Contains(resultado.Errores, e => e.Codigo == Error.NotFound.Codigo);
            _repoPregMock.Verify(r => r.GetByNombreUsuarioAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(u => u.GeneratePasswordResetTokenAsync(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_RespuestasIncorrectas_RetornaUnauthorized()
        {
            // Arrange: creamos un Estudiante real con una pregunta y hash distinto
            var estudiante = new LogicaNegocio.Entidades.Estudiante { UserName = "juan" };
            var pregunta = new PreguntaRespuestaSeguridad
            {
                Id = 1,
                Respuesta = _hasher.HashPassword(estudiante, "respuesta-correcta")
            };
            estudiante.PreguntasSeguridad = new List<PreguntaRespuestaSeguridad> { pregunta };

            _repoUsuariosMock
                .Setup(r => r.GetUsuarioPorNombreAsync("juan"))
                .ReturnsAsync(Resultado<Usuario>.Exitoso(estudiante));
            _repoPregMock
                .Setup(r => r.GetByNombreUsuarioAsync("juan"))
                .ReturnsAsync(Resultado<List<PreguntaRespuestaSeguridad>>.Exitoso(estudiante.PreguntasSeguridad));

            var dto = new InformacionParaRestablecerContrasenaDto
            {
                NombreUsuario = "juan",
                Respuestas = new List<RespuestaDto>
                {
                    // enviamos una respuesta distinta => debe fallar
                    new RespuestaDto {
                        PreguntaRespuestaSeguridadId = 1,
                        PreguntaDeSeguridadId        = pregunta.PreguntaDeSeguridadId,
                        Respuesta                    = "respuesta-incorrecta"
                    }
                },
                NuevaContrasena = "NoSeUsa"
            };

            var svc = CreateService();

            // Act
            var resultado = await svc.EjecutarAsync(dto);

            // Assert
            Assert.False(resultado.EsExitoso);
            Assert.Contains(resultado.Errores, e => e.Codigo == Error.Unauthorized.Codigo);
            _userManagerMock.Verify(u => u.GeneratePasswordResetTokenAsync(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_ResetPasswordFails_RetornaValidationErrors()
        {
            // Arrange: creamos un Estudiante real con respuestas que coinciden
            var estudiante = new LogicaNegocio.Entidades.Estudiante { UserName = "ana" };
            var pregunta = new PreguntaRespuestaSeguridad
            {
                Id = 42,
                Respuesta = _hasher.HashPassword(estudiante, "mi-segunda")
            };
            estudiante.PreguntasSeguridad = new List<PreguntaRespuestaSeguridad> { pregunta };

            _repoUsuariosMock
                .Setup(r => r.GetUsuarioPorNombreAsync("ana"))
                .ReturnsAsync(Resultado<Usuario>.Exitoso(estudiante));
            _repoPregMock
                .Setup(r => r.GetByNombreUsuarioAsync("ana"))
                .ReturnsAsync(Resultado<List<PreguntaRespuestaSeguridad>>.Exitoso(estudiante.PreguntasSeguridad));

            // Forzamos fallo en ResetPasswordAsync
            _userManagerMock
                .Setup(u => u.GeneratePasswordResetTokenAsync(estudiante))
                .ReturnsAsync("tok-valido");
            _userManagerMock
                .Setup(u => u.ResetPasswordAsync(estudiante, "tok-valido", "nuevaPwd"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Pwd inválida" }));

            var dto = new InformacionParaRestablecerContrasenaDto
            {
                NombreUsuario = "ana",
                Respuestas = new List<RespuestaDto>
                {
                    new RespuestaDto {
                        PreguntaRespuestaSeguridadId = 42,
                        PreguntaDeSeguridadId        = pregunta.PreguntaDeSeguridadId,
                        Respuesta                    = "mi-segunda"
                    }
                },
                NuevaContrasena = "nuevaPwd"
            };

            var svc = CreateService();

            // Act
            var resultado = await svc.EjecutarAsync(dto);

            // Assert
            Assert.False(resultado.EsExitoso);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation");
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("Pwd inválida"));
        }

        [Fact]
        public async Task EjecutarAsync_DatosValidos_DevuelveExitoso()
        {
            // Arrange: Estudiante real y hasheo correcto
            var estudiante = new LogicaNegocio.Entidades.Estudiante { UserName = "luis" };
            var preg = new PreguntaRespuestaSeguridad
            {
                Id = 99,
                Respuesta = _hasher.HashPassword(estudiante, "ok-pass")
            };
            estudiante.PreguntasSeguridad = new List<PreguntaRespuestaSeguridad> { preg };

            _repoUsuariosMock
                .Setup(r => r.GetUsuarioPorNombreAsync("luis"))
                .ReturnsAsync(Resultado<Usuario>.Exitoso(estudiante));
            _repoPregMock
                .Setup(r => r.GetByNombreUsuarioAsync("luis"))
                .ReturnsAsync(Resultado<List<PreguntaRespuestaSeguridad>>.Exitoso(estudiante.PreguntasSeguridad));

            _userManagerMock
                .Setup(u => u.GeneratePasswordResetTokenAsync(estudiante))
                .ReturnsAsync("token-ok");
            _userManagerMock
                .Setup(u => u.ResetPasswordAsync(estudiante, "token-ok", "ok-pass"))
                .ReturnsAsync(IdentityResult.Success);

            var dto = new InformacionParaRestablecerContrasenaDto
            {
                NombreUsuario = "luis",
                Respuestas = new List<RespuestaDto>
                {
                    new RespuestaDto {
                        PreguntaRespuestaSeguridadId = 99,
                        PreguntaDeSeguridadId        = preg.PreguntaDeSeguridadId,
                        Respuesta                    = "ok-pass"
                    }
                },
                NuevaContrasena = "ok-pass"
            };

            var svc = CreateService();

            // Act
            var resultado = await svc.EjecutarAsync(dto);

            // Assert
            Assert.True(resultado.EsExitoso);
        }
    }
}