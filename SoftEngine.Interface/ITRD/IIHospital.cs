using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IHospital
    {
        public IEnumerable<Hospital> GetHospitalList();
        public Hospital GetHospitalById(int id);
        public Task<DataBaseResponse> SaveHospitalInfo(Hospital model);
        public Task<DataBaseResponse> UpdateHospitalInfo(Hospital model);
        public Task<DataBaseResponse> DeleteHospitalInfo(int id); 
        public Hospital GetHospitalByUserId(int id);
    }
}
