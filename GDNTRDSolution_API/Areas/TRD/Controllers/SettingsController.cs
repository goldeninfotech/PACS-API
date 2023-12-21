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
    public class SettingsController : Controller
    {
        private readonly ISettings _settings;

        public SettingsController(ISettings settings)
        {
            _settings = settings;
        }

        #region Settings CRUD
        [Authorize]
        [HttpGet]
        [Route("GetSettingsList")]
        public IActionResult GetSettingsList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _settings.GetSettingList(search);
            IEnumerable<Setting> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Setting>
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
        [Route("GetSettingsById")]
        public IActionResult GetSettingsById(int id)
        {
            var data = _settings.GetSettingById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveSettingsInfo")]
        public async Task<IActionResult> SaveSettingsInfo(Setting model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _settings.SaveSettingInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateSettingsInfo")]
        public async Task<IActionResult> UpdateSettingsInfo(Setting model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _settings.UpdateSettingInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteSettingsInfo")]
        public async Task<IActionResult> DeleteSettingsInfo(int id)
        {
            if (id > 0)
            {
                var data = await _settings.DeleteSettingInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }
        #endregion

    }
}
