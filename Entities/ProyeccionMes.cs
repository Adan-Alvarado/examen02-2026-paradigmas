using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenU2Paradigmas.Entities
{
    public class ProyeccionMes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SimulacionId { get; set; }

        [Required]
        public int Mes { get; set; }

        public decimal SaldoAcumulado { get; set; }

        public decimal InteresMes { get; set; }

        // Relación de navegación
        [ForeignKey("SimulacionId")]
        public Simulacion Simulacion { get; set; }
    }
}
