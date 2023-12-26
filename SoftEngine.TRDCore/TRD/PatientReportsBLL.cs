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
    public class PatientReportsBLL : IPatientReports
    {

        private readonly ConnectionStrings _dbSettings;
        public PatientReportsBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region Patient Reports CRUD
        public IEnumerable<ReportsViewModel> GetPatientReportsList( string statusType, string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select r.Id,r.Hospital_Id,r.Patient_Id,r.PatientName,r.ModalityType_Id,r.StudyInstance_Id,r.SeriesInstance_id,r.SopInstance_Id,
                r.Doctor_Id,r.DoctorAssign_Id,r.Status, h.Name HospitalName,d.DoctorName from Reports r
                left join Hospital h on h.Id=r.Hospital_Id
                left join Doctor d on d.Id=r.Doctor_Id where 1=1 ";
                if (!string.IsNullOrEmpty(statusType))
                    sql += " and r.Status= '" + statusType + "'  ";

                if (!string.IsNullOrEmpty(search))
                    sql += " and ( r.PatientName = '" + search + "'  or h.Name='"+search+ "' or d.DoctorName='"+search+"' )";
                var models = connection.Query<ReportsViewModel>(sql).ToList();
                return models;
            }
        }
        public ReportsViewModel GetPatientReportsById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(@" Select r.Id,r.Hospital_Id,r.Patient_Id,r.PatientName,r.ModalityType_Id,r.StudyInstance_Id,r.SeriesInstance_id,r.SopInstance_Id,
                r.Doctor_Id,r.DoctorAssign_Id,r.Status, h.Name HospitalName,d.DoctorName from Reports r
                left join Hospital h on h.Id=r.Hospital_Id
                left join Doctor d on d.Id=r.Doctor_Id where 1=1");
                strSql.AppendLine(" and r.Id=@Id ");
                var models = connection.Query<ReportsViewModel>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
                return models;
            }
        }
        public async Task<DataBaseResponse> SavePatientReportsInfo(Reports model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  Reports");
                strSql.AppendLine(" ( Hospital_Id,Patient_Id,PatientName,ModalityType_Id,StudyInstance_Id,SeriesInstance_id,Doctor_Id,DoctorAssign_Id,Status,AddedBy,AddedDate) VALUES ");
                strSql.AppendLine(" ( @Hospital_Id,@Patient_Id,@PatientName,@ModalityType_Id,@StudyInstance_Id,@SeriesInstance_id,@Doctor_Id,@DoctorAssign_Id,@Status,@AddedBy,@AddedDate);");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Hospital_Id = model.Hospital_Id,
                                        Patient_Id = model.Patient_Id,
                                        PatientName = model.PatientName,
                                        ModalityType_Id = model.ModalityType_Id,
                                        StudyInstance_Id = model.StudyInstance_Id,
                                        SeriesInstance_id = model.SeriesInstance_id,
                                        Doctor_Id = model.Doctor_Id,
                                        DoctorAssign_Id = model.DoctorAssign_Id,
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

        public async Task<DataBaseResponse> UpdatePatientReportsInfo(Reports model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(@" UPDATE  Reports SET ");
                strSql.AppendLine(" Hospital_Id=@Hospital_Id,Patient_Id=@Patient_Id,PatientName=@PatientName,ModalityType_Id=@ModalityType_Id," +
                "StudyInstance_Id=@StudyInstance_Id,SeriesInstance_id=@SeriesInstance_id," +
                "Doctor_Id=@Doctor_Id,DoctorAssign_Id=@DoctorAssign_Id," +
                " Status=@Status, UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate ");
                strSql.AppendLine(" Where Id=@Id ");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
                                        Hospital_Id = model.Hospital_Id,
                                        Patient_Id = model.Patient_Id,
                                        PatientName = model.PatientName,
                                        ModalityType_Id = model.ModalityType_Id,
                                        StudyInstance_Id = model.StudyInstance_Id,
                                        SeriesInstance_id = model.SeriesInstance_id,
                                        Doctor_Id = model.Doctor_Id,
                                        DoctorAssign_Id = model.DoctorAssign_Id,
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
        public async Task<DataBaseResponse> DeletePatientReportsInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Delete from Reports where WHERE Id=@Id ");
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
