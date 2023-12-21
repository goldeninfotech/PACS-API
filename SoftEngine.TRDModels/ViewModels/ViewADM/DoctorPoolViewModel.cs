using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ViewModels.ViewADM
{
    public class DoctorPoolViewModel
    {
        public int Id { get; set; }
        public int Doctor_Id { get; set; }
        public int Hospital_Id { get; set; }
        public int Status { get; set; }
        public string? DoctorName { get; set; }
        public string? HospitalName { get; set; } 
    }
}
