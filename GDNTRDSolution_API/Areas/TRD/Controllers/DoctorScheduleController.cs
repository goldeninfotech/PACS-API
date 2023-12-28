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
    public class DoctorScheduleController : Controller
    {
        private readonly IDoctorSchedule _doctorSchedule;

        public DoctorScheduleController(IDoctorSchedule doctorSchedule)
        {
            _doctorSchedule = doctorSchedule;
        }

        #region Doctor Schedule CURD
        [Authorize]
        [HttpGet]
        [Route("GetDoctorScheduleList")]
        public IActionResult GetDoctorScheduleList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _doctorSchedule.GetDoctorScheduleList(search);
            IEnumerable<DoctorSchedule> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<DoctorSchedule>
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
        [Route("GetDoctorScheduleById")]
        public IActionResult GetDoctorScheduleById(int id)
        {
            var data = _doctorSchedule.GetDoctorScheduleById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDoctorScheduleInfo")]
        public async Task<IActionResult> SaveDoctorScheduleInfo(DoctorSchedule model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _doctorSchedule.SaveDoctorScheduleInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorScheduleInfo")]
        public async Task<IActionResult> UpdateDoctorScheduleInfo(DoctorSchedule model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctorSchedule.UpdateDoctorScheduleInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDoctorScheduleInfo")]
        public async Task<IActionResult> DeleteDoctorScheduleInfo(int id)
        {
            if (id > 0)
            {
                var data = await _doctorSchedule.DeleteDoctorScheduleInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion


    }
}
