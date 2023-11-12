using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.ADM
{
    public class ADM_RoleInfo
    {
        public int RoleId
        {
            get;
            set;
        }
        public string? RoleName
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

    }
}
