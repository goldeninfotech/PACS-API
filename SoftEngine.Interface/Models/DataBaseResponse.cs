using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.Models
{
    [Serializable]
    public class DataBaseResponse
    {
        public int ReturnValue { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public string? ReferenceNumber { get; set; }
        public int TotalData { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int DataLimit { get; set; }
    }
}
