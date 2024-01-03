using GDNTRDSolution_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DicomViewerController : Controller
    {
       // public string root_path = AppDomain.CurrentDomain.BaseDirectory + "/PACS";
        public string root_path = Path.Combine(Environment.CurrentDirectory, "PACS");
        // [HttpGet("json/{id}")]
        [HttpGet("index")]
        public ActionResult<Dictionary<string, dynamic>> index(string study_id)
        {
            //string study_id = id;
            string host_name = (HttpContext.Request.Scheme + "://" + HttpContext.Request.Host).ToString();
            string study_path = root_path + "/" + study_id;

            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

            if (study_id != "" && study_id != null)
            {
                data = ViewerData.GetJson(host_name, study_path);
            }

            return data;
        }


        [HttpGet("file/{study}/{series}/{file}")]
        public IActionResult files(string study, string series, string file)
        {
            string study_id = study;
            string s_id = series;
            string f_id = file + ".dcm";

            string file_path = root_path + "/" + study_id + "/" + s_id + "/" + f_id;

            if (System.IO.File.Exists(file_path))
            {
                return File(System.IO.File.OpenRead(file_path), "application/octet-stream", Path.GetFileName(file_path));
            }

            return NotFound();
        }
    }
}
