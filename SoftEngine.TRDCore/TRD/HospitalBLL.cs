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
    public class HospitalBLL : IHospital
    {
        private readonly ConnectionStrings _dbSettings;
        public HospitalBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }
        #region Hospital CRUD
        public IEnumerable<Hospital> GetHospitalList(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,User_Id,Name,Description,Email,HospitalCategory_Id,Country,City,Full_Address,Phone,Status from Hospital where Status=1";
                if (!string.IsNullOrEmpty(search))
                    sql += " and ( Name='"+search+"' or City='"+search+ "' or Phone='" + search+"' )";
                var models = connection.Query<Hospital>(sql).ToList();
                return models;
            }
        }
        public Hospital GetHospitalById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,User_Id,Name,Description,Email,HospitalCategory_Id,Country,City,Full_Address,Phone,Image,Status from Hospital where Status=1 ");
                strSql.AppendLine(" and Id=@Id ");
                var models = connection.Query<Hospital>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models; 
            }
        }
        public async Task<DataBaseResponse> SaveHospitalInfo2(Hospital model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateHospital(model.Name, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  Hospital");
                    strSql.AppendLine(" ( User_Id,Name,Description,Email,HospitalCategory_Id,Country,City,Full_Address,Phone,Status,AddedBy,AddedDate) VALUES ");
                    strSql.AppendLine(" ( @User_Id,@Name,@Description,@Email,@HospitalCategory_Id,@Country,@City,@Full_Address,@Phone,@Status,@AddedBy,@AddedDate)");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            User_Id = model.User_Id,
                                            Name = model.Name,
                                            Description = model.Description,
                                            Email = model.Email,
                                            HospitalCategory_Id = model.HospitalCategory_Id,
                                            Country = model.Country,
                                            City = model.City,
                                            Full_Address = model.Full_Address,
                                            Phone = model.Phone,
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

        public async Task<DataBaseResponse> SaveHospitalInfo(Hospital model)
        {
            var response = new DataBaseResponse();
            bool duplicateresult = GetDuplicateHospital(model.Name, 0);
            if (duplicateresult)
            {
                try
                {
                    await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
                    {
                        StringBuilder strSql = new StringBuilder();
                        await connection.OpenAsync();
                        using (var transaction = connection.BeginTransaction())
                        {
                            strSql.AppendLine(" INSERT INTO  [User]");
                            strSql.AppendLine(" ( Full_Name,UserName,Password,Gender,DOB,Country,City,Full_Address,Phone,Role_id,UserType,Status,AddedBy,AddedDate)  ");
                            strSql.AppendLine(" Output Inserted.Id VALUES ( @Full_Name,@UserName,@Password,@Gender,@DOB,@Country,@City,@Full_Address,@Phone,@Role_id,@UserType,@Status,@AddedBy,@AddedDate);");

                            int Saveresult = connection.ExecuteScalar<int>(strSql.ToString(), new
                            {
                                Full_Name = model.Name,
                                UserName = model.UserName,
                                Password = model.Password,
                                Gender = "",
                                DOB = "",
                                Country = model.Country,
                                City = model.City,
                                Full_Address = model.Full_Address,
                                Phone = model.Phone,
                                Role_id = model.RoleId,
                                UserType = "2",
                                Status = model.Status,
                                AddedBy = model.AddedBy,
                                AddedDate = model.AddedDate,

                            }, transaction);
                            strSql = new StringBuilder();
                            strSql.AppendLine(" INSERT INTO  Hospital");
                            strSql.AppendLine(" ( User_Id,Name,Description,Email,HospitalCategory_Id,Country,City,Full_Address,Phone,Status,AddedBy,AddedDate) VALUES ");
                            strSql.AppendLine(" ( @User_Id,@Name,@Description,@Email,@HospitalCategory_Id,@Country,@City,@Full_Address,@Phone,@Status,@AddedBy,@AddedDate)");
                            var resul2t = await connection.ExecuteAsync(strSql.ToString(), new
                            {
                                User_Id = Saveresult,
                                Name = model.Name,
                                Description = model.Description,
                                Email = model.Email,
                                HospitalCategory_Id = model.HospitalCategory_Id,
                                Country = model.Country,
                                City = model.City,
                                Full_Address = model.Full_Address,
                                Phone = model.Phone,
                                Status = model.Status,
                                AddedBy = model.AddedBy,
                                AddedDate = model.AddedDate
                            }, transaction);

                            transaction.Commit();

                            response.ReturnValue = 1;
                            response.Message = GlobalConst.INSERT_SUCCESS_MESSAGE;
                            response.IsSuccess = true;
                        }

                    }
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
            return response;
        }


        public async Task<DataBaseResponse> UpdateHospitalInfo(Hospital model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateHospital(model.Name, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  Hospital SET ");
                    strSql.AppendLine(" Name=@Name,Description=@Description,Email=@Email,HospitalCategory_Id=@HospitalCategory_Id,Country=@Country,City=@City, ");
                    strSql.AppendLine(" Full_Address=@Full_Address,Phone=@Phone,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate Where Id=@Id ");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            Name = model.Name,
                                            Description = model.Description,
                                            Email = model.Email,
                                            HospitalCategory_Id = model.HospitalCategory_Id,
                                            Country = model.Country,
                                            City = model.City,
                                            Full_Address = model.Full_Address,
                                            Phone = model.Phone,
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
        public async Task<DataBaseResponse> DeleteHospitalInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE Hospital SET Status=@Status WHERE Id=@Id");
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

        public bool GetDuplicateHospital(string name, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Name,Status from Hospital where Status=1  ";
                if (!string.IsNullOrEmpty(name))
                    sql += @" and REPLACE(LOWER(Name), ' ', '') =REPLACE('" + name + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<Hospital>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion
        public Hospital GetHospitalByUserId(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,User_Id,Name,Description,Email,HospitalCategory_Id,Country,City,Full_Address,Phone,Status from Hospital where Status=1 ");
                strSql.AppendLine(" and User_Id=@User_Id ");
                var models = connection.Query<Hospital>(strSql.ToString(), new { User_Id = id, }).FirstOrDefault(); 
                return models;
            }
        }

    }
}
