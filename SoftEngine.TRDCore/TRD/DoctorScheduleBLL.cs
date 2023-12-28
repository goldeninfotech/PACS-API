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
    public class DoctorScheduleBLL : IDoctorSchedule
    {
        private readonly ConnectionStrings _dbSettings;
        public DoctorScheduleBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings; 
        }

        #region DoctorSchedule CRUD
        public IEnumerable<DoctorSchedule> GetDoctorScheduleList(string Search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,Doctor_Id,ScheduleDate,ScheduleTime,Status from DoctorSchedule where Status=1";
                if (!string.IsNullOrEmpty(Search))
                    sql += " and Doctor_Id= '" + Search + "' ";
                var models = connection.Query<DoctorSchedule>(sql).ToList();
                return models;
            }
        }
        public DoctorSchedule GetDoctorScheduleById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,Doctor_Id,ScheduleDate,ScheduleTime,Status from DoctorSchedule where Status=1 ");
                strSql.AppendLine(" and Id=@Id ");
                var models = connection.Query<DoctorSchedule>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveDoctorScheduleInfo(DoctorSchedule model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDepartment(model.Doctor_Id.ToString(), model.ScheduleDate.ToString(), 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  DoctorSchedule");
                    strSql.AppendLine(" ( Doctor_Id,ScheduleDate,ScheduleTime,Status,AddedBy,AddedDate) VALUES ");
                    strSql.AppendLine(" ( @Doctor_Id,@ScheduleDate,@ScheduleTime,@Status,@AddedBy,@AddedDate);");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Doctor_Id = model.Doctor_Id,
                                            ScheduleDate = model.ScheduleDate,
                                            ScheduleTime = model.ScheduleTime,
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

        public async Task<DataBaseResponse> UpdateDoctorScheduleInfo(DoctorSchedule model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDepartment(model.Doctor_Id.ToString(), model.ScheduleDate.ToString(), model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  DoctorSchedule SET ");
                    strSql.AppendLine(" Doctor_Id=@Doctor_Id,ScheduleDate=@ScheduleDate,ScheduleTime=@ScheduleTime, Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            Doctor_Id = model.Doctor_Id,
                                            ScheduleDate = model.ScheduleDate,
                                            ScheduleTime = model.ScheduleTime,
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
        public async Task<DataBaseResponse> DeleteDoctorScheduleInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE DoctorSchedule SET Status=@Status WHERE Id=@Id");
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

        public bool GetDuplicateDepartment(string Doctor_Id, string ScheduleDate, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Doctor_Id,ScheduleDate,Status from DoctorSchedule where Status=1  ";
                if (!string.IsNullOrEmpty(Doctor_Id))
                    sql += @" and Doctor_Id =" + Doctor_Id + @" ";
                if (!string.IsNullOrEmpty(ScheduleDate))
                    sql += @" and ScheduleDate =" + ScheduleDate + @" ";
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
