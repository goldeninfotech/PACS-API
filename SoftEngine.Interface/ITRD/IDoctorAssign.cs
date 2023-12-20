using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDoctorAssign
    {
        public IEnumerable<DoctorAssign> GetDoctorAssignList(string Name);
        public DoctorAssign GetDoctorAssignById(int id);
        public Task<DataBaseResponse> SaveDoctorAssignInfo(DoctorAssign model);
        public Task<DataBaseResponse> UpdateDoctorAssignInfo(DoctorAssign model);
        public Task<DataBaseResponse> DeleteDoctorAssignInfo(int id);
    }
}
