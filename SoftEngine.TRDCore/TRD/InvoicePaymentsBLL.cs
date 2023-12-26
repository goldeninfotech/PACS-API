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
    public class InvoicePaymentsBLL : IInvoicePayments
    {
        private readonly ConnectionStrings _dbSettings;
        public InvoicePaymentsBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region Invoice Payments
        public IEnumerable<Invoicepayments> GetInvoicepaymentsList(string Name)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,Inv_Id,Payment_Mode,Amount,Status from Invoicepayments where Status=1 ";
                if (!string.IsNullOrEmpty(Name))
                    sql += " and Name= '" + Name + "' ";
                var models = connection.Query<Invoicepayments>(sql).ToList();
                return models;
            }
        }
        public Invoicepayments GetInvoicepaymentsById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select Id,Inv_Id,Payment_Mode,Amount,Status from Invoicepayments where Status=1 ");
                strSql.AppendLine(" and Id=@Id ");
                var models = connection.Query<Invoicepayments>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveInvoicepaymentsInfo(Invoicepayments model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  Invoicepayments ");
                strSql.AppendLine(" ( Inv_Id,Payment_Mode,Amount,Status,AddedBy,AddedDate) VALUES ");
                strSql.AppendLine(" ( @Inv_Id,@Payment_Mode,@Amount,@Status,@AddedBy,@AddedDate);");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Inv_Id = model.Inv_Id,
                                        Payment_Mode = model.Payment_Mode,
                                        Amount = model.Amount,
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

        public async Task<DataBaseResponse> UpdateInvoicepaymentsInfo(Invoicepayments model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  Invoicepayments SET ");
                strSql.AppendLine(" Inv_Id=@Inv_Id,Payment_Mode=@Payment_Mode,Amount=@Amount,Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                strSql.AppendLine(" Where Id=@Id;");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
                                        Inv_Id = model.Inv_Id,
                                        Payment_Mode = model.Payment_Mode,
                                        Amount = model.Amount,
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
        public async Task<DataBaseResponse> DeleteInvoicepaymentsInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE Invoicepayments SET Status=@Status WHERE Id=@Id");
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
