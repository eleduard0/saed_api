using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saed.api.Services;

namespace saed.api.Controllers
{
    [ApiController]
    [Route("api/calificaciones")]
    [Authorize(Roles = "Admin")]
    public class CalificacionesController : ControllerBase
    {
        private readonly CalificacionService _service;

        public CalificacionesController(CalificacionService service)
        {
            _service = service;
        }

        // GET api/calificaciones/procesos/{procesoId}
        // Lista todos los maestros del proceso con su calificación global (para el ranking)
        [HttpGet("procesos/{procesoId}")]
        public async Task<IActionResult> ListarPorProceso(int procesoId)
        {
            try
            {
                var resultado = await _service.ListarCalificaciones(procesoId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/calificaciones/procesos/{procesoId}/maestros/{idMaestro}
        // Calificación detallada de un maestro (para el reporte PDF)
        [HttpGet("procesos/{procesoId}/maestros/{idMaestro}")]
        public async Task<IActionResult> ObtenerCalificacion(int procesoId, string idMaestro)
        {
            try
            {
                var resultado = await _service.ObtenerCalificacion(idMaestro, procesoId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}