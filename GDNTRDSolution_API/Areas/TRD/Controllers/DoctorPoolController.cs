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
    public class DoctorPoolController : Controller
    {
        private readonly IDoctorPool _doctorPool;

        public DoctorPoolController(IDoctorPool doctorPool)
        {
            _doctorPool = doctorPool;
        }
        #region DoctorPool CRUD
        [Authorize]
        [HttpGet]
        [Route("GetDoctorPoolList")]
        public IActionResult GetDoctorPoolList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _doctorPool.GetDoctorPoolList(search);
            IEnumerable<DoctorPoolViewModel> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<DoctorPoolViewModel>
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
        [Route("GetDoctorPoolById")]
        public IActionResult GetDoctorPoolById(int id)
        {
            var data = _doctorPool.GetDoctorPoolById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDoctorPoolInfo")]
        public async Task<IActionResult> SaveDoctorPoolInfo(DoctorPool model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _doctorPool.SaveDoctorPoolInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorPoolInfo")]
        public async Task<IActionResult> UpdateDoctorPoolInfo(DoctorPool model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctorPool.UpdateDoctorPoolInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDoctorPoolInfo")]
        public async Task<IActionResult> DeleteDoctorPoolInfo(int id)
        {
            if (id > 0)
            {
                var data = await _doctorPool.DeleteDoctorPoolInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }


        #endregion
    }
}
