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
    public class DoctorBLL : IDoctor
    {
        private readonly ConnectionStrings _dbSettings;
        public DoctorBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region Doctor CRUD
        public IEnumerable<Doctor> GetDoctorList()
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select  Id,User_Id,DoctorName,BMDC_No,Email,Gender,DOB,Hospital_Id,Department_Id,Designation_Id,Category_Id,Degree,Country,City,Full_Address,
                Immergency_Contact,Image,Status from Doctor where Status=1";
                var models = connection.Query<Doctor>(sql).ToList();
                return models;
            }
        }
        public Doctor GetDoctorById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,User_Id,DoctorName,BMDC_No,Email,Gender,DOB,Hospital_Id,Department_Id,Designation_Id,Category_Id,Degree,Country,City,Full_Address,Immergency_Contact,Status from Doctor where Status=1 ");
                strSql.AppendLine(" and Id=@Id ");
                var models = connection.Query<Doctor>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
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

                var models = connection.Query<Departments>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion
    }
}
