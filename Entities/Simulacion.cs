using System.ComponentModel.DataAnnotations;

namespace ExamenU2Paradigmas.Entities
{
    public class Simulacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal CapitalInicial { get; set; }

        [Required]
        public decimal TasaAnual { get; set; } // en decimal, ej. 0.05 = 5%

        [Required]
        public int PlazoAnios { get; set; }

        public decimal MontoFinal { get; set; }

        public decimal InteresTotal { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Relaciones de navegación
        public List<ProyeccionMes> ProyeccionesMensuales { get; set; } = new();
        public List<ProyeccionAnio> ProyeccionesAnuales { get; set; } = new();
    }
}
