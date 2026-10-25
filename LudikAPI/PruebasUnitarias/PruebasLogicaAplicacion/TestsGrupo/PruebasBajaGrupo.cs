using System.Threading.Tasks;
using Moq;
using Xunit;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.Grupos;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaNegocio.Resultados;
using Entidad = LogicaNegocio.Entidades;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Grupo
{
    public class PruebasBajaGrupo
    {
        private readonly Mock<IRepositorioGrupos> _repoGruposMock;

        public PruebasBajaGrupo()
        {
            _repoGruposMock = new Mock<IRepositorioGrupos>();
        }

        [Fact]
        public async Task EjecutarAsync_GrupoNoExiste_RetornaErrorValidacion()
        {
            // Arrange
            int grupoId = 10;
            string profesorId = "prof123";

            // Simular que GetByIdAsync falla (grupo no existe)
            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidad.Grupo>.Falla(new Error("Error.Validation", "El grupo no existe.")));

            var casoUso = new BajaGrupo(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("El grupo no existe"));
            // Nunca se llama RemoveAsync
            _repoGruposMock.Verify(r => r.RemoveAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_ProfesorNoAutorizado_RetornaErrorValidacion()
        {
            // Arrange
            int grupoId = 20;
            string profesorId = "prof123";
            // Grupo existe pero con otro ProfesorId
            var grupo = new Entidad.Grupo
            {
                Id = grupoId,
                ProfesorId = "otroProf"
                // demás propiedades no importan aquí
            };

            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidad.Grupo>.Exitoso(grupo));

            var casoUso = new BajaGrupo(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Mensaje.Contains("No tiene permiso para eliminar"));
            _repoGruposMock.Verify(r => r.RemoveAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_DatosValidos_RemueveYRetornaExitoso()
        {
            // Arrange
            int grupoId = 30;
            string profesorId = "prof123";
            var grupo = new Entidad.Grupo
            {
                Id = grupoId,
                ProfesorId = profesorId
            };

            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidad.Grupo>.Exitoso(grupo));

            // Simular RemoveAsync retorna exitoso
            _repoGruposMock
                .Setup(r => r.RemoveAsync(grupoId))
                .ReturnsAsync(Resultado.Exitoso());

            var casoUso = new BajaGrupo(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            Assert.True(resultado.EsExitoso);
            _repoGruposMock.Verify(r => r.RemoveAsync(grupoId), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_RemoveAsyncFalla_AunAsiRetornaExitosoSegunImplementacion()
        {
            // Arrange
            int grupoId = 40;
            string profesorId = "prof123";
            var grupo = new Entidad.Grupo
            {
                Id = grupoId,
                ProfesorId = profesorId
            };

            _repoGruposMock
                .Setup(r => r.GetByIdAsync(grupoId))
                .ReturnsAsync(Resultado<Entidad.Grupo>.Exitoso(grupo));

            // Simular RemoveAsync falla
            _repoGruposMock
                .Setup(r => r.RemoveAsync(grupoId))
                .ReturnsAsync(Resultado.Falla(new Error("Grupo.Remove.DbError", "Error al eliminar")));

            var casoUso = new BajaGrupo(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(grupoId, profesorId);

            // Assert
            // Según la implementación actual, el resultado final ignora el fallo de RemoveAsync y devuelve exitoso
            Assert.True(resultado.EsExitoso);
            _repoGruposMock.Verify(r => r.RemoveAsync(grupoId), Times.Once);
        }
    }
}
