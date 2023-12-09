using SoftEngine.Interface.Models;
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

        public Task<DataBaseResponse> PasswordRecovery(string email, string recNumber);
        public Task<DataBaseResponse> ChangePassword(string newPassword, string recCode);
        public bool CheckRecoveryCode(int code);
        ADM_UserLogin GetUserinfoByEmail(string email);
    }
}
