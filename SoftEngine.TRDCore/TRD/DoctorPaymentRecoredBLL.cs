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
    public class DoctorPaymentRecoredBLL : IDoctorPaymentRecored
    {
        private readonly ConnectionStrings _dbSettings;
        public DoctorPaymentRecoredBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region DoctorPaymentRecored CRUD
        public IEnumerable<DoctorPaymentRecored> GetDoctorPaymentRecoredList(string Search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select dp.Id,dp.Doctor_Id,dp.PaidAmount,dp.PaymentNotes,dp.PaymentsMathodId,dp.PaidBy,dp.PaidDate,dp.Status,d.DoctorName
                from DoctorPaymentRecored dp 
                left join Doctor d on d.Id=dp.Doctor_Id where dp.Status=1 ";
                if (!string.IsNullOrEmpty(Search))
                    sql += " and d.DoctorName= '" + Search + "' ";
                var models = connection.Query<DoctorPaymentRecored>(sql).ToList();
                return models;
            }
        }
        public DoctorPaymentRecored GetDoctorPaymentRecoredById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(@" Select dp.Id,dp.Doctor_Id,dp.PaidAmount,dp.PaymentNotes,dp.PaymentsMathodId,dp.PaidBy,dp.PaidDate,dp.Status,d.DoctorName
                from DoctorPaymentRecored dp 
                left join Doctor d on d.Id=dp.Doctor_Id where dp.Status=1 ");
                strSql.AppendLine(" and dp.Id=@Id ");
                var models = connection.Query<DoctorPaymentRecored>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }
        public async Task<DataBaseResponse> SaveDoctorPaymentRecoredInfo(DoctorPaymentRecored model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  DoctorPaymentRecored ");
                strSql.AppendLine(" ( Doctor_Id,PaidAmount,PaymentNotes,PaymentsMathodId,PaidBy,PaidDate,Status,AddedBy,AddedDate) VALUES ");
                strSql.AppendLine(" ( @Doctor_Id,@PaidAmount,@PaymentNotes,@PaymentsMathodId,@PaidBy,@PaidDate,@Status,@AddedBy,@AddedDate) ");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Doctor_Id = model.Doctor_Id,
                                        PaidAmount = model.PaidAmount,
                                        PaymentNotes = model.PaymentNotes,
                                        PaymentsMathodId = model.PaymentsMathodId,
                                        PaidBy = model.PaidBy,
                                        PaidDate = model.PaidDate,
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

        public async Task<DataBaseResponse> UpdateDoctorPaymentRecoredInfo(DoctorPaymentRecored model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  DoctorPaymentRecored SET ");
                strSql.AppendLine(" Doctor_Id=@Doctor_Id,PaidAmount=@PaidAmount,PaymentNotes=@PaymentNotes,PaymentsMathodId=@PaymentsMathodId,PaidBy=@PaidBy,PaidDate=@PaidDate,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                strSql.AppendLine(" Where Id=@Id;");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
                                        Doctor_Id = model.Doctor_Id,
                                        PaidAmount = model.PaidAmount,
                                        PaymentNotes = model.PaymentNotes,
                                        PaymentsMathodId = model.PaymentsMathodId,
                                        PaidBy = model.PaidBy,
                                        PaidDate = model.PaidDate,
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
        public async Task<DataBaseResponse> DeleteDoctorPaymentRecoredInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE DoctorPaymentRecored SET Status=@Status WHERE Id=@Id");
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
