using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.EstablecerMetaCalificacionDto;
using LogicaAplicacion.ImplementacionCasosUsos.PerfilEstudiante;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.PruebasPerfilEstudiante
{
    public class PruebasEstablecerMetaCalificacion
    {
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockPerfilRepo;
        private readonly EstablecerMetaCalificacion _casoUso;

        public PruebasEstablecerMetaCalificacion()
        {
            _mockPerfilRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _casoUso = new EstablecerMetaCalificacion(_mockPerfilRepo.Object);
        }

        [Fact]
        public async Task PerfilNoExiste_RetornaNotFound()
        {
            var dto = new EstablecerMetaCalificacionDto { PerfilEstudianteId = 1, MetaCalificacion = 0 };
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(dto.PerfilEstudianteId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Falla(Error.NotFound));

            var res = await _casoUso.EjecutarAsync(dto, "user-1");

            Assert.True(res.EsFallo);
            Assert.Equal("Error.NotFound", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task UsuarioNoPropietario_RetornaForbidden()
        {
            var dto = new EstablecerMetaCalificacionDto { PerfilEstudianteId = 2, MetaCalificacion = 0 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = dto.PerfilEstudianteId, EstudianteId = "otro-user" };
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(dto.PerfilEstudianteId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            var res = await _casoUso.EjecutarAsync(dto, "user-1");

            Assert.True(res.EsFallo);
            Assert.Equal("Error.Forbidden", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task MetaNegativa_RetornaValidationError()
        {
            var dto = new EstablecerMetaCalificacionDto { PerfilEstudianteId = 3, MetaCalificacion = -1 };
            var tabla = new TablaEquivalencia { Equivalencias = new List<Equivalencia> { new Equivalencia { Nota = 1 } } };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = dto.PerfilEstudianteId, EstudianteId = "user-1", Grupo = new LogicaNegocio.Entidades.Grupo { TablaEquivalencia = tabla } };
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(dto.PerfilEstudianteId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            var res = await _casoUso.EjecutarAsync(dto, "user-1");

            Assert.True(res.EsFallo);
            Assert.Equal("Error.Validation", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task MetaMayorMaxima_RetornaValidationError()
        {
            var dto = new EstablecerMetaCalificacionDto { PerfilEstudianteId = 4, MetaCalificacion = 5 };
            var eq1 = new Equivalencia { Nota = 1 };
            var eq2 = new Equivalencia { Nota = 2 };
            var tabla = new TablaEquivalencia { Equivalencias = new List<Equivalencia> { eq1, eq2 } };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = dto.PerfilEstudianteId, EstudianteId = "user-1", Grupo = new LogicaNegocio.Entidades.Grupo { TablaEquivalencia = tabla } };
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(dto.PerfilEstudianteId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            var res = await _casoUso.EjecutarAsync(dto, "user-1");

            Assert.True(res.EsFallo);
            Assert.Equal("Error.Validation", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task CaminoFeliz_EstableceMetaCorrectamente()
        {
            var dto = new EstablecerMetaCalificacionDto { PerfilEstudianteId = 5, MetaCalificacion = 2 };
            var eq1 = new Equivalencia { Nota = 1 };
            var eq2 = new Equivalencia { Nota = 2 };
            var tabla = new TablaEquivalencia { Equivalencias = new List<Equivalencia> { eq1, eq2 } };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = dto.PerfilEstudianteId,
                EstudianteId = "user-1",
                Grupo = new LogicaNegocio.Entidades.Grupo { TablaEquivalencia = tabla },
                MetaCalificacion = 0
            };
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(dto.PerfilEstudianteId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            // Mockear UpdateAsync para que no falle
            _mockPerfilRepo
                .Setup(r => r.UpdateAsync(It.IsAny<LogicaNegocio.Entidades.PerfilEstudiante>()))
                .ReturnsAsync(Resultado.Exitoso());

            var res = await _casoUso.EjecutarAsync(dto, "user-1");

            Assert.True(res.EsExitoso);
            Assert.Equal(2, perfil.MetaCalificacion);
        }
    }
}
