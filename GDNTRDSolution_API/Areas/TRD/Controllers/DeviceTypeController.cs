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
    public class DeviceTypeController : Controller
    {
        private readonly IDeviceType _deviceType;

        public DeviceTypeController(IDeviceType deviceType)
        {
            _deviceType = deviceType;
        }

        #region DeviceType CRUD
        [Authorize]
        [HttpGet]
        [Route("GetDeviceTypeList")]
        public IActionResult GetDeviceTypeList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _deviceType.GetDeviceTypeList(search);
            IEnumerable<DeviceType> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<DeviceType>
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
        [Route("GetDeviceTypeById")]
        public IActionResult GetDeviceTypeById(int id)
        {
            var data = _deviceType.GetDeviceTypeById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDeviceTypeInfo")]
        public async Task<IActionResult> SaveDeviceTypeInfo(DeviceType model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _deviceType.SaveDeviceTypeInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDeviceTypeInfo")]
        public async Task<IActionResult> UpdateDeviceTypeInfo(DeviceType model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _deviceType.UpdateDeviceTypeInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDeviceTypeInfo")]
        public async Task<IActionResult> DeleteDeviceTypeInfo(int id)
        {
            if (id > 0)
            {
                var data = await _deviceType.DeleteDeviceTypeInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion
    }
}
