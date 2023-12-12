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
    public class HospitalController : Controller
    {
        private readonly IHospital _hospital;

        public HospitalController(IHospital hospital)
        {
            _hospital = hospital;
        }

        #region Hospital CRUD
        [Authorize]
        [HttpGet]
        [Route("GetHospitalList")]
        public IActionResult GetHospitalList(int pageNumber = 1, int limit = 10)
        {
            var data = _hospital.GetHospitalList();
            IEnumerable<Hospital> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Hospital>
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
        [Route("GetHospitalById")]
        public IActionResult GetHospitalById(int id)
        {
            var data = _hospital.GetHospitalById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveHospitalInfo")]
        public async Task<IActionResult> SaveHospitalInfo(Hospital model)
        {
            if (ModelState.IsValid)
            {
                model.Password = ReturnData.GenerateMD5(model.Password);
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _hospital.SaveHospitalInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateHospitalInfo")]
        public async Task<IActionResult> UpdateHospitalInfo(Hospital model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _hospital.UpdateHospitalInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteHospitalInfo")]
        public async Task<IActionResult> DeleteHospitalInfo(int id)
        {
            if (id > 0)
            {
                var data = await _hospital.DeleteHospitalInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }
        #endregion


        #region Access Desktop Hospital 
        [HttpPost]
        [Route("GetHospitalByUserId")]
        public IActionResult GetHospitalByUserId(string id)
        {
            var data = _hospital.GetHospitalByUserId(Convert.ToInt32(id));
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }
        #endregion
    }
}
