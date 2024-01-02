using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ViewModels.ViewADM
{
    public class HospitalViewModel
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public int HospitalCategory_Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Full_Address { get; set; }
        public string? Phone { get; set; }
        public int Status { get; set; }
        public string? Full_Name { get; set; }
        public string? HospitalCategoryName { get; set; }
}
}
