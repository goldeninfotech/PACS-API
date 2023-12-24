using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.ITRD;
using SoftEngine.TRDModels.Models.TRD;
using SoftEngine.TRDModels.ViewModels.ViewADM;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingSettingsController : Controller
    {
        private readonly IBillingSettings _billingSettings;

        public BillingSettingsController(IBillingSettings billingSettings)
        {
            _billingSettings = billingSettings;
        }

        #region Billing Settings
        [Authorize]
        [HttpGet]
        [Route("GetBillingSettingsList")]
        public IActionResult GetBillingSettingsList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _billingSettings.GetBillingSettingsList(search);
            IEnumerable<BillingSettingsViewModels> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<BillingSettingsViewModels>
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
        [Route("GetBillingSettingsById")]
        public IActionResult GetBillingSettingsById(int id)
        {
            var data = _billingSettings.GetBillingSettingsById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveBillingSettingsInfo")]
        public async Task<IActionResult> SaveBillingSettingsInfo(BillingSettings model)
        {
            if (ModelState.IsValid && model.BillSettingsDetails.Count>0)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _billingSettings.SaveBillingSettingsInfo(model, model.BillSettingsDetails);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateBillingSettingsInfo")]
        public async Task<IActionResult> UpdateBillingSettingsInfo(BillingSettings model)
        {
            if (model.Id > 0 && model.BillSettingsDetails.Count > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _billingSettings.UpdateBillingSettingsInfo(model, model.BillSettingsDetails);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteBillingSettingsInfo")]
        public async Task<IActionResult> DeleteBillingSettingsInfo(int id)
        {
            if (id > 0)
            {
                var data = await _billingSettings.DeleteBillingSettingsInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }
        #endregion

        #region  BillingSettings Details
        [Authorize]
        [HttpGet]
        [Route("GetBillingSettingsDetailsList")]
        public IActionResult GetBillingSettingsDetailsList(int pageNumber = 1, int limit = 10 )
        {
            var data = _billingSettings.GetBillingSettingsDetailsList();
            IEnumerable<BillingSettingsDetailsViewModels> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<BillingSettingsDetailsViewModels>
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

        #endregion
    }
}
