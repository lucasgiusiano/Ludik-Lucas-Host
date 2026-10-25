using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.Profesores;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsProfesor
{
    public class PruebasReiniciarLogrosDeTodosLosGrupos
    {
        private readonly Mock<IRepositorioGrupos> _mockGrupoRepo;
        private readonly Mock<IRepositorioRendimientoPeriodos> _mockRendRepo;
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockPerfilRepo;
        private readonly ReinicioLogrosDeTodosLosGrupos _casoUso;

        private const string ProfesorId = "prof-1";
        private const int PerfilId = 100;

        public PruebasReiniciarLogrosDeTodosLosGrupos()
        {
            _mockGrupoRepo = new Mock<IRepositorioGrupos>();
            _mockRendRepo = new Mock<IRepositorioRendimientoPeriodos>();
            _mockPerfilRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();

            _casoUso = new ReinicioLogrosDeTodosLosGrupos(
                _mockGrupoRepo.Object,
                _mockRendRepo.Object,
                _mockPerfilRepo.Object
            );
        }

        [Fact]
        public async Task RepositorioFalla_RetornaError()
        {
            _mockGrupoRepo
                .Setup(r => r.obtenerGruposPorProfesorAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Grupo>>.Falla(Error.NotFound));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.NotFound", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task SinGrupos_RetornaValidationError()
        {
            _mockGrupoRepo
                .Setup(r => r.obtenerGruposPorProfesorAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Grupo>>.Exitoso(new List<LogicaNegocio.Entidades.Grupo>()));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Validation", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task FallaAlAgregarRendimientoDePrimerGrupo_RetornaError()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = 1,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-5),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId } }
            };

            _mockGrupoRepo
                .Setup(r => r.obtenerGruposPorProfesorAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Grupo>>.Exitoso(new[] { grupo }));
            _mockRendRepo
                .Setup(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Fallo al agregar")));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Unexpected", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task FallaAlActualizarPrimerGrupo_RetornaError()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = 1,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-5),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId } }
            };

            _mockGrupoRepo
                .Setup(r => r.obtenerGruposPorProfesorAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Grupo>>.Exitoso(new[] { grupo }));
            _mockRendRepo
                .Setup(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockGrupoRepo
                .Setup(r => r.UpdateAsync(grupo))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Fallo al actualizar grupo")));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Unexpected", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task FallaAlGuardarCambios_RetornaError()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = 1,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-5),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId } }
            };

            _mockGrupoRepo
                .Setup(r => r.obtenerGruposPorProfesorAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Grupo>>.Exitoso(new[] { grupo }));
            _mockRendRepo
                .Setup(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockGrupoRepo
                .Setup(r => r.UpdateAsync(grupo))
                .ReturnsAsync(Resultado.Exitoso());
            _mockPerfilRepo
                .Setup(r => r.SaveCambiosAsync())
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Fallo al guardar cambios")));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Unexpected", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task CaminoFeliz_ReinicioCorrectoParaVariosGrupos()
        {
            var g1 = new LogicaNegocio.Entidades.Grupo
            {
                Id = 1,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-7),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId } }
            };
            var g2 = new LogicaNegocio.Entidades.Grupo
            {
                Id = 2,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-3),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId + 1 } }
            };

            _mockGrupoRepo
                .Setup(r => r.obtenerGruposPorProfesorAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Grupo>>.Exitoso(new[] { g1, g2 }));
            _mockRendRepo
                .Setup(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockGrupoRepo
                .Setup(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.Grupo>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockPerfilRepo
                .Setup(r => r.SaveCambiosAsync())
                .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _casoUso.EjecutarAsync(ProfesorId);

            Assert.True(resultado.EsExitoso);
            _mockRendRepo.Verify(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()), Times.Exactly(2));
            _mockGrupoRepo.Verify(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.Grupo>()), Times.Exactly(2));
            _mockPerfilRepo.Verify(r => r.SaveCambiosAsync(), Times.Once);
        }
    }
}
