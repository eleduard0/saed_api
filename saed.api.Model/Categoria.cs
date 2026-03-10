using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.Model
{
    public class Categoria
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int InstrumentoId { get; set; }

        public int Orden { get; set; }

        public Instrumento Instrumento { get; set; }

        public ICollection<Pregunta> Preguntas { get; set; }
    }
}
