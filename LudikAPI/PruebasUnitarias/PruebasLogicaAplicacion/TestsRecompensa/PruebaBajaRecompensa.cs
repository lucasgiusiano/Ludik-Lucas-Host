using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.Recompensa;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsRecompensa
{
    public class PruebaBajaRecompensa
    {
        private readonly Mock<IRepositorioRecompensas> _mockRepoRecompensas;
        private readonly Mock<IRepositorioPerfilEstudianteRecompensa> _mockRepoPerfilEstudianteRecompensa;
        private readonly BajaRecompensa _useCase;
        private const string ProfesorId = "prof-1";

        public PruebaBajaRecompensa()
        {
            _mockRepoRecompensas = new Mock<IRepositorioRecompensas>();
            _mockRepoPerfilEstudianteRecompensa = new Mock<IRepositorioPerfilEstudianteRecompensa>();
            _useCase = new BajaRecompensa(_mockRepoRecompensas.Object, _mockRepoPerfilEstudianteRecompensa.Object);
        }

        [Fact]
        public async Task EjecutarAsync_IdNoEntero_RetornaFalloInvalidId()
        {
            // Act
            var resultado = await _useCase.EjecutarAsync("no-int", ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Collection(resultado.Errores, e =>
            {
                Assert.Equal("Error.InvalidId", e.Codigo);
                Assert.Contains("ID de recompensa inválido", e.Mensaje);
            });

            // No debe haberse consultado si fue canjeada ni el repositorio de recompensas
            _mockRepoPerfilEstudianteRecompensa.Verify(r => r.FueCanjeadaAsync(It.IsAny<int>()), Times.Never);
            _mockRepoRecompensas.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _mockRepoRecompensas.Verify(r => r.RemoveAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_RecompensaNoExiste_RetornaFalloNotFound()
        {
            // Arrange
            int id = 5;
            _mockRepoPerfilEstudianteRecompensa
                .Setup(r => r.FueCanjeadaAsync(id))
                .ReturnsAsync(false);

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Falla(
                    new Error("Error.NotFound", "No existe")
                ));

            // Act
            var resultado = await _useCase.EjecutarAsync(id.ToString(), ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Collection(resultado.Errores, e =>
            {
                Assert.Equal("Error.NotFound", e.Codigo);
                Assert.Contains("No se encontró la recompensa", e.Mensaje);
            });

            _mockRepoRecompensas.Verify(r => r.RemoveAsync(It.IsAny<int>()), Times.Never);
            _mockRepoPerfilEstudianteRecompensa.Verify(r => r.FueCanjeadaAsync(id), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_RecompensaFueCanjeada_RetornaErrorValidacion()
        {
            // Arrange
            int id = 6;
            _mockRepoPerfilEstudianteRecompensa
                .Setup(r => r.FueCanjeadaAsync(id))
                .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.EjecutarAsync(id.ToString(), ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Collection(resultado.Errores, e =>
            {
                Assert.Equal("Error.Validation", e.Codigo);
                Assert.Contains("ya fue canjeada", e.Mensaje);
            });

            // No debe haberse consultado el repositorio de recompensas ni intentado eliminar
            _mockRepoRecompensas.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _mockRepoRecompensas.Verify(r => r.RemoveAsync(It.IsAny<int>()), Times.Never);
            _mockRepoPerfilEstudianteRecompensa.Verify(r => r.FueCanjeadaAsync(id), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_ErrorAlEliminar_RetornaFalloUnexpected()
        {
            // Arrange
            int id = 7;
            // No fue canjeada
            _mockRepoPerfilEstudianteRecompensa
                .Setup(r => r.FueCanjeadaAsync(id))
                .ReturnsAsync(false);

            // GetByIdAsync devuelve un objeto válido
            var recompensa = new LogicaNegocio.Entidades.RecompensaSimple { Id = id };
            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));

            // RemoveAsync falla
            _mockRepoRecompensas
                .Setup(r => r.RemoveAsync(id))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "fail remove")));

            // Act
            var resultado = await _useCase.EjecutarAsync(id.ToString(), ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Collection(resultado.Errores, e =>
            {
                Assert.Equal("Error.Unexpected", e.Codigo);
                Assert.Contains("No se pudo eliminar la recompensa", e.Mensaje);
            });

            _mockRepoPerfilEstudianteRecompensa.Verify(r => r.FueCanjeadaAsync(id), Times.Once);
            _mockRepoRecompensas.Verify(r => r.RemoveAsync(id), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_CaminoFeliz_RetornaExitoso()
        {
            // Arrange
            int id = 9;
            _mockRepoPerfilEstudianteRecompensa
                .Setup(r => r.FueCanjeadaAsync(id))
                .ReturnsAsync(false);

            var recompensa = new LogicaNegocio.Entidades.RecompensaSimple { Id = id };
            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));
            _mockRepoRecompensas
                .Setup(r => r.RemoveAsync(id))
                .ReturnsAsync(Resultado.Exitoso());

            // Act
            var resultado = await _useCase.EjecutarAsync(id.ToString(), ProfesorId);

            // Assert
            Assert.True(resultado.EsExitoso);
            _mockRepoRecompensas.Verify(r => r.RemoveAsync(id), Times.Once);
            _mockRepoPerfilEstudianteRecompensa.Verify(r => r.FueCanjeadaAsync(id), Times.Once);
        }
    }
}
