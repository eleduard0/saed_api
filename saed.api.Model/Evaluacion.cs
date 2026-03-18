using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.Model
{
    public class Evaluacion
    {
        public int Id { get; set; }

        public int ProcesoId { get; set; }

        public string MatriculaAlumno { get; set; } = string.Empty;

        public string IdMaestro { get; set; } = string.Empty;

        public string MateriaId { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Relaciones
        public List<Respuesta> Respuestas { get; set; } = new();

        public Comentario? Comentario { get; set; }
    }
}