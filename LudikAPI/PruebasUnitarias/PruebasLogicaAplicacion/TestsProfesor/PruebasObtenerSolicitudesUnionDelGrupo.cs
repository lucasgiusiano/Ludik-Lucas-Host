using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Profesores;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;
using Entidad = LogicaNegocio.Entidades;

using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Profesor
{
    public class PruebasObtenerSolicitudesUnionDelGrupo
    {
        private readonly Mock<IRepositorioGrupos> _repoGruposMock;
        private readonly Mock<IRepositorioSolicitudesUnion> _repoSolicitudesMock;
        private readonly Mock<IRepositorioProfesores> _repoProfesoresMock;

        public PruebasObtenerSolicitudesUnionDelGrupo()
        {
            _repoGruposMock = new Mock<IRepositorioGrupos>();
            _repoSolicitudesMock = new Mock<IRepositorioSolicitudesUnion>();
            _repoProfesoresMock = new Mock<IRepositorioProfesores>();
        }

        [Fact]
        public async Task EjecutarAsync_GrupoNoExiste_RetornaFalloNotFound()
        {
            // Arrange
            int grupoId = 10;
            string profesorId = "profX";
            // Simular GetByIdAsync falla
            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidad.Grupo>.Falla(Error.NotFound));

            var casoUso = new ObtenerSolicitudesUnionDelGrupo(
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object,
                _repoProfesoresMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            // Debe contener el error NotFound
            Assert.Contains(resultado.Errores, e => e.Codigo == Error.NotFound.Codigo);
            _repoSolicitudesMock.Verify(r => r.ObtenerSolicitudesPendientesPorGrupoAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_GrupoExistePeroNoPerteneceProfesor_RetornaFalloValidation()
        {
            // Arrange
            int grupoId = 20;
            string profesorId = "profA";
            var grupo = new Entidad.Grupo
            {
                Id = grupoId,
                ProfesorId = "otroProfesor",
                Nombre = "G",
                Institucion = "I",
                Materia = "M",
                TablaEquivalencia = new TablaEquivalencia(),
                EnlaceUnion = null,
                Tienda = null
            };
            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidad.Grupo>.Exitoso(grupo));

            var casoUso = new ObtenerSolicitudesUnionDelGrupo(
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object,
                _repoProfesoresMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e =>
                e.Codigo == "Error.Validation" &&
                e.Mensaje.Contains("El grupo no pertenece al profesor", StringComparison.OrdinalIgnoreCase));
            _repoSolicitudesMock.Verify(r => r.ObtenerSolicitudesPendientesPorGrupoAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_GrupoExiste_PendientesVacio_RetornaListaVacia()
        {
            // Arrange
            int grupoId = 30;
            string profesorId = "profX";
            var grupo = new Entidad.Grupo
            {
                Id = grupoId,
                ProfesorId = profesorId,
                Nombre = "G",
                Institucion = "I",
                Materia = "M",
                TablaEquivalencia = new TablaEquivalencia(),
                EnlaceUnion = null,
                Tienda = null
            };
            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidad.Grupo>.Exitoso(grupo));

            _repoSolicitudesMock
                .Setup(r => r.ObtenerSolicitudesPendientesPorGrupoAsync(grupoId))
                .ReturnsAsync(new List<LogicaNegocio.Entidades.SolicitudUnion>()); // sin solicitudes pendientes

            var casoUso = new ObtenerSolicitudesUnionDelGrupo(
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object,
                _repoProfesoresMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Empty(resultado.Valor);
            _repoSolicitudesMock.Verify(r => r.ObtenerSolicitudesPendientesPorGrupoAsync(grupoId), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_GrupoExiste_ConPendientes_RetornaDtosMapeados()
        {
            // Arrange
            int grupoId = 40;
            string profesorId = "profY";
            var fecha1 = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
            var fecha2 = DateOnly.FromDateTime(DateTime.UtcNow);
            var solicitud1 = new LogicaNegocio.Entidades.SolicitudUnion
            {
                EstudianteId = "est1",
                Fecha = fecha1,
                Estado = EstadoSolicitud.Pendiente,
                GrupoId = grupoId
            };
            var solicitud2 = new LogicaNegocio.Entidades.SolicitudUnion
            {
                EstudianteId = "est2",
                Fecha = fecha2,
                Estado = EstadoSolicitud.Pendiente,
                GrupoId = grupoId
            };
            var grupo = new Entidad.Grupo
            {
                Id = grupoId,
                ProfesorId = profesorId,
                Nombre = "G",
                Institucion = "I",
                Materia = "M",
                TablaEquivalencia = new TablaEquivalencia(),
                EnlaceUnion = null,
                Tienda = null
            };
            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidad.Grupo>.Exitoso(grupo));

            _repoSolicitudesMock
                .Setup(r => r.ObtenerSolicitudesPendientesPorGrupoAsync(grupoId))
                .ReturnsAsync(new List<LogicaNegocio.Entidades.SolicitudUnion> { solicitud1, solicitud2 });

            var casoUso = new ObtenerSolicitudesUnionDelGrupo(
                _repoGruposMock.Object,
                _repoSolicitudesMock.Object,
                _repoProfesoresMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal(2, resultado.Valor.Count);

            // Verificar mapeo de cada solicitud
            var dto1 = resultado.Valor[0];
            Assert.Equal("est1", dto1.IdEstudiante);
            Assert.Equal(fecha1, dto1.fecha);

            var dto2 = resultado.Valor[1];
            Assert.Equal("est2", dto2.IdEstudiante);
            Assert.Equal(fecha2, dto2.fecha);

            _repoSolicitudesMock.Verify(r => r.ObtenerSolicitudesPendientesPorGrupoAsync(grupoId), Times.Once);
        }
    }
}
