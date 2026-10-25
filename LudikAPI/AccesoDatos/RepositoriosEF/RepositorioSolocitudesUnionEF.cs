using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioSolocitudesUnionEF : IRepositorioSolicitudesUnion
    {
        private readonly ContextoDb _db;
        public RepositorioSolocitudesUnionEF(ContextoDb db)
        {
            _db = db;
        }

        public async Task<Resultado> AddAsync(SolicitudUnion unObjeto)
        {
            if (unObjeto == null)
                return Resultado.Falla( new Error("Validation", "La solicitud de unión no puede ser nula."));

            await _db.SolicitudesUnion.AddAsync(unObjeto);
            await _db.SaveChangesAsync();
            return Resultado.Exitoso();
        }

        public async Task<bool> ExisteSolicitudPendiente(string idEstudiante, int idGrupo)
        {
            return await _db.SolicitudesUnion
                .AnyAsync(s => s.EstudianteId == idEstudiante
                            && s.Grupo.Id == idGrupo
                            && s.Estado == EstadoSolicitud.Pendiente);
        }

        public Task<Resultado<IEnumerable<SolicitudUnion>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<SolicitudUnion>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SolicitudUnion>> ObtenerSolicitudesPendientesPorGrupoAsync(int grupoId)
        {
            return await _db.SolicitudesUnion
                .Include(s => s.Estudiante) 
                .Where(s => s.GrupoId == grupoId && s.Estado == EstadoSolicitud.Pendiente)
                .ToListAsync();
        }

        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(SolicitudUnion unObjeto)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado> UpdateAsync(SolicitudUnion unObjeto)
        {
            if (unObjeto == null)
                return Resultado.Falla(new Error("Error.Validation", "El objeto no puede ser nulo."));

            try
            {
                _db.SolicitudesUnion.Update(unObjeto);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", "Error al actualizar la solicitud: " + ex.Message));
            }
        }
        public async Task<SolicitudUnion> GetSolicitudConEstudianteYGrupoPorIdAsync(int id)
        {
            return await _db.SolicitudesUnion
                .Include(s => s.Estudiante)
                .Include(s => s.Grupo)
                    .ThenInclude(g => g.Alumnos)
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
