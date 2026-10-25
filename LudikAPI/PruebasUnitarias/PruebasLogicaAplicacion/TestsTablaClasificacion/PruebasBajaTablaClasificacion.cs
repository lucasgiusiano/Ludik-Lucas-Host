using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsTablaClasificacion
{
    public class PruebasBajaTablaClasificacion
    {
        private readonly Mock<IRepositorioTablasClasificacion> _repoTablaMock;
        private readonly Mock<IRepositorioProfesores> _repoProfesorMock;
        private readonly BajaTablaClasificacion _casoUso;
        private const int TablaId = 42;
        private const string ProfesorId = "profesor123";

        public PruebasBajaTablaClasificacion()
        {
            _repoTablaMock = new Mock<IRepositorioTablasClasificacion>();
            _repoProfesorMock = new Mock<IRepositorioProfesores>();
            _casoUso = new BajaTablaClasificacion(_repoTablaMock.Object, _repoProfesorMock.Object);
        }

        [Fact]
        public async Task EjecutarAsync_GrupoNoPerteneceAProfesor_RetornaFallo()
        {
            var tabla = new TablaClasificacion { Id = TablaId, GrupoId = 99 };
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                Id = ProfesorId,
                Grupos = new System.Collections.Generic.List<LogicaNegocio.Entidades.Grupo> { new LogicaNegocio.Entidades.Grupo { Id = 100 } }
            };

            _repoTablaMock.Setup(r => r.GetByIdAsync(TablaId))
                          .ReturnsAsync(Resultado<TablaClasificacion>.Exitoso(tabla));
            _repoProfesorMock.Setup(r => r.GetByStringIdAsync(ProfesorId))
                             .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));

            var resultado = await _casoUso.EjecutarAsync(TablaId, ProfesorId);

            Assert.True(resultado.EsFallo);
            // <-- cambio aquí: verificamos el código que el caso de uso realmente devuelve
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Forbidden");
        }

        [Fact]
        public async Task EjecutarAsync_ErrorAlEliminar_RetornaFallo()
        {
            var tabla = new TablaClasificacion { Id = TablaId, GrupoId = 1 };
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                Id = ProfesorId,
                Grupos = new System.Collections.Generic.List<LogicaNegocio.Entidades.Grupo> { new LogicaNegocio.Entidades.Grupo { Id = 1 } }
            };

            _repoTablaMock.Setup(r => r.GetByIdAsync(TablaId))
                          .ReturnsAsync(Resultado<TablaClasificacion>.Exitoso(tabla));
            _repoProfesorMock.Setup(r => r.GetByStringIdAsync(ProfesorId))
                             .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));
            _repoTablaMock.Setup(r => r.RemoveAsync(TablaId))
                          .ReturnsAsync(Resultado.Falla(new Error("Error.DB", "Falló al eliminar")));

            var resultado = await _casoUso.EjecutarAsync(TablaId, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.DB");
        }

        [Fact]
        public async Task EjecutarAsync_DatosValidos_RetornaExitoso()
        {
            var tabla = new TablaClasificacion { Id = TablaId, GrupoId = 1 };
            var profesor = new LogicaNegocio.Entidades.Profesor
            {
                Id = ProfesorId,
                Grupos = new System.Collections.Generic.List<LogicaNegocio.Entidades.Grupo> { new LogicaNegocio.Entidades.Grupo { Id = 1 } }
            };

            _repoTablaMock.Setup(r => r.GetByIdAsync(TablaId))
                          .ReturnsAsync(Resultado<TablaClasificacion>.Exitoso(tabla));
            _repoProfesorMock.Setup(r => r.GetByStringIdAsync(ProfesorId))
                             .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(profesor));
            _repoTablaMock.Setup(r => r.RemoveAsync(TablaId))
                          .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _casoUso.EjecutarAsync(TablaId, ProfesorId);

            Assert.True(resultado.EsExitoso);
            _repoTablaMock.Verify(r => r.RemoveAsync(TablaId), Times.Once);
        }
    }
}
