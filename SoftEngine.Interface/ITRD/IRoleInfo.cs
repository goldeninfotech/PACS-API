using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IRoleInfo
    {
        public IEnumerable<Roles> GetRolesList(string search);
        public Roles GetRolesById(int id);
        public Task<DataBaseResponse> SaveRolesInfo(Roles model);
        public Task<DataBaseResponse> UpdateRolesInfo(Roles model);
        public Task<DataBaseResponse> DeleteRolesInfo(int id);
    }
}
