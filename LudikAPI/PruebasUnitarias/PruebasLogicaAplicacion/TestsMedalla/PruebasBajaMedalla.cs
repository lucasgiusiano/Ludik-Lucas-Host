using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.Medallas;
using LogicaNegocio.Resultados;
using LogicaNegocio.InterfacesRepositorios;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Medalla
{
    public class PruebasBajaMedalla
    {
        private readonly Mock<IRepositorioMedallas> _repoMedallasMock;
        private readonly Mock<IRepositorioProfesores> _repoProfesoresMock;
        private readonly Mock<IRepositorioTablasClasificacion> _repoTablasClasificacionMock;
        private readonly Mock<IRepositorioTablasEquivalencia> _repoTablasEquivalenciaMock;
        private readonly Mock<IRepositorioPerfilEstudianteMedalla> _repoPerfilEstudianteMedallaMock;
        private readonly Mock<IRepositorioRendimientoPeriodos> _repoRendimientoPeriodosMock;
        private readonly BajaMedalla _servicio;
        private const string ProfesorId = "prof123";

        public PruebasBajaMedalla()
        {
            _repoMedallasMock = new Mock<IRepositorioMedallas>();
            _repoProfesoresMock = new Mock<IRepositorioProfesores>();
            _repoTablasClasificacionMock = new Mock<IRepositorioTablasClasificacion>();
            _repoTablasEquivalenciaMock = new Mock<IRepositorioTablasEquivalencia>();
            _repoPerfilEstudianteMedallaMock = new Mock<IRepositorioPerfilEstudianteMedalla>();
            _repoRendimientoPeriodosMock = new Mock<IRepositorioRendimientoPeriodos>();

            // Asegúrate de que la firma real del constructor de BajaMedalla incluye
            // los 6 repositorios en este orden; si la tuya difiere, ajusta aquí.
            _servicio = new BajaMedalla(
                _repoMedallasMock.Object,
                _repoProfesoresMock.Object,
                _repoTablasClasificacionMock.Object,
                _repoTablasEquivalenciaMock.Object,
                _repoPerfilEstudianteMedallaMock.Object,
                _repoRendimientoPeriodosMock.Object
            );
        }

        [Fact]
        public async Task EjecutarAsync_IdInvalido_RetornaErrorValidacion()
        {
            int idInvalido = 0;

            // PoseeMedallaAsync debe devolver éxito (true) para que la comprobación
            // pase y se evalúe el id inválido (el método llama a PoseeMedallaAsync primero).
            _repoProfesoresMock
                .Setup(r => r.PoseeMedallaAsync(It.IsAny<string>(), idInvalido))
                .ReturnsAsync(Resultado<bool>.Exitoso(true));

            var resultado = await _servicio.EjecutarAsync(idInvalido, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains("entero positivo", resultado.Errores.First().Mensaje);

            _repoMedallasMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _repoProfesoresMock.Verify(r => r.PoseeMedallaAsync(It.IsAny<string>(), idInvalido), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_MedallaNoExiste_RetornaNotFound()
        {
            int id = 5;

            _repoProfesoresMock
                .Setup(r => r.PoseeMedallaAsync(It.IsAny<string>(), id))
                .ReturnsAsync(Resultado<bool>.Exitoso(true));

            _repoMedallasMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Medalla>.Falla(Error.NotFound));

            var resultado = await _servicio.EjecutarAsync(id, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains("No se encontró ninguna medalla", resultado.Errores.First().Mensaje);
            _repoMedallasMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_GetByIdRetornaNull_RetornaNotFound()
        {
            int id = 6;

            _repoProfesoresMock
                .Setup(r => r.PoseeMedallaAsync(It.IsAny<string>(), id))
                .ReturnsAsync(Resultado<bool>.Exitoso(true));

            _repoMedallasMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Medalla>.Exitoso(null!));

            var resultado = await _servicio.EjecutarAsync(id, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains("No se encontró ninguna medalla", resultado.Errores.First().Mensaje);
            _repoMedallasMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_MedallaNoPerteneceAProfesor_RetornaErrorValidacion()
        {
            int id = 9;
            var entidad = new LogicaNegocio.Entidades.Medalla
            {
                Id = id,
                Nombre = "X",
                ProfesorId = "otroProfesor"
            };

            // Hacemos que PoseeMedallaAsync devuelva éxito para que se llegue
            // al chequeo 'existente.ProfesorId != profesorId' dentro del método.
            _repoProfesoresMock
                .Setup(r => r.PoseeMedallaAsync(It.IsAny<string>(), id))
                .ReturnsAsync(Resultado<bool>.Exitoso(true));

            _repoMedallasMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Medalla>.Exitoso(entidad));

            var resultado = await _servicio.EjecutarAsync(id, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains("No se encuentra dentro de la lista de medallas", resultado.Errores.First().Mensaje);
            _repoMedallasMock.Verify(r => r.RemoveAsync(It.IsAny<LogicaNegocio.Entidades.Medalla>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_RemoveSuccess_RetornaExitoso()
        {
            int id = 7;
            var entidad = new LogicaNegocio.Entidades.Medalla
            {
                Id = id,
                Nombre = "NombreValido",
                Descripcion = "Desc",
                NombreIcono = "url",
                MonedasOtorgadas = 2,
                ProfesorId = ProfesorId
            };

            _repoProfesoresMock
                .Setup(r => r.PoseeMedallaAsync(It.IsAny<string>(), id))
                .ReturnsAsync(Resultado<bool>.Exitoso(true));

            _repoMedallasMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Medalla>.Exitoso(entidad));

            // Todos los checks que impiden la eliminación devuelven false
            _repoTablasClasificacionMock
                .Setup(r => r.ExisteTablaClasificacionConMedallaAsync(id))
                .ReturnsAsync(false);
            _repoRendimientoPeriodosMock
                .Setup(r => r.ExisteEnRendimientoPeriodoAsync(id))
                .ReturnsAsync(false);
            _repoTablasEquivalenciaMock
                .Setup(r => r.ExisteTablaEquivalenciaConMedallaAsync(id))
                .ReturnsAsync(false);
            _repoPerfilEstudianteMedallaMock
                .Setup(r => r.ExistePerfilEstudianteMedallaAsync(id))
                .ReturnsAsync(false);

            _repoMedallasMock
                .Setup(r => r.RemoveAsync(entidad))
                .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _servicio.EjecutarAsync(id, ProfesorId);

            Assert.True(resultado.EsExitoso);
            _repoMedallasMock.Verify(r => r.RemoveAsync(entidad), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_RemoveFail_PropagaError()
        {
            int id = 8;
            var entidad = new LogicaNegocio.Entidades.Medalla
            {
                Id = id,
                Nombre = "NombreValido",
                Descripcion = "Desc",
                NombreIcono = "url",
                MonedasOtorgadas = 3,
                ProfesorId = ProfesorId
            };

            _repoProfesoresMock
                .Setup(r => r.PoseeMedallaAsync(It.IsAny<string>(), id))
                .ReturnsAsync(Resultado<bool>.Exitoso(true));

            _repoMedallasMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Medalla>.Exitoso(entidad));

            _repoTablasClasificacionMock
                .Setup(r => r.ExisteTablaClasificacionConMedallaAsync(id))
                .ReturnsAsync(false);
            _repoRendimientoPeriodosMock
                .Setup(r => r.ExisteEnRendimientoPeriodoAsync(id))
                .ReturnsAsync(false);
            _repoTablasEquivalenciaMock
                .Setup(r => r.ExisteTablaEquivalenciaConMedallaAsync(id))
                .ReturnsAsync(false);
            _repoPerfilEstudianteMedallaMock
                .Setup(r => r.ExistePerfilEstudianteMedallaAsync(id))
                .ReturnsAsync(false);

            var mensajeError = "Error al eliminar la medalla: restricción";
            _repoMedallasMock
                .Setup(r => r.RemoveAsync(entidad))
                .ReturnsAsync(Resultado.Falla(new Error("Repo.Medalla.Remove", mensajeError)));

            var resultado = await _servicio.EjecutarAsync(id, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(mensajeError, resultado.Errores.First().Mensaje);
            _repoMedallasMock.Verify(r => r.RemoveAsync(entidad), Times.Once);
        }
    }
}