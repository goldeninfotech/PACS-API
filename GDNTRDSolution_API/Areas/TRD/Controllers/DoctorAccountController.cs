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
    public class DoctorAccountController : Controller
    {
        private readonly IDoctorAccount _doctorAccount;

        public DoctorAccountController(IDoctorAccount doctorAccount)
        {
            _doctorAccount = doctorAccount;
        }

        #region DoctorAccount CRUD
        [Authorize]
        [HttpGet]
        [Route("GetDoctorAccountList")]
        public IActionResult GetDoctorAccountList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _doctorAccount.GetDoctorAccountList(search);
            IEnumerable<DoctorAccount> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<DoctorAccount>
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
        [Route("GetDoctorAccountById")]
        public IActionResult GetDoctorAccountById(int id)
        {
            var data = _doctorAccount.GetDoctorAccountById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDoctorAccountInfo")]
        public async Task<IActionResult> SaveDoctorAccountInfo(DoctorAccount model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _doctorAccount.SaveDoctorAccountInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorAccountInfo")]
        public async Task<IActionResult> UpdateDoctorAccountInfo(DoctorAccount model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctorAccount.UpdateDoctorAccountInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDoctorAccountInfo")]
        public async Task<IActionResult> DeleteDoctorAccountInfo(int id)
        {
            if (id > 0)
            {
                var data = await _doctorAccount.DeleteDoctorAccountInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion
    }
}
