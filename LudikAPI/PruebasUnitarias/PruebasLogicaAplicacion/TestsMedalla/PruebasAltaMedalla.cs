using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Medallas;
using LogicaNegocio.Resultados;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Medalla
{
    public class PruebasAltaMedalla
    {
        private readonly Mock<IRepositorioMedallas> _repoMedallasMock;
        private readonly AltaMedalla _servicio;
        private const string ProfesorId = "prof-123";

        public PruebasAltaMedalla()
        {
            _repoMedallasMock = new Mock<IRepositorioMedallas>();
            _servicio = new AltaMedalla(_repoMedallasMock.Object);
        }

        [Fact]
        public async Task EjecutarAsync_DtoNulo_RetornaErrorValidacion()
        {
            // Act
            var resultado = await _servicio.EjecutarAsync(null, ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.True(resultado.Errores.Any(e =>
                e.Mensaje.Contains("no pueden ser nulos", StringComparison.OrdinalIgnoreCase)));
            _repoMedallasMock.Verify(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task EjecutarAsync_NombreInvalido_RetornaErrorValidacion(string nombreInvalido)
        {
            // Arrange
            var dto = new MedallaAltaDto
            {
                NombreIcono = "url.png",
                Nombre = nombreInvalido,
                Descripcion = "Descripción válida",
                CantidadMonedasBrinda = 1,
            };

            // Act
            var resultado = await _servicio.EjecutarAsync(dto, ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.True(resultado.Errores.Any(e =>
                e.Mensaje.Contains("nombre", StringComparison.OrdinalIgnoreCase)));
            _repoMedallasMock.Verify(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_DescripcionMuyLarga_RetornaErrorValidacion()
        {
            // Arrange
            var dto = new MedallaAltaDto
            {
                NombreIcono = "url.png",
                Nombre = "NombreVálido",
                Descripcion = new string('a', 151), // supera el límite de 150 caracteres
                CantidadMonedasBrinda = 1,
            };

            // Act
            var resultado = await _servicio.EjecutarAsync(dto, ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo, "Se esperaba EsFallo=true para descripción >150 caracteres");

            var errores = resultado.Errores ?? Enumerable.Empty<Error>();
            Assert.NotEmpty(errores);

            // Validación segura del mensaje de error
            Assert.Contains(errores, e =>
                !string.IsNullOrWhiteSpace(e.Mensaje)
                && e.Mensaje.IndexOf("descripción", StringComparison.OrdinalIgnoreCase) >= 0
                && (
                    e.Mensaje.Contains("150") ||
                    e.Mensaje.Contains("caracteres", StringComparison.OrdinalIgnoreCase) ||
                    e.Mensaje.Contains("superar", StringComparison.OrdinalIgnoreCase)
                )
            );

            // Verifica que NO se haya llamado al repositorio si hay error de validación
            _repoMedallasMock.Verify(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_MonedasNegativas_RetornaErrorValidacion()
        {
            // Arrange
            var dto = new MedallaAltaDto
            {
                NombreIcono = "url.png",
                Nombre = "NombreVálido",
                Descripcion = "Descripción válida",
                CantidadMonedasBrinda = -5,
            };

            // Act
            var resultado = await _servicio.EjecutarAsync(dto, ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.True(resultado.Errores.Any(e =>
                e.Mensaje.Contains("monedas") &&
                e.Mensaje.Contains("no puede ser menor a 0", StringComparison.OrdinalIgnoreCase)));
            _repoMedallasMock.Verify(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_AddAsyncExitoso_RetornaExitosoYMapeaEntidad()
        {
            // Arrange
            var dto = new MedallaAltaDto
            {
                NombreIcono = "icono.png",
                Nombre = "NombreVálido",
                Descripcion = "Descripción válida",
                CantidadMonedasBrinda = 10,
            };

            LogicaNegocio.Entidades.Medalla capturada = null;
            _repoMedallasMock
                .Setup(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()))
                .Callback<LogicaNegocio.Entidades.Medalla>(m => capturada = m)
                .ReturnsAsync(Resultado.Exitoso());

            // Act
            var resultado = await _servicio.EjecutarAsync(dto, ProfesorId);

            // Assert
            Assert.True(resultado.EsExitoso);
            _repoMedallasMock.Verify(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()), Times.Once);
            Assert.NotNull(capturada);
            Assert.Equal(dto.Nombre, capturada.Nombre);
            Assert.Equal(dto.Descripcion, capturada.Descripcion);
            Assert.Equal(dto.NombreIcono, capturada.NombreIcono);
            Assert.Equal(dto.CantidadMonedasBrinda, capturada.MonedasOtorgadas);
            Assert.Equal(ProfesorId, capturada.ProfesorId);
        }

        [Fact]
        public async Task EjecutarAsync_AddAsyncFalla_PropagaErrores()
        {
            // Arrange
            var dto = new MedallaAltaDto
            {
                NombreIcono = "icono.png",
                Nombre = "NombreVálido",
                Descripcion = "Descripción válida",
                CantidadMonedasBrinda = 3,
            };
            var errorRepo = new Error("Repo.Error", "Fallo BD");
            _repoMedallasMock
                .Setup(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()))
                .ReturnsAsync(Resultado.Falla(errorRepo));

            // Act
            var resultado = await _servicio.EjecutarAsync(dto, ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == errorRepo.Codigo && e.Mensaje == errorRepo.Mensaje);
            _repoMedallasMock.Verify(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()), Times.Once);
        }
    }
}
