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
    public class HospitalDeviceBLL : IHospitalDevice
    {
        private readonly ConnectionStrings _dbSettings;
        public HospitalDeviceBLL(ConnectionStrings dbSettings) 
        {
            _dbSettings = dbSettings;
        }

        #region Hospital Device CRUD
        public IEnumerable<HospitalDevices> GetHospitalDevicesList(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select hd.Id,hd.Hospital_Id,hd.Name,hd.Description,hd.DeviceTypeId,hd.Status, h.Name HospitalName
                from HospitalDevices hd left join Hospital h on h.id=hd.Hospital_Id where hd.Status=1";
             
                if (!string.IsNullOrEmpty(search))
                    sql += " and ( hd.Name= '" + search + "' or h.Name= '" + search + "' ) ";
                var models = connection.Query<HospitalDevices>(sql).ToList();
                return models;
            }
        }

        public HospitalDevices GetHospitalDevicesById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" select hd.Id,hd.Hospital_Id,hd.Name,hd.Description,hd.DeviceTypeId,hd.Status, h.Name HospitalName from HospitalDevices hd left join Hospital h on h.id=hd.Hospital_Id where hd.Status=1 ");
                strSql.AppendLine(" and hd.Id=@Id ");
                var models = connection.Query<HospitalDevices>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveHospitalDevicesInfo(HospitalDevices model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateHospitalDevices(model.Name, 0, model.Hospital_Id,model.DeviceTypeId);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  HospitalDevices");
                    strSql.AppendLine(" ( Hospital_Id,Name,Description,DeviceTypeId,Status,AddedBy,AddedDate) VALUES ");
                    strSql.AppendLine(" ( @Hospital_Id,@Name,@Description,@DeviceTypeId,@Status,@AddedBy,@AddedDate);");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Hospital_Id = model.Hospital_Id,
                                            Name = model.Name,
                                            Description = model.Description,
                                            DeviceTypeId = model.DeviceTypeId,
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

        public async Task<DataBaseResponse> UpdateHospitalDevicesInfo(HospitalDevices model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateHospitalDevices(model.Name, model.Id,model.Hospital_Id,model.DeviceTypeId);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  HospitalDevices SET ");
                    strSql.AppendLine(" Hospital_Id=@Hospital_Id, Name=@Name,Description=@Description,DeviceTypeId=@DeviceTypeId,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            Hospital_Id = model.Hospital_Id,
                                            Name = model.Name,
                                            Description = model.Description,
                                            DeviceTypeId = model.DeviceTypeId,
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
        public async Task<DataBaseResponse> DeleteHospitalDevicesInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE HospitalDevices SET Status=@Status WHERE Id=@Id");
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

        public bool GetDuplicateHospitalDevices(string name, int id, int hospitalId, int DeviceTypeId)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Name,Status from HospitalDevices where Status=1  ";
                if (!string.IsNullOrEmpty(name))
                    sql += @" and REPLACE(LOWER(Name), ' ', '') =REPLACE('" + name + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";
                if (hospitalId > 0)
                    sql += @" and Hospital_Id!=" + hospitalId + " ";
                if (DeviceTypeId > 0)
                    sql += @" and DeviceTypeId!=" + DeviceTypeId + " ";

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
