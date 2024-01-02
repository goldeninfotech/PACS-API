using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ViewModels.ViewADM
{
    public class DoctorViewModel
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string? DoctorName { get; set; }
        public string? BMDC_No { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? DOB { get; set; }
        public int Hospital_Id { get; set; }
        public int Department_Id { get; set; }
        public int Designation_Id { get; set; }
        public int Category_Id { get; set; }
        public string? Degree { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Full_Address { get; set; }
        public string? Immergency_Contact { get; set; }
        public int Status { get; set; }
        public string? Full_Name { get; set; }
        public string? HodpitalName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public string? DoctorCategoryName { get; set; }
    }
}
