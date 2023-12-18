using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.ViewModels.ViewADM
{
    public class FilesRecordViewModel
    {
        public int Id { get; set; }
        public string? HospitalName{ get; set; }
        public string? Patient_Id { get; set; }
        public string? PatientName { get; set; }
        public string? ModalityType_Id { get; set; }
        public string? StudyInstance_Id { get; set; }
        public string? SeriesInstance_id { get; set; }
        public int Status { get; set; }
        public int TotalFiles { get; set; }
    }
}
