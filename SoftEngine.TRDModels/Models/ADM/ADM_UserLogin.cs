using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.ADM
{
    public class ADM_UserLogin
    {
        public int Id { get; set; }
        public string? Full_Name { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? DoctorName { get; set; }
        public string? BMDC_No { get; set; }
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        public string? HospitalName { get; set; }
        public string? PasswordRecCode { get; set; } 
        public string? Email { get; set; } 
    }
}
