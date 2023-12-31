using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IPermission
    {
        public IEnumerable<Permission> GetPermissionList(string Name);
        public Permission GetPermissionById(int id);
        public Task<DataBaseResponse> SavePermissionInfo(Permission model);
        public Task<DataBaseResponse> UpdatePermissionInfo(Permission model);
        public Task<DataBaseResponse> DeletePermissionInfo(int id); 
    }
}
