using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.TablaClasificacionDTOs
{
    public class TablaClasificacionInfoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int MedallaAsociadaId { get; set; }
        public string MedallaAsociadaNombre { get; set; }
        public List<ParticipanteTablaDto> Participantes { get; set; }
    }
}
