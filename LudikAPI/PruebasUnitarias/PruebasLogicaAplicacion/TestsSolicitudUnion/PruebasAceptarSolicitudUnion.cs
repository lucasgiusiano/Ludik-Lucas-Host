using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using LogicaAplicacion.ImplementacionCasosUsos.SolicitudUnion;
using InterfacesRepositorio;
using LogicaNegocio.Entidades;
using LogicaNegocio.ValueObject;
using LogicaNegocio.Resultados;

namespace PruebasUnitarias.PruebasLogicaAplicacion.SolicitudUnion
{
    public class PruebasAceptarSolicitudUnion
    {
        private readonly Mock<IRepositorioSolicitudesUnion> _mockRepoSolicitudes;
        private readonly Mock<IRepositorioGrupos> _mockRepoGrupos;
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockRepoPerfil;
        private readonly Mock<IRepositorioProfesores> _mockRepoProfesores;
        private readonly AceptarSolicitudUnion _casoUso;

        public PruebasAceptarSolicitudUnion()
        {
            _mockRepoSolicitudes = new Mock<IRepositorioSolicitudesUnion>();
            _mockRepoGrupos = new Mock<IRepositorioGrupos>();
            _mockRepoPerfil = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _mockRepoProfesores = new Mock<IRepositorioProfesores>();

            _casoUso = new AceptarSolicitudUnion(
                _mockRepoSolicitudes.Object,
                _mockRepoGrupos.Object,
                _mockRepoPerfil.Object,
                _mockRepoProfesores.Object
            );
        }

        [Fact]
        public async Task EjecutarAsync_SolicitudNoExiste_RetornaNotFound()
        {
            _mockRepoSolicitudes
                .Setup(r => r.GetSolicitudConEstudianteYGrupoPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((LogicaNegocio.Entidades.SolicitudUnion)null!);

            var resultado = await _casoUso.EjecutarAsync(1, "prof1");

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("no existe"));
        }

        [Fact]
        public async Task EjecutarAsync_ProfesorNoExiste_RetornaFalloAutorizacion()
        {
            var solicitud = new LogicaNegocio.Entidades.SolicitudUnion { Estado = EstadoSolicitud.Pendiente, Grupo = new LogicaNegocio.Entidades.Grupo() };

            _mockRepoSolicitudes
                .Setup(r => r.GetSolicitudConEstudianteYGrupoPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(solicitud);

            _mockRepoProfesores
                .Setup(r => r.GetByStringIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Falla(new Error("Error.Autorizacion", "No se encontró el profesor logueado.")));

            var resultado = await _casoUso.EjecutarAsync(1, "prof1");

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("No se encontró el profesor logueado"));
        }

        [Fact]
        public async Task EjecutarAsync_GrupoNoPerteneceAProfesor_RetornaFalloAutorizacion()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo { Id = 10 };
            var solicitud = new LogicaNegocio.Entidades.SolicitudUnion { Estado = EstadoSolicitud.Pendiente, Grupo = grupo };

            _mockRepoSolicitudes
                .Setup(r => r.GetSolicitudConEstudianteYGrupoPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(solicitud);

            _mockRepoProfesores
                .Setup(r => r.GetByStringIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(new LogicaNegocio.Entidades.Profesor { Grupos = new List<LogicaNegocio.Entidades.Grupo>() }));

            var resultado = await _casoUso.EjecutarAsync(1, "prof1");

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("no pertenece al profesor"));
        }

        [Fact]
        public async Task EjecutarAsync_Exitoso_RetornaResultadoExitoso()
        {
            var grupo = new LogicaNegocio.Entidades.Grupo { Id = 1, Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante>() };
            var solicitud = new LogicaNegocio.Entidades.SolicitudUnion
            {
                Estado = EstadoSolicitud.Pendiente,
                EstudianteId = "estu123",
                Grupo = grupo
            };
            var profesor = new LogicaNegocio.Entidades.Profesor { Grupos = new List<LogicaNegocio.Entidades.Grupo> { grupo } };

            _mockRepoSolicitudes
                .Setup(r => r.GetSolicitudConEstudianteYGrupoPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(solicitud);

            _mockRepoProfesores
                .Setup(r => r.GetByStringIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));

            _mockRepoPerfil
                .Setup(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.PerfilEstudiante>()))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(new LogicaNegocio.Entidades.PerfilEstudiante()));

            _mockRepoSolicitudes
                .Setup(r => r.UpdateAsync(solicitud))
                .ReturnsAsync(Resultado.Exitoso());

            _mockRepoGrupos
                .Setup(r => r.UpdateAsync(grupo))
                .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _casoUso.EjecutarAsync(1, "prof1");

            Assert.True(resultado.EsExitoso);
            Assert.Equal(EstadoSolicitud.Aceptada, solicitud.Estado);
            _mockRepoPerfil.Verify(r => r.AddAsync(It.IsAny<LogicaNegocio.Entidades.PerfilEstudiante>()), Times.Once);
            _mockRepoSolicitudes.Verify(r => r.UpdateAsync(solicitud), Times.Once);
            _mockRepoGrupos.Verify(r => r.UpdateAsync(grupo), Times.Once);
        }
    }
}