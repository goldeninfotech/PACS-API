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



        #region Status List
        //(status : Pending -1 , Approved-2, Reject-3, Re-submited-4) 
        public List<CommonDataListModel> StatusList()
        {
            List<CommonDataListModel> state = new List<CommonDataListModel>();
            state.Add(new CommonDataListModel
            {
                Id = "Pending",
                Value = "Pending"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Approved",
                Value = "Approved"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Reject",
                Value = "Reject"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Re-submited",
                Value = "Re-submited"
            });
            
            return state;
        }
        #endregion 
        #region payment method
        //bkash/cash/bank
        public List<CommonDataListModel> PaymentMethodList() 
        {
            List<CommonDataListModel> state = new List<CommonDataListModel>();
            state.Add(new CommonDataListModel
            {
                Id = "Bkash",
                Value = "Bkash"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Cash",
                Value = "Cash"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Bank",
                Value = "Bank"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Rocket",
                Value = "Rocket"
            });
            state.Add(new CommonDataListModel
            {
                Id = "Nagad",
                Value = "Nagad"
            });
            
            return state;
        }
        #endregion
    }
}
