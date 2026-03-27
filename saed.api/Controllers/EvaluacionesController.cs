using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saed.api.DTOs.Evaluaciones;
using saed.api.Services;

namespace saed.api.Controllers
{
    [ApiController]
    [Route("api/evaluaciones")]
    [Authorize]
    public class EvaluacionesController : ControllerBase
    {
        private readonly EvaluacionService _service;

        public EvaluacionesController(EvaluacionService service)
        {
            _service = service;
        }

        // POST api/evaluaciones
        [HttpPost]
        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> Crear(CrearEvaluacionDto dto)
        {
            try
            {
                var id = await _service.CrearEvaluacion(dto);
                return Ok(new { id });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/evaluaciones/disponibles
        [HttpGet("disponibles")]
        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> ObtenerDisponibles()
        {
            try
            {
                var result = await _service.ObtenerDisponibles();
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}