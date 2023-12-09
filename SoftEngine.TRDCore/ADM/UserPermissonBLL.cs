using SoftEngine.Interface.IADM;
using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.ADM;
using SoftEngine.TRDModels.ViewModels.ViewADM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDCore.ADM
{
    public class UserPermissonBLL : IUserPermisson
    {
        public List<ADM_DashbordMenuListViewModel> DashbordMenuList(int empCode, string empName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ADM_UserPermission> GetUserPermissionList(int Usercode, string search)
        {
            throw new NotImplementedException();
        }

        public Task<DataBaseResponse> UpdateUserPermissionInfo(IEnumerable<ADM_UserPermission> model, string addedBy, string addedDate, int userCode)
        {
            throw new NotImplementedException();
        }
    }
}
