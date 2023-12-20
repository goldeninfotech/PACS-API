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
    public class HospitalDeviceController : Controller
    {
        private readonly IHospitalDevice _hospitalDevice;

        public HospitalDeviceController(IHospitalDevice hospitalDevice)
        {
            _hospitalDevice = hospitalDevice;
        }

        #region HospitalDevice CRUD
        [Authorize]
        [HttpGet]
        [Route("GetHospitalDeviceList")]
        public IActionResult GetHospitalDeviceList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _hospitalDevice.GetHospitalDevicesList(search);
            IEnumerable<HospitalDevices> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<HospitalDevices>
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
        [Route("GetHospitalDeviceById")]
        public IActionResult GetHospitalDeviceById(int id)
        {
            var data = _hospitalDevice.GetHospitalDevicesById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveHospitalDeviceInfo")]
        public async Task<IActionResult> SaveHospitalDeviceInfo(HospitalDevices model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _hospitalDevice.SaveHospitalDevicesInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateHospitalDeviceInfo")]
        public async Task<IActionResult> UpdateHospitalDeviceInfo(HospitalDevices model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _hospitalDevice.UpdateHospitalDevicesInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteHospitalDeviceInfo")]
        public async Task<IActionResult> DeleteHospitalDeviceInfo(int id)
        {
            if (id > 0)
            {
                var data = await _hospitalDevice.DeleteHospitalDevicesInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }


        #endregion
    }
}
