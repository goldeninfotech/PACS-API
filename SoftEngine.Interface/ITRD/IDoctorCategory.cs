using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDoctorCategory
    {
        public IEnumerable<DoctorCategory> GetDoctorCategoryList(string Name);
        public DoctorCategory GetDoctorCategoryById(int id);
        public Task<DataBaseResponse> SaveDoctorCategoryInfo(DoctorCategory model);
        public Task<DataBaseResponse> UpdateDoctorCategoryInfo(DoctorCategory model);
        public Task<DataBaseResponse> DeleteDoctorCategoryInfo(int id);
    }
}
