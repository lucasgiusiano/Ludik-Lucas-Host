using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion;
using LogicaAplicacion.DTOs.TablaClasificacionDTOs;
using LogicaNegocio.Entidades;
using LogicaNegocio.ValueObjects;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsTablaClasificacion
{
    public class PruebasObtenerTablaClasificacion
    {
        private readonly Mock<IRepositorioTablasClasificacion> _repoMock;
        private readonly ObtenerTablaClasificacion _casoUso;
        private const int TablaId = 7;

        public PruebasObtenerTablaClasificacion()
        {
            _repoMock = new Mock<IRepositorioTablasClasificacion>();
            _casoUso = new ObtenerTablaClasificacion(_repoMock.Object);
        }


        [Fact]
        public async Task EjecutarAsync_RepoFalla_RetornaFallo()
        {
            // Arrange
            var errorRepo = new Error("Error.DB", "Error repositorio");
            _repoMock.Setup(r => r.GetByIdAsync(TablaId))
                     .ReturnsAsync(Resultado<TablaClasificacion>.Falla(errorRepo));

            // Act
            var resultado = await _casoUso.EjecutarAsync(TablaId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == errorRepo.Codigo);
            _repoMock.Verify(r => r.GetByIdAsync(TablaId), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_TablaNoExiste_RetornaFalloNotFound()
        {
            // Arrange
  
            _repoMock.Setup(r => r.GetByIdAsync(TablaId))
                     .ReturnsAsync(Resultado<TablaClasificacion>.Falla(Error.NotFound));

            // Act
            var resultado = await _casoUso.EjecutarAsync(TablaId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Equal(Error.NotFound.Codigo, resultado.Errores.First().Codigo);
            _repoMock.Verify(r => r.GetByIdAsync(TablaId), Times.Once);
        }


        [Fact]
        public async Task EjecutarAsync_ValidoConAlumnos_RetornaDtoConParticipantesOrdenados()
        {
            // --- Arrange ---
            var medallaId = 100;
            var medalla = new LogicaNegocio.Entidades.Medalla { Id = medallaId, Nombre = "M100" };

            // Estudiante 1 (Luis) - 1 medalla de este tipo
            var perfilLuis = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = 1,
                Estudiante = new LogicaNegocio.Entidades.Estudiante { NombreCompleto = NombreCompleto.Crear("Luis", "Gómez").Valor },
                MedallasObtenidas = new List<PerfilEstudianteMedalla>
                {
                    new PerfilEstudianteMedalla { MedallaId = medallaId }
                }
            };


            var perfilAna = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = 2,
                Estudiante = new LogicaNegocio.Entidades.Estudiante { NombreCompleto = NombreCompleto.Crear("Ana", "Pérez").Valor },
                MedallasObtenidas = new List<PerfilEstudianteMedalla>
                {
                    new PerfilEstudianteMedalla { MedallaId = medallaId },
                    new PerfilEstudianteMedalla { MedallaId = medallaId }
                }
            };

            // Simular la data COMPLETA que el Repositorio GetByIdAsync debe cargar
            var tablaMock = new TablaClasificacion
            {
                Id = TablaId,
                Nombre = "TablaX",
                MedallaAsociadaId = medallaId,
                MedallaAsociada = medalla,
                Grupo = new LogicaNegocio.Entidades.Grupo
                {
                    Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante> { perfilLuis, perfilAna }
                }
            };

            _repoMock.Setup(r => r.GetByIdAsync(TablaId))
                     .ReturnsAsync(Resultado<TablaClasificacion>.Exitoso(tablaMock));

            // --- Act ---
            var resultado = await _casoUso.EjecutarAsync(TablaId);

            // --- Assert ---
            Assert.True(resultado.EsExitoso);
            var dto = resultado.Valor;
            Assert.NotNull(dto);
            Assert.Equal(tablaMock.Nombre, dto.Nombre);
            Assert.Equal(2, dto.Participantes.Count);

            // Verificar el orden: Ana (2 medallas) debe ir primero
            Assert.Equal(perfilAna.Id, dto.Participantes[0].PerfilEstudianteId);
            Assert.Equal("Ana Pérez", dto.Participantes[0].NombreEstudiante);
            Assert.Equal(2, dto.Participantes[0].CantidadMedallas);

            // Luis (1 medalla) debe ir segundo
            Assert.Equal(perfilLuis.Id, dto.Participantes[1].PerfilEstudianteId);
            Assert.Equal("Luis Gómez", dto.Participantes[1].NombreEstudiante);
            Assert.Equal(1, dto.Participantes[1].CantidadMedallas);
        }


        [Fact]
        public async Task EjecutarAsync_GrupoSinAlumnos_RetornaDtoExitosoConParticipantesVacios()
        {
            // --- Arrange ---
            var medalla = new LogicaNegocio.Entidades.Medalla { Id = 100, Nombre = "M100" };

            // Simular data del repo: Tabla con Grupo, pero Grupo SIN Alumnos
            var tablaMock = new TablaClasificacion
            {
                Id = TablaId,
                Nombre = "Tabla Vacía",
                MedallaAsociadaId = medalla.Id,
                MedallaAsociada = medalla,
                Grupo = new LogicaNegocio.Entidades.Grupo
                {
                    Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante>() // <-- Lista vacía
                }
            };

            _repoMock.Setup(r => r.GetByIdAsync(TablaId))
                     .ReturnsAsync(Resultado<TablaClasificacion>.Exitoso(tablaMock));

            // --- Act ---
            var resultado = await _casoUso.EjecutarAsync(TablaId);

            // --- Assert ---
            // El resultado DEBE ser exitoso
            Assert.True(resultado.EsExitoso);
            var dto = resultado.Valor;
            Assert.NotNull(dto);
            Assert.Equal(tablaMock.Nombre, dto.Nombre);

            // La lista de participantes debe existir, pero estar vacía
            Assert.NotNull(dto.Participantes);
            Assert.Empty(dto.Participantes);
        }
    }
}
