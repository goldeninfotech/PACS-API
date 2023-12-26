using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
    public class ReportsDescriptipns
    {
        public int Id { get; set; }
        public int Reports_Id { get; set; }
        public string key { get; set; }
        public string Value { get; set; }
        public int Status { get; set; }
        public string AddedDate { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
