using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.TablaClasificacionDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsTablaClasificacion
{
    public class PruebasAltaTablaClasificacion
    {
        private readonly Mock<IRepositorioTablasClasificacion> _repoTablaMock;
        private readonly Mock<IRepositorioGrupos> _repoGrupoMock;
        private readonly Mock<IRepositorioProfesores> _repoProfesoresMock;
        private readonly AltaTablaClasificacion _casoUso;
        private readonly LogicaNegocio.Entidades.Grupo _grupo;
        private const int GrupoId = 1;
        private const int MedallaId = 10;
        private const string ProfesorId = "prof-123";

        public PruebasAltaTablaClasificacion()
        {
            _repoTablaMock = new Mock<IRepositorioTablasClasificacion>();
            _repoGrupoMock = new Mock<IRepositorioGrupos>();
            _repoProfesoresMock = new Mock<IRepositorioProfesores>();

            _casoUso = new AltaTablaClasificacion(_repoTablaMock.Object, _repoGrupoMock.Object, _repoProfesoresMock.Object);

            // Preparamos un grupo válido con alumnos
            _grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = GrupoId,
                Alumnos = new List<LogicaNegocio.Entidades.PerfilEstudiante>
                {
                    new LogicaNegocio.Entidades.PerfilEstudiante { Id = 1 },
                    new LogicaNegocio.Entidades.PerfilEstudiante { Id = 2 }
                },
                ProfesorId = ProfesorId,
            };

            // CONFIGURACIÓN POR DEFECTO: cuando pidan el profesor, devolvemos uno con la medalla requerida.
            var profesorPorDefecto = new LogicaNegocio.Entidades.Profesor
            {
                Id = ProfesorId,
                Medallas = new List<LogicaNegocio.Entidades.Medalla>
                {
                    new LogicaNegocio.Entidades.Medalla { Id = MedallaId }
                }
            };

            _repoProfesoresMock
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesorPorDefecto));
        }


        [Fact]
        public async Task EjecutarAsync_GrupoNoExiste_RetornaFalloValidation()
        {
            // DEVOLVEMOS UN RESULTADO DE FALLA, NO null
            _repoGrupoMock
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Falla(
                    new Error("Error.Validation", "El grupo no existe")
                ));

            var dto = new TablaClasificacionAltaDto { Nombre = "TablaValida", MedallaAsociadaId = MedallaId };

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, GrupoId, dto);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e =>
                e.Codigo == "Error.Validation" && e.Mensaje.Contains("El grupo no existe")
            );
            _repoTablaMock.Verify(r => r.AddAsync(It.IsAny<TablaClasificacion>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_DtoInvalido_RetornaFalloValidation()
        {
            _repoGrupoMock
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(_grupo));

            var dto = new TablaClasificacionAltaDto { Nombre = "Ab", MedallaAsociadaId = MedallaId };

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, GrupoId, dto);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e =>
                e.Codigo == "Error.Validation" && e.Mensaje.Contains("nombre")
            );
            _repoTablaMock.Verify(r => r.AddAsync(It.IsAny<TablaClasificacion>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_ErrorEnAddAsync_RetornaFallo()
        {
            _repoGrupoMock
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(_grupo));

            _repoTablaMock
                .Setup(r => r.AddAsync(It.IsAny<TablaClasificacion>()))
                .ReturnsAsync(Resultado.Falla(new Error("Error.DB", "Fallo DB")));

            var dto = new TablaClasificacionAltaDto { Nombre = "TablaValida", MedallaAsociadaId = MedallaId };

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, GrupoId, dto);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.DB");
            _repoTablaMock.Verify(r => r.AddAsync(It.IsAny<TablaClasificacion>()), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_DatosValidos_YAgregaTabla()
        {
            _repoGrupoMock
                .Setup(r => r.GetByIdAsync(GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(_grupo));

            TablaClasificacion capturada = null!;
            _repoTablaMock
                .Setup(r => r.AddAsync(It.IsAny<TablaClasificacion>()))
                .Callback<TablaClasificacion>(tc => capturada = tc)
                .ReturnsAsync(Resultado.Exitoso());

            var dto = new TablaClasificacionAltaDto
            {
                Nombre = "TablaOK",
                MedallaAsociadaId = MedallaId
            };

            var resultado = await _casoUso.EjecutarAsync(ProfesorId, GrupoId, dto);

            Assert.True(resultado.EsExitoso);
            _repoTablaMock.Verify(r => r.AddAsync(It.IsAny<TablaClasificacion>()), Times.Once);
            Assert.Equal(dto.Nombre, capturada.Nombre);
            Assert.Equal(dto.MedallaAsociadaId, capturada.MedallaAsociadaId);
            Assert.Equal(_grupo.Id, capturada.GrupoId);
            Assert.Equal(_grupo, capturada.Grupo);
        }
    }
}