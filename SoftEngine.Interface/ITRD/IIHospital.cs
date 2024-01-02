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
    public interface IHospital
    {
        public IEnumerable<HospitalViewModel> GetHospitalList(string search);
        public HospitalViewModel GetHospitalById(int id);
        public Task<DataBaseResponse> SaveHospitalInfo(Hospital model);
        public Task<DataBaseResponse> UpdateHospitalInfo(Hospital model);
        public Task<DataBaseResponse> DeleteHospitalInfo(int id); 
        public Hospital GetHospitalByUserId(int id);
        public Task<DataBaseResponse> UpdateHospitalStatusInfo(Hospital model);
        public Task<DataBaseResponse> UpdateHospitalPhoneInfo(string phone, int userId);
        public Task<DataBaseResponse> UpdateHospitalPasswordInfo(string password, int userid);
    }
}
