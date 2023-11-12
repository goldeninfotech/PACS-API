using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.ADM;
using SoftEngine.TRDModels.ViewModels.ViewADM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.IADM
{
    public interface IUserPermisson
    {
        public IEnumerable<ADM_UserPermission> GetUserPermissionList(int Usercode, string search);
        public Task<DataBaseResponse> UpdateUserPermissionInfo(IEnumerable<ADM_UserPermission> model, string addedBy, string addedDate, int userCode);
        List<ADM_DashbordMenuListViewModel> DashbordMenuList(int empCode, string empName);
    }
}
