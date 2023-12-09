using SoftEngine.TRDModels.Models.ADM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.IADM
{
    public interface IUserLogin
    {
        public ADM_UserLogin UserLoginAsync(string username, string password);
    }
}
