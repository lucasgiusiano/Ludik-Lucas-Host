using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Estudiantes;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Estudiante
{
    public class PruebasAltaEstudiante
    {
        private readonly Mock<UserManager<Usuario>> _userManagerMock;
        private readonly Mock<IPasswordHasher<Usuario>> _passwordHasherMock;
        private readonly AltaEstudiante _service;

        public PruebasAltaEstudiante()
        {
            var storeMock = new Mock<IUserStore<Usuario>>();
            _userManagerMock = new Mock<UserManager<Usuario>>(
                storeMock.Object, null, null, null, null, null, null, null, null);
            _passwordHasherMock = new Mock<IPasswordHasher<Usuario>>();
            _service = new AltaEstudiante(_userManagerMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Ejecutar_DtoNulo_RetornaValidationError()
        {
            var resultado = await _service.EjecutarAsync(null!);

            Assert.True(resultado.EsFallo);
            var error = resultado.Errores.First();
            Assert.Equal("Error.Validation", error.Codigo);
            Assert.Equal("Los datos del estudiante no pueden ser nulos.", error.Mensaje);
        }

        [Fact]
        public async Task Ejecutar_SinPreguntasDeSeguridad_RetornaValidationError()
        {
            var dto = new EstudianteAltaDto
            {
                NombreUsuario = "user1",
                Nombre = "Juan",
                Apellido = "Perez",
                Contrasenia = "Pwd1",
                PreguntasDeSeguridad = null
            };

            var resultado = await _service.EjecutarAsync(dto);

            Assert.True(resultado.EsFallo);
            var error = resultado.Errores.First();
            Assert.Equal("Error.Validation", error.Codigo);
            Assert.Equal("Se deben proporcionar exactamente 2 preguntas de seguridad.", error.Mensaje);
        }

        [Fact]
        public async Task Ejecutar_PreguntasDuplicadas_RetornaValidationError()
        {
            var dto = new EstudianteAltaDto
            {
                NombreUsuario = "user2",
                Nombre = "Juan",
                Apellido = "Perez",
                Contrasenia = "Pwd2",
                PreguntasDeSeguridad = new List<PreguntaSeguridadSeleccionadaDto>
                {
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 10, Respuesta = "R1" },
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 10, Respuesta = "R2" }
                }
            };

            var resultado = await _service.EjecutarAsync(dto);

            Assert.True(resultado.EsFallo);
            var error = resultado.Errores.First();
            Assert.Equal("Error.Validation", error.Codigo);
            Assert.Equal("Debe seleccionar dos preguntas de seguridad diferentes.", error.Mensaje);
        }

        [Fact]
        public async Task Ejecutar_UsuarioExistente_RetornaConflictError()
        {
            var dto = new EstudianteAltaDto
            {
                NombreUsuario = "user3",
                Nombre = "Juan",
                Apellido = "Perez",
                Contrasenia = "Pwd3",
                PreguntasDeSeguridad = new List<PreguntaSeguridadSeleccionadaDto>
                {
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 1, Respuesta = "R1" },
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 2, Respuesta = "R2" }
                }
            };
            _userManagerMock.Setup(u => u.FindByNameAsync("user3"))
                .ReturnsAsync(new Usuario());

            var resultado = await _service.EjecutarAsync(dto);

            Assert.True(resultado.EsFallo);
            var error = resultado.Errores.First();
            Assert.Equal("Error.Conflict", error.Codigo);
            Assert.Equal("El nombre de usuario ya está en uso.", error.Mensaje);
        }

        [Fact]
        public async Task Ejecutar_CreacionFalla_RetornaValidationErrors()
        {
            var dto = new EstudianteAltaDto
            {
                NombreUsuario = "user4",
                Nombre = "Juan",
                Apellido = "Perez",
                Contrasenia = "Pwd4",
                PreguntasDeSeguridad = new List<PreguntaSeguridadSeleccionadaDto>
                {
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 1, Respuesta = "R1" },
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 2, Respuesta = "R2" }
                }
            };
            _userManagerMock.Setup(u => u.FindByNameAsync("user4"))
                .ReturnsAsync((Usuario)null!);
            var identityErrors = new[] { new IdentityError { Description = "Invalid pass" } };
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<Usuario>(), dto.Contrasenia))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            var resultado = await _service.EjecutarAsync(dto);

            Assert.True(resultado.EsFallo);
            var err = resultado.Errores.First();
            Assert.Equal("Error.Validation", err.Codigo);
            Assert.Contains("Invalid pass", err.Mensaje);
        }

        [Fact]
        public async Task Ejecutar_RolFalla_EliminaUsuarioYRetornaUnexpectedError()
        {
            var dto = new EstudianteAltaDto
            {
                NombreUsuario = "user5",
                Nombre = "Juan",
                Apellido = "Perez",
                Contrasenia = "Pwd5",
                PreguntasDeSeguridad = new List<PreguntaSeguridadSeleccionadaDto>
                {
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 1, Respuesta = "R1" },
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 2, Respuesta = "R2" }
                }
            };
            _userManagerMock.Setup(u => u.FindByNameAsync("user5"))
                .ReturnsAsync((Usuario)null!);
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<Usuario>(), dto.Contrasenia))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<Usuario>(), "Estudiante"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "No role" }));

            var resultado = await _service.EjecutarAsync(dto);

            Assert.True(resultado.EsFallo);
            var err = resultado.Errores.First();
            Assert.Equal("Error.Unexpected", err.Codigo);
            _userManagerMock.Verify(u => u.DeleteAsync(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task Ejecutar_DatosValidos_CreaUsuarioYAsignaRol()
        {
            var dto = new EstudianteAltaDto
            {
                NombreUsuario = "user6",
                Nombre = "Juan",
                Apellido = "Perez",
                Contrasenia = "Pwd6",
                PreguntasDeSeguridad = new List<PreguntaSeguridadSeleccionadaDto>
                {
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 1, Respuesta = "R1" },
                    new PreguntaSeguridadSeleccionadaDto { PreguntaId = 2, Respuesta = "R2" }
                }
            };
            _userManagerMock.Setup(u => u.FindByNameAsync("user6"))
                .ReturnsAsync((Usuario)null!);
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<Usuario>(), dto.Contrasenia))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<Usuario>(), "Estudiante"))
                .ReturnsAsync(IdentityResult.Success);

            var resultado = await _service.EjecutarAsync(dto);

            Assert.True(resultado.EsExitoso);
            _userManagerMock.Verify(u => u.CreateAsync(It.IsAny<Usuario>(), dto.Contrasenia), Times.Once);
            _userManagerMock.Verify(u => u.AddToRoleAsync(It.IsAny<Usuario>(), "Estudiante"), Times.Once);
        }
    }
}
