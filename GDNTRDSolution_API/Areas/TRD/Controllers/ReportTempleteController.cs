using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.ITRD;
using SoftEngine.TRDModels.Models.TRD;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportTempleteController : Controller
    {
        private readonly IReportTemplate _reportTemplate;

        public ReportTempleteController(IReportTemplate reportTemplate)
        {
            _reportTemplate = reportTemplate;
        }

        #region ReportTemplate

        [Authorize]
        [HttpGet]
        [Route("GetReportTemplateList")]
        public IActionResult GetReportTemplateList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _reportTemplate.GetReportTemplateList(search);
            IEnumerable<ReportTemplate> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<ReportTemplate>
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
        [Route("GetReportTemplateById")]
        public IActionResult GetReportTemplateById(int id)
        {
            var data = _reportTemplate.GetReportTemplateById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveReportTemplateInfo")]
        public async Task<IActionResult> SaveReportTemplateInfo(ReportTemplate model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _reportTemplate.SaveReportTemplateInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateReportTemplateInfo")]
        public async Task<IActionResult> UpdateReportTemplateInfo(ReportTemplate model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _reportTemplate.UpdateReportTemplateInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteReportTemplateInfo")]
        public async Task<IActionResult> DeleteReportTemplateInfo(int id)
        {
            if (id > 0)
            {
                var data = await _reportTemplate.DeleteReportTemplateInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion

    }
}
