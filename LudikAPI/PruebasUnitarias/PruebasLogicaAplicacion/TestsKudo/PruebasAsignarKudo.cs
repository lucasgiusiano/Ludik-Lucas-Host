using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.KudoDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Kudo;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsKudo
{
    public class PruebasAsignarKudo
    {
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockPerfilRepo;
        private readonly Mock<IRepositorioTiposKudo> _mockTiposKudoRepo;
        private readonly Mock<IRepositorioEstudiantes> _mockEstudiantesRepo;
        private readonly Mock<IRepositorioUmbralesParaMedallasPorKudos> _mockUmbralesRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly AsignarKudo _casoUso;

        private const string EmisorId = "emisor-1";
        private const int PerfilEmisorId = 10;
        private const int PerfilReceptorId = 20;

        public PruebasAsignarKudo()
        {
            _mockPerfilRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _mockTiposKudoRepo = new Mock<IRepositorioTiposKudo>();
            _mockEstudiantesRepo = new Mock<IRepositorioEstudiantes>();
            _mockUmbralesRepo = new Mock<IRepositorioUmbralesParaMedallasPorKudos>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _casoUso = new AsignarKudo(
                _mockPerfilRepo.Object,
                _mockTiposKudoRepo.Object,
               _mockEstudiantesRepo.Object,
                _mockUmbralesRepo.Object,
               _mockUnitOfWork.Object
            );

            // Default: SaveChanges devuelve 1
            _mockUnitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Default: Estudiante emisor con un perfil válido y kudos disponibles
            var perfilEmisor = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilEmisorId,
                GrupoId = 1,
                Grupo = new LogicaNegocio.Entidades.Grupo { Id = 1, ProfesorId = "prof-1" },
                KudosDisponiblesParaOtorgar = 5
            };
            var estudianteEmisor = new LogicaNegocio.Entidades.Estudiante
            {
                Id = EmisorId,
                Perfiles = new List<LogicaNegocio.Entidades.PerfilEstudiante> { perfilEmisor }
            };
            _mockEstudiantesRepo
                .Setup(r => r.GetByStringIdAsync(EmisorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Estudiante>.Exitoso(estudianteEmisor));

            // Default: Perfil receptor existe
            var perfilReceptor = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilReceptorId,
                GrupoId = 1,
                Grupo = new LogicaNegocio.Entidades.Grupo { Id = 1, ProfesorId = "prof-1" }
            };
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilReceptorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfilReceptor));

            // Default: TipoKudo existe
            var tipoKudo = new TipoKudo
            {
                Id = 5,
                Nombre = "Amistad",
                Descripcion = "Promueve la amistad",
                NombreIcono = "amistad.png"
            };
            _mockTiposKudoRepo
                .Setup(r => r.GetByIdAsync(tipoKudo.Id))
                .ReturnsAsync(Resultado<TipoKudo>.Exitoso(tipoKudo));

            // Default: Umbral para medalla
            var umbral = new UmbralParaMedallaPorKudos
            {
                Id = 1,
                TipoKudoId = tipoKudo.Id,
                CantidadKudos = 1,
                Medalla = new LogicaNegocio.Entidades.Medalla { Id = 2 }
            };
            _mockUmbralesRepo
                .Setup(r => r.GetAllByGrupoIdAsync(1))
                .ReturnsAsync(Resultado<IEnumerable<UmbralParaMedallaPorKudos>>.Exitoso(new[] { umbral }));
        }

        [Fact]
        public async Task EmisorNoExiste_RetornaErrorRepoEstudiantes()
        {
            _mockEstudiantesRepo
                .Setup(r => r.GetByStringIdAsync(EmisorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Estudiante>.Falla(new Error("Error.NotFound", "Estudiante emisor no encontrado")));

            var dto = new AsignarKudoDto
            {
                IdPerfilEstudianteEmisor = PerfilEmisorId,
                IdPerfilEstudianteRecibe = PerfilReceptorId,
                Kudo = new TipoKudoDto { Id = 5 }
            };

            var res = await _casoUso.EjecutarAsync(EmisorId, dto);

            Assert.True(res.EsFallo);
            Assert.Contains(res.Errores, e => e.Codigo == "Error.NotFound");
        }

        [Fact]
        public async Task PerfilEmisorNoExiste_RetornaErrorDominio()
        {
            var estudianteEmisor = new LogicaNegocio.Entidades.Estudiante { Id = EmisorId, Perfiles = new List<LogicaNegocio.Entidades.PerfilEstudiante>() };
            _mockEstudiantesRepo
                .Setup(r => r.GetByStringIdAsync(EmisorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Estudiante>.Exitoso(estudianteEmisor));

            var dto = new AsignarKudoDto
            {
                IdPerfilEstudianteEmisor = PerfilEmisorId,
                IdPerfilEstudianteRecibe = PerfilReceptorId,
                Kudo = new TipoKudoDto { Id = 5 }
            };
            var res = await _casoUso.EjecutarAsync(EmisorId, dto);

            Assert.True(res.EsFallo);
            Assert.Contains(res.Errores, e => e.Codigo == "Error.NotFound");
        }

        [Fact]
        public async Task TipoKudoNoExiste_RetornaErrorRepoTipos()
        {
            _mockTiposKudoRepo
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(Resultado<TipoKudo>.Falla(new Error("Error.NotFound", "Tipo de kudo no encontrado")));

            var dto = new AsignarKudoDto
            {
                IdPerfilEstudianteEmisor = PerfilEmisorId,
                IdPerfilEstudianteRecibe = PerfilReceptorId,
                Kudo = new TipoKudoDto { Id = 5 }
            };
            var res = await _casoUso.EjecutarAsync(EmisorId, dto);

            Assert.True(res.EsFallo);
            Assert.Contains(res.Errores, e => e.Codigo == "Error.NotFound");
        }

        [Fact]
        public async Task ErrorAlEvaluarUmbrales_RetornaEseError()
        {
            _mockUmbralesRepo
                .Setup(r => r.GetAllByGrupoIdAsync(1))
                .ReturnsAsync(Resultado<IEnumerable<UmbralParaMedallaPorKudos>>.Falla(new Error("Error.Validation", "Umbral inválido")));

            var dto = new AsignarKudoDto
            {
                IdPerfilEstudianteEmisor = PerfilEmisorId,
                IdPerfilEstudianteRecibe = PerfilReceptorId,
                Kudo = new TipoKudoDto { Id = 5 }
            };
            var res = await _casoUso.EjecutarAsync(EmisorId, dto);

            Assert.True(res.EsFallo);
            Assert.Contains(res.Errores, e => e.Codigo == "Error.Validation");
        }

        [Fact]
        public async Task ErrorSaveChanges_RetornaUnexpected()
        {
            // Arrange
            var umbralNoCoincidente = new UmbralParaMedallaPorKudos { Id = 99, TipoKudoId = 999 };
            _mockUmbralesRepo
                .Setup(r => r.GetAllByGrupoIdAsync(1))
                .ReturnsAsync(Resultado<IEnumerable<UmbralParaMedallaPorKudos>>.Exitoso(new[] { umbralNoCoincidente }));

            _mockUnitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("fail"));

            var dto = new AsignarKudoDto
            {
                IdPerfilEstudianteEmisor = PerfilEmisorId,
                IdPerfilEstudianteRecibe = PerfilReceptorId,
                Kudo = new TipoKudoDto { Id = 5 }
            };
            var res = await _casoUso.EjecutarAsync(EmisorId, dto);

            Assert.True(res.EsFallo);
            Assert.Contains(res.Errores, e => e.Codigo == "Error.Unexpected");
        }

        [Fact]
        public async Task CaminoFeliz_RetornaExitoso()
        {    // Arrange
            var umbralNoCoincidente = new UmbralParaMedallaPorKudos
            {
                Id = 99,
                TipoKudoId = 999, // Un ID que no sea 5
                CantidadKudos = 10
            };
            _mockUmbralesRepo
                .Setup(r => r.GetAllByGrupoIdAsync(1))
                .ReturnsAsync(Resultado<IEnumerable<UmbralParaMedallaPorKudos>>.Exitoso(new[] { umbralNoCoincidente }));

            var dto = new AsignarKudoDto
            {
                IdPerfilEstudianteEmisor = PerfilEmisorId,
                IdPerfilEstudianteRecibe = PerfilReceptorId,
                Kudo = new TipoKudoDto { Id = 5 }
            };

            // Act
            var res = await _casoUso.EjecutarAsync(EmisorId, dto);

            // Assert
            Assert.True(res.EsExitoso);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
