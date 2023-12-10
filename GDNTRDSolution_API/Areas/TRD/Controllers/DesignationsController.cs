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
    public class DesignationsController : Controller
    {
        private readonly IDesignations _designations;

        public DesignationsController(IDesignations designations)
        {
            _designations = designations;
        }

        #region Designations CRUD
        [Authorize]
        [HttpGet]
        [Route("GetDesignationsList")]
        public IActionResult GetDesignationsList(int pageNumber = 1, int limit = 10)
        {
            var data = _designations.GetDesignationsList();
            IEnumerable<Designations> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Designations>
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
        [Route("GetDesignationsById")]
        public IActionResult GetDesignationsById(int id)
        {
            var data = _designations.GetDesignationsById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveDesignationsInfo")]
        public async Task<IActionResult> SaveDesignationsInfo(Designations model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _designations.SaveDesignationsInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateDesignationsInfo")]
        public async Task<IActionResult> UpdateDesignationsInfo(Designations model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _designations.UpdateDesignationsInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteDesignationsInfo")]
        public async Task<IActionResult> DeleteDesignationsInfo(int id)
        {
            if (id > 0)
            {
                var data = await _designations.DeleteDesignationsInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }
        #endregion
    }
}
