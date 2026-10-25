using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.Estudiantes;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsCanjearRecompensa
{
    public class PruebaCanjearRecompensa
    {
        private readonly Mock<IRepositorioRecompensas> _mockRepoRecompensas;
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockRepoPerfil;
        private readonly Mock<IRepositorioPerfilEstudianteRecompensa> _mockRepoPerfilRecompensa;
        private readonly CanjearRecompensa _casoUso;

        private const int RecompensaId = 1;
        private const int PerfilId = 2;
        private const string EstudianteId = "est-123";

        public PruebaCanjearRecompensa()
        {
            _mockRepoRecompensas = new Mock<IRepositorioRecompensas>();
            _mockRepoPerfil = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _mockRepoPerfilRecompensa = new Mock<IRepositorioPerfilEstudianteRecompensa>();

            _casoUso = new CanjearRecompensa(
                _mockRepoRecompensas.Object,
                _mockRepoPerfil.Object,
                _mockRepoPerfilRecompensa.Object);
        }

        [Fact]
        public async Task RecompensaNoExiste_RetornaErrorNotFound()
        {
            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(RecompensaId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Falla(new Error("X", "Error genérico")));

            var resultado = await _casoUso.EjecutarAsync(RecompensaId, PerfilId, EstudianteId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.NotFound" && e.Mensaje.Contains("recompensa"));
            _mockRepoPerfilRecompensa.Verify(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()), Times.Never);
            _mockRepoPerfil.Verify(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.PerfilEstudiante>()), Times.Never);
        }

        [Fact]
        public async Task PerfilNoExiste_RetornaErrorNotFound()
        {
            var recompensa = new RecompensaSimple { Id = RecompensaId, Precio = 10 };
            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(RecompensaId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));
            _mockRepoPerfil
                .Setup(p => p.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Falla(new Error("X", "Error genérico")));

            var resultado = await _casoUso.EjecutarAsync(RecompensaId, PerfilId, EstudianteId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.NotFound" && e.Mensaje.Contains("perfil"));
            _mockRepoPerfilRecompensa.Verify(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()), Times.Never);
            _mockRepoPerfil.Verify(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.PerfilEstudiante>()), Times.Never);
        }

        [Fact]
        public async Task MonedasInsuficientes_RetornaErrorValidation()
        {
            var recompensa = new RecompensaSimple { Id = RecompensaId, Precio = 20 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, Monedas = 5, EstudianteId = EstudianteId };

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(RecompensaId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));
            _mockRepoPerfil
                .Setup(p => p.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            var resultado = await _casoUso.EjecutarAsync(RecompensaId, PerfilId, EstudianteId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation" && e.Mensaje.Contains("puntos"));
            _mockRepoPerfilRecompensa.Verify(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()), Times.Never);
            _mockRepoPerfil.Verify(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.PerfilEstudiante>()), Times.Never);
        }

        [Fact]
        public async Task EstudianteNoEsElDueño_RetornaErrorForbidden()
        {
            var recompensa = new RecompensaSimple { Id = RecompensaId, Precio = 10 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, Monedas = 100, EstudianteId = "otro-est" };

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(RecompensaId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));
            _mockRepoPerfil
                .Setup(p => p.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            var resultado = await _casoUso.EjecutarAsync(RecompensaId, PerfilId, EstudianteId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Forbidden");
            _mockRepoPerfilRecompensa.Verify(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()), Times.Never);
            _mockRepoPerfil.Verify(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.PerfilEstudiante>()), Times.Never);
        }

        [Fact]
        public async Task FallaAlAgregarRecompensa_RetornaErrorDatabase()
        {
            var recompensa = new RecompensaSimple { Id = RecompensaId, Precio = 10 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, Monedas = 50, EstudianteId = EstudianteId, InventarioRecompensas = new List<PerfilEstudianteRecompensa>() };

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(RecompensaId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));
            _mockRepoPerfil
                .Setup(p => p.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockRepoPerfilRecompensa
                .Setup(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Database", "No se pudo agregar la recompensa al perfil.")));

            var resultado = await _casoUso.EjecutarAsync(RecompensaId, PerfilId, EstudianteId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Database" && e.Mensaje.Contains("agregar la recompensa"));
            _mockRepoPerfilRecompensa.Verify(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()), Times.Once);
            _mockRepoPerfil.Verify(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.PerfilEstudiante>()), Times.Never);
        }

        [Fact]
        public async Task FallaAlActualizarPerfil_RetornaErrorDatabase()
        {
            var recompensa = new RecompensaSimple { Id = RecompensaId, Precio = 10 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, Monedas = 50, EstudianteId = EstudianteId, InventarioRecompensas = new List<PerfilEstudianteRecompensa>() };

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(RecompensaId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));
            _mockRepoPerfil
                .Setup(p => p.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockRepoPerfilRecompensa
                .Setup(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockRepoPerfil
                .Setup(p => p.UpdateAsync(perfil))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Database", "No se pudo actualizar el perfil del estudiante.")));

            var resultado = await _casoUso.EjecutarAsync(RecompensaId, PerfilId, EstudianteId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Database" && e.Mensaje.Contains("actualizar el perfil"));
            _mockRepoPerfilRecompensa.Verify(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()), Times.Once);
            _mockRepoPerfil.Verify(r => r.UpdateAsync(perfil), Times.Once);
        }

        [Fact]
        public async Task CaminoFeliz_CanjeExitoso()
        {
            var recompensa = new RecompensaSimple { Id = RecompensaId, Precio = 15 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, Monedas = 20, EstudianteId = EstudianteId, InventarioRecompensas = new List<PerfilEstudianteRecompensa>() };

            _mockRepoRecompensas
                .Setup(r => r.GetByIdAsync(RecompensaId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Recompensa>.Exitoso(recompensa));
            _mockRepoPerfil
                .Setup(p => p.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockRepoPerfilRecompensa
                .Setup(r => r.AddAsync(It.IsAny<PerfilEstudianteRecompensa>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockRepoPerfil
                .Setup(p => p.UpdateAsync(perfil))
                .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _casoUso.EjecutarAsync(RecompensaId, PerfilId, EstudianteId);

            Assert.True(resultado.EsExitoso);
            Assert.Equal(5, perfil.Monedas);
            _mockRepoPerfilRecompensa.Verify(r => r.AddAsync(
                It.Is<PerfilEstudianteRecompensa>(pr => pr.RecompensaId == RecompensaId && pr.PerfilEstudianteId == PerfilId)
            ), Times.Once);
            _mockRepoPerfil.Verify(r => r.UpdateAsync(perfil), Times.Once);
        }
    }
}

