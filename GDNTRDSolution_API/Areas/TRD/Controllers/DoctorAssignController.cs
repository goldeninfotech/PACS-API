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
    public class DoctorAssignController : Controller
    {
        private readonly IDoctorAssign _doctorAssign;

        public DoctorAssignController(IDoctorAssign doctorAssign)
        {
            _doctorAssign = doctorAssign;
        }

        #region  DoctorAssign
        [Authorize]
        [HttpGet]
        [Route("GetDoctorAssignList")]
        public IActionResult GetDoctorAssignList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _doctorAssign.GetDoctorAssignList(search);
            IEnumerable<DoctorAssign> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<DoctorAssign>
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
        [Route("GetDoctorAssignById")]
        public IActionResult GetDoctorAssignById(int id)
        {
            var data = _doctorAssign.GetDoctorAssignById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDoctorAssignInfo")]
        public async Task<IActionResult> SaveDoctorAssignInfo(DoctorAssign model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _doctorAssign.SaveDoctorAssignInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorAssignInfo")]
        public async Task<IActionResult> UpdateDoctorAssignInfo(DoctorAssign model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctorAssign.UpdateDoctorAssignInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDoctorAssignInfo")]
        public async Task<IActionResult> DeleteDoctorAssignInfo(int id)
        {
            if (id > 0)
            {
                var data = await _doctorAssign.DeleteDoctorAssignInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }
        #endregion
    }
}
