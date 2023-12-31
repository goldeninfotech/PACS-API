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
    public class DoctorController : Controller
    {
        private readonly IDoctor _doctor;
        public DoctorController(IDoctor doctor)
        {
            _doctor = doctor;
        }
        #region Doctor CRUD
        [Authorize]
        [HttpGet]
        [Route("GetDoctorList")]
        public IActionResult GetDoctorList(int pageNumber = 1, int limit = 10, string? search ="", string ? statustype="")
        {
            var data = _doctor.GetDoctorList(search, statustype); 
            IEnumerable<Doctor> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Doctor>
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
        [Route("GetDoctorById")]
        public IActionResult GetDoctorById(int id)
        {
            var data = _doctor.GetDoctorById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }


        [HttpPost]
        [Route("SaveDoctorInfo")]
        public async Task<IActionResult> SaveDoctorInfo(Doctor model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.DoctorName) )
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.Password = ReturnData.GenerateMD5(model.Password);
                var data = await _doctor.SaveDoctorInfo(model);
                return Ok(data);
            }
            else
            {
                var response = new
                {
                    ReturnValue = -1,
                    Message = " Please Insert The Required Data ",
                    IsSuccess = false
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorInfo")]
        public async Task<IActionResult> UpdateDoctorInfo(Doctor model)
        {
            if (model.Id > 0 && !string.IsNullOrEmpty(model.DoctorName))
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctor.UpdateDoctorInfo(model);
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

        [Authorize]
        [HttpDelete]
        [Route("DeleteDoctorInfo")]
        public async Task<IActionResult> DeleteDoctorInfo(int id)
        {
            if (id > 0)
            {
                var data = await _doctor.DeleteDoctorInfo(id);
                return Ok(data);
            }
            else
            {
                var response = new
                {
                    ReturnValue = -1,
                    Message = " Please Insert The Required Data ",
                    IsSuccess = false
                };
                return Ok(response);
            }
        }

        #endregion

        #region change doctor status
        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorStatusInfo")]
        public async Task<IActionResult> UpdateDoctorStatusInfo(int status, int id)
        {
            if (id > 0 )
            {
                Doctor model=new Doctor();
                model.Status = status;
                model.Id = id;
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _doctor.UpdateDoctorStatusInfo(model);
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

        #region change doctor phone
        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorPhoneInfo")]
        public async Task<IActionResult> UpdateDoctorPhoneInfo(string phone, int userId)
        {
            if (userId > 0 && !string.IsNullOrEmpty(phone))
            {
                var data = await _doctor.UpdateDoctorPhoneInfo(phone, userId);
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

        #region change doctor password
        [Authorize]
        [HttpPut]
        [Route("UpdateDoctorPasswordInfo")]
        public async Task<IActionResult> UpdateDoctorPasswordInfo(string password, string confirmPassword, int userId)
        {
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
            {
                var password2 = password.Replace(" ", "");
                var confirmPassword2 = confirmPassword.Replace(" ", "");
                if (password2.Equals(confirmPassword2, StringComparison.OrdinalIgnoreCase) == true)
                {
                    password=ReturnData.GenerateMD5(password);
                    var data = await _doctor.UpdateDoctorPasswordInfo(password, userId);
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
