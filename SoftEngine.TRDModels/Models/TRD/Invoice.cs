using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? DateTime  {get;set;}
        public string? InvoiceDate { get; set; }
        public string? InvoiceDueDate { get; set; }
        public int BillSetting_Id { get; set; }
        public string? InvoiceAmount { get; set; }
        public string? DiscountAmount { get; set; }
        public string? NetAmount { get; set; }
        public string? Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
