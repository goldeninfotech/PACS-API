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
using System.Xml.Linq;

namespace SoftEngine.TRDCore.TRD
{
    public class BillingSettingsBLL : IBillingSettings
    {
        private readonly ConnectionStrings _dbSettings;
        public BillingSettingsBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region Billing Settings BLL 
        public IEnumerable<BillingSettingsViewModels> GetBillingSettingsList(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"  Select b.Id,b.BillNumber,b.BillFor,b.Hospital_Id,b.Doctor_Id,b.Payment_Mode,b.Active_Payment_Date,b.Status , h.Name HospitalName, d.DoctorName
                from BillingSettings b
                Left join Hospital h on h.Id=b.Hospital_Id
                left join Doctor d on d.Id=b.Doctor_Id where b.Status=1 ";
                if (!string.IsNullOrEmpty(search))
                    sql += " and (b.BillNumber= '" + search + "' or h.Name= '" + search + "' or d.DoctorName= '" + search + "' ) ";
                var models = connection.Query<BillingSettingsViewModels>(sql).ToList();
                return models;
            }
        }
        public BillingSettingsViewModels GetBillingSettingsById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(@" Select b.Id,b.BillNumber,b.BillFor,b.Hospital_Id,b.Doctor_Id,b.Payment_Mode,b.Active_Payment_Date,b.Status , h.Name HospitalName, d.DoctorName
                from BillingSettings b
                Left join Hospital h on h.Id=b.Hospital_Id
                left join Doctor d on d.Id=b.Doctor_Id where b.Status=1 ");
                strSql.AppendLine(@" and b.Id=@Id ");
                var models = connection.Query<BillingSettingsViewModels>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveBillingSettingsInfo(BillingSettings model, List<BillSettingsDetails> dmodel)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateBillingSettings(model.Hospital_Id.ToString(), model.Doctor_Id.ToString(), 0);
                if (duplicateresult)
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.AppendLine(" INSERT INTO  BillingSettings");
                        strSql.AppendLine(" ( BillNumber,BillFor,Hospital_Id,Doctor_Id,Payment_Mode,Active_Payment_Date,Status,AddedDate,AddedBy) Output Inserted.Id VALUES ");
                        strSql.AppendLine(" ( @BillNumber,@BillFor,@Hospital_Id,@Doctor_Id,@Payment_Mode,@Active_Payment_Date,@Status,@AddedDate,@AddedBy)");

                        int Saveresult = connection.ExecuteScalar<int>(strSql.ToString(), new
                        {
                            BillNumber = model.BillNumber,
                            BillFor = model.BillFor,
                            Hospital_Id = model.Hospital_Id,
                            Doctor_Id = model.Doctor_Id,
                            Payment_Mode = model.Payment_Mode,
                            Active_Payment_Date = model.Active_Payment_Date,
                            Status = model.Status,
                            AddedDate = model.AddedDate,
                            AddedBy = model.AddedBy,
                        }, transaction);

                        // Details Insert 
                        strSql = new StringBuilder();
                        strSql.Append(" Insert into BillSettingsDetails ");
                        strSql.Append(" ( Bill_Id,DeviceTypeId,NumOfDevice,UnitPrice,RR_CommonAmount,Status,AddedBy,AddedDate ) ");
                        strSql.Append(" values ( @Bill_Id,@DeviceTypeId,@NumOfDevice,@UnitPrice,@RR_CommonAmount,@Status,@AddedBy,@AddedDate) ");
                        foreach (var item in dmodel)
                        {
                            var resul2t = await connection.ExecuteAsync(strSql.ToString(), new
                            {
                                Bill_Id = Saveresult,
                                DeviceTypeId = item.DeviceTypeId,
                                NumOfDevice = item.NumOfDevice,
                                UnitPrice = item.UnitPrice,
                                RR_CommonAmount = item.RR_CommonAmount,
                                Status = model.Status,
                                AddedBy = model.AddedBy,
                                AddedDate = model.AddedDate,
                            }, transaction);
                        }
                        transaction.Commit();

                        response.ReturnValue = Saveresult;
                        response.Message = GlobalConst.INSERT_SUCCESS_MESSAGE;
                        response.IsSuccess = true;
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

        public async Task<DataBaseResponse> UpdateBillingSettingsInfo(BillingSettings model, List<BillSettingsDetails> dmodel)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateBillingSettings(model.Hospital_Id.ToString(), model.Doctor_Id.ToString(), model.Id);
                if (duplicateresult)
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.AppendLine(" UPDATE  BillingSettings SET ");
                        strSql.AppendLine(" BillNumber=@BillNumber,BillFor=@BillFor,Hospital_Id=@Hospital_Id,Doctor_Id=@Doctor_Id,Payment_Mode=@Payment_Mode,Active_Payment_Date=@Active_Payment_Date ,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                        strSql.AppendLine(" Where Id=@Id");
                        int Saveresult = connection.ExecuteScalar<int>(strSql.ToString(), new
                        {
                            Id = model.Id,
                            BillNumber = model.BillNumber,
                            BillFor = model.BillFor,
                            Hospital_Id = model.Hospital_Id,
                            Doctor_Id = model.Doctor_Id,
                            Payment_Mode = model.Payment_Mode,
                            Active_Payment_Date = model.Active_Payment_Date,
                            Status = model.Status,
                            UpdatedBy = model.UpdatedBy,
                            UpdatedDate = model.UpdatedDate

                        }, transaction);

                        strSql = new StringBuilder();
                        strSql.Append(" Insert into BillSettingsDetails ");
                        strSql.Append(" ( Bill_Id,DeviceTypeId,NumOfDevice,UnitPrice,RR_CommonAmount,Status,AddedBy,AddedDate ) ");
                        strSql.Append(" values ( @Bill_Id,@DeviceTypeId,@NumOfDevice,@UnitPrice,@RR_CommonAmount,@Status,@AddedBy,@AddedDate) ");

                        StringBuilder strSql2 = new StringBuilder();
                        strSql2.Append(" Update BillSettingsDetails Set  ");
                        strSql2.Append(" Bill_Id=@Bill_Id,DeviceTypeId=@DeviceTypeId,NumOfDevice=@NumOfDevice,UnitPrice=@UnitPrice,RR_CommonAmount=@RR_CommonAmount,Status=@Status,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                        strSql2.Append(" where Id=@Id ");
                       
                        foreach (var item in dmodel)
                        {
                            if (item.Id > 0)
                            {
                                var resul2t = await connection.ExecuteAsync(strSql2.ToString(), new
                                {
                                    Id = item.Id,
                                    Bill_Id = model.Id,
                                    DeviceTypeId = item.DeviceTypeId,
                                    NumOfDevice = item.NumOfDevice,
                                    UnitPrice = item.UnitPrice,
                                    RR_CommonAmount = item.RR_CommonAmount,
                                    Status = model.Status,
                                    UpdatedBy = model.UpdatedBy,
                                    UpdatedDate = model.UpdatedDate
                                }, transaction);
                            }
                            else
                            {
                                var resul2t = await connection.ExecuteAsync(strSql.ToString(), new
                                {
                                    Bill_Id = Saveresult,
                                    DeviceTypeId = item.DeviceTypeId,
                                    NumOfDevice = item.NumOfDevice,
                                    UnitPrice = item.UnitPrice,
                                    RR_CommonAmount = item.RR_CommonAmount,
                                    Status = item.Status,
                                    AddedBy = item.AddedBy,
                                    AddedDate = item.AddedDate,
                                }, transaction);
                            }
                        }
                        transaction.Commit();

                        response.ReturnValue = Saveresult;
                        response.Message = GlobalConst.UPDATE_SUCCESS_MESSAGE;
                        response.IsSuccess = true;

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
        public async Task<DataBaseResponse> DeleteBillingSettingsInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE BillingSettings SET Status=@Status WHERE Id=@Id");

                StringBuilder strSql2 = new StringBuilder();
                strSql2.AppendLine(" UPDATE BillSettingsDetails SET Status=@Status WHERE Bill_Id=@Id");
                try
                {
                    var result = connection.Execute(strSql.ToString(), new { Status = 0, Id = id, });
                    var result2 = connection.Execute(strSql2.ToString(), new { Status = 0, Id = id, });
                    
                    
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

        public bool GetDuplicateBillingSettings(string Hospital_Id, string Doctor_Id, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,BillNumber,BillFor,Hospital_Id,Doctor_Id,Status from BillingSettings where Status=1  ";
                if (!string.IsNullOrEmpty(Hospital_Id))
                    sql += @" and Hospital_Id="+ Hospital_Id + " ";
                if (!string.IsNullOrEmpty(Hospital_Id))
                    sql += @" and  Doctor_Id=" + Doctor_Id + " ";
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


        #region Get BillingSettings Details
        public IEnumerable<BillingSettingsDetailsViewModels> GetBillingSettingsDetailsList()
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select bd.Id,bd.Bill_Id,bd.DeviceTypeId,bd.NumOfDevice,bd.UnitPrice,bd.RR_CommonAmount,bd.Status,dt.DeviceName  
                from BillSettingsDetails bd
                left join DeviceType dt on dt.Id=bd.DeviceTypeId  where bd.Status=1 ";
                var models = connection.Query<BillingSettingsDetailsViewModels>(sql).ToList();
                return models;
            }
        }
        #endregion

    }
}
