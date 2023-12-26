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
    public class PatientReportsController : Controller
    {
        private readonly IPatientReports _patientReports;

        public PatientReportsController(IPatientReports patientReports)
        {
            _patientReports = patientReports;
        }

        #region PatientReports CRUD
        [Authorize]
        [HttpGet]
        [Route("GetPatientReportsList")]
        public IActionResult GetPatientReportsList(int pageNumber = 1, int limit = 10, string? statusType="" , string? search = "")
        {
            var data = _patientReports.GetPatientReportsList(statusType,search);
            IEnumerable<ReportsViewModel> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<ReportsViewModel>
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
        [Route("GetPatientReportsById")]
        public IActionResult GetPatientReportsById(int id)
        {
            var data = _patientReports.GetPatientReportsById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SavePatientReportsInfo")]
        public async Task<IActionResult> SavePatientReportsInfo(Reports model)
        {
            if (ModelState.IsValid)
            {
                model.Status = "Pending";
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _patientReports.SavePatientReportsInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdatePatientReportsInfo")]
        public async Task<IActionResult> UpdatePatientReportsInfo(Reports model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _patientReports.UpdatePatientReportsInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeletePatientReportsInfo")]
        public async Task<IActionResult> DeletePatientReportsInfo(int id)
        {
            if (id > 0)
            {
                var data = await _patientReports.DeletePatientReportsInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion
    }
}
