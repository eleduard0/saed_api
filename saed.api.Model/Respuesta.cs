using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.Model
{
    public class Respuesta
    {
        public int Id { get; set; }

        public int EvaluacionId { get; set; }

        public int PreguntaId { get; set; }

        public int OpcionId { get; set; }

        // Relación
        public Evaluacion Evaluacion { get; set; } = null!;
    }
}
