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
    public class PermissionBLL : IPermission
    {
        private readonly ConnectionStrings _dbSettings;
        public PermissionBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }
         
        #region Permission
        public IEnumerable<Permission> GetPermissionList(string Search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select p.Id,p.User_Id,p.Role_id,p.Route,p.Status,u.Full_Name, r.Name RoleName from Permission p
                left join [User] u on u.Id=p.User_Id 
                left join Roles r on r.Id=p.Role_id  where p.Status=1";
                if (!string.IsNullOrEmpty(Search))
                    sql += " and ( u.Full_Name= '" + Search + "'  or r.Name='"+Search+"' )";
                var models = connection.Query<Permission>(sql).ToList();
                return models;
            }
        }
        public Permission GetPermissionById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(@"  Select p.Id,p.User_Id,p.Role_id,p.Route,p.Status,u.Full_Name, r.Name RoleName from Permission p
                left join [User] u on u.Id=p.User_Id 
                left join Roles r on r.Id=p.Role_id  where p.Status=1 ");
                strSql.AppendLine(" and p.Id=@Id ");
                var models = connection.Query<Permission>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }
        public async Task<DataBaseResponse> SavePermissionInfo(Permission model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  Permission");
                strSql.AppendLine(" ( User_Id,Role_id,Route,Status,AddedBy,AddedDate) VALUES ");
                strSql.AppendLine(" ( @User_Id,@Role_id,@Route,@Status,@AddedBy,@AddedDate) ");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        User_Id = model.User_Id,
                                        Role_id = model.Role_id,
                                        Route = model.Route,
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

        public async Task<DataBaseResponse> UpdatePermissionInfo(Permission model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  Permission SET ");
                strSql.AppendLine(" User_Id=@User_Id,Role_id=@Role_id,Route=@Route,Status=@Status,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                strSql.AppendLine(" Where Id=@Id;");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
                                        User_Id = model.User_Id,
                                        Role_id = model.Role_id,
                                        Route = model.Route,
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
        public async Task<DataBaseResponse> DeletePermissionInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE Departments SET Status=@Status WHERE Id=@Id");
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
