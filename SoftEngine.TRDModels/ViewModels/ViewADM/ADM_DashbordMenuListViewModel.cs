using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ViewModels.ViewADM
{
    public class ADM_DashbordMenuListViewModel
    {
        public int MenuId { get; set; }
        public string? MenuName { get; set; }
        public int SubMenuId { get; set; }
        public string? SubMenuName { get; set; }
        public string? ActionLink { get; set; }
        public bool DataView { get; set; }
        public bool DataInsert { get; set; }
        public bool DataUpdate { get; set; }
        public bool DataDelete { get; set; }
    }
}
