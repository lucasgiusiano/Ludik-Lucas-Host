using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion;
using LogicaAplicacion.InterfacesCasosUsos.TablaClasificacion;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObjects;
using Microsoft.Extensions.Azure;
using Moq;
using Entidades = LogicaNegocio.Entidades;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsTablaClasificacion;

public class PruebasObtenerTodasLasTablasClasificacionGrupo
{
    public class ObtenerTodasLasTablasClasificacionGrupoTests
    {
        private readonly Mock<IRepositorioGrupos> _mockRepositorioGrupos;
        private readonly Mock<IRepositorioTablasClasificacion> _mockRepositorioTablasClasificacion;
        private readonly IObtenerTodasLasTablasClasificacionGrupo _casoUso;

        public ObtenerTodasLasTablasClasificacionGrupoTests()
        {
            _mockRepositorioGrupos = new Mock<IRepositorioGrupos>();
            _mockRepositorioTablasClasificacion = new Mock<IRepositorioTablasClasificacion>();
            _casoUso = new ObtenerTodasLasTablasClasificacionGrupo(
                _mockRepositorioGrupos.Object,
                _mockRepositorioTablasClasificacion.Object
            );
        }

        //==================================
        // PRUEBAS DE ERROR
        //==================================

        [Fact]
        public async Task EjecutarAsync_GrupoNoExistente_DebeRetornarErrorNotFound()
        {
            // Arrange
            var grupoIdInexistente = 999;
            var usuarioId = "usuario-valido";
            _mockRepositorioGrupos
                .Setup(repo => repo.GetByIdAsync(grupoIdInexistente))
                .ReturnsAsync(Resultado<Entidades.Grupo>.Falla(Error.NotFound));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoIdInexistente, usuarioId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Equal(Error.NotFound.Codigo, resultado.Errores.First().Codigo);
            _mockRepositorioTablasClasificacion.Verify(repo => repo.GetAllByAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_UsuarioNoEsMiembroDelGrupo_DebeRetornarErrorForbidden()
        {
            // Arrange
            var grupoId = 1;
            var profesorId = "profesor-1";
            var usuarioNoAutorizadoId = "usuario-externo";
            var grupo = new Entidades.Grupo { Id = grupoId, ProfesorId = profesorId, Alumnos = new List<Entidades.PerfilEstudiante>() };

            _mockRepositorioGrupos
                .Setup(repo => repo.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidades.Grupo>.Exitoso(grupo));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoId, usuarioNoAutorizadoId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Equal(Error.Forbidden.Codigo, resultado.Errores.First().Codigo);
            _mockRepositorioTablasClasificacion.Verify(repo => repo.GetAllByAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_ErrorAlObtenerTablas_DebePropagarErrorDelRepositorio()
        {
            // Arrange
            var grupoId = 1;
            var profesorId = "profesor-1";
            var grupo = new Entidades.Grupo { Id = grupoId, ProfesorId = profesorId, Alumnos = new List<Entidades.PerfilEstudiante>() };
            var errorDB = new Error("Error.Unexpected", "Fallo en la base de datos");

            _mockRepositorioGrupos
                .Setup(repo => repo.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidades.Grupo>.Exitoso(grupo));

            _mockRepositorioTablasClasificacion
                .Setup(repo => repo.GetAllByAsync(grupoId))
                .ReturnsAsync(Resultado<IEnumerable<Entidades.TablaClasificacion>>.Falla(errorDB));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Equal(errorDB.Codigo, resultado.Errores.First().Codigo);
        }

        //==================================
        // PRUEBAS DE ÉXITO
        //==================================

        [Fact]
        public async Task EjecutarAsync_UsuarioEsProfesorYExistenRankings_DebeRetornarListaDeDtos()
        {
            // Arrange
            var grupoId = 1;
            var profesorId = "profesor-1";

            // Aunque este test es para un profesor, si el grupo TIENE alumnos,
            // debemos simularlos correctamente para que el mapper no falle.
            var alumnoMock = new Entidades.PerfilEstudiante
            {
                Id = 1,
                EstudianteId = "alumno-1",
                Estudiante = new Entidades.Estudiante { NombreCompleto = NombreCompleto.Crear("Alumno", "Test").Valor },
                MedallasObtenidas = new List<Entidades.PerfilEstudianteMedalla>() // Lista vacía, NO nula
            };

            var grupo = new Entidades.Grupo
            {
                Id = grupoId,
                ProfesorId = profesorId,
                Alumnos = new List<Entidades.PerfilEstudiante> { alumnoMock } // Asignamos el alumno mockeado
            };
     

            var tablas = new List<Entidades.TablaClasificacion>
            {
                new Entidades.TablaClasificacion { Id = 10, Nombre = "Ranking 1", MedallaAsociadaId = 1, MedallaAsociada = new Entidades.Medalla { Id = 1, Nombre = "Medalla Oro" } },
                new Entidades.TablaClasificacion { Id = 11, Nombre = "Ranking 2", MedallaAsociadaId = 2, MedallaAsociada = new Entidades.Medalla { Id = 2, Nombre = "Medalla Plata" } }
            };

            _mockRepositorioGrupos
                .Setup(repo => repo.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidades.Grupo>.Exitoso(grupo));

            _mockRepositorioTablasClasificacion
                .Setup(repo => repo.GetAllByAsync(grupoId))
                .ReturnsAsync(Resultado<IEnumerable<Entidades.TablaClasificacion>>.Exitoso(tablas));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal(2, resultado.Valor.Count());
            Assert.Equal("Ranking 1", resultado.Valor.First().Nombre);
        }

        [Fact]
        public async Task EjecutarAsync_UsuarioEsAlumnoYExistenRankings_DebeRetornarListaDeDtos()
        {
            // Arrange
            var grupoId = 1;
            var profesorId = "profesor-1";
            var alumnoId = "alumno-que-pertenece";

            // --- INICIO CORRECCIÓN ---
            // Creamos un PerfilEstudiante simulado que incluya las propiedades
            // que el caso de uso y el mapper van a necesitar.
            var alumnoEnGrupo = new Entidades.PerfilEstudiante
            {
                EstudianteId = alumnoId,
                GrupoId = grupoId,
                Id = 5, // ID de ejemplo para el PerfilEstudiante

                // 1. Soluciona 'p.MedallasObtenidas.Count' en el OrderByDescending
                MedallasObtenidas = new List<Entidades.PerfilEstudianteMedalla>(),

                // 2. Soluciona 'p.Estudiante.NombreCompleto' en el Mapper
                Estudiante = new Entidades.Estudiante
                {
                    Id = alumnoId,
                    NombreCompleto = NombreCompleto.Crear("Alumno", "Prueba").Valor
                }
            };
            // --- FIN CORRECCIÓN ---

            // Inicializamos el grupo correctamente
            var grupo = new Entidades.Grupo
            {
                Id = grupoId,
                ProfesorId = profesorId,
                Alumnos = new List<Entidades.PerfilEstudiante> { alumnoEnGrupo } // Usamos el objeto completo
            };

            var tablas = new List<Entidades.TablaClasificacion>
            {
                new Entidades.TablaClasificacion {
                    Id = 10,
                    Nombre = "Ranking General",
                    MedallaAsociadaId = 1,
                    MedallaAsociada = new Entidades.Medalla{ Id = 1, Nombre="Medalla Default"},
                }
            };

            _mockRepositorioGrupos
                .Setup(repo => repo.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidades.Grupo>.Exitoso(grupo));

            _mockRepositorioTablasClasificacion
                .Setup(repo => repo.GetAllByAsync(grupoId))
                .ReturnsAsync(Resultado<IEnumerable<Entidades.TablaClasificacion>>.Exitoso(tablas));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoId, alumnoId);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Single(resultado.Valor);
            // Verificamos que el participante (alumno) se mapeó correctamente
            Assert.Single(resultado.Valor.First().Participantes);
            Assert.Equal("Alumno Prueba", resultado.Valor.First().Participantes.First().NombreEstudiante);
        }

        [Fact]
        public async Task EjecutarAsync_GrupoNoTieneRankings_DebeRetornarListaVaciaExitosamente()
        {
            // Arrange
            var grupoId = 1;
            var profesorId = "profesor-1";
            var grupo = new Entidades.Grupo { Id = grupoId, ProfesorId = profesorId, Alumnos = new List<Entidades.PerfilEstudiante>() };

            _mockRepositorioGrupos
                .Setup(repo => repo.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidades.Grupo>.Exitoso(grupo));

            _mockRepositorioTablasClasificacion
                .Setup(repo => repo.GetAllByAsync(grupoId))
                .ReturnsAsync(Resultado<IEnumerable<Entidades.TablaClasificacion>>.Exitoso(new List<Entidades.TablaClasificacion>()));

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Empty(resultado.Valor);
        }
    }
}