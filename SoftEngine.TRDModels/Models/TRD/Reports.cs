using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class Reports
    {
        public int Id { get; set; }
        public int Hospital_Id { get; set; }
        public int Patient_Id { get; set; }
        public string? PatientName { get; set; }
        public string? ModalityType_Id { get; set; }
        public string? StudyInstance_Id { get; set; }
        public string? SeriesInstance_id { get; set; }
        public string? SopInstance_Id { get; set; }
        public int Doctor_Id { get; set; }
        public int DoctorAssign_Id { get; set; }
        public string? Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
