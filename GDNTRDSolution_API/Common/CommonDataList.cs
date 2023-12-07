using GDNTRDSolution_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using System.Web.Http.Results;

namespace GDNTRDSolution_API.Common
{
    public class CommonDataList: ControllerBase
    {
        #region IsActive List
        public List<IsActiveList> IsActiveList()
        {
            List<IsActiveList> state = new List<IsActiveList>();
            state.Add(new IsActiveList
            {
                Id = "Active",
                Value = "Active"
            });
            state.Add(new IsActiveList
            {
                Id = "InActive",
                Value = "InActive"
            });
            return state;
        }
        #endregion

        #region IsActive List
        public List<CommonDataListModel> CountryList() 
        {
            List<CommonDataListModel> state = new List<CommonDataListModel>();
            state.Add(new CommonDataListModel
            {
                Id = "Afghanistan",
                Value = "Afghanistan"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Afghanistan",
                Value = "Afghanistan"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Bangladesh",
                Value = "Bangladesh"
            });
            state.Add(new CommonDataListModel
            {
                Id = "India",
                Value = "India"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Napal",
                Value = "Napal"
            });
            return state;
        }
        #endregion
    }
}
