namespace GDNTRDSolution_API.Models
{
    public class ListMetaData<T>
    {
        public int TotalData { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int DataLimit { get; set; }
        public int DataFound { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
