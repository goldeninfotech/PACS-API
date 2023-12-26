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
    public class ReportsDescriptipnsController : Controller
    {
        private readonly IReportsDescriptipns _reportsDescriptipns;

        public ReportsDescriptipnsController(IReportsDescriptipns reportsDescriptipns)
        {
            _reportsDescriptipns = reportsDescriptipns;
        }

        #region Reports Descriptipns CRUD
        [Authorize]
        [HttpGet]
        [Route("GetReportsDescriptipnsList")]
        public IActionResult GetReportsDescriptipnsList(int pageNumber = 1, int limit = 10, string? statusType = "", string? search = "")
        {
            var data = _reportsDescriptipns.GetReportsDescriptipnsList();
            IEnumerable<ReportsDescriptipns> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<ReportsDescriptipns>
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

        [Authorize]
        [HttpGet]
        [Route("GetReportsDescriptipnsById")]
        public IActionResult GetReportsDescriptipnsById(int id)
        {
            var data = _reportsDescriptipns.GetReportsDescriptipnsById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveReportsDescriptipnsInfo")]
        public async Task<IActionResult> SaveReportsDescriptipnsInfo(ReportsDescriptipns model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _reportsDescriptipns.SaveReportsDescriptipnsInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateReportsDescriptipnsInfo")]
        public async Task<IActionResult> UpdateReportsDescriptipnsInfo(ReportsDescriptipns model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _reportsDescriptipns.UpdateReportsDescriptipnsInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteReportsDescriptipnsInfo")]
        public async Task<IActionResult> DeleteReportsDescriptipnsInfo(int id)
        {
            if (id > 0)
            {
                var data = await _reportsDescriptipns.DeleteReportsDescriptipnsInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion
    }
}
