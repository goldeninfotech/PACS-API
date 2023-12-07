using Dapper;
using SoftEngine.Interface.ITRD;
using SoftEngine.Interface.Models;
using SoftEngine.TRDCore.Configurations;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDCore.TRD
{
    public class UsersBLL : IUsers
    {
        private readonly ConnectionStrings _dbSettings;
        public UsersBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings; 
        }
        public IEnumerable<User> GetUsersList()
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Full_Name,Gender,DOB,Country,City,Full_Address,Phone,Role_id,UserType,Status from [User] where Status=1";
                var models = connection.Query<User>(sql).ToList();
                return models;
            }
        }

        public User GetUsersById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,Full_Name,Gender,DOB,Country,City,Full_Address,Phone,Role_id,UserType,Status from [User] where Status=1 ");
                strSql.AppendLine(" and Id=@Id ");
                var models = connection.Query<User>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveUsersInfo(User model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateUsers(model.UserName, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  [User]");
                    strSql.AppendLine(" ( Full_Name,UserName,Password,Gender,DOB,Country,City,Full_Address,Phone,Role_id,UserType,Status,AddedBy,AddedDate) VALUES ");
                    strSql.AppendLine(" ( @Full_Name,@UserName,@Password,@Gender,@DOB,@Country,@City,@Full_Address,@Phone,@Role_id,@UserType,@Status,@AddedBy,@AddedDate);");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Full_Name = model.Full_Name,
                                            UserName = model.UserName,
                                            Password = model.Password,
                                            Gender = model.Gender,
                                            DOB = model.DOB,
                                            Country = model.Country,
                                            City = model.City,
                                            Full_Address = model.Full_Address,
                                            Phone = model.Phone,
                                            Role_id = model.Role_id,
                                            UserType = model.UserType, 
                                            Status = model.Status,
                                            AddedBy = model.AddedBy,
                                            AddedDate = model.AddedDate,
                                        }
                                        );
                        response.ReturnValue = Saveresult;
                        response.Message = GlobalConst.INSERT_SUCCESS_MESSAGE;
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
                    response.Message = GlobalConst.GET_DUPLICATEDATA;
                    response.IsSuccess = false;
                }
            }
            return response;
        }

        public async Task<DataBaseResponse> UpdateUsersInfo(User model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateUsers(model.UserName, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  [User] SET ");
                    strSql.AppendLine(" Full_Name=@Full_Name,Gender=@Gender,DOB=@DOB,Country=@Country,City=@City,Full_Address=@Full_Address," +
                        " Phone=@Phone,Role_id=@Role_id,UserType=@UserType,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id=model.Id,
                                            Full_Name = model.Full_Name,
                                            Gender = model.Gender,
                                            DOB = model.DOB,
                                            Country = model.Country,
                                            City = model.City,
                                            Full_Address = model.Full_Address,
                                            Phone = model.Phone,
                                            Role_id = model.Role_id,
                                            UserType = model.UserType,
                                            Status = model.Status,
                                            UpdatedBy = model.UpdatedBy,
                                            UpdatedDate = model.UpdatedDate
                                        }
                                        );
                        response.ReturnValue = Saveresult;
                        response.Message = GlobalConst.UPDATE_SUCCESS_MESSAGE;
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
                    response.Message = GlobalConst.GET_DUPLICATEDATA;
                    response.IsSuccess = false;
                }
            }
            return response;
        }
        public async Task<DataBaseResponse> DeleteUsersInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE [User] SET Status=@Status WHERE Id=@Id");
                try
                {
                    var result = connection.Execute(strSql.ToString(), new { Status = 0, Id = id, });
                    response.ReturnValue = result;
                    response.Message = GlobalConst.DELETE_SUCCESS_MESSAGE;
                    response.IsSuccess = true;
                }
                catch (Exception exp)
                {
                    response.ReturnValue = -1;
                    response.Message = exp.Message;
                    response.IsSuccess = false;
                }
            }
            return response;
        }

        public bool GetDuplicateUsers(string name, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,UserName,Status from [User] where Status=1  ";
                if (!string.IsNullOrEmpty(name))
                    sql += @" and REPLACE(LOWER(UserName), ' ', '') =REPLACE('" + name + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<User>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
