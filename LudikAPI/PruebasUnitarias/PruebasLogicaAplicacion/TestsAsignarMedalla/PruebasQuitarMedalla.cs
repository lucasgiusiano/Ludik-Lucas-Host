using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.AsignarMedalla;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.InterfacesRepositorios;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsBrindarMedalla
{
    public class PruebasQuitarMedalla
    {
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockPerfilRepo;
        private readonly Mock<IRepositorioProfesores> _mockProfesorRepo;
        private readonly Mock<IRepositorioPerfilEstudianteMedalla> _mockMedallaRepo;
        private readonly QuitarMedalla _casoUso;

        private const string ProfesorId = "prof-1";
        private const int PerfilId = 100;
        private const int MedallaId = 200;

        public PruebasQuitarMedalla()
        {
            _mockPerfilRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _mockProfesorRepo = new Mock<IRepositorioProfesores>();
            _mockMedallaRepo = new Mock<IRepositorioPerfilEstudianteMedalla>();
            _casoUso = new QuitarMedalla(_mockPerfilRepo.Object, _mockProfesorRepo.Object, _mockMedallaRepo.Object);
        }

        [Fact]
        public async Task ProfesorNoExiste_RetornaNotFound()
        {
            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Falla(Error.NotFound));

            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Falla(Error.NotFound));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.NotFound", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task PerfilNoExiste_RetornaNotFound()
        {
            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(new LogicaNegocio.Entidades.Profesor()));

            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Falla(Error.NotFound));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.NotFound", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task PerfilNoPerteneceAProfesor_RetornaForbidden()
        {
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                Grupos = new List<LogicaNegocio.Entidades.Grupo> { new LogicaNegocio.Entidades.Grupo { Id = 1 } }
            };

            var perfil = new   LogicaNegocio.Entidades.PerfilEstudiante
            {
                GrupoId = 999 // no coincide con grupo del profesor
            };

            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));

            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Forbidden", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task MedallaNoAsignadaAlPerfil_RetornaValidationError()
        {
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                Grupos = new List<LogicaNegocio.Entidades.Grupo> { new LogicaNegocio.Entidades.Grupo { Id = 1 } }
            };

            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, GrupoId = 1 };

            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockMedallaRepo
                .Setup(r => r.GetByPerfilYMedallaAsync(PerfilId, MedallaId))
                .ReturnsAsync(Resultado<PerfilEstudianteMedalla>.Falla(Error.NotFound));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e =>
                e.Codigo == "Error.Validation" &&
                e.Mensaje.Contains("no posee la medalla"));
        }

        [Fact]
        public async Task FallaAlEliminarMedalla_RetornaError()
        {
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                Grupos = new List<LogicaNegocio.Entidades.Grupo> { new LogicaNegocio.Entidades.Grupo { Id = 1 } }
            };

            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, GrupoId = 1 };
            var asignacion = new PerfilEstudianteMedalla { PerfilEstudianteId = PerfilId, MedallaId = MedallaId };

            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockMedallaRepo
                .Setup(r => r.GetByPerfilYMedallaAsync(PerfilId, MedallaId))
                .ReturnsAsync(Resultado<PerfilEstudianteMedalla>.Exitoso(asignacion));
            _mockMedallaRepo
                .Setup(r => r.RemoveAsync(asignacion))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Fallo al eliminar")));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Unexpected");
        }

        [Fact]
        public async Task CaminoFeliz_MedallaRemovidaCorrectamente()
        {
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                Grupos = new List<LogicaNegocio.Entidades.Grupo> { new LogicaNegocio.Entidades.Grupo { Id = 1 } }
            };

            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, GrupoId = 1 };
            var asignacion = new PerfilEstudianteMedalla { PerfilEstudianteId = PerfilId, MedallaId = MedallaId };

            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockMedallaRepo
                .Setup(r => r.GetByPerfilYMedallaAsync(PerfilId, MedallaId))
                .ReturnsAsync(Resultado<PerfilEstudianteMedalla>.Exitoso(asignacion));
            _mockMedallaRepo
                .Setup(r => r.RemoveAsync(asignacion))
                .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, PerfilId, MedallaId);

            Assert.True(resultado.EsExitoso);
        }
    }
}
