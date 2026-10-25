using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs
{
    public class ProyectoAulaColaborativoDto
    {
        public int Id { get; set; }
        public int GrupoId { get; set; }
        public string Nombre { get; set; }
        public int CantidadMedallasNecesarias { get; set; }
        public int TotalContribuciones { get; set; }
        public int RecompensaClaseId { get; set; }
        public EstadoPAC Estado { get; set; }
    }
}
