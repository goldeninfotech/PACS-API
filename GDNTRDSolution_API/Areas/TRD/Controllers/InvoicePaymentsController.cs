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
    public class InvoicepaymentsController : Controller
    {
        private readonly IInvoicePayments _invoicePayments;

        public InvoicepaymentsController(IInvoicePayments invoicePayments)
        {
            _invoicePayments = invoicePayments;
        }

        #region Invoice payments CRUD
        [Authorize]
        [HttpGet]
        [Route("GetInvoicepaymentsList")]
        public IActionResult GetInvoicepaymentsList(int pageNumber = 1, int limit = 10, string? search = "")
        {
            var data = _invoicePayments.GetInvoicepaymentsList(search);
            IEnumerable<Invoicepayments> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<Invoicepayments>
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
        [Route("GetInvoicepaymentsById")]
        public IActionResult GetInvoicepaymentsById(int id)
        {
            var data = _invoicePayments.GetInvoicepaymentsById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [Authorize]
        [HttpPost]
        [Route("SaveInvoicepaymentsInfo")]
        public async Task<IActionResult> SaveInvoicepaymentsInfo( Invoicepayments model)
        {
            if (ModelState.IsValid)
            {
                model.Status = 1;
                model.AddedBy = "";
                model.AddedDate = DateTime.Now.ToString("dd-mm-yyyy");
                var data = await _invoicePayments.SaveInvoicepaymentsInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateInvoicepaymentsInfo")]
        public async Task<IActionResult> UpdateInvoicepaymentsInfo( Invoicepayments model)
        {
            if (model.Id > 0)
            {
                model.UpdatedDate = DateTime.Now.ToString("dd-mm-yyyy");
                model.UpdatedBy = "";
                var data = await _invoicePayments.UpdateInvoicepaymentsInfo(model);
                return Ok(data);
            }
            else
                return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteInvoicepaymentsInfo")]
        public async Task<IActionResult> DeleteInvoicepaymentsInfo(int id)
        {
            if (id > 0)
            {
                var data = await _invoicePayments.DeleteInvoicepaymentsInfo(id);
                return Ok(data);
            }
            else
                return BadRequest();
        }


        #endregion

    }
}
