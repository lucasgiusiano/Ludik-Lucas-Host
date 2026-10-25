using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Grupos;
using LogicaNegocio.Resultados;
using LogicaNegocio.Entidades;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Grupo
{
    public class PruebasObtenerInformacionGrupo
    {
        private readonly Mock<IRepositorioGrupos> _repoGruposMock;
        private readonly ObtenerInformacionGrupo _casoUso;

        public PruebasObtenerInformacionGrupo()
        {
            _repoGruposMock = new Mock<IRepositorioGrupos>();
            _casoUso = new ObtenerInformacionGrupo(_repoGruposMock.Object);
        }

        [Fact]
        public async Task EjecutarAsync_GrupoNoExiste_RetornaFallo()
        {
            // Arrange
            int grupoId = 1;
            var errores = new[] { new Error("Error.NotFound", "No existe el grupo.") };
            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Falla(errores));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoId, "profX");

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("No existe el grupo."));
        }

        [Fact]
        public async Task EjecutarAsync_GrupoExisteConEnlace_RetornaDtoMapeado()
        {
            // Arrange
            int grupoId = 2;
            var fechaCreacion = new DateTime(2025, 1, 15, 10, 30, 0);

            var tablaEq = new TablaEquivalencia
            {
                Id = 5,
                Equivalencias = new List<Equivalencia>
                {
                    new Equivalencia(5, new List<LogicaNegocio.Entidades.Medalla> { new LogicaNegocio.Entidades.Medalla { Id = 1 } }),
                    new Equivalencia(2, new List<LogicaNegocio.Entidades.Medalla> { new LogicaNegocio.Entidades.Medalla { Id = 2 } })
                }
            };

            var enlace = new EnlaceUnion("http://url.com/inv", "codigo123")
            {
                UrlCompleta = "http://url.com/inv"
            };

            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = grupoId,
                Nombre = "Grupo Test",
                TablaEquivalencia = tablaEq,
                ProfesorId = "profX",
                Institucion = "InstX",
                Materia = "Matemáticas",
                FCreacion = fechaCreacion,
                EnlaceUnion = enlace,
                Tienda = new LogicaNegocio.Entidades.Tienda { Id = 99 }
            };

            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(grupo));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoId, "profX");

            // Assert
            Assert.True(resultado.EsExitoso);
            var dto = resultado.Valor!;
            Assert.Equal("Grupo Test", dto.Nombre);
            Assert.Equal(5, dto.TablaEquivalenciaId);
            Assert.Equal(5, dto.TablaEquivalenciaNotaMaxima); // el mayor entre 5 y 2
            Assert.Equal("profX", dto.ProfesorId);
            Assert.Equal("InstX", dto.Institucion);
            Assert.Equal("Matemáticas", dto.Materia);
            Assert.Equal(fechaCreacion, dto.fCreacion);
            Assert.Equal("http://url.com/inv", dto.UrlCompleta);
            Assert.Equal(99, dto.IdTienda);
        }

        [Fact]
        public async Task EjecutarAsync_GrupoExisteSinEnlace_RetornaDtoConUrlCompletaNull()
        {
            // Arrange
            int grupoId = 3;
            var fechaCreacion = new DateTime(2025, 6, 1, 14, 0, 0);

            var tablaEq = new TablaEquivalencia
            {
                Id = 7,
                Equivalencias = new List<Equivalencia>
                {
                    new Equivalencia
                    {
                        Nota = 2,
                        MedallasNecesarias = new List<LogicaNegocio.Entidades.Medalla>
                        {
                            new LogicaNegocio.Entidades.Medalla { Id = 1 }
                        }
                    }
                }
            };

            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = grupoId,
                Nombre = "Otro Grupo",
                TablaEquivalencia = tablaEq,
                ProfesorId = "profY",
                Institucion = null,
                Materia = null,
                FCreacion = fechaCreacion,
                EnlaceUnion = null,
                Tienda = new LogicaNegocio.Entidades.Tienda { Id = 0 }
            };

            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(grupo));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoId, "profY");

            // Assert
            Assert.True(resultado.EsExitoso);
            var dto = resultado.Valor!;
            Assert.Equal("Otro Grupo", dto.Nombre);
            Assert.Equal(7, dto.TablaEquivalenciaId);
            Assert.Equal(2, dto.TablaEquivalenciaNotaMaxima);
            Assert.Equal("profY", dto.ProfesorId);
            Assert.Null(dto.Institucion);
            Assert.Null(dto.Materia);
            Assert.Equal(fechaCreacion, dto.fCreacion);
            Assert.Null(dto.UrlCompleta);
            Assert.Equal(0, dto.IdTienda);
        }

        
    }
}