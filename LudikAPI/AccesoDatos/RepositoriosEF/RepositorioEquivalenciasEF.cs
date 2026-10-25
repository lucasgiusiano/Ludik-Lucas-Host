using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace AccesoDatos.RepositoriosEF
{
    public class RepositorioEquivalenciasEF : IRepositorioEquivalencias
    {
        private readonly ContextoDb _db;
        public RepositorioEquivalenciasEF(ContextoDb db)
        {
            _db = db;
        }

        public Task<Resultado> AddAsync(Equivalencia unObjeto)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<IEnumerable<Equivalencia>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<Equivalencia>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> RemoveAsync(Equivalencia unObjeto)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado> UpdateAsync(Equivalencia unObjeto)
        {
            throw new NotImplementedException();
        }
    }
}
