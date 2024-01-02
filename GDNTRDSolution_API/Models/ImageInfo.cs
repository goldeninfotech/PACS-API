namespace GDNTRDSolution_API.Models
{
    public class ImageFiles 
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public string? ImagePath { get; set; }
        public int? User_Id { get; set; }
        public string? ImageType { get; set; }
        public string? ImageFolderType { get; set; }
        public int Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
