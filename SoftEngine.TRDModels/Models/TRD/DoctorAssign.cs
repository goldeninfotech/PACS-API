﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class DoctorAssign
    {
        public int Id { get; set; }
        public int Doctor_Id { get; set; }
        public string? ModalityType_Id { get; set; }
        public string? StudyInstance_Id { get; set; }
        public string? SeriesInstance_id { get; set; }
        public int Status { get; set; }
        public string? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
