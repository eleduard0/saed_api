using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.DTOs.Procesos
{
    public class CrearProcesoDto
    {
        public int Anio { get; set; }

        public int Periodo { get; set; } 

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }
    }
}
