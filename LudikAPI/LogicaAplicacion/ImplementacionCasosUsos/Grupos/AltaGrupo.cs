using Entidades=LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOsMappers.GrupoMappers;
using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Http;



namespace LogicaAplicacion.ImplementacionCasosUsos.Grupos
{
    public class AltaGrupo: IAltaGrupo
    {
        private readonly IRepositorioGrupos _repositorioGrupos;
        private readonly IRepositorioTablasEquivalencia _repoTablasEquivalencia;
        private readonly IGeneradorEnlaceGrupo _generadorEnlace;
        private readonly IRepositorioProfesores _repositorioProfesores;
        private readonly ICrearTiendaPorDefecto _crearTiendaPorDefecto;

        public AltaGrupo(
            IRepositorioGrupos repositorioGrupo,
            IRepositorioTablasEquivalencia repoTablasEquivalencia,
            IGeneradorEnlaceGrupo generadorEnlace,
            IRepositorioProfesores repositorioProfesores,
            ICrearTiendaPorDefecto crearTiendaPorDefecto)
        {
            _repositorioGrupos = repositorioGrupo;
            _repoTablasEquivalencia = repoTablasEquivalencia;
            _generadorEnlace = generadorEnlace;
            _repositorioProfesores = repositorioProfesores;
            _crearTiendaPorDefecto = crearTiendaPorDefecto;
        }

        public async Task<Resultado> EjecutarAsync(GrupoAltaRequestDto grupoRequestDto, string profesorId)
        {
            if (grupoRequestDto == null)
                return Resultado.Falla(new Error("Error.Validation", "No hay información para poder dar de alta el grupo."));

            var resultadoTabla = await _repoTablasEquivalencia.GetByIdAsync(grupoRequestDto.TablaEquivalenciaId);
            if (resultadoTabla == null)
                return Resultado.Falla(new Error("Error.Validation", "No se encontró la tabla de equivalencia especificada."));

            var resultadoProfesor = await _repositorioProfesores.GetByStringIdAsync(profesorId);
            if (resultadoProfesor.EsFallo)
                return Resultado.Falla(resultadoProfesor.Errores);
            var profesor = resultadoProfesor.Valor;
            if(!profesor.TablasEquivalencia!.Any(g => g.Id == resultadoTabla.Valor.Id))
                return Resultado.Falla(new Error("Error.Unauthorized", "La tabla no se encuentra dentro de las tablas del profesor logueado."));
            

            var codigoUnicoInvitacion = Guid.NewGuid().ToString("N");

            var resultadoUrlInvitacion = _generadorEnlace.GenerarEnlace(codigoUnicoInvitacion);

            if (string.IsNullOrEmpty(resultadoUrlInvitacion.Valor))
                return Resultado<GrupoDto>.Falla(new Error("Error.Validation", "No se pudo generar la URL de invitación para el grupo."));
            
            var resultadoTiendaPorDefecto = await _crearTiendaPorDefecto.CrearAsync();

            if (resultadoTiendaPorDefecto.EsFallo)
                return resultadoTiendaPorDefecto;

            var grupo =new Entidades.Grupo
            {
                Nombre = grupoRequestDto.Nombre,
                TablaEquivalencia = resultadoTabla.Valor,
                ProfesorId = profesorId,
                Institucion = grupoRequestDto.Institucion,
                Materia = grupoRequestDto.Materia,
                EnlaceUnion = new Entidades.EnlaceUnion(resultadoUrlInvitacion.Valor, codigoUnicoInvitacion),
                Tienda = resultadoTiendaPorDefecto.Valor
            };

            var resultadoValidacion = grupo.esValido();
            if (resultadoValidacion.EsFallo)
                return resultadoValidacion;

            await _repositorioGrupos.AddAsync(grupo);

            return Resultado.Exitoso();

        }

    }

}
