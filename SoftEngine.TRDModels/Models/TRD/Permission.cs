using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class Permission
    {
        public int Id 
        { 
            get;
            set; 
        }
        public int User_Id 
        { 
            get;
            set;
        }
        public int Role_id 
        { 
            get;
            set;
        }
        public string? Route
        { 
            get;
            set; 
        }
        public int Status 
        { 
            get;
            set; 
        }
        public string? AddedDate 
        { 
            get; 
            set; 
        }
        public string? AddedBy 
        {
            get;
            set;
        }
        public string? UpdatedDate 
        { 
            get; 
            set; 
        }
        public string? UpdatedBy 
        { 
            get;
            set;
        }
        public string? Full_Name { get; set; }
        public string? RoleName { get; set; }
    }
}
