using System;
using Moq;
using Xunit;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.ProfesorDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Profesores;
using LogicaAplicacion.Servicios;
using LogicaNegocio.ValueObjects;
using LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Identity;
using LogicaNegocio.Resultados;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Profesor
{
    public class PruebasAltaProfesor
    {
        private readonly Mock<UserManager<Usuario>> _userManagerMock;
        private readonly Mock<IServicioCrearObjetosParaProfesor> _servicioCrearObjetosMock;

        public PruebasAltaProfesor()
        {
            var storeMock = new Mock<IUserStore<Usuario>>();
            _userManagerMock = new Mock<UserManager<Usuario>>(
                storeMock.Object, null, null, null, null, null, null, null, null
            );
            _servicioCrearObjetosMock = new Mock<IServicioCrearObjetosParaProfesor>();
        }

        [Fact]
        public async Task EjecutarAsync_DtoNulo_DevuelveResultadoFallido()
        {
            // Arrange
            var service = new AltaProfesor(_userManagerMock.Object, _servicioCrearObjetosMock.Object);
            ProfesorAltaDto dto = null!;

            // Act
            var resultado = await service.EjecutarAsync(dto);

            // Assert
            Assert.False(resultado.EsExitoso);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation");
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("no pueden ser nulos"));
        }

        [Fact]
        public async Task EjecutarAsync_NombreUsuarioExiste_DevuelveResultadoFallido()
        {
            // Arrange
            var dto = new ProfesorAltaDto
            {
                Correo = "pepito@Entidad.com",
                NombreUsuario = "pepito123",
                Nombre = "Pepito",
                Apellido = "González",
                Contrasenia = "Abc12345!"
            };

            // Simulamos que ya existe un usuario con ese username
            _userManagerMock
                .Setup(x => x.FindByNameAsync("pepito123"))
                .ReturnsAsync(new Usuario());

            // IMPORTANTE: le pasamos también el mock de servicioCrearObjetos, aunque no se use aquí
            var service = new AltaProfesor(
                _userManagerMock.Object,
                _servicioCrearObjetosMock.Object);

            // Act
            var resultado = await service.EjecutarAsync(dto);

            // Assert
            Assert.False(resultado.EsExitoso);
            Assert.Contains(resultado.Errores, e => e.Codigo == Error.Conflict.Codigo);
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("ya está en uso"));

            // Verificamos que NO se intentó crear usuario ni asignar rol
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<Usuario>(), It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<Usuario>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_DatosValidos_CreaUsuarioYAsignaRol()
        {
            // Arrange
            var dto = new ProfesorAltaDto
            {
                Correo = "ana@ejemplo.com",
                NombreUsuario = "anaprof",
                Nombre = "Ana",
                Apellido = "López",
                Contrasenia = "Secret#123"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync("anaprof"))
                .ReturnsAsync((Usuario)null!);
            _userManagerMock.Setup(x => x.FindByEmailAsync("ana@ejemplo.com"))
                .ReturnsAsync((Usuario)null!);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Usuario>(), "Secret#123"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<Usuario>(), "Profesor"))
                .ReturnsAsync(IdentityResult.Success);

            // **Aquí los setups correctos con Profesor, no Usuario**
            _servicioCrearObjetosMock
                .Setup(s => s.CrearMedallasPredeterminadas(It.IsAny<LogicaNegocio.Entidades.Profesor>()))
                .Returns(new List<LogicaNegocio.Entidades.Medalla>());  // devuelve lista vacía

            _servicioCrearObjetosMock
                .Setup(s => s.CrearTablasEquivalenciaPredeterminadas(
                    It.IsAny<LogicaNegocio.Entidades.Profesor>(),
                    It.IsAny<List<LogicaNegocio.Entidades.Medalla>>()))
                .Returns(new List<TablaEquivalencia>());  // idem

            _userManagerMock
                .Setup(x => x.UpdateAsync(It.IsAny<Usuario>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new AltaProfesor(
                _userManagerMock.Object,
                _servicioCrearObjetosMock.Object);

            // Act
            var resultado = await service.EjecutarAsync(dto);

            // Assert
            Assert.True(resultado.EsExitoso);
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<Usuario>(), "Secret#123"), Times.Once);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<Usuario>(), "Profesor"), Times.Once);
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<Usuario>()), Times.Once);
        }
        [Fact]
        public async Task EjecutarAsync_CreacionUsuarioFalla_DevuelveErroresDeIdentity()
        {
            // Arrange
            var dto = new ProfesorAltaDto
            {
                Correo = "nuevo@correo.com",
                NombreUsuario = "nuevoUser",
                Nombre = "Nuevo",
                Apellido = "Apellido",
                Contrasenia = "123" // débil
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(dto.NombreUsuario))
                .ReturnsAsync((Usuario)null!);
            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Correo))
                .ReturnsAsync((Usuario)null!);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Usuario>(), dto.Contrasenia))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "La contraseña es muy débil." }));

            var service = new AltaProfesor(_userManagerMock.Object, _servicioCrearObjetosMock.Object);

            // Act
            var resultado = await service.EjecutarAsync(dto);

            // Assert
            Assert.False(resultado.EsExitoso);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation");
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("débil"));
        }
        [Fact]
        public async Task EjecutarAsync_AsignacionRolFalla_EliminaUsuarioYDevuelveError()
        {
            // Arrange
            var dto = new ProfesorAltaDto
            {
                Correo = "nuevo@correo.com",
                NombreUsuario = "nuevoUser",
                Nombre = "Nuevo",
                Apellido = "Apellido",
                Contrasenia = "Abc12345!"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(dto.NombreUsuario))
                .ReturnsAsync((Usuario)null!);
            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Correo))
                .ReturnsAsync((Usuario)null!);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Usuario>(), dto.Contrasenia))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<Usuario>(), "Profesor"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Rol inválido" }));
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<Usuario>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new AltaProfesor(_userManagerMock.Object, _servicioCrearObjetosMock.Object);

            // Act
            var resultado = await service.EjecutarAsync(dto);

            // Assert
            Assert.False(resultado.EsExitoso);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Unexpected");
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("asignar rol"));
            _userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<Usuario>()), Times.Once);
        }
    }
    //faltan una pruebas mas que tiene que ver con el chequeo de cada campo


}
