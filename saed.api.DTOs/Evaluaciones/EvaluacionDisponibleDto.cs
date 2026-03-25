using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.DTOs.Evaluaciones
{
    public class EvaluacionDisponibleDto
    {
        public string IdMaestro { get; set; } = string.Empty;

        public string NombreMaestro { get; set; } = string.Empty;

        public string MateriaId { get; set; } = string.Empty;

        public string Materia { get; set; } = string.Empty;

        public string Grupo { get; set; } = string.Empty;
    }
}
