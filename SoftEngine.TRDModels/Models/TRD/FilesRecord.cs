﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class FilesRecord
    {
        public int Id { get; set; }
        public int Hospital_Id { get; set; }
        public string? Patient_Id { get; set; }
        public string? PatientName { get; set; }
        public string? ModalityType_Id { get; set; }
        public string? StudyInstance_Id { get; set; }
        public string? SeriesInstance_id { get; set; }
        public string? SopInstance_Id { get; set; }
        public string? SeriesNumber { get; set; }
        public string? InstanceNumber { get; set; }
        public int Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
