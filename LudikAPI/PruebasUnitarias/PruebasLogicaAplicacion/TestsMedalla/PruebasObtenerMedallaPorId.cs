using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Medallas;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsMedalla
{
    public class PruebasObtenerMedallaPorId
    {
        private readonly Mock<IRepositorioMedallas> _repoMock;
        private readonly Mock<IRepositorioProfesores> _repoProfesoresMock;
        private readonly ObtenerMedallaPorId _casoUso;
        private const int IdMedalla = 1;
        private const string ProfesorId = "prof123";

        public PruebasObtenerMedallaPorId()
        {
            _repoMock = new Mock<IRepositorioMedallas>();
            _repoProfesoresMock = new Mock<IRepositorioProfesores>();

            _casoUso = new ObtenerMedallaPorId(_repoMock.Object, _repoProfesoresMock.Object);
        }

        [Fact]
        public async Task MedallaNoExiste_RetornaFallo()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(IdMedalla))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Medalla>.Falla(Error.NotFound));

            // Act
            var resultado = await _casoUso.EjecutarAsync(IdMedalla, ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.NotFound");
        }

        [Fact]
        public async Task MedallaExiste_YProfesorPosee_RetornaDtoCorrecto()
        {
            // Arrange
            var medalla = new LogicaNegocio.Entidades.Medalla
            {
                Id = IdMedalla,
                Nombre = "Colaborador",
                Descripcion = "Ayuda frecuentemente a sus compañeros",
                NombreIcono = "img/colaborador.png",
                MonedasOtorgadas = 50,
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(IdMedalla))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Medalla>.Exitoso(medalla));

            // Profesor posee la medalla
            _repoProfesoresMock
                .Setup(r => r.PoseeMedallaAsync(ProfesorId, IdMedalla))
                .ReturnsAsync(Resultado<bool>.Exitoso(true));

            // Act
            var resultado = await _casoUso.EjecutarAsync(IdMedalla, ProfesorId);

            // Assert
            Assert.True(resultado.EsExitoso);

            var dto = resultado.Valor!;
            Assert.Equal(medalla.Id, dto.Id);
            Assert.Equal(medalla.Nombre, dto.Nombre);
            Assert.Equal(medalla.Descripcion, dto.Descripcion);
            Assert.Equal(medalla.NombreIcono, dto.NombreIcono);
            Assert.Equal(medalla.MonedasOtorgadas, dto.CantidadMedallasBrinda);
        }

        [Fact]
        public async Task MedallaExiste_PeroProfesorNoPosee_RetornaForbidden()
        {
            // Arrange
            var medalla = new LogicaNegocio.Entidades.Medalla
            {
                Id = IdMedalla,
                Nombre = "Colaborador",
                Descripcion = "Ayuda frecuentemente a sus compañeros",
                NombreIcono = "img/colaborador.png",
                MonedasOtorgadas = 50,
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(IdMedalla))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Medalla>.Exitoso(medalla));

            // Profesor NO posee la medalla (resultado exitoso con Valor == false)
            _repoProfesoresMock
                .Setup(r => r.PoseeMedallaAsync(ProfesorId, IdMedalla))
                .ReturnsAsync(Resultado<bool>.Exitoso(false));

            // Act
            var resultado = await _casoUso.EjecutarAsync(IdMedalla, ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Forbidden" || e.Mensaje.Contains("No tienes permiso"));
        }
    }
}