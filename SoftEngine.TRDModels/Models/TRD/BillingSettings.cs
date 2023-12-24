using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class BillingSettings
    {
        public int Id { get; set; }
        public string? BillNumber { get; set; }
        public string? BillFor { get; set; }
        public int Hospital_Id { get; set; }
        public int Doctor_Id { get; set; }
        public string? Payment_Mode { get; set; }
        public string? Active_Payment_Date { get; set; }
        public int Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public List<BillSettingsDetails> BillSettingsDetails { get; set; } 


    }
}
