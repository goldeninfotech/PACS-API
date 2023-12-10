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
    public class HospitalCategoryController : Controller
    {
        private readonly IHospitalCategory _hospitalCategory;

        public HospitalCategoryController(IHospitalCategory hospitalCategory)
        {
            _hospitalCategory = hospitalCategory;
        }

        #region HospitalCategory CRUD
        [Authorize]
        [HttpGet]
        [Route("GetHospitalCategoryList")]
        public IActionResult GetHospitalCategoryList(int pageNumber = 1, int limit = 10)
        {
            var data = _hospitalCategory.GetHospitalCategoryList();
            IEnumerable<HospitalCategory> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<HospitalCategory>
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
        [Route("GetHospitalCategoryById")]
        public IActionResult GetHospitalCategoryById(int id)
        {
            var data = _hospitalCategory.GetHospitalCategoryById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveHospitalCategoryInfo")]
        public async Task<IActionResult> SaveHospitalCategoryInfo(HospitalCategory model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.Name))
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _hospitalCategory.SaveHospitalCategoryInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateHospitalCategoryInfo")]
        public async Task<IActionResult> UpdateHospitalCategoryInfo(HospitalCategory model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _hospitalCategory.UpdateHospitalCategoryInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteHospitalCategoryInfo")]
        public async Task<IActionResult> DeleteHospitalCategoryInfo(int id)
        {
            if (id > 0)
            {
                var data = await _hospitalCategory.DeleteHospitalCategoryInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }
        #endregion
    }
}
