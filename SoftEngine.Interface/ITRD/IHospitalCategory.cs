using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IHospitalCategory
    {
        public IEnumerable<HospitalCategory> GetHospitalCategoryList(string search);
        public HospitalCategory GetHospitalCategoryById(int id); 
        public Task<DataBaseResponse> SaveHospitalCategoryInfo(HospitalCategory model);
        public Task<DataBaseResponse> UpdateHospitalCategoryInfo(HospitalCategory model);
        public Task<DataBaseResponse> DeleteHospitalCategoryInfo(int id); 
    }
}
