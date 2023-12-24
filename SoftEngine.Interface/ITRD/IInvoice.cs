using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IInvoice
    {
        public IEnumerable<Invoice> GetInvoiceList(string search);
        public Invoice GetInvoiceById(int id);
        public Task<DataBaseResponse> SaveInvoiceInfo(Invoice model);
        public Task<DataBaseResponse> UpdateInvoiceInfo(Invoice model);
        public Task<DataBaseResponse> DeleteInvoiceInfo(int id);
    }
}
