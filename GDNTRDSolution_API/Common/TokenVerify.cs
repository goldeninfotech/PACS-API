using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.IADM;
using SoftEngine.TRDModels.Models.ADM;
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
        public ADM_UserLogin GetCurrentUser()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new ADM_UserLogin
                {
                    Full_Name = userClaims.FirstOrDefault(x => x.Type == "Full_Name")?.Value,
                    Id = Convert.ToInt32((userClaims.FirstOrDefault(x => x.Type == "Id")?.Value)),
                    DoctorId = Convert.ToInt32((userClaims.FirstOrDefault(x => x.Type == "DoctorId")?.Value)),
                    DoctorName = userClaims.FirstOrDefault(x => x.Type == "DoctorName")?.Value,
                    BMDC_No = userClaims.FirstOrDefault(x => x.Type == "BMDC_No")?.Value,
                    HospitalName = userClaims.FirstOrDefault(x => x.Type == "HospitalName")?.Value,
                    HospitalId = Convert.ToInt32((userClaims.FirstOrDefault(x => x.Type == "HospitalId")?.Value)),
                };
            }
            return null;
        }

    }
}
