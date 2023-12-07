using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonDataListController : Controller
    {
        #region 
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

        #region Country List

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
    }
}
