using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IReportTemplate
    {
        public IEnumerable<ReportTemplate> GetReportTemplateList(string Name);
        public ReportTemplate GetReportTemplateById(int id);
        public Task<DataBaseResponse> SaveReportTemplateInfo(ReportTemplate model);
        public Task<DataBaseResponse> UpdateReportTemplateInfo(ReportTemplate model);
        public Task<DataBaseResponse> DeleteReportTemplateInfo(int id);
    }
}
