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
        public string Periodo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }
    }
}
