using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDesignations
    {
        public IEnumerable<Designations> GetDesignationsList(string Name);
        public Designations GetDesignationsById(int id);
        public Task<DataBaseResponse> SaveDesignationsInfo(Designations model);
        public Task<DataBaseResponse> UpdateDesignationsInfo(Designations model);
        public Task<DataBaseResponse> DeleteDesignationsInfo(int id);
    }
}
