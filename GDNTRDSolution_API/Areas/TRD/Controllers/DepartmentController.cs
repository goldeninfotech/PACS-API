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
    public class DepartmentController : Controller
    {
        private readonly IDepartment _department ;
        public DepartmentController(IDepartment department)
        {
            _department = department;
        }

        [HttpGet]
        [Route("GetDepartmentList")]
        public IActionResult GetDepartmentList(int pageNumber = 1, int limit = 10)
        {
            var data = _department.GetDepartmentList();
            return Ok(data);
        }
    }
}
