using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IDeviceType
    {
        public IEnumerable<DeviceType> GetDeviceTypeList(string Name);
        public DeviceType GetDeviceTypeById(int id);
        public Task<DataBaseResponse> SaveDeviceTypeInfo(DeviceType model);
        public Task<DataBaseResponse> UpdateDeviceTypeInfo(DeviceType model);
        public Task<DataBaseResponse> DeleteDeviceTypeInfo(int id); 
    }
}
