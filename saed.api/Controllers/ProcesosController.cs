using Microsoft.AspNetCore.Mvc;
using saed.api.DTOs.Procesos;
using saed.api.Services;

namespace saed.api.Controllers
{
    [ApiController]
    [Route("api/procesos")]
    public class ProcesosController : ControllerBase
    {
        private readonly ProcesoService _service;

        public ProcesosController(ProcesoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CrearProceso(CrearProcesoDto dto)
        {
            var proceso = await _service.CrearProceso(
                dto.Anio,
                dto.Periodo,
                dto.FechaInicio,
                dto.FechaFin
            );

            return Ok(proceso);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProcesos()
        {
            var procesos = await _service.ObtenerProcesos();
            return Ok(procesos);
        }

        [HttpGet("activo")]
        public async Task<IActionResult> ObtenerActivo()
        {
            var proceso = await _service.ObtenerActivo();

            if (proceso == null)
                return NotFound("No hay proceso activo");

            return Ok(proceso);
           
        }
        [HttpPut("{id}/cerrar")]
        public async Task<IActionResult> CerrarProceso(int id)
        {
            var proceso = await _service.CerrarProceso(id);

            return Ok(proceso);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var proceso = await _service.ObtenerPorId(id);

            return Ok(proceso);
        }
    }
}