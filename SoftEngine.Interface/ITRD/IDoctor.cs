using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDoctor 
    {
        public IEnumerable<Doctor> GetDoctorList(string search);
        public Doctor GetDoctorById(int id);
        public Task<DataBaseResponse> SaveDoctorInfo(Doctor model);
        public Task<DataBaseResponse> UpdateDoctorInfo(Doctor model);
        public Task<DataBaseResponse> DeleteDoctorInfo(int id);
    }
}
