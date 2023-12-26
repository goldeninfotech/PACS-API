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
    public class ReportsDescriptipnsBLL : IReportsDescriptipns
    {
        private readonly ConnectionStrings _dbSettings;
        public ReportsDescriptipnsBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }


        #region ReportsDescriptipns CRUD
       
        public IEnumerable<ReportsDescriptipns> GetReportsDescriptipnsList()
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Reports_Id,key,Value,Status from ReportsDescriptipns where Status=1 ";

                var models = connection.Query<ReportsDescriptipns>(sql).ToList();
                return models;
            }
        }
        public ReportsDescriptipns GetReportsDescriptipnsById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,Reports_Id,key,Value,Status from ReportsDescriptipns where Status=1 ");
                strSql.AppendLine(" and Id=@Id ");
                var models = connection.Query<ReportsDescriptipns>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }
        public async Task<DataBaseResponse> SaveReportsDescriptipnsInfo(ReportsDescriptipns model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  ReportsDescriptipns");
                strSql.AppendLine(" ( Reports_Id,key,Value,Status,AddedBy,AddedDate) VALUES ");
                strSql.AppendLine(" ( @Reports_Id,@key,@Value,@Status,@AddedBy,@AddedDate);");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Reports_Id = model.Reports_Id,
                                        key = model.key,
                                        Value = model.Value,
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
            return response;
        }

        public async Task<DataBaseResponse> UpdateReportsDescriptipnsInfo(ReportsDescriptipns model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  ReportsDescriptipns SET ");
                strSql.AppendLine(" Reports_Id=@Reports_Id,key=@key,Value=@Value,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                strSql.AppendLine(" Where Id=@Id;");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
                                        Reports_Id = model.Reports_Id,
                                        key = model.key,
                                        Value = model.Value,
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
        public async Task<DataBaseResponse> DeleteReportsDescriptipnsInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE ReportsDescriptipns SET Status=@Status WHERE Id=@Id");
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

        #endregion
    }
}
