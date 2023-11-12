using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.IADM;
using System.Security.Claims;

namespace GDNTRDSolution_API.Common
{
    public class TokenVerify : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserPermisson _userPermisson;
        public TokenVerify(IHttpContextAccessor httpContextAccessor, IUserPermisson userPermisson)
        {
            _httpContextAccessor = httpContextAccessor;
            _userPermisson = userPermisson;
        }
        //public ADM_UserLogin GetCurrentUser()
        //{
        //    var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        var userClaims = identity.Claims;
        //        return new ADM_UserLogin
        //        {
        //            EmpName = userClaims.FirstOrDefault(x => x.Type == "EmpName")?.Value,
        //            // Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
        //            EmpCode = Convert.ToInt32((userClaims.FirstOrDefault(x => x.Type == "EmpCode")?.Value)),
        //            CompanyID = Convert.ToInt32((userClaims.FirstOrDefault(x => x.Type == "CompanyID")?.Value)),
        //            CompanyName = userClaims.FirstOrDefault(x => x.Type == "CompanyName")?.Value,

        //            //EmpName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
        //            //Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
        //            //EmpCode = Convert.ToInt32((userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value)),
        //            //CompanyID = Convert.ToInt32((userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Spn)?.Value)),
        //            //CompanyName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
        //        };
        //    }
        //    return null;
        //}


        //public bool GetUserPermissionByEmpId(int subMenuId, string type, int EmpCode)
        //{
        //    bool status = false;
        //    List<ADM_DashbordMenuListViewModel> objectList = new List<ADM_DashbordMenuListViewModel>();

        //    if (EmpCode > 0)
        //        objectList = _userPermisson.DashbordMenuList(EmpCode, "");

        //    if (objectList != null || objectList.Count() > 0)
        //    {
        //        if (type == "DataView")
        //        {
        //            var objectList2 = objectList.Where(x => x.SubMenuId == subMenuId && x.DataView == true).ToList();
        //            if (objectList2.Count == 1)
        //                status = true;
        //            else
        //                status = false;
        //        }
        //        else if (type == "DataInsert")
        //        {
        //            var objectList2 = objectList.Where(x => x.SubMenuId == subMenuId && x.DataInsert == true).ToList();
        //            if (objectList2.Count == 1)
        //                status = true;
        //            else
        //                status = false;
        //        }
        //        else if (type == "DataUpdate")
        //        {
        //            var objectList2 = objectList.Where(x => x.SubMenuId == subMenuId && x.DataUpdate == true).ToList();
        //            if (objectList2.Count == 1)
        //                status = true;
        //            else
        //                status = false;
        //        }
        //        else if (type == "DataDelete")
        //        {
        //            var objectList2 = objectList.Where(x => x.SubMenuId == subMenuId && x.DataDelete == true).ToList();
        //            if (objectList2.Count == 1)
        //                status = true;
        //            else
        //                status = false;
        //        }
        //        else
        //            status = false;
        //    }
        //    return status;
        //}
    }
}
