using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.SolicitudUnion;
using LogicaNegocio.Resultados;
using Entidad = LogicaNegocio.Entidades;

namespace PruebasUnitarias.PruebasLogicaAplicacion.SolicitudUnion
{
    public class PruebasCrearSolicitudUnion
    {
        private readonly Mock<IRepositorioEnlacesUnionGrupo> _repoEnlacesMock;
        private readonly Mock<IRepositorioEstudiantes> _repoEstudiantesMock;
        private readonly Mock<IRepositorioGrupos> _repoGruposMock;
        private readonly Mock<IRepositorioSolicitudesUnion> _repoSolicitudesMock;

        public PruebasCrearSolicitudUnion()
        {
            _repoEnlacesMock = new Mock<IRepositorioEnlacesUnionGrupo>();
            _repoEstudiantesMock = new Mock<IRepositorioEstudiantes>();
            _repoGruposMock = new Mock<IRepositorioGrupos>();
            _repoSolicitudesMock = new Mock<IRepositorioSolicitudesUnion>();
        }

        [Fact]
        public async Task Ejecutar_DtoNulo_RetornaErrorValidacion()
        {
            // Arrange
            var servicio = new CrearSolicitudUnion(
                _repoEnlacesMock.Object,
                _repoEstudiantesMock.Object,
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object
            );

            // Act
            var resultado = await servicio.EjecutarAsync(null!);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains("no pueden ser nulos", resultado.Errores[0].Mensaje);
        }

        [Fact]
        public async Task Ejecutar_EnlaceInvalido_RetornaError()
        {
            // Arrange
            var dto = new SolicitudUnionDto
            {
                IdEstudiante = "estu123",
                CodigoEnlace = "codigoInvalido"
            };

            _repoEnlacesMock.Setup(r => r.ObtenerPorCodigoAsync("codigoInvalido"))
                .ReturnsAsync((EnlaceUnion)null!);

            var servicio = new CrearSolicitudUnion(
                _repoEnlacesMock.Object,
                _repoEstudiantesMock.Object,
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object
            );

            // Act
            var resultado = await servicio.EjecutarAsync(dto);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains("inválido", resultado.Errores[0].Mensaje);
        }
        

        [Fact]
        public async Task Ejecutar_GrupoNoEncontrado_RetornaError()
        {
            // Arrange
            var enlace = new EnlaceUnion("http://fakeurl", "codigo123")
            {
                Expiracion = DateTime.UtcNow.AddMinutes(10)
            };
            var estudiante = new Entidad.Estudiante { Id = "estu123" };
            var dto = new SolicitudUnionDto
            {
                IdEstudiante = "estu123",
                CodigoEnlace = "codigo123"
            };

            _repoEnlacesMock.Setup(r => r.ObtenerPorCodigoAsync("codigo123"))
                .ReturnsAsync(enlace);
            _repoEstudiantesMock.Setup(r => r.GetByStringIdAsync("estu123"))
                .ReturnsAsync(Resultado<Entidad.Estudiante>.Exitoso(estudiante));
            _repoGruposMock.Setup(r => r.ObtenerPorEnlaceAsync("codigo123"))
                .ReturnsAsync((Entidad.Grupo)null!);

            var servicio = new CrearSolicitudUnion(
                _repoEnlacesMock.Object,
                _repoEstudiantesMock.Object,
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object
            );

            // Act
            var resultado = await servicio.EjecutarAsync(dto);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains("grupo asociado", resultado.Errores[0].Mensaje);
        }

