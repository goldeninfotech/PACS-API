using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using GDNTRDSolution_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.ITRD;
using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : Controller
    {
        private readonly IDoctor _doctor;
        private readonly IImageUpload _imageUpload;
        public DoctorController(IDoctor doctor, IImageUpload imageUpload)
        {
            _doctor = doctor;
            _imageUpload = imageUpload;
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


        #region Doctor Image Upload 
        //[Authorize]
        [HttpPost]
        [Route("UploadDoctorImageAsync")]
        public async Task<IActionResult> UploadDoctorImageAsync([FromForm] Images images)
        {
            DataBaseResponse obj = new DataBaseResponse();
            if (images == null)
            {
                return BadRequest(new { Success = false, ErrorCode = "S01", Error = "Invalid post request" });
            }
            if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
            {
                return BadRequest(new { Success = false, ErrorCode = "S02", Error = "Invalid post header" });
            }
            var extension = System.IO.Path.GetExtension(images.Image.FileName);

            if (images.Image != null && (extension == ".jpg" || extension == ".JPG" || extension == ".png" || extension == ".PNG"))
            {
                ImageFiles imageFiles = new ImageFiles();
                imageFiles.Id = images.Id;
                imageFiles.Image = images.Image;
                imageFiles.User_Id = images.User_Id;
                imageFiles.ImageFolderType = "Doctor";
                imageFiles.ImageType = images.ImageType;
                imageFiles.Status = 1;
                imageFiles.AddedDate = DateTime.Now.ToString();
                imageFiles.AddedDate = "";
                imageFiles.UpdatedDate = DateTime.Now.ToString();
                imageFiles.UpdatedBy = "";
                obj = await _imageUpload.SaveImage(imageFiles);
                return Ok(new { IsSuccess = obj.IsSuccess, Message = obj.Message });
            }

            return Ok(new { IsSuccess = false, Message = "Please select only .jpg or .png image." });
        }

        #endregion

        #region Get Image File

        [HttpGet("GetImageById")]
        public IActionResult GetImageById(int userid, string imageType)
        {
            List<ImageFiles> data = _imageUpload.GetImageById(userid, imageType);
            string filepath = "";
            if (data.Count > 0)
                filepath = data.FirstOrDefault().ImagePath;
            var imagePath = Path.Combine(filepath);

            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg");

            }
            else
                return Ok(new { IsSuccess = false, Message = "File Not Found!!" });
        }
        #endregion
    }
}
