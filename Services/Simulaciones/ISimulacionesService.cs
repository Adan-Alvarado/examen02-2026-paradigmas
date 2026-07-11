using ExamenU2Paradigmas.Dtos.Common;
using ExamenU2Paradigmas.Dtos.Simulaciones;

namespace ExamenU2Paradigmas.Services.Simulaciones
{
    public interface ISimulacionesService
    {
        Task<ResponseDto<ActionResponseDto>> CreateAsync(SimulacionCreateDto dto);
        Task<ResponseDto<SimulacionDto>> GetOneByIdAsync(int id);
        Task<ResponseDto<ItemsDto<List<SimulacionDto>>>> GetAllAsync();
        Task<ResponseDto<ItemsDto<List<ProyeccionMesDto>>>> GetProyeccionMensualAsync(int id);
        Task<ResponseDto<ItemsDto<List<ProyeccionAnioDto>>>> GetProyeccionAnualAsync(int id);
    }
}
