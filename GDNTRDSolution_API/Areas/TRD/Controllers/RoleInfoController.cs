using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SoftEngine.Interface.ITRD;
using SoftEngine.TRDModels.Models.TRD;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleInfoController : Controller
    {
        private readonly IRoleInfo _roleInfo;
        public RoleInfoController(IRoleInfo roleInfo) 
        {
            _roleInfo = roleInfo;
        }
        #region RoleInfo CRUD
        [Authorize]
        [HttpGet]
        [Route("GetRolesInfoList")]
        public IActionResult GetRolesInfoList(int pageNumber = 1, int limit = 10, string? search="")
        {
            var data = _roleInfo.GetRolesList(search);
            IEnumerable<Roles> paginatedData;  
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Roles>
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
        [Route("GetGetRolesInfoById")]
        public IActionResult GetGetRolesInfoById(int id)
        {
            var data = _roleInfo.GetRolesById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveRolesInfo")]
        public async Task<IActionResult> SaveRolesInfo(Roles model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.Name))
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _roleInfo.SaveRolesInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateRolesInfo")]
        public async Task<IActionResult> UpdateRolesInfo(Roles model) 
        {
            if (model.Id > 0 && !string.IsNullOrEmpty(model.Name))
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _roleInfo.UpdateRolesInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteRolesInfo")]
        public async Task<IActionResult> DeleteRolesInfo(int id) 
        {
            if (id > 0)
            {
                var data = await _roleInfo.DeleteRolesInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }
        #endregion
    }
}
