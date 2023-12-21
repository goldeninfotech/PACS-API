using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDoctorAccount
    {
        public IEnumerable<DoctorAccount> GetDoctorAccountList(string Name);
        public DoctorAccount GetDoctorAccountById(int id);
        public Task<DataBaseResponse> SaveDoctorAccountInfo(DoctorAccount model);
        public Task<DataBaseResponse> UpdateDoctorAccountInfo(DoctorAccount model);
        public Task<DataBaseResponse> DeleteDoctorAccountInfo(int id); 
    }
}
