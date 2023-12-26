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
    public interface IPatientReports
    {
        public IEnumerable<ReportsViewModel> GetPatientReportsList(string statusType,string Name);
        public ReportsViewModel GetPatientReportsById(int id);
        public Task<DataBaseResponse> SavePatientReportsInfo(Reports model);
        public Task<DataBaseResponse> UpdatePatientReportsInfo(Reports model);
        public Task<DataBaseResponse> DeletePatientReportsInfo(int id);
    }
}
