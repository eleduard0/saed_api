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
            //  Validar proceso activo
            var proceso = await _context.Procesos
                .FirstOrDefaultAsync(p => p.Activo);

            if (proceso == null)
                throw new Exception("No hay proceso activo");

            //  Validar que no exista evaluación duplicada
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

            //  Guardar respuestas
            var respuestas = dto.Respuestas.Select(r => new Respuesta
            {
                EvaluacionId = evaluacion.Id,
                PreguntaId = r.PreguntaId,
                OpcionId = r.OpcionId
            }).ToList();

            _context.Respuestas.AddRange(respuestas);

            //  Guardar comentario
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

        public async Task<List<EvaluacionDisponibleDto>> ObtenerDisponibles(string matricula, string token)
        {
            //  1. Obtener proceso activo
            var proceso = await _context.Procesos
                .FirstOrDefaultAsync(p => p.Activo);

            if (proceso == null)
                throw new Exception("No hay proceso activo");

            //  2. Obtener materias desde API externa
            var materias = await _universidadApi.GetStudentSubjects(
                token,
                matricula,
                proceso.Anio,
                proceso.Periodo
            );

            //  3. Obtener evaluaciones ya hechas (SOLO DEL PROCESO ACTUAL)
            var evaluadas = await _context.Evaluaciones
                .Where(e => e.MatriculaAlumno == matricula && e.ProcesoId == proceso.Id)
                .Select(e => new { e.IdMaestro, e.MateriaId })
                .ToListAsync();

            // 🔥 4. Filtrar disponibles
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