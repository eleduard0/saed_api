using Microsoft.EntityFrameworkCore;
using saed.api.Data;
using saed.api.DTOs.Evaluaciones;
using saed.api.Model;
using saed.api.Services.Universidad;
using System.Security.Claims;

namespace saed.api.Services
{
    public class EvaluacionService
    {
        private readonly AppDbContext _context;
        private readonly UniversidadApiService _universidadApi;
        private readonly IHttpContextAccessor _httpContext;

        public EvaluacionService(
            AppDbContext context,
            UniversidadApiService universidadApi,
            IHttpContextAccessor httpContext)
        {
            _context = context;
            _universidadApi = universidadApi;
            _httpContext = httpContext;
        }

        // ─── Helpers privados para leer claims del JWT ────────────────────────

        private string ObtenerMatricula()
        {
            var matricula = _httpContext.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(matricula))
                throw new UnauthorizedAccessException("No se pudo obtener la matrícula del token");

            return matricula;
        }

        private string ObtenerTokenUniversidad()
        {
            var token = _httpContext.HttpContext?.User
                .FindFirstValue("token_universidad");

            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("No se encontró el token universitario en la sesión");

            return token;
        }

        // ─── Crear evaluación ─────────────────────────────────────────────────

        public async Task<int> CrearEvaluacion(CrearEvaluacionDto dto)
        {
            var matriculaAlumno = ObtenerMatricula();

            var proceso = await _context.Procesos
                .FirstOrDefaultAsync(p => p.Activo);

            if (proceso == null)
                throw new Exception("No hay proceso activo");

            var existe = await _context.Evaluaciones.AnyAsync(e =>
                e.ProcesoId == proceso.Id &&
                e.MatriculaAlumno == matriculaAlumno &&
                e.IdMaestro == dto.IdMaestro &&
                e.MateriaId == dto.MateriaId
            );

            if (existe)
                throw new Exception("Ya evaluaste este maestro en esta materia");

            var evaluacion = new Evaluacion
            {
                ProcesoId = proceso.Id,
                MatriculaAlumno = matriculaAlumno,
                IdMaestro = dto.IdMaestro,
                MateriaId = dto.MateriaId,
                Fecha = DateTime.UtcNow
            };

            _context.Evaluaciones.Add(evaluacion);
            await _context.SaveChangesAsync();

            var respuestas = dto.Respuestas.Select(r => new Respuesta
            {
                EvaluacionId = evaluacion.Id,
                PreguntaId = r.PreguntaId,
                OpcionId = r.OpcionId
            }).ToList();

            _context.Respuestas.AddRange(respuestas);

            if (!string.IsNullOrEmpty(dto.Comentario))
            {
                _context.Comentarios.Add(new Comentario
                {
                    EvaluacionId = evaluacion.Id,
                    Texto = dto.Comentario
                });
            }

            await _context.SaveChangesAsync();

            return evaluacion.Id;
        }

        // ─── Obtener evaluaciones disponibles ─────────────────────────────────

        public async Task<List<EvaluacionDisponibleDto>> ObtenerDisponibles()
        {
            var matricula = ObtenerMatricula();
            var tokenUniversidad = ObtenerTokenUniversidad();

            var proceso = await _context.Procesos
                .FirstOrDefaultAsync(p => p.Activo);

            if (proceso == null)
                throw new Exception("No hay proceso activo");

            var materias = await _universidadApi.GetStudentSubjects(
                tokenUniversidad,
                matricula,
                proceso.Anio,
                proceso.Periodo
            );

            var evaluadas = await _context.Evaluaciones
                .Where(e => e.MatriculaAlumno == matricula && e.ProcesoId == proceso.Id)
                .Select(e => new { e.IdMaestro, e.MateriaId })
                .ToListAsync();

            return materias
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
        }
    }
}