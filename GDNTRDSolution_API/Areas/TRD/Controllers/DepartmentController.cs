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
    public class DepartmentController : Controller
    {
        private readonly IDepartment _department ;
        public DepartmentController(IDepartment department)
        {
            _department = department;
        }

        #region Department CRUD
        [Authorize]
        [HttpGet]
        [Route("GetDepartmentList")]
        public IActionResult GetDepartmentList(int pageNumber = 1, int limit = 10)
        {
            var data = _department.GetDepartmentList();
            IEnumerable<Departments> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Departments>
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
        [Route("GetDepartmentById")]
        public IActionResult GetDepartmentById(int id)
        {
            var data = _department.GetDepartmentById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDepartmentInfo")]
        public async Task<IActionResult> SaveDepartmentInfo(Departments model ) 
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _department.SaveDepartmentInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDepartmentInfo")]
        public async Task<IActionResult> UpdateDepartmentInfo(Departments model, int subMenuId, string type)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy ="";
                var data = await _department.UpdateDepartmentInfo(model);
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
                var data = await _department.DeleteDepartmentInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }


        #endregion
    }
}
