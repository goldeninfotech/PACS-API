using Dapper;
using SoftEngine.Interface.IADM;
using SoftEngine.Interface.Models;
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

        public async Task<DataBaseResponse> PasswordRecovery(string email, string recNumber)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {

                ADM_UserLogin obj = new ADM_UserLogin();
                string command = @"Select u.Id,u.Full_Name,u.UserName,u.Email from [User] u  
                    where u.Email='"
                + email + "' ";

                var obj2 = connection.Query<ADM_UserLogin>(command.ToString()).FirstOrDefault();
                if (obj2 != null)
                {

                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  [User] SET ");
                    strSql.AppendLine(" PasswordRecCode=@PasswordRecCode, PasswordRecStatus=@PasswordRecStatus ");
                    strSql.AppendLine(" Where EmpCode=EmpCode and Email=@Email;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                        new
                        {
                            EmpCode = obj2.Id,
                            Email = email,
                            PasswordRecCode = recNumber,
                            PasswordRecStatus = false
                        });

                        response.ReturnValue = Saveresult;
                        response.Message = "The email was sent successfully. Please check your email.";
                        response.IsSuccess = true;
                    }
                    catch (Exception exp)
                    {
                        response.ReturnValue = -1;
                        response.Message = exp.Message;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    response.ReturnValue = -1;
                    response.Message = GlobalConst.EMAIL_NOT_FOUND;
                    response.IsSuccess = false;

                }
            }
            return response;
        }

        public bool CheckRecoveryCode(int code)
        {
            string command = @"Select u.Id,u.Full_Name,u.UserName,u.Email,u.PasswordRecCode from [User] u 
            where PasswordRecCode='" + code + "' ";

            var connectionString = _dbSettings.DefaultConnection;
            ADM_UserLogin obj = new ADM_UserLogin();
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                obj = connection.Query<ADM_UserLogin>(command).FirstOrDefault();
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.PasswordRecCode) && obj.PasswordRecCode == code.ToString())
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.AppendLine(" UPDATE  [User] SET ");
                        strSql.AppendLine(" PasswordRecStatus=@PasswordRecStatus ");
                        strSql.AppendLine(" Where Id=Id and PasswordRecCode=@PasswordRecCode ;");
                        var Saveresult = connection.Execute(strSql.ToString(),
                            new
                            {
                                EmpCode = obj.Id,
                                PasswordRecCode = code,
                                PasswordRecStatus = true
                            });
                        return true;
                    }
                }
                else
                    return false;
            }

            return false;
        }


        public async Task<DataBaseResponse> ChangePassword(string newPassword, string recCode)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {

                ADM_UserLogin obj = new ADM_UserLogin();
                string command = @"Select u.Id,u.Full_Name,u.UserName,u.Email from [User] u 
                    where u.PasswordRecCode=" + recCode + " ";

                var obj2 = connection.Query<ADM_UserLogin>(command.ToString()).FirstOrDefault();
                if (obj2 != null)
                {

                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  [User] SET ");
                    strSql.AppendLine(" Password=@Password ");
                    strSql.AppendLine(" ,PasswordRecCode='' , PasswordRecStatus=0 ");
                    strSql.AppendLine(" Where Id=@Id and PasswordRecCode=@PasswordRecCode and PasswordRecStatus=@PasswordRecStatus ;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                        new
                        {
                            EmpCode = obj2.Id,
                            Password = newPassword,
                            PasswordRecCode = recCode,
                            PasswordRecStatus = true,
                        });

                        response.ReturnValue = Saveresult;
                        response.Message = "Successfully changed the password.";
                        response.IsSuccess = true;
                    }
                    catch (Exception exp)
                    {
                        response.ReturnValue = -1;
                        response.Message = exp.Message;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    response.ReturnValue = -1;
                    response.Message = GlobalConst.EMAIL_NOT_FOUND;
                    response.IsSuccess = false;

                }
            }
            return response;
        }

        public ADM_UserLogin GetUserinfoByEmail(string email)
        {
            string sql = @"Select u.Id,u.Full_Name,u.UserName,u.Email
            from [User] u 
            where 1=1";

            if (!string.IsNullOrEmpty(email))
                sql += " and u.Email='" + email + "'";

            var connectionString = _dbSettings.DefaultConnection;
            ADM_UserLogin obj = new ADM_UserLogin();
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                try
                {
                    obj = connection.Query<ADM_UserLogin>(sql).FirstOrDefault();
                }
                catch
                {
                    throw;
                    // Handle exceptions that occur during query execution
                }
            }

            return obj;
        }
    }
}
