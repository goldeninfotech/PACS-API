using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDepartment
    {
        public IEnumerable<Departments> GetDepartmentList(string Name);
        public Departments GetDepartmentById(int id);
        public Task<DataBaseResponse> SaveDepartmentInfo(Departments model);
        public Task<DataBaseResponse> UpdateDepartmentInfo(Departments model);
        public Task<DataBaseResponse> DeleteDepartmentInfo(int id); 
    }
}