        [Fact]
        public async Task Ejecutar_SolicitudYaExiste_RetornaError()
        {
            // Arrange
            var enlace = new EnlaceUnion("http://fakeurl", "codigo123") { Expiracion = DateTime.UtcNow.AddMinutes(10) };
            var estudiante = new Entidad.Estudiante { Id = "estu123" };
            var grupo = new Entidad.Grupo { Id = 5 };

            var dto = new SolicitudUnionDto
            {
                IdEstudiante = "estu123",
                CodigoEnlace = "codigo123"
            };

            _repoEnlacesMock.Setup(r => r.ObtenerPorCodigoAsync("codigo123")).ReturnsAsync(enlace);
            _repoEstudiantesMock.Setup(r => r.GetByStringIdAsync("estu123"))
                .ReturnsAsync(Resultado<Entidad.Estudiante>.Exitoso(estudiante));
            _repoGruposMock.Setup(r => r.ObtenerPorEnlaceAsync("codigo123")).ReturnsAsync(grupo);
            _repoSolicitudesMock.Setup(r => r.ExisteSolicitudPendiente("estu123", 5)).ReturnsAsync(true);

            var servicio = new CrearSolicitudUnion(
                _repoEnlacesMock.Object,
                _repoEstudiantesMock.Object,
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object
            );

            // Act
            var resultado = await servicio.EjecutarAsync(dto);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains("pendiente", resultado.Errores[0].Mensaje);
        }

        [Fact]
        public async Task Ejecutar_ErrorAlAgregar_RetornaFallo()
        {
            // Arrange
            var enlace = new EnlaceUnion("http://fakeurl", "codigo123") { Expiracion = DateTime.UtcNow.AddMinutes(10) };
            var estudiante = new Entidad.Estudiante { Id = "estu123" };
            var grupo = new Entidad.Grupo { Id = 5 };

            var dto = new SolicitudUnionDto
            {
                IdEstudiante = "estu123",
                CodigoEnlace = "codigo123"
            };

            _repoEnlacesMock.Setup(r => r.ObtenerPorCodigoAsync("codigo123")).ReturnsAsync(enlace);
            _repoEstudiantesMock.Setup(r => r.GetByStringIdAsync("estu123"))
                .ReturnsAsync(Resultado<Entidad.Estudiante>.Exitoso(estudiante));
            _repoGruposMock.Setup(r => r.ObtenerPorEnlaceAsync("codigo123")).ReturnsAsync(grupo);
            _repoSolicitudesMock.Setup(r => r.ExisteSolicitudPendiente("estu123", 5)).ReturnsAsync(false);
            _repoSolicitudesMock
                .Setup(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.SolicitudUnion>()))
                .ReturnsAsync(Resultado.Falla(new Error("Repo.Add", "Error al guardar")));

            var servicio = new CrearSolicitudUnion(
                _repoEnlacesMock.Object,
                _repoEstudiantesMock.Object,
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object
            );

            // Act
            var resultado = await servicio.EjecutarAsync(dto);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains("guardar", resultado.Errores[0].Mensaje);
        }

        [Fact]
        public async Task Ejecutar_DatosValidos_CreaSolicitud()
        {
            // Arrange
            var enlace = new EnlaceUnion("http://fakeurl", "codigo123") { Expiracion = DateTime.UtcNow.AddMinutes(10) };
            var estudiante = new Entidad.Estudiante { Id = "estu123" };
            var grupo = new Entidad.Grupo { Id = 5 };

            var dto = new SolicitudUnionDto
            {
                IdEstudiante = "estu123",
                CodigoEnlace = "codigo123"
            };

            _repoEnlacesMock.Setup(r => r.ObtenerPorCodigoAsync("codigo123")).ReturnsAsync(enlace);
            _repoEstudiantesMock.Setup(r => r.GetByStringIdAsync("estu123"))
                .ReturnsAsync(Resultado<Entidad.Estudiante>.Exitoso(estudiante));
            _repoGruposMock.Setup(r => r.ObtenerPorEnlaceAsync("codigo123")).ReturnsAsync(grupo);
            _repoSolicitudesMock.Setup(r => r.ExisteSolicitudPendiente("estu123", 5)).ReturnsAsync(false);
            _repoSolicitudesMock
                .Setup(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.SolicitudUnion>()))
                .ReturnsAsync(Resultado.Exitoso());

            var servicio = new CrearSolicitudUnion(
                _repoEnlacesMock.Object,
                _repoEstudiantesMock.Object,
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object
            );

            // Act
            var resultado = await servicio.EjecutarAsync(dto);

            // Assert
            Assert.True(resultado.EsExitoso);
            _repoSolicitudesMock.Verify(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.SolicitudUnion>()), Times.Once);
        }
    }
}