using Microsoft.AspNetCore.Mvc;
using SoftEngine.Interface.ITRD;
using SoftEngine.TRDModels.Models.TRD;

namespace GDNTRDSolution_API.Areas.TRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadFilesController : Controller
    {
        private readonly IFileUpload _fileUpload;
        public UploadFilesController(IFileUpload fileUpload)
        {
            _fileUpload = fileUpload;
        }

        [HttpPost]
        [Route("UploadFiles")]
        public  IActionResult UploadFiles()
        {
            bool IsSuccess = false;
            string dir_path = Path.Combine(Environment.CurrentDirectory, "PACS");
           //tring wwwrootPath = Path.Combine(Environment.CurrentDirectory, "wwwroot");
           // string dir_path = Path.Combine(wwwrootPath, "PACS");
            HttpContext context = HttpContext;

            string hospitalid = context.Request.Query["hospitalid"];
            string userId = context.Request.Query["userId"];

            string modality = context.Request.Query["modality"];
            string study_instance_id = context.Request.Query["study_instance_id"];
            string series_instance_id = context.Request.Query["series_instance_id"];
            string sop_instance_id = context.Request.Query["sop_instance_id"];
            string series_number = context.Request.Query["series_number"];
            string instance_number = context.Request.Query["instance_number"];
            string patientid = context.Request.Query["patientid"];
            string patient_name = context.Request.Query["patient_name"];
            string study_date = context.Request.Query["study_date"];
            string study_time = context.Request.Query["study_time"];

            IFormFileCollection file = context.Request.Form.Files;

            string upload_path = dir_path + "/" + study_instance_id + "/" + series_instance_id;

            if (file.Count > 0)
            {

                if (Directory.Exists(upload_path) == false) { Directory.CreateDirectory(upload_path); }

                string fileName = sop_instance_id + ".dcm";

                if (System.IO.File.Exists(upload_path + "/" + fileName) == false)
                {
                    IFormFile postedFile = file[0];

                    using (FileStream stream = new FileStream(Path.Combine(upload_path, fileName), FileMode.Create))
                    {
                        FilesRecord obj = new FilesRecord();
                        postedFile.CopyTo(stream);

                        if (!string.IsNullOrEmpty(hospitalid) && !string.IsNullOrEmpty(userId))
                        {
                            obj.Hospital_Id = Convert.ToInt32(hospitalid);
                            obj.ModalityType_Id = modality;
                            obj.Patient_Id = patientid;
                            obj.PatientName = patient_name;
                            obj.SeriesInstance_id = series_instance_id;
                            obj.StudyInstance_Id = study_instance_id;
                            obj.SopInstance_Id = sop_instance_id;
                            obj.SeriesNumber = series_number;
                            obj.InstanceNumber = instance_number;
                            obj.Status = 1;
                            obj.AddedBy = userId;
                            obj.AddedDate = DateTime.Now.ToString();
                            var data = _fileUpload.SaveFilesInfo(obj);
                        }
                        else
                        {
                            //obj.Hospital_Id = 2;
                            obj.ModalityType_Id = modality;
                            obj.Patient_Id = patientid;
                            obj.PatientName = patient_name;
                            obj.SeriesInstance_id = series_instance_id;
                            obj.StudyInstance_Id = study_instance_id;
                            obj.SopInstance_Id = sop_instance_id;
                            obj.SeriesNumber = series_number;
                            obj.InstanceNumber = instance_number;
                            obj.Status = 1;
                            //obj.AddedBy = "";
                            obj.AddedDate = DateTime.Now.ToString();
                            var data = _fileUpload.SaveFilesInfo(obj);
                        }



                    }
                }

            }
            return Ok("");
        }
    }
}
