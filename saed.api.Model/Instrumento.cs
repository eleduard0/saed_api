using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.Model
{
    public class Instrumento
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int AcademiaId { get; set; }

        public bool Activo { get; set; }

        public Academia Academia { get; set; }

        public ICollection<Categoria> Categorias { get; set; }
    }
}
