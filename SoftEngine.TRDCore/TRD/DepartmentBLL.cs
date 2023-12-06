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
    public class DepartmentBLL : IDepartment
    {
        private readonly ConnectionStrings _dbSettings;
        public DepartmentBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }
        public IEnumerable<Departments> GetDepartmentList()
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,Name,Status from Departments where Status=1";
                var models = connection.Query<Departments>(sql).ToList();
                return models;
            }
        }
        public Departments GetDepartmentById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataBaseResponse> SaveDepartmentInfo(Departments model)
        {
            throw new NotImplementedException();
        }

        public Task<DataBaseResponse> UpdateDepartmentInfo(Departments model)
        {
            throw new NotImplementedException();
        }
        public Task<DataBaseResponse> DeleteDepartmentInfo(int id)
        {
            throw new NotImplementedException();
        }

    }
}
