using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saed.api.DTOs.Evaluaciones
{
    public class CrearEvaluacionDto
    {
        public int ProcesoId { get; set; }

        public string IdMaestro { get; set; } = string.Empty;

        public string MateriaId { get; set; } = string.Empty;

        public List<RespuestaDto> Respuestas { get; set; } = new();

        public string? Comentario { get; set; }
    }

    public class RespuestaDto
    {
        public int PreguntaId { get; set; }

        public int OpcionId { get; set; }
    }
}
