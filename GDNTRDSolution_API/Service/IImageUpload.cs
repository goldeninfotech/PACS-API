using GDNTRDSolution_API.Models;
using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;

namespace GDNTRDSolution_API.Service
{
    public interface IImageUpload
    {
        public Task<DataBaseResponse> SaveImage(ImageFiles imageFiles);
        public List<ImageFiles> GetImageById(int id, string imageType);
    }
}
