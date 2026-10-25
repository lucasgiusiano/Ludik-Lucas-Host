using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioEstudiantesEF : IRepositorioEstudiantes
    {
        private readonly ContextoDb _db;
        public RepositorioEstudiantesEF(ContextoDb db)
        {
            _db = db;
        }

        public async Task<Resultado> AddAsync(Estudiante estudianteNuevo)
        {
            if (estudianteNuevo == null)
                return Resultado.Falla(new Error("Error.Validation", "El estudiante no puede ser nulo.")); 

            try
            {
                _db.Estudiantes.Add(estudianteNuevo);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (DbUpdateException dbEx)
            {
                var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
                return Resultado.Falla(new Error("Error.Unexpected", $"Error al guardar el estudiante: {detalle}")); 
            }
            catch (Exception e)
            {
                return Resultado.Falla(new Error("Error.Unexpected", e.Message)); 
            }
        }


       

        public async Task<Resultado<Estudiante>> GetByIdAsync(int id)
        {
            try
            {
                var estudiante = await _db.Estudiantes.FindAsync(id);
                if (estudiante == null)
                    return Resultado<Estudiante>.Falla(Error.NotFound); 
                
                return Resultado<Estudiante>.Exitoso(estudiante); 
            }
            catch (Exception e)
            {
                return Resultado<Estudiante>.Falla(new Error("Error.Unexpected", e.Message));
            }
        }
        public async Task<Resultado<Estudiante>> GetByStringIdAsync(string id)
        {
            try
            {
                var estudiante = await _db.Estudiantes
                    .Include(e => e.Perfiles)             
                    .Include(e => e.Hitos)                
                    .Include(e => e.PreguntasSeguridad)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (estudiante == null)
                    return Resultado<Estudiante>.Falla(Error.NotFound);
                
 
                return Resultado<Estudiante>.Exitoso(estudiante);
            }
            catch (Exception e)
            {

                return Resultado<Estudiante>.Falla(new Error("Error.Unexpected", e.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Estudiante>>> GetAllAsync()
        {
            try
            {
                var estudiantes = await _db.Estudiantes.Include(e => e.Perfiles).ToListAsync();
                return Resultado<IEnumerable<Estudiante>>.Exitoso(estudiantes);
            }
            catch (Exception e)
            {
                return Resultado<IEnumerable<Estudiante>>.Falla(Error.Unexpected);
            }
        }


        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(Estudiante unObjeto)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado> UpdateAsync(Estudiante unObjeto)
        {
            try
            {
                _db.Estudiantes.Update(unObjeto);

                return Resultado.Exitoso();
            }
            catch (Exception ex)
            {
                return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
            }
        }

        public List<Medalla> getMedallasAlumno(int idAlumno, int idGrupo)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<Estudiante>> GetByPerfilIdAsync(int perfilId)
        {
            try
            {
                // 1) Recuperar el Perfil para saber su EstudianteId
                var perfil = await _db.PerfilesEstudiantes
                    .AsNoTracking()
                    .Include(p => p.Estudiante)          // trae el estudiante
                    .FirstOrDefaultAsync(p => p.Id == perfilId);

                if (perfil == null)
                    return Resultado<Estudiante>.Falla(
                        new Error("Error.NotFound", $"No se encontró el perfil con Id {perfilId}."));

                var estudianteId = perfil.EstudianteId;

                // 2) Recuperar EL ESTUDIANTE completo con todos sus perfiles y medallas
                var estudiante = await _db.Estudiantes
                    .AsNoTracking()
                    .Where(e => e.Id == estudianteId)
                    .Include(e => e.EstPotenciador)
                    .Include(e => e.Perfiles)
                        .ThenInclude(p => p.MedallasObtenidas)
                            .ThenInclude(pm => pm.Medalla)
                    // si persistes PotenciadorActivo, inclúyelo también:
                    .Include(e => e.Perfiles)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();

                if (estudiante == null)
                    return Resultado<Estudiante>.Falla(
                        new Error("Error.NotFound", $"Estudiante {estudianteId} no encontrado."));

                return Resultado<Estudiante>.Exitoso(estudiante);
            }
            catch (Exception ex)
            {
                return Resultado<Estudiante>.Falla(
                    new Error("Error.Unexpected", ex.Message));
            }
        }

        public Task<Estudiante> GetByIdAsyncString(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<Resultado<Estudiante>> GetByIdConHitosAsync(string id)
        {
            try
            {
                var estudiante = await _db.Estudiantes
                    .Include(e => e.Hitos)          
                    .Include(e => e.Perfiles)   
                    .Include(e => e.EstPotenciador)
                        .ThenInclude(ep => ep.Potenciador)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (estudiante == null)
                    return Resultado<Estudiante>.Falla(Error.NotFound);

                return Resultado<Estudiante>.Exitoso(estudiante);
            }
            catch (Exception e)
            {
                return Resultado<Estudiante>.Falla(new Error("Error.Unexpected", e.Message));
            }
        }
    }
}
