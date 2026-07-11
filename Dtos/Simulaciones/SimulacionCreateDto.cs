using System.ComponentModel.DataAnnotations;

namespace ExamenU2Paradigmas.Dtos.Simulaciones
{
    public class SimulacionCreateDto
    {
        [Required(ErrorMessage = "El capital inicial es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El capital inicial debe ser un valor positivo mayor a cero.")]
        public decimal CapitalInicial { get; set; }

        [Required(ErrorMessage = "La tasa de interés anual es requerida.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "La tasa de interés anual debe ser un valor positivo mayor a cero.")]
        public decimal TasaAnual { get; set; } // Ej: 0.05 para 5%

        [Required(ErrorMessage = "El plazo en años es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El plazo debe ser al menos 1 año.")]
        public int PlazoAnios { get; set; }
    }
}
