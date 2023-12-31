using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonDataListController : Controller
    {
        #region GetIsActive List 
        [Authorize]
        [HttpGet]
        [Route("GetIsActiveList")]
        public IActionResult GetIsActiveList(int pageNumber = 1, int limit = 10)
        {
            CommonDataList datalist = new CommonDataList();
            var data = datalist.IsActiveList();
            IEnumerable<IsActiveList> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<IsActiveList>
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
        #endregion

        #region GetIsActive List 
        [Authorize]
        [HttpGet]
        [Route("GetIsActiveListNum")]
        public IActionResult GetIsActiveListNum(int pageNumber = 1, int limit = 10)
        {
            CommonDataList datalist = new CommonDataList();
            var data = datalist.StatusListNum();
            IEnumerable<CommonDataListModel> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<CommonDataListModel>
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
        #endregion

        #region Country List
        [Authorize]
        [HttpGet]
        [Route("GetCountryList")]
        public IActionResult GetCountryList(int pageNumber = 1, int limit = 10)
        {
            CommonDataList datalist = new CommonDataList();
            var data = datalist.CountryList();
            IEnumerable<CommonDataListModel> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<CommonDataListModel>
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
        #endregion 

        #region PaymentMethod List
        [Authorize]
        [HttpGet]
        [Route("GetPaymentMethodList")]
        public IActionResult GetPaymentMethodList(int pageNumber = 1, int limit = 10) 
        {
            CommonDataList datalist = new CommonDataList();
            var data = datalist.PaymentMethodList();
            IEnumerable<CommonDataListModel> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<CommonDataListModel>
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
        #endregion

        #region Status List
        [Authorize]
        [HttpGet]
        [Route("GetStatusList")]
        public IActionResult GetStatusList(int pageNumber = 1, int limit = 10) 
        {
            CommonDataList datalist = new CommonDataList();
            var data = datalist.StatusList();
            IEnumerable<CommonDataListModel> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<CommonDataListModel>
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
        #endregion
    }
}
