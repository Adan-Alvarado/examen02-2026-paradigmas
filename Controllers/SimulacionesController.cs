using ExamenU2Paradigmas.Dtos.Simulaciones;
using ExamenU2Paradigmas.Services.Simulaciones;
using Microsoft.AspNetCore.Mvc;

namespace ExamenU2Paradigmas.Controllers
{
    [Route("api/simulaciones")]
    [ApiController]
    public class SimulacionesController : ControllerBase
    {
        private readonly ISimulacionesService _simulacionesService;

        public SimulacionesController(ISimulacionesService simulacionesService)
        {
            _simulacionesService = simulacionesService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SimulacionCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _simulacionesService.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var response = await _simulacionesService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOne(int id)
        {
            var result = await _simulacionesService.GetOneByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/proyeccion-mensual")]
        public async Task<ActionResult> GetProyeccionMensual(int id)
        {
            var result = await _simulacionesService.GetProyeccionMensualAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/proyeccion-anual")]
        public async Task<ActionResult> GetProyeccionAnual(int id)
        {
            var result = await _simulacionesService.GetProyeccionAnualAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
