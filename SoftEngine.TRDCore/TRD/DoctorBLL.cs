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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDCore.TRD
{
    public class DoctorBLL : IDoctor
    {
        private readonly ConnectionStrings _dbSettings;
        public DoctorBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region Doctor CRUD
        public IEnumerable<DoctorViewModel> GetDoctorList( string search, string statustype )
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select d.Id,d.User_Id,d.DoctorName,d.BMDC_No,d.Email,d.Gender,d.DOB,d.Hospital_Id,d.Department_Id,d.Designation_Id,d.Category_Id,d.Degree,d.Country,d.
                City,d.Full_Address,d.Immergency_Contact,d.Status,u.Full_Name,h.Name HodpitalName, 
                dep.Name DepartmentName,desi.Name DesignationName,dc.Name DoctorCategoryName
                from Doctor d 
                left join [User] u on u.Id=d.User_Id
                left join Hospital h on h.Id=d.Hospital_Id
                left join Departments dep on dep.Id=d.Department_Id
                left join Designations desi on desi.Id=d.Designation_Id
                left join DoctorCategory dc on dc.Id=d.Category_Id
                where  1=1 ";
                if (!string.IsNullOrEmpty(statustype))
                    sql += " and d.Status= " + statustype + "";
                if (!string.IsNullOrEmpty(search) )
                    sql += " and ( d.DoctorName='"+search+"' or d.BMDC_No='"+search+"' or d.City='"+search+"' or h.Name='"+search+"') ";
                
                var models = connection.Query<DoctorViewModel>(sql).ToList();
                return models;
            }
        }
        public DoctorViewModel GetDoctorById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(@" Select d.Id,d.User_Id,d.DoctorName,d.BMDC_No,d.Email,d.Gender,d.DOB,d.Hospital_Id,d.Department_Id,d.Designation_Id,d.Category_Id,d.Degree,d.Country,d.
                City,d.Full_Address,d.Immergency_Contact,d.Status,u.Full_Name,h.Name HodpitalName, 
                dep.Name DepartmentName,desi.Name DesignationName,dc.Name DoctorCategoryName
                from Doctor d 
                left join [User] u on u.Id=d.User_Id
                left join Hospital h on h.Id=d.Hospital_Id
                left join Departments dep on dep.Id=d.Department_Id
                left join Designations desi on desi.Id=d.Designation_Id
                left join DoctorCategory dc on dc.Id=d.Category_Id where d.Status=1 ");
                strSql.AppendLine(" and d.Id=@Id ");
                var models = connection.Query<DoctorViewModel>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }
        public async Task<DataBaseResponse> SaveDoctorInfo(Doctor model)
        {
            var response = new DataBaseResponse();
            bool duplicateresult = GetDuplicateDoctor(model.DoctorName, 0);
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
                                Full_Name = model.DoctorName,
                                UserName = model.UserName,
                                Password = model.Password,
                                Gender = model.Gender,
                                DOB = model.DOB,
                                Country = model.Country,
                                City = model.City,
                                Full_Address = model.Full_Address,
                                Phone = model.Immergency_Contact,
                                Role_id = model.RoleId,
                                UserType = "3",
                                Status = model.Status,
                                AddedBy = model.AddedBy,
                                AddedDate = model.AddedDate,

                            }, transaction);
                            strSql = new StringBuilder();
                            strSql.AppendLine(" INSERT INTO  Doctor");
                            strSql.AppendLine(" ( User_Id,DoctorName,BMDC_No,Email,Gender,DOB,Hospital_Id,Department_Id,Designation_Id,Category_Id,Degree,Country,City,Full_Address, Immergency_Contact,Status,AddedBy,AddedDate) VALUES ");
                            strSql.AppendLine(" ( @User_Id,@DoctorName,@BMDC_No,@Email,@Gender,@DOB,@Hospital_Id,@Department_Id,@Designation_Id,@Category_Id,@Degree,@Country,@City,@Full_Address, @Immergency_Contact,@Status,@AddedBy,@AddedDate) ");

                            var resul2t = await connection.ExecuteAsync(strSql.ToString(), new
                            {
                                User_Id = Saveresult,
                                DoctorName = model.DoctorName,
                                BMDC_No = model.BMDC_No,
                                Email = model.Email,
                                Gender = model.Gender,
                                DOB = model.DOB,
                                Hospital_Id = model.Hospital_Id,
                                Department_Id = model.Department_Id,
                                Designation_Id = model.Designation_Id,
                                Category_Id = model.Category_Id,
                                Degree = model.Degree,
                                Country = model.Country,
                                City = model.City,
                                Full_Address = model.Full_Address,
                                Immergency_Contact = model.Immergency_Contact,
                                Status = model.Status,
                                AddedBy = model.AddedBy,
                                AddedDate = model.AddedDate,
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
        public async Task<DataBaseResponse> UpdateDoctorInfo(Doctor model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctor(model.DoctorName, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  Doctor SET ");
                    strSql.AppendLine(" DoctorName=@DoctorName,BMDC_No=@BMDC_No,Email=@Email,Gender=@Gender,DOB=@DOB,Hospital_Id=@Hospital_Id, ");
                    strSql.AppendLine(" Department_Id=@Department_Id,Designation_Id=@Designation_Id,Category_Id=@Category_Id,Degree=@Degree,Country=@Country,City=@City,Full_Address=@Full_Address,Immergency_Contact=@Immergency_Contact ");
                    strSql.AppendLine(" Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id ");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            DoctorName = model.DoctorName,
                                            BMDC_No = model.BMDC_No,
                                            Email = model.Email,
                                            Gender = model.Gender,
                                            DOB = model.DOB,
                                            Hospital_Id = model.Hospital_Id,
                                            Department_Id = model.Department_Id,
                                            Designation_Id = model.Designation_Id,
                                            Category_Id = model.Category_Id,
                                            Degree = model.Degree,
                                            Country = model.Country,
                                            City = model.City,
                                            Full_Address = model.Full_Address,
                                            Immergency_Contact = model.Immergency_Contact,
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
        public async Task<DataBaseResponse> DeleteDoctorInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE Doctor SET Status=@Status WHERE Id=@Id");
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
        public bool GetDuplicateDoctor(string name, int id) 
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,DoctorName,Status from Doctor where Status=1  ";
                if (!string.IsNullOrEmpty(name))
                    sql += @" and REPLACE(LOWER(DoctorName), ' ', '') =REPLACE('" + name + @"', ' ', '') ";
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

        #region DuplicateDoctorInfo
        public bool GetDuplicateDoctorInfo(string Phone, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Phone,Status from [User] where Status=1  ";
                if (!string.IsNullOrEmpty(Phone))
                    sql += @" and Phone='"+ Phone + "' ";
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


        #region Change Doctor Status
        public async Task<DataBaseResponse> UpdateDoctorStatusInfo(Doctor model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctor(model.DoctorName, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  Doctor SET ");
                    strSql.AppendLine(" Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id ");
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

        #region Change Doctor Phone
        public async Task<DataBaseResponse> UpdateDoctorPhoneInfo(string phone, int userid)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorInfo( phone, userid);
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


        #region Change Doctor Password
        public async Task<DataBaseResponse> UpdateDoctorPasswordInfo(string password, int userid)
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
