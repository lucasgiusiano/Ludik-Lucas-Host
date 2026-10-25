using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.UsuarioDTOs
{
    public class UsuarioBaseDto
    {
        public string Id { get; set; }            
        public string UserName { get; set; }

        [JsonIgnore]
        public IList<string> Roles { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
