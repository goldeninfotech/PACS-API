using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IReportsDescriptipns
    {
        public IEnumerable<ReportsDescriptipns> GetReportsDescriptipnsList();
        public ReportsDescriptipns GetReportsDescriptipnsById(int id);
        public Task<DataBaseResponse> SaveReportsDescriptipnsInfo(ReportsDescriptipns model);
        public Task<DataBaseResponse> UpdateReportsDescriptipnsInfo(ReportsDescriptipns model);
        public Task<DataBaseResponse> DeleteReportsDescriptipnsInfo(int id);
    }
}
