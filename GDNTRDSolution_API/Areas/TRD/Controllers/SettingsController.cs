using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.ITRD;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private readonly ISettings _settings;

        public SettingsController(ISettings settings)
        {
            _settings = settings;
        }


        #region Settings CRUD

        #endregion

    }
}
