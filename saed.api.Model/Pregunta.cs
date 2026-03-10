using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.Model
{
    public class Pregunta
    {
        public int Id { get; set; }

        public string Texto { get; set; }

        public int CategoriaId { get; set; }

        public string Tipo { get; set; }

        public int Orden { get; set; }

        public Categoria Categoria { get; set; }

        public ICollection<OpcionRespuesta> Opciones { get; set; }
    }
}
