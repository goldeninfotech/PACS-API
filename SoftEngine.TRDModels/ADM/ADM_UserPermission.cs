using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ADM
{
    public class ADM_UserPermission
    {
        public int PermissionId
        {
            get;
            set;
        }
        public int UserCode
        {
            get;
            set;
        }
        public int SubMenuId
        {
            get;
            set;
        }
        public bool DataView
        {
            get;
            set;
        }
        public bool DataInsert
        {
            get;
            set;
        }
        public bool DataUpdate
        {
            get;
            set;
        }
        public bool DataDelete
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

        public string? SubMenuName { get; set; }
        public string? MenuName { get; set; }
        public string? pSubMenuId { get; set; }
        public string? pDataView { get; set; }
        public string? pDataInsert { get; set; }
        public string? pDataUpdate { get; set; }
        public string? pDataDelete { get; set; }
        public string? MenuId { get; set; }
    }
}
