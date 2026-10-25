using InterfacesRepositorio;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Recompensa;
using LogicaNegocio.Entidades;
using LogicaNegocio.EntidadesAuxiliares;
using LogicaNegocio.Resultados;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsRecompensa
{
    public class PruebaEditarRecompensa
    {
        private readonly Mock<IRepositorioRecompensas> _mockRepoRecompensas;
        private readonly Mock<IRepositorioTiendas> _mockRepoTiendas;
        private readonly Mock<IRepositorioProfesores> _mockRepoProfesores;
        private readonly EditarRecompensa _useCase;

        public PruebaEditarRecompensa()
        {
            _mockRepoRecompensas = new Mock<IRepositorioRecompensas>();
            _mockRepoTiendas = new Mock<IRepositorioTiendas>();
            _mockRepoProfesores = new Mock<IRepositorioProfesores>();

            _useCase = new EditarRecompensa(
                _mockRepoRecompensas.Object,
                _mockRepoTiendas.Object,
                _mockRepoProfesores.Object
            );
        }

        [Fact]
        public async Task EjecutarAsync_IdNoEntero_RetornaFalloInvalidId()
        {
            var dto = new RecompensaSimpleEditarDto { Nombre = "X", Precio = 10, NombreIcono = "icon" };

            var resultado = await _useCase.EjecutarAsync("no-int", dto, "prof-1");

            Assert.True(resultado.EsFallo);
            Assert.Collection(resultado.Errores, e =>
            {
                Assert.Equal("Error.InvalidId", e.Codigo);
                Assert.Contains("ID de recompensa inválido", e.Mensaje);
            });
            _mockRepoRecompensas.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_RecompensaNoPerteneceAProfesor_RetornaFalloValidation()
        {
            _mockRepoProfesores
                .Setup(r => r.EsRecompensaDeAsync("prof-1", 5))
                .ReturnsAsync(false);

            var dto = new RecompensaSimpleEditarDto { Nombre = "X", Precio = 10, NombreIcono = "icon" };

            var resultado = await _useCase.EjecutarAsync("5", dto, "prof-1");

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e =>
                e.Codigo == "Error.Validation" &&
                e.Mensaje.Contains("no pertenece al profesor"));
        }

        [Fact]
        public async Task EjecutarAsync_RecompensaNoExiste_RetornaFalloNotFound()
        {
            _mockRepoProfesores
                .Setup(r => r.EsRecompensaDeAsync("prof-1", 5))
                .ReturnsAsync(true);

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Falla(new Error("X", "")));

            var dto = new RecompensaSimpleEditarDto { Nombre = "X", Precio = 10, NombreIcono = "icon" };

            var resultado = await _useCase.EjecutarAsync("5", dto, "prof-1");

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.NotFound");
        }

        [Fact]
        public async Task EjecutarAsync_DominioInvalido_RetornaFalloValidation()
        {
            var recompensa = new RecompensaSimple { Id = 7, Nombre = "Original", Precio = 100 };
            ((RepresentacionIcono)recompensa.Representacion).NombreIcono = "original-icon";

            _mockRepoProfesores
                .Setup(r => r.EsRecompensaDeAsync("prof-1", 7))
                .ReturnsAsync(true);

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));

            var dto = new RecompensaSimpleEditarDto
            {
                Nombre = "Edited",
                NombreIcono = "edited-icon",
                Precio = -50 // Forzamos fallo de validación
            };

            var resultado = await _useCase.EjecutarAsync("7", dto, "prof-1");

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation");
            _mockRepoRecompensas.Verify(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.Recompensa>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_ErrorAlActualizar_RetornaFalloUnexpected()
        {
            var recompensa = new RecompensaSimple { Id = 9, Nombre = "Original", Precio = 20 };
            ((RepresentacionIcono)recompensa.Representacion).NombreIcono = "original-icon";

            _mockRepoProfesores
                .Setup(r => r.EsRecompensaDeAsync("prof-1", 9))
                .ReturnsAsync(true);

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(9))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));

            var dto = new RecompensaSimpleEditarDto { Nombre = "Edited", Precio = 30, NombreIcono = "edited-icon" };

            _mockRepoRecompensas
                .Setup(r => r.UpdateAsync(It.Is<LogicaNegocio.Entidades.Recompensa>(r =>
                    r.Id == 9 && r.Nombre == dto.Nombre && r.Precio == dto.Precio)))
                .ReturnsAsync(Resultado.Falla(new Error("X", "")));

            var resultado = await _useCase.EjecutarAsync("9", dto, "prof-1");

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Unexpected");
        }

        [Fact]
        public async Task EjecutarAsync_CaminoFeliz_RetornaExitoso()
        {
            var recompensa = new RecompensaSimple { Id = 15, Nombre = "Original", Precio = 5 };
            ((RepresentacionIcono)recompensa.Representacion).NombreIcono = "original-icon";

            _mockRepoProfesores
                .Setup(r => r.EsRecompensaDeAsync("prof-1", 15))
                .ReturnsAsync(true);

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(15))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));

            var dto = new RecompensaSimpleEditarDto
            {
                Nombre = "NuevoNombre",
                Precio = 50,
                NombreIcono = "nuevo-icon"
            };

            LogicaNegocio.Entidades.Recompensa recompensaCapturada = null;

            _mockRepoRecompensas
                .Setup(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.Recompensa>()))
                .Callback<LogicaNegocio.Entidades.Recompensa>(r => recompensaCapturada = r)
                .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _useCase.EjecutarAsync("15", dto, "prof-1");

            Assert.True(resultado.EsExitoso);
            _mockRepoRecompensas.Verify(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.Recompensa>()), Times.Once);

            Assert.NotNull(recompensaCapturada);
            Assert.Equal(dto.Nombre, recompensaCapturada.Nombre);
            Assert.Equal(dto.Precio, recompensaCapturada.Precio);

            var representacion = Assert.IsType<RepresentacionIcono>(recompensaCapturada.Representacion);
            Assert.Equal(dto.NombreIcono, representacion.NombreIcono);
        }
    }
}