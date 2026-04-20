using Microsoft.EntityFrameworkCore;
using saed.api.Data;
using saed.api.DTOs.Evaluaciones;

namespace saed.api.Services
{
    public class CalificacionService
    {
        private readonly AppDbContext _context;

        public CalificacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CalificacionMaestroDto> ObtenerCalificacion(string idMaestro, int procesoId)
        {
            var evaluaciones = await _context.Evaluaciones
                .Where(e => e.IdMaestro == idMaestro && e.ProcesoId == procesoId)
                .Include(e => e.Respuestas)
                    .ThenInclude(r => r.Pregunta)   // ← texto real de la pregunta
                .Include(e => e.Respuestas)
                    .ThenInclude(r => r.Opcion)     // ← texto y valor de la opción
                .Include(e => e.Comentario)
                .ToListAsync();

            if (!evaluaciones.Any())
                throw new Exception("No hay evaluaciones para este maestro en el proceso indicado.");

            var proceso = await _context.Procesos.FindAsync(procesoId)
                ?? throw new Exception("Proceso no encontrado.");

            var todasRespuestas = evaluaciones
                .SelectMany(e => e.Respuestas)
                .Where(r => r.Opcion != null)
                .ToList();

            var calificacionGlobal = todasRespuestas.Any()
                ? Math.Round(todasRespuestas.Average(r => r.Opcion.Valor), 2)
                : 0;

            var porPregunta = todasRespuestas
                .GroupBy(r => r.PreguntaId)
                .Select(g =>
                {
                    var textoPregunta = g
                        .Select(r => r.Pregunta?.Texto)
                        .FirstOrDefault(t => !string.IsNullOrEmpty(t))
                        ?? "Sin texto";

                    return new CalificacionPreguntaDto
                    {
                        PreguntaId = g.Key,
                        TextoPregunta = textoPregunta,
                        Promedio = Math.Round(g.Average(r => r.Opcion.Valor), 2),
                        TotalRespuestas = g.Count(),
                        Distribucion = g
                            .GroupBy(r => r.Opcion?.Texto ?? "Sin opción")
                            .ToDictionary(d => d.Key, d => d.Count())
                    };
                })
                .OrderBy(p => p.PreguntaId)
                .ToList();

            var comentarios = evaluaciones
                .Where(e => e.Comentario != null && !string.IsNullOrWhiteSpace(e.Comentario.Texto))
                .Select(e => e.Comentario!.Texto)
                .ToList();

            return new CalificacionMaestroDto
            {
                IdMaestro = idMaestro,
                NombreMaestro = evaluaciones.First().NombreMaestro,
                ProcesoId = procesoId,
                TotalEvaluaciones = evaluaciones.Count,
                CalificacionGlobal = calificacionGlobal,
                PorPregunta = porPregunta,
                Comentarios = comentarios
            };
        }

        public async Task<List<ResumenCalificacionDto>> ListarCalificaciones(int procesoId)
        {
            var grupos = await _context.Evaluaciones
                .Where(e => e.ProcesoId == procesoId)
                .Include(e => e.Respuestas)
                    .ThenInclude(r => r.Opcion)
                .GroupBy(e => new { e.IdMaestro, e.NombreMaestro })
                .ToListAsync();

            return grupos.Select(g =>
            {
                var todasRespuestas = g
                    .SelectMany(e => e.Respuestas)
                    .Where(r => r.Opcion != null)
                    .ToList();

                return new ResumenCalificacionDto
                {
                    IdMaestro = g.Key.IdMaestro,
                    NombreMaestro = g.Key.NombreMaestro,
                    TotalEvaluaciones = g.Count(),
                    CalificacionGlobal = todasRespuestas.Any()
                        ? Math.Round(todasRespuestas.Average(r => r.Opcion.Valor), 2)
                        : 0
                };
            })
            .OrderByDescending(r => r.CalificacionGlobal)
            .ToList();
        }
    }
}