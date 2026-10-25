using InterfacesRepositorio;
using LogicaAplicacion.Eventos;
using LogicaAplicacion.ImplementacionCasosUsos.AsignarMedalla;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Entidad = LogicaNegocio.Entidades;

namespace PruebasUnitarias.PruebasLogicaAplicacion.BrindarMedalla
{
    public class PruebasAsignarMedalla
    {
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockPerfilRepo;
        private readonly Mock<IRepositorioMedallas> _mockMedallasRepo;
        private readonly Mock<IRepositorioProfesores> _mockProfesoresRepo;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly AsignarMedalla _casoUso;

        private const string ProfesorId = "prof-1";
        private const int PerfilId = 1;
        private const int MedallaId = 2;

        public PruebasAsignarMedalla()
        {
            _mockPerfilRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _mockMedallasRepo = new Mock<IRepositorioMedallas>();
            _mockProfesoresRepo = new Mock<IRepositorioProfesores>();
            _mockMediator = new Mock<IMediator>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _casoUso = new AsignarMedalla(
                _mockPerfilRepo.Object,
                _mockMedallasRepo.Object,
                _mockProfesoresRepo.Object,
                _mockMediator.Object,
                _mockUnitOfWork.Object
            );
        }
        [Fact]
        public async Task CantidadAOtorgarInvalida_RetornaErrorDeValidacion()
        {
            // Arrange
            int cantidadInvalida = 0;

            // Act
            var res = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId, cantidadInvalida);

            // Assert
            Assert.True(res.EsFallo);
            Assert.Equal("Error.Validation", res.Errores.First().Codigo);
        }
        [Fact]
        public async Task ProfesorNoPerteneceAGrupo_RetornaForbidden()
        {
            // Arrange
            // Simulamos directamente el resultado de la verificación de permisos.
            _mockProfesoresRepo
                .Setup(r => r.PerteneceGrupoAsync(ProfesorId, PerfilId))
                .ReturnsAsync(Resultado<bool>.Exitoso(false));

            // Act
            var res = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId, 1);

            // Assert
            Assert.True(res.EsFallo);
            Assert.Equal(Error.Forbidden.Codigo, res.Errores.First().Codigo);
        }

        [Fact]
        public async Task ProfesorNoPoseeMedalla_RetornaForbidden()
        {
            // Arrange
            // Simulamos que la primera verificación (pertenencia al grupo) es exitosa.
            _mockProfesoresRepo
                .Setup(r => r.PerteneceGrupoAsync(ProfesorId, PerfilId))
                .ReturnsAsync(Resultado<bool>.Exitoso(true));

            // Simulamos que la segunda verificación (posesión de la medalla) falla.
            _mockProfesoresRepo
                .Setup(r => r.PoseeMedallaAsync(ProfesorId, MedallaId))
                .ReturnsAsync(Resultado<bool>.Exitoso(false));

            // Act
            var res = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId, 1);

            // Assert
            Assert.True(res.EsFallo);
            Assert.Equal(Error.Forbidden.Codigo, res.Errores.First().Codigo);
        }

        [Fact]
        public async Task MedallaNoEncontrada_RetornaNotFound()
        {
            // Arrange
            _mockProfesoresRepo.Setup(r => r.PerteneceGrupoAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(Resultado<bool>.Exitoso(true));
            _mockProfesoresRepo.Setup(r => r.PoseeMedallaAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(Resultado<bool>.Exitoso(true));

            _mockMedallasRepo
                .Setup(r => r.GetByIdAsync(MedallaId))
                .ReturnsAsync(Resultado<Entidad.Medalla>.Falla(Error.NotFound));

            // Act
            var res = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId, 1);

            // Assert
            Assert.True(res.EsFallo);
            Assert.Equal(Error.NotFound.Codigo, res.Errores.First().Codigo);
        }

        [Fact]
        public async Task PerfilNoEncontrado_RetornaNotFound()
        {
            // Arrange
            _mockProfesoresRepo.Setup(r => r.PerteneceGrupoAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(Resultado<bool>.Exitoso(true));
            _mockProfesoresRepo.Setup(r => r.PoseeMedallaAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(Resultado<bool>.Exitoso(true));
            _mockMedallasRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(Resultado<Entidad.Medalla>.Exitoso(new Entidad.Medalla()));

            // Se actualiza el mock para usar el método correcto del repositorio.
            _mockPerfilRepo
                .Setup(r => r.GetParaAsignacionMedallaAsync(PerfilId))
                .ReturnsAsync(Resultado<Entidad.PerfilEstudiante>.Falla(Error.NotFound));

            // Act
            var res = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId, 1);

            // Assert
            Assert.True(res.EsFallo);
            Assert.Equal(Error.NotFound.Codigo, res.Errores.First().Codigo);
        }

        [Fact]
        public async Task AsignacionExitosa_ModificaPerfilGuardaCambiosYPublicaEvento()
        {
            // Arrange
            int cantidadAOtorgar = 3;
            int monedasIniciales = 10;
            int monedasPorMedalla = 5;

            // Creamos las entidades con estado inicial para verificar los cambios.
            var perfilEst = new Entidad.PerfilEstudiante
            {
                Id = PerfilId,
                Monedas = monedasIniciales,
                Estudiante = new Entidad.Estudiante() // Necesario para el evento
            };
            var medalla = new Entidad.Medalla { Id = MedallaId, MonedasOtorgadas = monedasPorMedalla };

            // Configuración de mocks para el "camino feliz"
            _mockProfesoresRepo.Setup(r => r.PerteneceGrupoAsync(ProfesorId, PerfilId)).ReturnsAsync(Resultado<bool>.Exitoso(true));
            _mockProfesoresRepo.Setup(r => r.PoseeMedallaAsync(ProfesorId, MedallaId)).ReturnsAsync(Resultado<bool>.Exitoso(true));
            _mockMedallasRepo.Setup(r => r.GetByIdAsync(MedallaId)).ReturnsAsync(Resultado<Entidad.Medalla>.Exitoso(medalla));
            _mockPerfilRepo.Setup(r => r.GetParaAsignacionMedallaAsync(PerfilId)).ReturnsAsync(Resultado<Entidad.PerfilEstudiante>.Exitoso(perfilEst));

            // Act
            var res = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId, cantidadAOtorgar);

            // Assert
            Assert.True(res.EsExitoso);

            // 1. Verificar el estado final de la entidad PerfilEstudiante
            int monedasEsperadas = monedasIniciales + (monedasPorMedalla * cantidadAOtorgar);
            Assert.Equal(monedasEsperadas, perfilEst.Monedas);
            Assert.Equal(cantidadAOtorgar, perfilEst.MedallasObtenidas.Count);
            Assert.All(perfilEst.MedallasObtenidas, asignacion => Assert.Equal(MedallaId, asignacion.MedallaId));

            // 2. Verificar que se intentó guardar los cambios
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // 3. Verificar que el evento de dominio fue publicado
            _mockMediator.Verify(m => m.Publish(It.IsAny<AsignacionMedallaCompletadaEvento>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}