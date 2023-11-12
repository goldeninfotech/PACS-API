using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ADM
{
    public class ADM_SubMenu
    {
        public int SubMenuId
        {
            get;
            set;
        }
        public string? SubMenuName
        {
            get;
            set;
        }
        public int MenuId
        {
            get;
            set;
        }
        public string? ActionLink
        {
            get;
            set;
        }
        public string? IsActive
        {
            get;
            set;
        }
        public string? AddedBy
        {
            get;
            set;
        }
        public string? AddedDate
        {
            get;
            set;
        }
        public string? UpdatedBy
        {
            get;
            set;
        }
        public string? UpdatedDate
        {
            get;
            set;
        }
        public string? PositionNumber
        {
            get;
            set;
        }

        public string? MenuName { get; set; }
    }
}
