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
    public class FileUploadBLL : IFileUpload
    {
        private readonly ConnectionStrings _dbSettings;
        public FileUploadBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }
        public async Task<DataBaseResponse> SaveFilesInfo(FilesRecord model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  FilesRecord");
                strSql.AppendLine(" ( Hospital_Id,Patient_Id,PatientName,ModalityType_Id,StudyInstance_Id,SeriesInstance_id,SopInstance_Id,SeriesNumber,InstanceNumber,Status,AddedDate,AddedBy ) VALUES ");
                strSql.AppendLine(" ( @Hospital_Id,@Patient_Id,@PatientName,@ModalityType_Id,@StudyInstance_Id,@SeriesInstance_id,@SopInstance_Id,@SeriesNumber,@InstanceNumber,@Status,@AddedDate,@AddedBy ) ");
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
                                        SopInstance_Id = model.SopInstance_Id,
                                        SeriesNumber = model.SeriesNumber,
                                        InstanceNumber = model.InstanceNumber,
                                        Status = true,
                                        AddedDate = model.AddedDate,
                                        AddedBy = model.AddedBy,
                                    }
                                    );
                    response.ReturnValue = Saveresult;
                    response.Message = "Data Inserted Successfully.";
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
    }
}
