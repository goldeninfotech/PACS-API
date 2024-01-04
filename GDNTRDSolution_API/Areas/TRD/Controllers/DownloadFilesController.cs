using GDNTRDSolution_API.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.IO;
using Azure;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadFilesController : Controller
    {
        public string root_path = Path.Combine(Environment.CurrentDirectory, "PACS");

        
        [HttpGet("DownloadFiles")]
        public ActionResult<Dictionary<string, dynamic>> DownloadFiles(string study_id)
        {
            //string study_id = id;
            string host_name = (HttpContext.Request.Scheme + "://" + HttpContext.Request.Host).ToString();
            string study_path = root_path + "/" + study_id;
            if (Directory.Exists(study_path))
            {

                // Create a memory stream to hold the zip file content
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Create the zip file
                    ZipFiles(study_path, memoryStream);

                    // Set the content type and headers
                    Response.Headers.Add("content-disposition", "attachment;filename=files.zip");

                    // Return the zip file as the response
                    return File(memoryStream.ToArray(), "application/zip", "files.zip");
                }
            }
            return Ok(new { IsSuccess = false, Message = "File Not Found"});
        }
        private void ZipFiles(string sourceDirectory, Stream outputStream)
        {
            using (ZipArchive archive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
            {
                string[] files = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    string entryName = file.Substring(sourceDirectory.Length + 1); // Get relative path
                    archive.CreateEntryFromFile(file, entryName);
                }
            }
        }
        private void ZipFiles2(string sourceDirectory, Stream outputStream)
        {
            using (ZipArchive archive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
            {
                string[] files = Directory.GetFiles(sourceDirectory);

                foreach (string file in files)
                {
                    string entryName = Path.GetFileName(file);
                    archive.CreateEntryFromFile(file, entryName);
                }
            }
        }


    }
}
