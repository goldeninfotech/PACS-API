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
    public class InvoiceController : Controller
    {
        private readonly IInvoice _invoice;

        public InvoiceController(IInvoice invoice)
        {
            _invoice = invoice;
        }

        #region Invoice CRUD

        [Authorize]
        [HttpGet]
        [Route("GetInvoiceList")]
        public IActionResult GetInvoiceList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _invoice.GetInvoiceList(search);
            IEnumerable<Invoice> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Invoice>
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
        [Route("GetInvoiceById")]
        public IActionResult GetInvoiceById(int id)
        {
            var data = _invoice.GetInvoiceById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveInvoiceInfo")]
        public async Task<IActionResult> SaveInvoiceInfo(Invoice model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _invoice.SaveInvoiceInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateInvoiceInfo")]
        public async Task<IActionResult> UpdateInvoiceInfo(Invoice model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _invoice.UpdateInvoiceInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteInvoiceInfo")]
        public async Task<IActionResult> DeleteInvoiceInfo(int id)
        {
            if (id > 0)
            {
                var data = await _invoice.DeleteInvoiceInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }


        #endregion
    }
}
