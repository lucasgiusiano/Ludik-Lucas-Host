using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioPreguntasSeguridadEF : IRepositorioPreguntasSeguridad
    {
        private readonly ContextoDb _db;
        public RepositorioPreguntasSeguridadEF(ContextoDb db)
        {
            _db = db;
        }

        public Task<Resultado> AddAsync(PreguntaRespuestaSeguridad unObjeto)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<IEnumerable<PreguntaRespuestaSeguridad>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<List<PreguntaRespuestaSeguridad>>> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            try
            {

                // Se usa AsNoTracking() como optimización, ya que no vamos a modificar esta entidad
                var usuario = await _db.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserName == nombreUsuario);

                if (usuario == null)
                    return Resultado<List<PreguntaRespuestaSeguridad>>.Falla(Error.NotFound);
                
                var preguntas = await _db.PreguntasRespuestasSeguridad.Include(p => p.PreguntaDeSeguridad)
                    .Where(p => p.EstudianteId == usuario.Id).ToListAsync();

                return Resultado<List<PreguntaRespuestaSeguridad>>.Exitoso(preguntas);
            }
            catch (Exception e)
            {
                return Resultado<List<PreguntaRespuestaSeguridad>>.Falla(new Error("Error.Unexpected", e.Message));
            }
        }

        public Task<Resultado<PreguntaRespuestaSeguridad>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(PreguntaRespuestaSeguridad unObjeto)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> UpdateAsync(PreguntaRespuestaSeguridad unObjeto)
        {
            throw new NotImplementedException();
        }
    }
}
