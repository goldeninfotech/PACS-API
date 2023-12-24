using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using SoftEngine.TRDModels.ViewModels.ViewADM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IBillingSettings
    {
        public IEnumerable<BillingSettingsViewModels> GetBillingSettingsList(string search);
        public BillingSettingsViewModels GetBillingSettingsById(int id);
        public Task<DataBaseResponse> SaveBillingSettingsInfo(BillingSettings model, List<BillSettingsDetails> dmodel);
        public Task<DataBaseResponse> UpdateBillingSettingsInfo(BillingSettings model, List<BillSettingsDetails> dmodel);
        public Task<DataBaseResponse> DeleteBillingSettingsInfo(int id);
        public IEnumerable<BillingSettingsDetailsViewModels> GetBillingSettingsDetailsList();
    }
}
