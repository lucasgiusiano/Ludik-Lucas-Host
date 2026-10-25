using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObject
{

    public record RangoFechas
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }

        public RangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            this.fechaInicio= fechaInicio;
            this.fechaFin= fechaFin;
        }

        private RangoFechas() { }

    }
}
