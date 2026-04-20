namespace saed.api.DTOs.Evaluaciones
{
    // Calificación completa de un maestro en un proceso
    public class CalificacionMaestroDto
    {
        public string IdMaestro { get; set; } = string.Empty;
        public string NombreMaestro { get; set; } = string.Empty;
        public int ProcesoId { get; set; }
        public int TotalEvaluaciones { get; set; }

        /// <summary>
        /// Promedio global = suma de todos los valores / total de respuestas
        /// Escala de 1 a 5 (o la que defina el instrumento)
        /// </summary>
        public double CalificacionGlobal { get; set; }

        public List<CalificacionPreguntaDto> PorPregunta { get; set; } = new();
        public List<string> Comentarios { get; set; } = new();
    }

    // Calificación desglosada por pregunta
    public class CalificacionPreguntaDto
    {
        public int PreguntaId { get; set; }
        public string TextoPregunta { get; set; } = string.Empty;
        public double Promedio { get; set; }
        public int TotalRespuestas { get; set; }

        // Cuántas veces se eligió cada opción (texto de opción → conteo)
        public Dictionary<string, int> Distribucion { get; set; } = new();
    }

    // Resumen para el listado del admin (ranking de maestros)
    public class ResumenCalificacionDto
    {
        public string IdMaestro { get; set; } = string.Empty;
        public string NombreMaestro { get; set; } = string.Empty;
        public int TotalEvaluaciones { get; set; }
        public double CalificacionGlobal { get; set; }
    }
}