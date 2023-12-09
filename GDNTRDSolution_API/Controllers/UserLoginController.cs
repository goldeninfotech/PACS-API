using Azure;
using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoftEngine.Interface.IADM;
using SoftEngine.TRDModels.Models.ADM;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GDNTRDSolution_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLoginController : Controller
    {
        private IConfiguration _config;
        private readonly IUserLogin _userLogin ;
        private readonly TokenVerify _tokenVerify;
       // private readonly IMailService _mailService;

        public UserLoginController(IConfiguration config, IUserLogin userLogin, TokenVerify tokenVerify)
        {
            _config = config;
            _userLogin = userLogin;
            _tokenVerify = tokenVerify;
          //  _mailService = _MailService;
        }

        #region User Login 
        private ADM_UserLogin AuthenticateUser(UserLogin _obj)
        {
            ADM_UserLogin obj = null;
            ADM_UserLogin userList = _userLogin.UserLoginAsync(_obj.UserName, _obj.Password);
            if (userList != null)
            {
                obj = new ADM_UserLogin { Full_Name = userList.Full_Name, Id = userList.Id, DoctorId = userList.DoctorId, DoctorName = userList.DoctorName, BMDC_No = userList.BMDC_No, HospitalId=userList.HospitalId,HospitalName=userList.HospitalName };
            }
            return obj;
        }

        private string GenerateToken(ADM_UserLogin aDM_UserLogin)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Full_Name", aDM_UserLogin.Full_Name),
                new Claim("Id", aDM_UserLogin.Id.ToString()),
                //new Claim("DoctorId", aDM_UserLogin.DoctorId.ToString()),
                //new Claim("DoctorName", aDM_UserLogin.DoctorName),
                //new Claim("BMDC_No", aDM_UserLogin.BMDC_No),
                //new Claim("HospitalId", aDM_UserLogin.HospitalId.ToString()),
                //new Claim("HospitalName", aDM_UserLogin.HospitalName),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
             expires: DateTime.Now.AddDays(1),
             signingCredentials: credentials
             );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            bool IsSuccess = false;
            string Message = "Wrong username or password! ";
            IActionResult response = Unauthorized();
            if (!string.IsNullOrEmpty(userLogin.UserName) && !string.IsNullOrEmpty(userLogin.Password))
            {
                Encryption encryption = new Encryption();
                userLogin.Password = ReturnData.GenerateMD5(userLogin.Password.Trim()).ToString();

                var user = AuthenticateUser(userLogin);
                if (user != null)
                {
                    string tokenKey = GenerateToken(user);
                    IsSuccess = true;
                    Message = "Login Success...";

                    response = Ok(new { Token = tokenKey, Message = Message, IsSuccess = IsSuccess });
                }
                else
                    response = Ok(new { Message = Message, IsSuccess = IsSuccess });
            }
            return response;
        }

        #endregion

    }
}
