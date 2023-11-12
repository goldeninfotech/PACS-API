using SoftEngine.Interface.Models;
using System.Security.Cryptography;
using System.Text;

namespace GDNTRDSolution_API.Common
{
    public static class ReturnData
    {
        public static DataBaseResponse ReturnDataList(IEnumerable<object> list)
        {
            DataBaseResponse obj = new DataBaseResponse();
            if (list != null && list.Count() > 0)
            {
                obj.IsSuccess = true;
                obj.Message = "Get Data List ...";
                obj.TotalData = list.Count();
            }
            else
            {
                obj.IsSuccess = false;
                obj.Message = "Data Not Found !!!";
                obj.TotalData = 0;
            }

            return obj;
        }

        public static DataBaseResponse ReturnDataListById(object list)
        {
            DataBaseResponse obj = new DataBaseResponse();
            if (list != null)
            {
                obj.IsSuccess = true;
                obj.Message = "Get Data Object ...";
            }
            else
            {
                obj.IsSuccess = false;
                obj.Message = "Data Not Found !!!";
            }

            return obj;
        }

        public static string GenerateMD5(string password)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(password)).Select(s => s.ToString("x2")));
        }
    }
}
