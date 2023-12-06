using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? passwor { get; set; }
        public string? sex { get; set; }
        public string? Brthday { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Full_Address { get; set; }
        public string? Phone { get; set; }
        public int Role_id { get; set; }
        public string? Type { get; set; }
        public string? Otp { get; set; }
        public bool Service_Suspended { get; set; }
        public string? Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
