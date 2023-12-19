using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.Interface.ITRD
{
    public interface IUsers
    {
        public IEnumerable<User> GetUsersList(string search);
        public User GetUsersById(int id);
        public Task<DataBaseResponse> SaveUsersInfo(User model);
        public Task<DataBaseResponse> UpdateUsersInfo(User model);
        public Task<DataBaseResponse> DeleteUsersInfo(int id); 
    }
}
