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

        public int AcademiaId { get; set; }

        public string AlumnoMatricula { get; set; }

        public string ProfesorMatricula { get; set; }

        public string Grupo { get; set; }

        public string Materia { get; set; }

        public DateTime Fecha { get; set; }

        public Proceso Proceso { get; set; }

        public Academia Academia { get; set; }

        public ICollection<Respuesta> Respuestas { get; set; }

        public Comentario Comentario { get; set; }
    }
}
