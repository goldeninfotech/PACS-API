﻿using Dapper;
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
    public class DoctorCategoryBLL : IDoctorCategory
    {
        private readonly ConnectionStrings _dbSettings;
        public DoctorCategoryBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings; 
        }
        public IEnumerable<DoctorCategory> GetDoctorCategoryList(string Name)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,Name,Status from DoctorCategory where Status=1";
                if (!string.IsNullOrEmpty(Name))
                    sql += " and Name= '"+Name+"' ";
                var models = connection.Query<DoctorCategory>(sql).ToList();
                return models;
            }
        }
        public DoctorCategory GetDoctorCategoryById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,Name,Status from DoctorCategory where Status=1 ");
                strSql.AppendLine(" and Id=@Id ");
                var models = connection.Query<DoctorCategory>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveDoctorCategoryInfo(DoctorCategory model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorCategory(model.Name, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  DoctorCategory");
                    strSql.AppendLine(" ( Name,Status,AddedBy,AddedDate) VALUES ");
                    strSql.AppendLine(" ( @Name,@Status,@AddedBy,@AddedDate);");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Name = model.Name,
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

        public async Task<DataBaseResponse> UpdateDoctorCategoryInfo(DoctorCategory model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorCategory(model.Name, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  DoctorCategory SET ");
                    strSql.AppendLine(" Name=@Name,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            Name = model.Name,
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
        public async Task<DataBaseResponse> DeleteDoctorCategoryInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE DoctorCategory SET Status=@Status WHERE Id=@Id");
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

        public bool GetDuplicateDoctorCategory(string name, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Name,Status from DoctorCategory where Status=1  ";
                if (!string.IsNullOrEmpty(name))
                    sql += @" and REPLACE(LOWER(Name), ' ', '') =REPLACE('" + name + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<Departments>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
