namespace Hairdresser.DTOs
{  
    public class TreatmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public double Price { get; set; }
    }
}

