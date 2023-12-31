using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDoctorPaymentRecored
    {
        public IEnumerable<DoctorPaymentRecored> GetDoctorPaymentRecoredList(string Name);
        public DoctorPaymentRecored GetDoctorPaymentRecoredById(int id);
        public Task<DataBaseResponse> SaveDoctorPaymentRecoredInfo(DoctorPaymentRecored model);
        public Task<DataBaseResponse> UpdateDoctorPaymentRecoredInfo(DoctorPaymentRecored model);
        public Task<DataBaseResponse> DeleteDoctorPaymentRecoredInfo(int id); 
    }
}
