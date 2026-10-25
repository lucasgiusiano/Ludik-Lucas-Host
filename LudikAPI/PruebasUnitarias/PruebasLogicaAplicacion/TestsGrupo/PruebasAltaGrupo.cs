using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using Xunit;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Grupos;
using LogicaNegocio.Resultados;
using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using LogicaAplicacion.Servicios;
using Entidad = LogicaNegocio.Entidades;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Grupo
{
    public class PruebasAltaGrupo
    {
        private readonly Mock<IRepositorioGrupos> _repoGruposMock;
        private readonly Mock<IRepositorioTablasEquivalencia> _repoTablasMock;
        private readonly Mock<IGeneradorEnlaceGrupo> _generadorEnlaceMock;
        private readonly Mock<IRepositorioProfesores> _repoProfesoresMock;
        private readonly Mock<ICrearTiendaPorDefecto> _crearTiendaPorDefectoMock;

        public PruebasAltaGrupo()
        {
            _repoGruposMock = new Mock<IRepositorioGrupos>();
            _repoTablasMock = new Mock<IRepositorioTablasEquivalencia>();
            _generadorEnlaceMock = new Mock<IGeneradorEnlaceGrupo>();
            _repoProfesoresMock = new Mock<IRepositorioProfesores>();
            _crearTiendaPorDefectoMock = new Mock<ICrearTiendaPorDefecto>();

            // Setups por defecto para evitar NullReferenceException y reducir repetición en tests.
            _crearTiendaPorDefectoMock
                .Setup(c => c.CrearAsync())
                .ReturnsAsync(Resultado<Entidad.Tienda>.Exitoso(new Entidad.Tienda()));

            _repoGruposMock
                       .Setup(r => r.AddAsync(It.IsAny<Entidad.Grupo>()))
                       .ReturnsAsync(Resultado.Exitoso());

            _generadorEnlaceMock
                .Setup(g => g.GenerarEnlace(It.IsAny<string>()))
                .Returns(Resultado<string>.Exitoso("https://fakeurl"));

            _repoTablasMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(new TablaEquivalencia { Id = 1 }));

            _repoProfesoresMock
                .Setup(r => r.GetByStringIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Resultado<Entidad.Profesor>.Exitoso(
                    new Entidad.Profesor
                    {
                        TablasEquivalencia = new List<TablaEquivalencia>
                        {
                            new TablaEquivalencia { Id = 1 }
                        }
                    }));
        }

        [Fact]
        public async Task Ejecutar_DtoNulo_RetornaErrorValidacion()
        {
            // Arrange
            var servicio = new AltaGrupo(
                _repoGruposMock.Object,
                _repoTablasMock.Object,
                _generadorEnlaceMock.Object,
                _repoProfesoresMock.Object,
                _crearTiendaPorDefectoMock.Object);

            // Act
            var resultado = await servicio.EjecutarAsync(null!, "profesor123");

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains("No hay información", resultado.Errores[0].Mensaje);
        }

        [Fact]
        public async Task Ejecutar_TablaEquivalenciaInexistente_RetornaErrorNotFound()
        {
            // Arrange
            var dto = new GrupoAltaRequestDto
            {
                Nombre = "Grupo Test",
                TablaEquivalenciaId = 99
            };

            // Antes devolvías null; ahora simulamos fallo explícito en el Resultado
            _repoTablasMock.Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Resultado<TablaEquivalencia>)null!);

            var servicio = new AltaGrupo(
                _repoGruposMock.Object,
                _repoTablasMock.Object,
                _generadorEnlaceMock.Object,
                _repoProfesoresMock.Object,
                _crearTiendaPorDefectoMock.Object);

            // Act
            var resultado = await servicio.EjecutarAsync(dto, "profesor123");

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains("tabla de equivalencia", resultado.Errores[0].Mensaje);
        }

        [Fact]
        public async Task Ejecutar_ErrorEnGeneracionUrl_RetornaErrorUnexpected()
        {
            // Arrange
            var dto = new GrupoAltaRequestDto
            {
                Nombre = "Grupo Test",
                TablaEquivalenciaId = 1
            };

            // tabla con Id explícito
            var tabla = new TablaEquivalencia { Id = 1 };
            _repoTablasMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(tabla));

            // Profesor que contiene la tabla con Id = 1
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                TablasEquivalencia = new List<TablaEquivalencia>
                {
                    new TablaEquivalencia { Id = 1 }
                }
            };
            _repoProfesoresMock.Setup(r => r.GetByStringIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));

            // Generador de enlace falla (sobrescribe el setup por defecto)
            _generadorEnlaceMock.Setup(g => g.GenerarEnlace(It.IsAny<string>()))
                .Returns(Resultado<string>.Falla(new Error("Error", "Falló enlace")));

            var servicio = new AltaGrupo(
                _repoGruposMock.Object,
                _repoTablasMock.Object,
                _generadorEnlaceMock.Object,
                _repoProfesoresMock.Object,
                _crearTiendaPorDefectoMock.Object);

            // Act
            var resultado = await servicio.EjecutarAsync(dto, "profesor123");

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains("URL", resultado.Errores[0].Mensaje);
        }

        [Fact]
        public async Task Ejecutar_DatosInvalidos_RetornaErroresDeValidacion()
        {
            // Arrange
            var dto = new GrupoAltaRequestDto
            {
                Nombre = "", // Inválido
                TablaEquivalenciaId = 1
            };

            var tabla = new TablaEquivalencia { Id = 1 };
            _repoTablasMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(tabla));

            _generadorEnlaceMock.Setup(g => g.GenerarEnlace(It.IsAny<string>()))
                .Returns(Resultado<string>.Exitoso("https://fakeurl"));

            // Profesor que contiene la tabla (para pasar la validación de pertenencia)
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                TablasEquivalencia = new List<TablaEquivalencia>
                {
                    new TablaEquivalencia { Id = 1 }
                }
            };
            _repoProfesoresMock.Setup(r => r.GetByStringIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));

            // NOTA: no forzamos que GetByStringIdAsync falle para la cadena vacía,
            // porque en tu implementación actual la validación de los campos (Nombre y ProfesorId)
            // ocurre en el método grupo.esValido() y queremos que ese método agregue ambos errores
            // (nombre vacío y profesorId vacío). Con los setups por defecto y el setup de CrearAsync
            // la ejecución llegará hasta la validación del grupo y retornará los errores esperados.

            var servicio = new AltaGrupo(
                _repoGruposMock.Object,
                _repoTablasMock.Object,
                _generadorEnlaceMock.Object,
                _repoProfesoresMock.Object,
                _crearTiendaPorDefectoMock.Object);

            // Act
            var resultado = await servicio.EjecutarAsync(dto, "");

            // Assert
            Assert.True(resultado.EsFallo);
            // Verificamos el mensaje de validación de nombre
            Assert.Contains(resultado.Errores, e =>
                e.Mensaje.Contains("El nombre del grupo") || e.Mensaje.Contains("El nombre del grupo debe"));
            // Verificamos el mensaje de validación de profesorId
            Assert.Contains(resultado.Errores, e =>
                e.Mensaje.Contains("profesor") || e.Mensaje.Contains("identificador"));
        }

        [Fact]
        public async Task Ejecutar_DatosValidos_CreaGrupo()
        {
            // Arrange
            var dto = new GrupoAltaRequestDto
            {
                Nombre = "Grupo Válido",
                TablaEquivalenciaId = 1,
                Institucion = "Instituto",
                Materia = "Matemática"
            };

            var tabla = new TablaEquivalencia { Id = 1 };
            _repoTablasMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(tabla));

            _generadorEnlaceMock.Setup(g => g.GenerarEnlace(It.IsAny<string>()))
                .Returns(Resultado<string>.Exitoso("https://fakeurl"));

            // Profesor que contiene la tabla (para pasar la validación de pertenencia)
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                TablasEquivalencia = new List<TablaEquivalencia>
                {
                    new TablaEquivalencia { Id = 1 }
                }
            };
            _repoProfesoresMock.Setup(r => r.GetByStringIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));

            var servicio = new AltaGrupo(
                _repoGruposMock.Object,
                _repoTablasMock.Object,
                _generadorEnlaceMock.Object,
                _repoProfesoresMock.Object,
                _crearTiendaPorDefectoMock.Object);

            // Act
            var resultado = await servicio.EjecutarAsync(dto, "profesor123");

            // Assert
            Assert.True(resultado.EsExitoso);
            _repoGruposMock.Verify(r => r.AddAsync(It.IsAny<Entidad.Grupo>()), Times.Once);
        }

        [Fact]
        public async Task Ejecutar_ProfesorIdInexistente_RetornaErrorNotFound()
        {
            // Arrange
            var dto = new GrupoAltaRequestDto { Nombre = "Grupo", TablaEquivalenciaId = 1 };

            _repoTablasMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(new TablaEquivalencia { Id = 1 }));

            _repoProfesoresMock.Setup(r => r.GetByStringIdAsync("idInexistente"))
                .ReturnsAsync(Resultado<Entidad.Profesor>.Falla(new Error("Error.NotFound", "Profesor no encontrado")));

            var servicio = new AltaGrupo(
                _repoGruposMock.Object,
                _repoTablasMock.Object,
                _generadorEnlaceMock.Object,
                _repoProfesoresMock.Object,
                _crearTiendaPorDefectoMock.Object);

            // Act
            var resultado = await servicio.EjecutarAsync(dto, "idInexistente");

            // Assert
            Assert.True(resultado.EsFallo);
        }

        [Fact]
        public async Task Ejecutar_TablaNoPerteneceAlProfesor_RetornaErrorUnauthorized()
        {
            // Arrange
            var dto = new GrupoAltaRequestDto { Nombre = "Grupo", TablaEquivalenciaId = 1 };
            var tabla = new TablaEquivalencia { Id = 1 };
            // El profesor tiene una lista de tablas, pero NO contiene la tabla con Id = 1
            var profesor = new Entidad.Profesor { TablasEquivalencia = new List<TablaEquivalencia> { new TablaEquivalencia { Id = 2 } } };

            _repoTablasMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(tabla));
            _repoProfesoresMock.Setup(r => r.GetByStringIdAsync(It.IsAny<string>())).ReturnsAsync(Resultado<Entidad.Profesor>.Exitoso(profesor));

            var servicio = new AltaGrupo(
                _repoGruposMock.Object,
                _repoTablasMock.Object,
                _generadorEnlaceMock.Object,
                _repoProfesoresMock.Object,
                _crearTiendaPorDefectoMock.Object);

            // Act
            var resultado = await servicio.EjecutarAsync(dto, "profesor123");

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Unauthorized", resultado.Errores[0].Codigo);
        }
    }
}
