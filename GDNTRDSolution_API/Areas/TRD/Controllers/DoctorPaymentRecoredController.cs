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
    public class DoctorPaymentRecoredController : Controller
    {
        private readonly IDoctorPaymentRecored _doctorPaymentRecored;
        public DoctorPaymentRecoredController(IDoctorPaymentRecored doctorPaymentRecored)
        {
            _doctorPaymentRecored = doctorPaymentRecored;
        }

        #region Doctor Payment Recored CRUD
        [Authorize]
        [HttpGet]
        [Route("GetDoctorPaymentRecoredList")]
        public IActionResult GetDoctorPaymentRecoredList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _doctorPaymentRecored.GetDoctorPaymentRecoredList(search);
            IEnumerable<DoctorPaymentRecored> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<DoctorPaymentRecored>
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
        [Route("GetDoctorPaymentRecoredById")]
        public IActionResult GetDoctorPaymentRecoredById(int id)
        {
            var data = _doctorPaymentRecored.GetDoctorPaymentRecoredById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDoctorPaymentRecoredInfo")]
        public async Task<IActionResult> SaveDoctorPaymentRecoredInfo(DoctorPaymentRecored model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _doctorPaymentRecored.SaveDoctorPaymentRecoredInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorPaymentRecoredInfo")]
        public async Task<IActionResult> UpdateDoctorPaymentRecoredInfo(DoctorPaymentRecored model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctorPaymentRecored.UpdateDoctorPaymentRecoredInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDoctorPaymentRecoredInfo")]
        public async Task<IActionResult> DeleteDoctorPaymentRecoredInfo(int id)
        {
            if (id > 0)
            {
                var data = await _doctorPaymentRecored.DeleteDoctorPaymentRecoredInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }


        #endregion
    }
}
