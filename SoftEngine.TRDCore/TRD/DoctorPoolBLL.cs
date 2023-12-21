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
    public class DoctorPoolBLL : IDoctorPool
    {
        private readonly ConnectionStrings _dbSettings;
        public DoctorPoolBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region Doctor Pool CRUD 
        public IEnumerable<DoctorPoolViewModel> GetDoctorPoolList(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select dp.Id,dp.Doctor_Id,dp.Hospital_Id,dp.Status, d.DoctorName,h.Name HospitalName from DoctorPool dp 
                left join Doctor d on d.Id=dp.Doctor_Id
                left join Hospital h on h.Id=dp.Hospital_Id
                where dp.Status=1 ";
                if (!string.IsNullOrEmpty(search))
                    sql += " and Name= '" + search + "' ";
                var models = connection.Query<DoctorPoolViewModel>(sql).ToList();
                return models;
            }
        } 
        public DoctorPoolViewModel GetDoctorPoolById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select dp.Id,dp.Doctor_Id,dp.Hospital_Id,dp.Status, d.DoctorName,h.Name HospitalName from DoctorPool dp  ");
                strSql.AppendLine(" left join Doctor d on d.Id=dp.Doctor_Id ");
                strSql.AppendLine(" left join Hospital h on h.Id=dp.Hospital_Id where dp.Status=1 ");
                strSql.AppendLine(" and dp.Id=@Id ");
                var models = connection.Query<DoctorPoolViewModel>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveDoctorPoolInfo(DoctorPool model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorPool(model.Doctor_Id,model.Hospital_Id, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  DoctorPool");
                    strSql.AppendLine(" ( Doctor_Id,Hospital_Id,Status,AddedBy,AddedDate) VALUES ");
                    strSql.AppendLine(" ( @Doctor_Id,@Hospital_Id,@Status,@AddedBy,@AddedDate) ");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Doctor_Id = model.Doctor_Id,
                                            Hospital_Id = model.Hospital_Id,
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

        public async Task<DataBaseResponse> UpdateDoctorPoolInfo(DoctorPool model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateDoctorPool(model.Doctor_Id,model.Hospital_Id, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  DoctorPool SET ");
                    strSql.AppendLine(" Doctor_Id=@Doctor_Id,Hospital_Id=@Hospital_Id,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            Doctor_Id = model.Doctor_Id,
                                            Hospital_Id = model.Hospital_Id,
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

        public async Task<DataBaseResponse> DeleteDoctorPoolInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE DoctorPool SET Status=@Status WHERE Id=@Id");
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

        public bool GetDuplicateDoctorPool(int Doctor_Id, int Hospital_Id, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Doctor_Id,Hospital_Id,Status from DoctorPool where Status=1  ";
                if (Hospital_Id>0)
                    sql += @" and Hospital_Id = "+ Hospital_Id + " ";
                if (Doctor_Id > 0)
                    sql += @" and Doctor_Id = " + Doctor_Id + " ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<DoctorPool>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion
    }
}
