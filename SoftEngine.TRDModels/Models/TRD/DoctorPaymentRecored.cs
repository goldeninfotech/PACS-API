using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class DoctorPaymentRecored
    {
        public int Id { get; set; }
        public int Doctor_Id { get; set; }
        public string? PaidAmount { get; set; }
        public string? PaymentNotes { get; set; }
        public int PaymentsMathodId { get; set; }
        public string? PaidBy { get; set; }
        public string? PaidDate { get; set; }
        public string? Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
