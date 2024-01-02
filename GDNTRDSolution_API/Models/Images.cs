namespace GDNTRDSolution_API.Models
{
    public class Images
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public int? User_Id { get; set; }
        public int Status { get; set; }
        public string? ImageType { get; set; } 
    }
}
