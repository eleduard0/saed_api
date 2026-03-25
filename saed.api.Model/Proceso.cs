 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.Model
{
    public class Proceso
    {
        public int Id { get; set; }

        public int Anio { get; set; }

        public int Periodo { get; set; } // 1, 2, 3

        public string Nombre { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public bool Activo { get; set; }
    }
}
