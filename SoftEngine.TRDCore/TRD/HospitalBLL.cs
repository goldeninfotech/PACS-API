using Dapper;
using SoftEngine.Interface.ITRD;
using SoftEngine.Interface.Models;
using SoftEngine.TRDCore.Configurations;
using SoftEngine.TRDModels.Models.TRD;
using SoftEngine.TRDModels.ViewModels.ViewADM;
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
        public IEnumerable<HospitalViewModel> GetHospitalList(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"select  h.Id,h.User_Id,h.Name ,h.Description,h.Email,h.HospitalCategory_Id,h.Country,h.City,h.Full_Address,h.Phone,
                h.Image,h.Status,u.Full_Name,hc.Name HospitalCategoryName
                from Hospital h
                left join [User] u on u.Id=h.User_Id
                left join HospitalCategory hc on hc.Id=h.HospitalCategory_Id where h.Status=1";
                if (!string.IsNullOrEmpty(search))
                    sql += " and ( h.Name='"+search+"' or h.City='"+search+ "' or h.Phone='" + search+"' )";
                var models = connection.Query<HospitalViewModel>(sql).ToList();
                return models;
            }
        }
        public HospitalViewModel GetHospitalById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(@" select  h.Id,h.User_Id,h.Name HospitalName,h.Description,h.Email,h.HospitalCategory_Id,h.Country,h.City,h.Full_Address,h.Phone,
                h.Image,h.Status,u.Full_Name,hc.Name HospitalCategoryName
                from Hospital h
                left join [User] u on u.Id=h.User_Id
                left join HospitalCategory hc on hc.Id=h.HospitalCategory_Id where h.Status=1 ");
                strSql.AppendLine(" and h.Id=@Id ");
                var models = connection.Query<HospitalViewModel>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
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


        #region Update Hospital Status Info
        public async Task<DataBaseResponse> UpdateHospitalStatusInfo(Hospital model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  Hospital SET ");
                strSql.AppendLine(" Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate Where Id=@Id ");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
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
            return response;
        }



        #endregion

        #region Duplicate Hospital Info
        public bool GetDuplicateHospitalInfo(string Phone, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Phone,Status from [User] where Status=1  ";
                if (!string.IsNullOrEmpty(Phone))
                    sql += @" and Phone='" + Phone + "' ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<Doctor>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region Update Hospital Phone Info
        public async Task<DataBaseResponse> UpdateHospitalPhoneInfo(string phone, int userid) 
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateHospitalInfo(phone, userid);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  [User] SET ");
                    strSql.AppendLine(" Phone=@Phone,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id ");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = userid,
                                            Status = phone,
                                            UpdatedDate = DateTime.Now,
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


        #endregion

        #region Update Hospital Password Info
        public async Task<DataBaseResponse> UpdateHospitalPasswordInfo(string password, int userid)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  [User] SET ");
                strSql.AppendLine(" Password=@Password,UpdatedDate=@UpdatedDate ");
                strSql.AppendLine(" Where Id=@Id ");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = userid,
                                        Password = password,
                                        UpdatedDate = DateTime.Now,
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
            return response;
        }
        #endregion

    }
}
