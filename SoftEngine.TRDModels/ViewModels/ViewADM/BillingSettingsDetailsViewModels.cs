using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ViewModels.ViewADM
{
    public class BillingSettingsDetailsViewModels
    {
        public int Id { get; set; }
        public int Bill_Id { get; set; }
        public int DeviceTypeId { get; set; }
        public string? NumOfDevice { get; set; }
        public string? UnitPrice { get; set; }
        public string? RR_CommonAmount { get; set; }
        public int Status { get; set; }
        public string? DeviceName { get; set; }
    }
}
