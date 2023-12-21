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
using System.Xml.Linq;

namespace SoftEngine.TRDCore.TRD
{
    public class SettingsBLL : ISettings
    {

        private readonly ConnectionStrings _dbSettings;
        public SettingsBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region Settings CRUD

        public IEnumerable<Setting> GetSettingList(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,Collum,Value,Status from Setting where Status=1";
                if (!string.IsNullOrEmpty(search))
                    sql += " and Name= '" + search + "' ";
                var models = connection.Query<Setting>(sql).ToList();
                return models;
            }
        }
        public Setting GetSettingById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,Collum,Value,Status from Setting where Status=1 ");
                strSql.AppendLine(" and Id=@Id ");
                var models = connection.Query<Setting>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveSettingInfo(Setting model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateSettings(model.Collum,model.Value, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  Setting");
                    strSql.AppendLine(" ( Collum,Value,Status,AddedBy,AddedDate) VALUES ");
                    strSql.AppendLine(" ( @Collum,@Value,@Status,@AddedBy,@AddedDate);");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Collum = model.Collum,
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
                else
                {
                    response.ReturnValue = -1;
                    response.Message = GlobalConst.GET_DUPLICATEDATA;
                    response.IsSuccess = false;
                }
            }
            return response;
        }
        public async Task<DataBaseResponse> UpdateSettingInfo(Setting model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateSettings(model.Collum, model.Value, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  Setting SET ");
                    strSql.AppendLine(" Collum=@Collum,Value=@Value,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            Collum = model.Collum,
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
                else
                {
                    response.ReturnValue = -1;
                    response.Message = GlobalConst.GET_DUPLICATEDATA;
                    response.IsSuccess = false;
                }
            }
            return response;
        }
        public async Task<DataBaseResponse> DeleteSettingInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE Setting SET Status=@Status WHERE Id=@Id ");
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
        public bool GetDuplicateSettings(string Collum, string Value, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Collum,Value,Status from Setting where Status=1  ";
                if (!string.IsNullOrEmpty(Collum))
                    sql += @" and Collum='" + Collum + "' ";
                if (!string.IsNullOrEmpty(Value))
                    sql += @" and Collum='" + Value + "' ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<Setting>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion
    }
}
