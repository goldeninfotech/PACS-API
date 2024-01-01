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
        public IActionResult GetHospitalList(int pageNumber = 1, int limit = 10, string? search="")
        {
            var data = _hospital.GetHospitalList(search);
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


        #region change Hospital status
        [Authorize]
        [HttpPut]
        [Route("UpdateHospitalStatusInfo")]
        public async Task<IActionResult> UpdateHospitalStatusInfo(int status, int id)
        {
            if (id > 0)
            {
                Hospital model = new Hospital();
                model.Status = status;
                model.Id = id;
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _hospital.UpdateHospitalStatusInfo(model);
                return Ok(data);
            }
            else
            {
                var response = new
                {
                    ReturnValue = -1,
                    Message = " Please Insert The Required Data",
                    IsSuccess = false
                };
                return Ok(response);
            }
        }
        #endregion

        #region change Hospital phone
        [Authorize]
        [HttpPut]
        [Route("UpdateHospitalPhoneInfo")]
        public async Task<IActionResult> UpdateHospitalPhoneInfo(string phone, int userId)
        {
            if (userId > 0 && !string.IsNullOrEmpty(phone))
            {
                var data = await _hospital.UpdateHospitalPhoneInfo(phone, userId);
                return Ok(data);
            }
            else
            {
                var response = new
                {
                    ReturnValue = -1,
                    Message = " Please Insert The Required Data  ",
                    IsSuccess = false
                };
                return Ok(response);
            }
        }
        #endregion

        #region change Hospital password
        [Authorize]
        [HttpPut]
        [Route("UpdateHospitalPasswordInfo")]
        public async Task<IActionResult> UpdateHospitalPasswordInfo(string password, string confirmPassword, int userId)
        {
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
            {
                var password2 = password.Replace(" ", "");
                var confirmPassword2 = confirmPassword.Replace(" ", "");
                if (password2.Equals(confirmPassword2, StringComparison.OrdinalIgnoreCase) == true)
                {
                    password = ReturnData.GenerateMD5(password);
                    var data = await _hospital.UpdateHospitalPasswordInfo(password, userId);
                    return Ok(data);
                }
                else
                {
                    var response = new
                    {
                        ReturnValue = -1,
                        Message = "Password Not Match",
                        IsSuccess = false
                    };
                    return Ok(response);
                }
            }
            else
            {
                var response = new
                {
                    ReturnValue = -1,
                    Message = "Password Not Match",
                    IsSuccess = false
                };
                return Ok(response);
            }
        }

        #endregion

    }
}
