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
    public class DoctorAssignBLL : IDoctorAssign
    {
        private readonly ConnectionStrings _dbSettings;
        public DoctorAssignBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region DoctorAssign CRUD

        public IEnumerable<DoctorAssign> GetDoctorAssignList(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select da.Id,da.Doctor_Id,da.ModalityType_Id,da.StudyInstance_Id,da.SeriesInstance_id,da.Status,d.DoctorName  
                from DoctorAssign da left join Doctor d on d.Id=da.Doctor_Id where da.Status=1";

                if (!string.IsNullOrEmpty(search))
                    sql += " and ( d.DoctorName= '" + search + "' ) ";
                var models = connection.Query<DoctorAssign>(sql).ToList();
                return models;
            }
        }
        public DoctorAssign GetDoctorAssignById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select da.Id,da.Doctor_Id,da.ModalityType_Id,da.StudyInstance_Id,da.SeriesInstance_id,da.Status,d.DoctorName ");
                strSql.AppendLine(" from DoctorAssign da left join Doctor d on d.Id=da.Doctor_Id where da.Status=1 ");
                strSql.AppendLine(" and da.Id=@Id ");
                var models = connection.Query<DoctorAssign>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveDoctorAssignInfo(DoctorAssign model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorAssign(model.ModalityType_Id, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  DoctorAssign");
                    strSql.AppendLine(" ( Doctor_Id,ModalityType_Id,StudyInstance_Id,SeriesInstance_id,Status,AddedBy,AddedDate)  ");
                    strSql.AppendLine(" VALUES ( @Doctor_Id,@ModalityType_Id,@StudyInstance_Id,@SeriesInstance_id,@Status,@AddedBy,@AddedDate) ");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Doctor_Id = model.Doctor_Id,
                                            ModalityType_Id = model.ModalityType_Id,
                                            StudyInstance_Id = model.StudyInstance_Id,
                                            SeriesInstance_id = model.SeriesInstance_id, 
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

        public async Task<DataBaseResponse> UpdateDoctorAssignInfo(DoctorAssign model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorAssign(model.ModalityType_Id, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  DoctorAssign SET ");
                    strSql.AppendLine(" Doctor_Id=@Doctor_Id,ModalityType_Id=@ModalityType_Id,StudyInstance_Id=@StudyInstance_Id,SeriesInstance_id=@SeriesInstance_id, Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            Doctor_Id = model.Doctor_Id,
                                            ModalityType_Id = model.ModalityType_Id,
                                            StudyInstance_Id = model.StudyInstance_Id,
                                            SeriesInstance_id = model.SeriesInstance_id,
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
        public async Task<DataBaseResponse> DeleteDoctorAssignInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
               // strSql.AppendLine(" UPDATE DeviceType SET Status=@Status WHERE Id=@Id");
                strSql.AppendLine(" Delete from DoctorAssign where Status=@Status and Id=@Id ");
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
        public bool GetDuplicateDoctorAssign(string name, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,ModalityType_Id,Status from DoctorAssign where Status=1  ";
                if (!string.IsNullOrEmpty(name))
                    sql += @" and REPLACE(LOWER(ModalityType_Id), ' ', '') =REPLACE('" + name + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<DeviceType>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }
}
