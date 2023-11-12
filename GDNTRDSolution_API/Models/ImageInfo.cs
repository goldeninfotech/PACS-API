namespace GDNTRDSolution_API.Models
{
    public class ImageInfo
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public IFormFile Image { get; set; }
        public string? CompanyId { get; set; }
        public string? ImagePath { get; set; }
    }
}
