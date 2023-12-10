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
    public class UsersController : Controller
    {
        private readonly IUsers _users; 
        public UsersController(IUsers users)
        {
            _users = users;
        }


        #region User CRUD
        [Authorize]
        [HttpGet]
        [Route("GetUsersList")]
        public IActionResult GetUsersList(int pageNumber = 1, int limit = 10)
        {
            var data = _users.GetUsersList();
            IEnumerable<User> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<User>
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
        [Route("GetUsersListById")]
        public IActionResult GetUsersListById(int id)
        {
            var data = _users.GetUsersById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveUserInfo")]
        public async Task<IActionResult> SaveUserInfo(User model)
        {
            if (ModelState.IsValid && (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password)))
            {
                model.Status = 1;
                model.AddedBy = "";
                model.Password = ReturnData.GenerateMD5(model.Password); 
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _users.SaveUsersInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo(User model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _users.UpdateUsersInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteUserInfo")]
        public async Task<IActionResult> DeleteUserInfo(int id)
        {
            if (id > 0)
            {
                var data = await _users.DeleteUsersInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        #endregion


       
    }
}
