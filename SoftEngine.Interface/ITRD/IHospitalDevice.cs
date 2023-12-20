using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IHospitalDevice
    {
        public IEnumerable<HospitalDevices> GetHospitalDevicesList(string Name); 
        public HospitalDevices GetHospitalDevicesById(int id);
        public Task<DataBaseResponse> SaveHospitalDevicesInfo(HospitalDevices model);
        public Task<DataBaseResponse> UpdateHospitalDevicesInfo(HospitalDevices model);
        public Task<DataBaseResponse> DeleteHospitalDevicesInfo(int id);
    }
}
