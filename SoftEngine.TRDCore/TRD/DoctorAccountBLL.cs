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
    public class DoctorAccountBLL : IDoctorAccount
    {
        private readonly ConnectionStrings _dbSettings;

        public DoctorAccountBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public IEnumerable<DoctorAccount> GetDoctorAccountList(string Search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"  Select da.Id,da.Doctor_Id,da.TotalAmount,da.TotalWithDraw,da.Status,d.DoctorName from DoctorAccount da
                left join Doctor d on d.Id=da.Doctor_Id where da.Status=1";
                if (!string.IsNullOrEmpty(Search))
                    sql += " and d.DoctorName= '" + Search + "' ";
                var models = connection.Query<DoctorAccount>(sql).ToList();
                return models;
            }
        }
        public DoctorAccount GetDoctorAccountById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select da.Id,da.Doctor_Id,da.TotalAmount,da.TotalWithDraw,da.Status,d.DoctorName from DoctorAccount da where da.Status=1 ");
                strSql.AppendLine(" and da.Id=@Id ");
                var models = connection.Query<DoctorAccount>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveDoctorAccountInfo(DoctorAccount model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorAccount(model.Doctor_Id, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  DoctorAccount");
                    strSql.AppendLine(" ( Doctor_Id,TotalAmount,TotalWithDraw,Status,AddedBy,AddedDate) VALUES ");
                    strSql.AppendLine(" ( @Doctor_Id,@TotalAmount,@TotalWithDraw,@Status,@AddedBy,@AddedDate);");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Doctor_Id = model.Doctor_Id,
                                            TotalAmount = model.TotalAmount,
                                            TotalWithDraw = model.TotalWithDraw,
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

        public async Task<DataBaseResponse> UpdateDoctorAccountInfo(DoctorAccount model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorAccount(model.Doctor_Id, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  DoctorAccount SET ");
                    strSql.AppendLine(" Doctor_Id=@Doctor_Id,TotalAmount=@TotalAmount,TotalWithDraw=@TotalWithDraw,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            Doctor_Id = model.Doctor_Id,
                                            TotalAmount = model.TotalAmount,
                                            TotalWithDraw = model.TotalWithDraw,
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
        public async Task<DataBaseResponse> DeleteDoctorAccountInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE DoctorAccount SET Status=@Status WHERE Id=@Id");
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

        public bool GetDuplicateDoctorAccount(int Doctor_Id, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Doctor_Id,Status from DoctorAccount where Status=1  ";
                if (Doctor_Id>0)
                    sql += @" and Doctor_Id="+ Doctor_Id + " ";
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
