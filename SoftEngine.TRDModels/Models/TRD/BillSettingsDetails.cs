using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class BillSettingsDetails
    {
        public int Id { get; set; }
        public int Bill_Id { get; set; }
        public int DeviceTypeId { get; set; }
        public string? NumOfDevice { get; set; }
        public string? UnitPrice { get; set; }
        public string? RR_CommonAmount { get; set; }
        public string? Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
