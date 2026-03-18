using Microsoft.AspNetCore.Mvc;
using saed.api.DTOs.Evaluaciones;
using saed.api.Services;

namespace saed.api.Controllers
{
    [ApiController]
    [Route("api/evaluaciones")]
    public class EvaluacionesController : ControllerBase
    {
        private readonly EvaluacionService _service;

        public EvaluacionesController(EvaluacionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearEvaluacionDto dto)
        {
            
            var matricula = "L532410081";

            var id = await _service.CrearEvaluacion(matricula, dto);

            return Ok(new { id });
        }

        [HttpGet("disponibles")]
        public async Task<IActionResult> ObtenerDisponibles()
        {
            
            var matricula = "L532410081";

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var result = await _service.ObtenerDisponibles(matricula, token, 2024, 1);

            return Ok(result);
        }
    }
}