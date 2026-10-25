using InterfacesRepositorio;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioUsuariosEF : IRepositorioUsuarios
    {
        private readonly ContextoDb _db;
        private readonly UserManager<Usuario> _userManager;
        public RepositorioUsuariosEF(ContextoDb db, UserManager<Usuario> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public Task<Resultado<Usuario>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<IEnumerable<Usuario>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<Resultado<Usuario>> GetUsuarioPorNombreAsync(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return Resultado<Usuario>.Falla(Error.Validation);

            var usuario = await _db.Users
                .SingleOrDefaultAsync(e => e.UserName == nombreUsuario);

            if (usuario != null)
                return Resultado<Usuario>.Exitoso(usuario);

            return Resultado<Usuario>.Falla(new Error( "Error.NotFound",$"Usuario con '{nombreUsuario}' no encontrado."));
        }


        public async Task<IList<string>> GetRolesAsync(Usuario usuario)
        {
            if(usuario == null) 
                throw new ArgumentNullException(nameof(usuario));
            return await _userManager.GetRolesAsync(usuario);
        }

        public async Task<Resultado<Usuario>> GetByStringIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Resultado<Usuario>.Falla(Error.Validation);

            var usuario = await _db.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            if (usuario != null)
                return Resultado<Usuario>.Exitoso(usuario);

            return Resultado<Usuario>.Falla(new Error("Error.NotFound", $"Usuario con '{id}' no encontrado."));
        }

        public async Task<Resultado> AddAsync(Usuario usuarioNuevo)
        {
            if(usuarioNuevo == null)
                return Resultado.Falla(new Error("Error.Validation", "El usuario proporcionado no es válido."));
            try
            {
                await _db.Users.AddAsync(usuarioNuevo);
                await _db.SaveChangesAsync();
                return Resultado.Exitoso();
            }
            catch (Exception e)
            {
                return Resultado.Falla(new Error ("Error.Unexpected", "Surgio un error al guardar el usuario"));
            }

            
        }

        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(Usuario unObjeto)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> UpdateAsync(Usuario unObjeto)
        {
            throw new NotImplementedException();
        }
    }
}
