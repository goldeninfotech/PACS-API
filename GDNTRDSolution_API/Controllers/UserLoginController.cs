using GDNTRDSolution_API.Common;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.IADM;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
            _mailService = _MailService;
        }

       
    }
}
