using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDoctorSchedule
    {
        public IEnumerable<DoctorSchedule> GetDoctorScheduleList(string Search);
        public DoctorSchedule GetDoctorScheduleById(int id);
        public Task<DataBaseResponse> SaveDoctorScheduleInfo(DoctorSchedule model);
        public Task<DataBaseResponse> UpdateDoctorScheduleInfo(DoctorSchedule model);
        public Task<DataBaseResponse> DeleteDoctorScheduleInfo(int id);
    }
}
