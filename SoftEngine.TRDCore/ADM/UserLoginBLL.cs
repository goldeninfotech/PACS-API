using Dapper;
using SoftEngine.Interface.IADM;
using SoftEngine.TRDCore.Configurations;
using SoftEngine.TRDModels.Models.ADM;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDCore.ADM
{
    public class UserLoginBLL: IUserLogin
    {
        private readonly ConnectionStrings _dbSettings;
        public UserLoginBLL(ConnectionStrings dbSettings) 
        {
            _dbSettings = dbSettings;
        }
        public ADM_UserLogin UserLoginAsync(string username, string password)
        {
            string command = @"Select u.Id,u.Full_Name,u.UserName,d.Id DoctorId ,d.DoctorName,d.BMDC_No,h.Id HospitalId,h.Name HospitalName from [User] u
            left join Doctor d on d.User_Id =u.id 
            left join Hospital h on h.User_Id=u.Id
            where u.UserName='" + username + "' and u.Password='" + password + "'";

            var connectionString = _dbSettings.DefaultConnection;
            ADM_UserLogin obj = new ADM_UserLogin();
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                try
                {
                    obj = connection.Query<ADM_UserLogin>(command).FirstOrDefault();
                }
                catch (Exception exp)
                {
                    throw;
                }
            }
            return obj;
        }
    }
}
