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
    public class InvoiceBLL : IInvoice
    {
        private readonly ConnectionStrings _dbSettings;
        public InvoiceBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }
        #region Invoice CRUD
        public IEnumerable<Invoice> GetInvoiceList(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select i.Id,i.Date,i.InvoiceDate,i.InvoiceDueDate,i.BillSetting_Id,i.InvoiceAmount,i.DiscountAmount,i.NetAmount,i.Status, b.BillNumber 
                from Invoice i left join BillingSettings b on b.Id=i.BillSetting_Id where i.Status=1 ";
                if (!string.IsNullOrEmpty(search))
                    sql += " and ( b.BillNumber = '" + search + "' or  i.InvoiceDate = '" + search + "') ";
                var models = connection.Query<Invoice>(sql).ToList();
                return models;
            }
        }
        public Invoice GetInvoiceById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(@" Select i.Id,i.Date,i.InvoiceDate,i.InvoiceDueDate,i.BillSetting_Id,i.InvoiceAmount,i.DiscountAmount,i.NetAmount,i.Status, b.BillNumber 
                from Invoice i left join BillingSettings b on b.Id=i.BillSetting_Id where i.Status=1 ");
                strSql.AppendLine(" and i.Id=@Id ");
                var models = connection.Query<Invoice>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models; 
            }
        }
        public async Task<DataBaseResponse> SaveInvoiceInfo(Invoice model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  Invoice");
                strSql.AppendLine(" ( Date,InvoiceDate,InvoiceDueDate,BillSetting_Id,InvoiceAmount,DiscountAmount,NetAmount,Status,AddedBy,AddedDate) VALUES ");
                strSql.AppendLine(" ( @Date,@InvoiceDate,@InvoiceDueDate,@BillSetting_Id,@InvoiceAmount,@DiscountAmount,@NetAmount,@Status,@AddedBy,@AddedDate);");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Date = model.Date,
                                        InvoiceDate = model.InvoiceDate,
                                        InvoiceDueDate = model.InvoiceDueDate,
                                        BillSetting_Id = model.BillSetting_Id,
                                        InvoiceAmount = model.InvoiceAmount,
                                        DiscountAmount = model.DiscountAmount,
                                        NetAmount = model.NetAmount,
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

        public async Task<DataBaseResponse> UpdateInvoiceInfo(Invoice model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  Invoice SET ");
                strSql.AppendLine(" Date=@Date,InvoiceDate=@InvoiceDate,InvoiceDueDate=@InvoiceDueDate,BillSetting_Id=@BillSetting_Id,InvoiceAmount=@InvoiceAmount,DiscountAmount=@DiscountAmount,NetAmount=@NetAmount, Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                strSql.AppendLine(" Where Id=@Id;");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
                                        Date = model.Date,
                                        InvoiceDate = model.InvoiceDate,
                                        InvoiceDueDate = model.InvoiceDueDate,
                                        BillSetting_Id = model.BillSetting_Id,
                                        InvoiceAmount = model.InvoiceAmount,
                                        DiscountAmount = model.DiscountAmount,
                                        NetAmount = model.NetAmount,
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
        public async Task<DataBaseResponse> DeleteInvoiceInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE Invoice SET Status=@Status WHERE Id=@Id");
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

        public bool GetDuplicateInvoice(string BillSetting_Id, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,BillSetting_Id,Status from Invoice where Status=1  ";
                if (!string.IsNullOrEmpty(BillSetting_Id))
                    sql += @" and BillSetting_Id = " + BillSetting_Id + @" ";
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
