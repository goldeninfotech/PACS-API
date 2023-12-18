using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.ITRD;
using SoftEngine.TRDModels.Models.TRD;
using SoftEngine.TRDModels.ViewModels.ViewADM;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesRecoredController : Controller
    {
        private readonly IFilesRecored _filesRecored;

        public FilesRecoredController(IFilesRecored filesRecored)
        {
            _filesRecored = filesRecored;
        }

        #region Files Record
        [Authorize]
        [HttpGet]
        [Route("GetFilesRecordList")]
        public IActionResult GetFilesRecordList(int pageNumber = 1, int limit = 10)
        {
            var data = _filesRecored.GetFilesRecordList();
            IEnumerable<FilesRecordViewModel> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<FilesRecordViewModel>
            {
                TotalData = data.Count(),
                DataFound = paginatedData.Count(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)data.Count() / limit),
                DataLimit = limit,
                Data = paginatedData
            };
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = result });
        }
        #endregion
    }
}
