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
                var sql = @"   Select COUNT(f.Id) TotalFiles, f.PatientName,f.ModalityType_Id,f.StudyInstance_Id,f.SeriesInstance_id
                ,h.Name HospitalName from FilesRecord f
                left join Hospital h on h.Id=f.Hospital_Id
                group by f.Patient_Id, f.PatientName ,f.ModalityType_Id,f.StudyInstance_Id,f.SeriesInstance_id, h.Name ";
                var models = connection.Query<FilesRecordViewModel>(sql).ToList();
                return models;
            }
        }
    }
}
