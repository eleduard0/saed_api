using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.Model
{
    public class OpcionRespuesta
    {
        public int Id { get; set; }

        public int PreguntaId { get; set; }

        public int Valor { get; set; }

        public string Texto { get; set; }

        public Pregunta Pregunta { get; set; }
    }
}
