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
    public class DoctorController : Controller
    {
        private readonly IDoctor _doctor;
        public DoctorController(IDoctor doctor)
        {
            _doctor = doctor;
        }
        #region Doctor CRUD
        [Authorize]
        [HttpGet]
        [Route("GetDoctorList")]
        public IActionResult GetDoctorList(int pageNumber = 1, int limit = 10, string? search ="")
        {
            var data = _doctor.GetDoctorList(search); 
            IEnumerable<Doctor> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Doctor>
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
        [Route("GetDoctorById")]
        public IActionResult GetDoctorById(int id)
        {
            var data = _doctor.GetDoctorById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }


        [HttpPost]
        [Route("SaveDoctorInfo")]
        public async Task<IActionResult> SaveDoctorInfo(Doctor model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.DoctorName) )
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.Password = ReturnData.GenerateMD5(model.Password);
                var data = await _doctor.SaveDoctorInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorInfo")]
        public async Task<IActionResult> UpdateDoctorInfo(Doctor model)
        {
            if (model.Id > 0 && !string.IsNullOrEmpty(model.DoctorName))
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctor.UpdateDoctorInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDoctorInfo")]
        public async Task<IActionResult> DeleteDoctorInfo(int id)
        {
            if (id > 0)
            {
                var data = await _doctor.DeleteDoctorInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion
    }
}
