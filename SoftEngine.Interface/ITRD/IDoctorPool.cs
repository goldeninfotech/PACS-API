using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using SoftEngine.TRDModels.ViewModels.ViewADM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDoctorPool
    {
        public IEnumerable<DoctorPoolViewModel> GetDoctorPoolList(string Name);
        public DoctorPoolViewModel GetDoctorPoolById(int id);
        public Task<DataBaseResponse> SaveDoctorPoolInfo(DoctorPool model);
        public Task<DataBaseResponse> UpdateDoctorPoolInfo(DoctorPool model);
        public Task<DataBaseResponse> DeleteDoctorPoolInfo(int id); 
    }
}
