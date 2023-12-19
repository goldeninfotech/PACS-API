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
    public class DoctorCategoryController : Controller
    {
        private readonly IDoctorCategory _doctorCategory;

        public DoctorCategoryController(IDoctorCategory doctorCategory)
        {
            _doctorCategory = doctorCategory;
        }
        #region Doctor Category 
        [Authorize]
        [HttpGet]
        [Route("GetDoctorCategoryList")]
        public IActionResult GetDoctorCategoryList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _doctorCategory.GetDoctorCategoryList(search);
            IEnumerable<DoctorCategory> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<DoctorCategory>
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
        [Route("GetDoctorCategoryById")]
        public IActionResult GetDoctorCategoryById(int id)
        {
            var data = _doctorCategory.GetDoctorCategoryById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDoctorCategoryInfo")]
        public async Task<IActionResult> SaveDoctorCategoryInfo(DoctorCategory model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _doctorCategory.SaveDoctorCategoryInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDepartmentInfo")]
        public async Task<IActionResult> UpdateDepartmentInfo(DoctorCategory model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctorCategory.UpdateDoctorCategoryInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDepartmentInfo")]
        public async Task<IActionResult> DeleteDepartmentInfo(int id)
        {
            if (id > 0)
            {
                var data = await _doctorCategory.DeleteDoctorCategoryInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion
    }
}
