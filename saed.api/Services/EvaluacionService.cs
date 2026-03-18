using Microsoft.EntityFrameworkCore;
using saed.api.Data;
using saed.api.DTOs.Evaluaciones;
using saed.api.Model;
using saed.api.Services.Universidad;

namespace saed.api.Services
{
    public class EvaluacionService
    {
        private readonly AppDbContext _context;
        private readonly UniversidadApiService _universidadApi;

        public EvaluacionService(AppDbContext context, UniversidadApiService universidadApi)
        {
            _context = context;
            _universidadApi = universidadApi;
        }

        public async Task<int> CrearEvaluacion(string matriculaAlumno, CrearEvaluacionDto dto)
        {
            
            var existe = _context.Evaluaciones.Any(e =>
                e.ProcesoId == dto.ProcesoId &&
                e.MatriculaAlumno == matriculaAlumno &&
                e.IdMaestro == dto.IdMaestro &&
                e.MateriaId == dto.MateriaId
            );

            if (existe)
                throw new Exception("Ya evaluaste este maestro en esta materia");

            var evaluacion = new Evaluacion
            {
                ProcesoId = dto.ProcesoId,
                MatriculaAlumno = matriculaAlumno,
                IdMaestro = dto.IdMaestro,
                MateriaId = dto.MateriaId,
                Fecha = DateTime.UtcNow
            };

            _context.Evaluaciones.Add(evaluacion);
            await _context.SaveChangesAsync();

            // Guardar respuestas
            var respuestas = dto.Respuestas.Select(r => new Respuesta
            {
                EvaluacionId = evaluacion.Id,
                PreguntaId = r.PreguntaId,
                OpcionId = r.OpcionId
            }).ToList();

            _context.Respuestas.AddRange(respuestas);

            // Guardar comentario
            if (!string.IsNullOrEmpty(dto.Comentario))
            {
                var comentario = new Comentario
                {
                    EvaluacionId = evaluacion.Id,
                    Texto = dto.Comentario
                };

                _context.Comentarios.Add(comentario);
            }

            await _context.SaveChangesAsync();

            return evaluacion.Id;
        }

        public async Task<List<EvaluacionDisponibleDto>> ObtenerDisponibles(string matricula, string token, int anio, int periodo)
        {
            // 1. Obtener materias del alumno desde universidad
            var materias = await _universidadApi.GetStudentSubjects(token, matricula, anio, periodo);

            // 2. Obtener evaluaciones ya hechas
            var evaluadas = await _context.Evaluaciones
                .Where(e => e.MatriculaAlumno == matricula)
                .Select(e => new { e.IdMaestro, e.MateriaId })
                .ToListAsync();

            // 3. Filtrar las que faltan
            var disponibles = materias
                .Where(m => !evaluadas.Any(e =>
                    e.IdMaestro == m.IdMaestro &&
                    e.MateriaId == m.IdMateria
                ))
                .Select(m => new EvaluacionDisponibleDto
                {
                    IdMaestro = m.IdMaestro,
                    NombreMaestro = m.NombreMaestro,
                    MateriaId = m.IdMateria,
                    Materia = m.Materia,
                    Grupo = m.Grupo
                })
                .ToList();

            return disponibles;
        }
    }
}