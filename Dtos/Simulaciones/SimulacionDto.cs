namespace ExamenU2Paradigmas.Dtos.Simulaciones
{
    public class SimulacionDto
    {
        public int Id { get; set; }
        public decimal CapitalInicial { get; set; }
        public decimal TasaAnual { get; set; }       // En decimal, ej. 0.05 = 5%
        public int PlazoAnios { get; set; }
        public decimal MontoFinal { get; set; }
        public decimal InteresTotal { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
