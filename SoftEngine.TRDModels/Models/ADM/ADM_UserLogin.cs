using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.ADM
{
    public class ADM_UserLogin
    {
        public int EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public int CompanyID { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? PasswordRecCode { get; set; }
    }
}
