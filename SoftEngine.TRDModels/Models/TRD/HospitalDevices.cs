using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class HospitalDevices
    {
        public int Id 
        {
            get;
            set;
        }
        public int Hospital_Id 
        {
            get; 
            set;
        }
        public string? Name 
        { 
            get;
            set;
        }
        public string? Description 
        {
            get; 
            set;
        }
        public int DeviceTypeId 
        {
            get; 
            set;
        }
        public int Status 
        { 
            get;
            set; 
        }
        public string? AddedDate 
        { 
            get;
            set;
        }
        public string? AddedBy
        { 
            get; 
            set; 
        }
        public string? UpdatedDate 
        { 
            get;
            set; 
        }
        public string? UpdatedBy 
        { 
            get;
            set;
        }
        public string? HospitalName { get; set; }
    }
}
