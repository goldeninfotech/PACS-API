using Dapper;
using SoftEngine.Interface.ITRD;
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
    public class FilesRecoredBLL : IFilesRecored
    {
        private readonly ConnectionStrings _dbSettings;
        public FilesRecoredBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }
        public IEnumerable<FilesRecordViewModel> GetFilesRecordList()
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"   Select Hospital_Id,PatientName,Patient_Id,StudyInstance_Id,ModalityType_Id,count(InstanceNumber) TotalFiles, FilesRecord.Status,
                  h.Name HospitalName
                  from FilesRecord 
                  left join Hospital h on h.Id=Hospital_Id
                  where FilesRecord.Status=1
                  group by StudyInstance_Id,PatientName,Patient_Id,ModalityType_Id,Hospital_Id,FilesRecord.Status, h.Name  ";
                var models = connection.Query<FilesRecordViewModel>(sql).ToList();
                return models;
            }
        }
    }
}
