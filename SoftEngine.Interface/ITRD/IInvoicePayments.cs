using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IInvoicePayments
    {
        public IEnumerable<Invoicepayments> GetInvoicepaymentsList(string Name);
        public Invoicepayments GetInvoicepaymentsById(int id);
        public Task<DataBaseResponse> SaveInvoicepaymentsInfo(Invoicepayments model);
        public Task<DataBaseResponse> UpdateInvoicepaymentsInfo(Invoicepayments model);
        public Task<DataBaseResponse> DeleteInvoicepaymentsInfo(int id);
    }
}
