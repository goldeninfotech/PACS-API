using Azure;
using GDNTRDSolution_API.Common;
using GDNTRDSolution_API.Models;
using GDNTRDSolution_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoftEngine.Interface.IADM;
using SoftEngine.Interface.Models;
using SoftEngine.TRDModels.Models.ADM;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace GDNTRDSolution_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLoginController : Controller
    {
        private IConfiguration _config;
        private readonly IUserLogin _userLogin ;
        private readonly TokenVerify _tokenVerify;
        private readonly IMailService _mailService;

        public UserLoginController(IConfiguration config, IUserLogin userLogin, TokenVerify tokenVerify, IMailService _MailService)
        {
            _config = config;
            _userLogin = userLogin;
            _tokenVerify = tokenVerify;
          //  _mailService = mailService;
            _mailService = _MailService;
        }

        #region User Login 
        private ADM_UserLogin AuthenticateUser(UserLogin _obj)
        {
            ADM_UserLogin obj = null;
            ADM_UserLogin userList = _userLogin.UserLoginAsync(_obj.UserName, _obj.Password);
            if (userList != null)
            {
                obj = new ADM_UserLogin { Full_Name = userList.Full_Name, Id = userList.Id, UserName = userList.UserName, Email = userList.Email, Phone = userList.Phone, DoctorId = userList.DoctorId, DoctorName = userList.DoctorName, BMDC_No = userList.BMDC_No, HospitalId=userList.HospitalId,HospitalName=userList.HospitalName };
            }
            return obj;
        }

        private string GenerateToken(ADM_UserLogin aDM_UserLogin)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            if (string.IsNullOrEmpty(aDM_UserLogin.Phone))
                aDM_UserLogin.Phone = ""; 
            if (string.IsNullOrEmpty(aDM_UserLogin.Email))
                aDM_UserLogin.Email = "";
            var claims = new[]
            {
                new Claim("Full_Name", aDM_UserLogin.Full_Name),
                new Claim("UserName", aDM_UserLogin.UserName),
                new Claim("Id", aDM_UserLogin.Id.ToString()),
                new Claim("Phone", aDM_UserLogin.Phone.ToString()),
                new Claim("Email", aDM_UserLogin.Email.ToString()),
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



        #region Password Recovery

        [HttpPut]
        [Route("PasswordRecovery")]
        public IActionResult PasswordRecovery(string email)
        {
            MailData obj = new MailData();
            obj.EmailToId = email;
            obj.EmailSubject = "Password Recovery";
            obj.EmailBody = "Password Recovery Code Is : ";
            obj.EmailToName = "GDN";
            var number = GenerateNewRandom();

            obj.EmailBody += number;
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (Regex.IsMatch(email, pattern))
            {
                var user = _userLogin.GetUserinfoByEmail(email);
                if (user == null)
                {
                    return Ok(new { IsSuccess = false, Message = "No user found with this email." });
                }
                else if (user.Id > 0)
                {
                    obj.EmailBody = "Hello " + user.Full_Name + ", Your Password Recovery Code Is : " + number + "";
                    bool sendmail = _mailService.SendMail(obj);
                    // bool sendmail = _mailService.SendMailAsync(obj);

                    if (sendmail)
                    {
                        var data = _userLogin.PasswordRecovery(email, number);
                        return Ok(data.Result);
                    }
                }
                else
                    return Ok(new { IsSuccess = false, Message = "Can't send the email. Please try again later." });
            }
            else
                return Ok(new { IsSuccess = false, Message = "Invalid email address." });

            return Ok(new { IsSuccess = false, Message = " Something is missing. Please try again later." });
        }

        private static string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }

        [HttpGet]
        [Route("CheckRecoveryCode")]
        public IActionResult CheckRecoveryCode(int code)
        {
            bool data = _userLogin.CheckRecoveryCode(code);
            DataBaseResponse obj = new DataBaseResponse();
            if (data)
            {
                obj.IsSuccess = data;
                obj.Message = "Successfully code matched.";
                obj.ReturnValue = 1;
            }
            else
            {
                obj.IsSuccess = false;
                obj.Message = "Code isn't matched.";
                obj.ReturnValue = -1;
            }
            return Ok(obj);
        }


        [HttpPut]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(PasswordChange passwordChange)
        {

            if (!string.IsNullOrEmpty(passwordChange.Password) && !string.IsNullOrEmpty(passwordChange.VerifyCode))
            {
                string password = ReturnData.GenerateMD5(passwordChange.Password); // md5 Hash Password
                var data = _userLogin.ChangePassword(password, passwordChange.VerifyCode);
                return Ok(data.Result);
            }
            else
            {
                DataBaseResponse obj = new DataBaseResponse();
                obj.IsSuccess = false;
                obj.ReturnValue = -1;
                obj.Message = "Failed! Password Change";
                return Ok(obj);
            }
        }

        #endregion
    }
}
