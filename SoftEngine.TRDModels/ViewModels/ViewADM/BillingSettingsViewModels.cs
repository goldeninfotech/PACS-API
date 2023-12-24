using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ViewModels.ViewADM
{
    public class BillingSettingsViewModels
    {
        public int Id { get; set; }
        public string? BillNumber { get; set; }
        public string? BillFor { get; set; }
        public int Hospital_Id { get; set; }
        public int Doctor_Id { get; set; }
        public string? Payment_Mode { get; set; }
        public string? Active_Payment_Date { get; set; }
        public int Status { get; set; }
        public string? HospitalName { get; set; }
        public string? DoctorName { get; set; } 
    }
}
