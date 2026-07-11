using ExamenU2Paradigmas.Constants;
using ExamenU2Paradigmas.Database;
using ExamenU2Paradigmas.Dtos.Common;
using ExamenU2Paradigmas.Dtos.Simulaciones;
using ExamenU2Paradigmas.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamenU2Paradigmas.Services.Simulaciones
{
    public class SimulacionesService : ISimulacionesService
    {
        private readonly AhorrosDbContext _context;

        public SimulacionesService(AhorrosDbContext context)
        {
            _context = context;
        }

        // Cálculo de saldo acumulado al mes

        private static decimal CalcularSaldo(decimal capital, decimal tasaMensual, int mes)
        {
            return capital * (decimal)Math.Pow((double)(1 + tasaMensual), mes);
        }


        // Crear nueva simulación y persistir proyecciones

        public async Task<ResponseDto<ActionResponseDto>> CreateAsync(SimulacionCreateDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            decimal capital = dto.CapitalInicial;
            decimal tasaAnual = dto.TasaAnual;
            int plazoAnios = dto.PlazoAnios;

            decimal tasaMensual = tasaAnual / 12m;
            int totalMeses = plazoAnios * 12;

            // Calcular monto final e interés total
            decimal montoFinal = CalcularSaldo(capital, tasaMensual, totalMeses);
            decimal interesTotal = montoFinal - capital;

            // Crear y guardar la simulación
            var simulacion = new Simulacion
            {
                CapitalInicial = capital,
                TasaAnual = tasaAnual,
                PlazoAnios = plazoAnios,
                MontoFinal = Math.Round(montoFinal, 2),
                InteresTotal = Math.Round(interesTotal, 2),
                FechaCreacion = DateTime.UtcNow
            };

            _context.Simulaciones.Add(simulacion);
            await _context.SaveChangesAsync(); // Obtener el ID generado

            // Generar proyecciones mensuales
            var proyMensuales = new List<ProyeccionMes>();
            for (int n = 1; n <= totalMeses; n++)
            {
                decimal saldoN = CalcularSaldo(capital, tasaMensual, n);
                decimal saldoN1 = CalcularSaldo(capital, tasaMensual, n - 1);
                decimal interesMes = saldoN - saldoN1;

                proyMensuales.Add(new ProyeccionMes
                {
                    SimulacionId = simulacion.Id,
                    Mes = n,
                    SaldoAcumulado = Math.Round(saldoN, 6),
                    InteresMes = Math.Round(interesMes, 6)
                });
            }

            // Generar proyecciones anuales
            var proyAnuales = new List<ProyeccionAnio>();
            for (int k = 1; k <= plazoAnios; k++)
            {
                decimal saldoAnioK = CalcularSaldo(capital, tasaMensual, k * 12);
                decimal saldoAnioK1 = CalcularSaldo(capital, tasaMensual, (k - 1) * 12);
                decimal interesAnio = saldoAnioK - saldoAnioK1;

                proyAnuales.Add(new ProyeccionAnio
                {
                    SimulacionId = simulacion.Id,
                    Anio = k,
                    SaldoAcumulado = Math.Round(saldoAnioK, 6),
                    InteresAnio = Math.Round(interesAnio, 6)
                });
            }

            _context.ProyeccionesMensuales.AddRange(proyMensuales);
            _context.ProyeccionesAnuales.AddRange(proyAnuales);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new ResponseDto<ActionResponseDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Message = HttpMessageResponse.REGISTER_CREATED,
                Status = true,
                Data = new ActionResponseDto { Id = simulacion.Id }
            };
        }

        //  Consulta general de una simulación por Id
        public async Task<ResponseDto<SimulacionDto>> GetOneByIdAsync(int id)
        {
            var simulacion = await _context.Simulaciones
                .FirstOrDefaultAsync(s => s.Id == id);

            if (simulacion is null)
            {
                return new ResponseDto<SimulacionDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                    Status = false
                };
            }

            return new ResponseDto<SimulacionDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = HttpMessageResponse.REGISTER_FOUND,
                Status = true,
                Data = MapToDto(simulacion)
            };
        }

        // Listado de todas las simulaciones
        public async Task<ResponseDto<ItemsDto<List<SimulacionDto>>>> GetAllAsync()
        {
            var simulaciones = await _context.Simulaciones
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();

            var dtos = simulaciones.Select(MapToDto).ToList();

            return new ResponseDto<ItemsDto<List<SimulacionDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Status = true,
                Data = new ItemsDto<List<SimulacionDto>> { Items = dtos }
            };
        }

        //Proyección mensual de una simulación
        public async Task<ResponseDto<ItemsDto<List<ProyeccionMesDto>>>> GetProyeccionMensualAsync(int id)
        {
            var existe = await _context.Simulaciones.AnyAsync(s => s.Id == id);
            if (!existe)
            {
                return new ResponseDto<ItemsDto<List<ProyeccionMesDto>>>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                    Status = false
                };
            }

            var proyecciones = await _context.ProyeccionesMensuales
                .Where(p => p.SimulacionId == id)
                .OrderBy(p => p.Mes)
                .Select(p => new ProyeccionMesDto
                {
                    Mes = p.Mes,
                    SaldoAcumulado = p.SaldoAcumulado,
                    InteresMes = p.InteresMes
                })
                .ToListAsync();

            return new ResponseDto<ItemsDto<List<ProyeccionMesDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Status = true,
                Data = new ItemsDto<List<ProyeccionMesDto>> { Items = proyecciones }
            };
        }

        //Proyección anual de una simulación
        public async Task<ResponseDto<ItemsDto<List<ProyeccionAnioDto>>>> GetProyeccionAnualAsync(int id)
        {
            var existe = await _context.Simulaciones.AnyAsync(s => s.Id == id);
            if (!existe)
            {
                return new ResponseDto<ItemsDto<List<ProyeccionAnioDto>>>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                    Status = false
                };
            }

            var proyecciones = await _context.ProyeccionesAnuales
                .Where(p => p.SimulacionId == id)
                .OrderBy(p => p.Anio)
                .Select(p => new ProyeccionAnioDto
                {
                    Anio = p.Anio,
                    SaldoAcumulado = p.SaldoAcumulado,
                    InteresAnio = p.InteresAnio
                })
                .ToListAsync();

            return new ResponseDto<ItemsDto<List<ProyeccionAnioDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Status = true,
                Data = new ItemsDto<List<ProyeccionAnioDto>> { Items = proyecciones }
            };
        }

        // mapear entidad → DTO de resumen
        private static SimulacionDto MapToDto(Simulacion s) => new SimulacionDto
        {
            Id = s.Id,
            CapitalInicial = s.CapitalInicial,
            TasaAnual = s.TasaAnual,
            PlazoAnios = s.PlazoAnios,
            MontoFinal = s.MontoFinal,
            InteresTotal = s.InteresTotal,
            FechaCreacion = s.FechaCreacion
        };
    }
}
