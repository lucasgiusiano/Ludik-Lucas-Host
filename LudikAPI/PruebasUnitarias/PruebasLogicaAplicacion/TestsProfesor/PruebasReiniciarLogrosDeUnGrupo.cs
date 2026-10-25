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
    public class PruebasReiniciarLogrosDeUnGrupo
    {
        private readonly Mock<IRepositorioGrupos> _mockGrupoRepo;
        private readonly Mock<IRepositorioRendimientoPeriodos> _mockRendRepo;
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockPerfilRepo;
        private readonly ReinicioLogrosDeUnGrupo _casoUso;

        private const int GrupoId = 1;
        private const string ProfesorId = "prof-1";
        private const int PerfilId = 100;

        public PruebasReiniciarLogrosDeUnGrupo()
        {
            _mockGrupoRepo = new Mock<IRepositorioGrupos>();
            _mockRendRepo = new Mock<IRepositorioRendimientoPeriodos>();
            _mockPerfilRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();

            _casoUso = new ReinicioLogrosDeUnGrupo(
                _mockGrupoRepo.Object,
                _mockRendRepo.Object,
                _mockPerfilRepo.Object
            );
        }

        [Fact]
        public async Task GrupoNoExiste_RetornaNotFound()
        {
            _mockGrupoRepo
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Falla(Error.NotFound));

            var resultado = await _casoUso.EjecutarAsync(GrupoId, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.NotFound", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task ProfesorNoEsPropietario_RetornaValidationError()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo { Id = GrupoId, ProfesorId = "otro-prof" };

            _mockGrupoRepo
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(grupo));

            var resultado = await _casoUso.EjecutarAsync(GrupoId, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Validation", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task FallaAlAgregarRendimiento_RetornaError()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = GrupoId,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-10),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId } }
            };

            _mockGrupoRepo
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(grupo));
            // Simula fallo al agregar el primer rendimiento
            _mockRendRepo
                .Setup(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Fallo al agregar")));

            var resultado = await _casoUso.EjecutarAsync(GrupoId, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Unexpected", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task FallaAlActualizarGrupo_RetornaError()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = GrupoId,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-10),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId } }
            };

            _mockGrupoRepo
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(grupo));
            _mockRendRepo
                .Setup(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockGrupoRepo
                .Setup(r => r.UpdateAsync(grupo))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Fallo al actualizar grupo")));

            var resultado = await _casoUso.EjecutarAsync(GrupoId, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Unexpected", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task FallaAlGuardarCambiosEnPerfiles_RetornaError()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = GrupoId,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-10),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId } }
            };

            _mockGrupoRepo
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(grupo));
            _mockRendRepo
                .Setup(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockGrupoRepo
                .Setup(r => r.UpdateAsync(grupo))
                .ReturnsAsync(Resultado.Exitoso());
            _mockPerfilRepo
                .Setup(r => r.SaveCambiosAsync())
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Fallo al guardar cambios")));

            var resultado = await _casoUso.EjecutarAsync(GrupoId, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal("Error.Unexpected", resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task CaminoFeliz_ReinicioCorrecto()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = GrupoId,
                ProfesorId = ProfesorId,
                FCreacion = DateTime.UtcNow.AddDays(-10),
                FechaUltimoReinicio = null,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId } }
            };

            _mockGrupoRepo
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(grupo));
            _mockRendRepo
                .Setup(r => r.AddAsync(It.IsAny<RendimientoPeriodo>()))
                .ReturnsAsync(Resultado.Exitoso());
            _mockGrupoRepo
                .Setup(r => r.UpdateAsync(grupo))
                .ReturnsAsync(Resultado.Exitoso());
            _mockPerfilRepo
                .Setup(r => r.SaveCambiosAsync())
                .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _casoUso.EjecutarAsync(GrupoId, ProfesorId);

            Assert.True(resultado.EsExitoso);
        }
    }
}

