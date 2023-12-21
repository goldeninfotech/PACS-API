using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface ISettings
    {
        public IEnumerable<Setting> GetSettingList(string Name);
        public Setting GetSettingById(int id);
        public Task<DataBaseResponse> SaveSettingInfo(Setting model);
        public Task<DataBaseResponse> UpdateSettingInfo(Setting model);
        public Task<DataBaseResponse> DeleteSettingInfo(int id);
    }
}
