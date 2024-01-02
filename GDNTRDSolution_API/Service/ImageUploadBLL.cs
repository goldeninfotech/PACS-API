using Dapper;
using GDNTRDSolution_API.Models;
using SoftEngine.Interface.Models;
using SoftEngine.TRDCore;
using SoftEngine.TRDCore.Configurations;
using SoftEngine.TRDModels.Models.TRD;
using System;
using System.Data.SqlClient;
using System.Text;

namespace GDNTRDSolution_API.Service
{
    public class ImageUploadBLL : IImageUpload
    {
        private readonly IWebHostEnvironment environment;
        private readonly ConnectionStrings _dbSettings;
        public ImageUploadBLL(ConnectionStrings dbSettings, IWebHostEnvironment environment)
        {
            _dbSettings = dbSettings;
            this.environment = environment;
        }
        public async Task<DataBaseResponse> SaveImage(ImageFiles imageFiles)
        {
            DataBaseResponse obj = new DataBaseResponse();
            DataBaseResponse obj2 = new DataBaseResponse();
            try
            {
                var uploads = Path.Combine(environment.WebRootPath, "ImageFolder", imageFiles.ImageFolderType+"\\"+ imageFiles.User_Id);
                string fileExtension = "" + imageFiles.User_Id + "_" + imageFiles.ImageType + Path.GetExtension(imageFiles.Image.FileName) + "";
                var filePath = Path.Combine(uploads, fileExtension);
               
                if (System.IO.File.Exists(uploads))
                    System.IO.File.Delete(uploads);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFiles.Image.CopyToAsync(stream);
                }

                imageFiles.ImagePath = filePath;

                var getimage = GetImageById(Convert.ToInt32(imageFiles.User_Id), imageFiles.ImageType);
                if (getimage.Count > 0)
                    obj2 = ImageUpdate(imageFiles);
                else
                    obj2 = ImageDataSave(imageFiles);

                if (obj2.IsSuccess)
                {
                    obj.IsSuccess = true;
                    obj.Message = "Successfully Image Uploaded.";
                    return obj;
                }
                else
                {
                    System.IO.File.Delete(uploads);
                    obj.IsSuccess = true;
                    obj.Message = "Failed Image Upload.";
                    return obj;

                }
            }
            catch (Exception ex)
            {
                obj.IsSuccess = true;
                obj.Message = "Failed Image Upload.";
                obj.ReferenceNumber = "";
                return obj;
            }
        }

        #region Check Total Image By Userid
        public List<ImageFiles> GetImageById(int id, string? imageType)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Select *  from ImageFiles where User_Id=@User_Id and ImageType=@ImageType ");
                var models = connection.Query<ImageFiles>(strSql.ToString(), new { User_Id = id, ImageType =imageType }).ToList();
                return models;
            }
        }
        #endregion

        #region Image Data Save and Update
        private DataBaseResponse ImageDataSave(ImageFiles model)
        {
            var response = new DataBaseResponse();
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  ImageFiles");
                strSql.AppendLine(" ( User_Id,ImagePath,ImageType,Status,AddedDate,AddedBy ) ");
                strSql.AppendLine(" values( @User_Id,@ImagePath,@ImageType,@Status,@AddedDate,@AddedBy ) ");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                    new
                    {
                        User_Id = model.User_Id,
                        ImagePath = model.ImagePath,
                        ImageType = model.ImageType,
                        Status = model.Status,
                        AddedDate = model.AddedDate,
                        AddedBy = model.AddedBy,
                    }
                                    );
                    response.ReturnValue = Saveresult;
                    response.Message = GlobalConst.INSERT_SUCCESS_MESSAGE;
                    response.IsSuccess = true;
                }
                catch (Exception exp)
                {
                    response.ReturnValue = -1;
                    response.Message = exp.Message;
                    response.IsSuccess = false;
                }
            }
            return response;
        }

        private DataBaseResponse ImageUpdate(ImageFiles model)
        {
            var response = new DataBaseResponse();
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Update  ImageFiles Set");
                strSql.AppendLine("  User_Id=@User_Id,ImagePath=@ImagePath,ImageType=@ImageType,Status=@Status,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy ");
                strSql.AppendLine(" where Id=@Id and User_Id=@User_Id ");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                    new
                    {
                        Id = model.Id,
                        User_Id = model.User_Id,
                        ImagePath = model.ImagePath,
                        ImageType = model.ImageType,
                        Status = model.Status,
                        UpdatedDate = model.UpdatedDate,
                        UpdatedBy = model.UpdatedBy,
                    }
                                    );
                    response.ReturnValue = Saveresult;
                    response.Message = GlobalConst.UPDATE_SUCCESS_MESSAGE;
                    response.IsSuccess = true;
                }
                catch (Exception exp)
                {
                    response.ReturnValue = -1;
                    response.Message = exp.Message;
                    response.IsSuccess = false;
                }
            }
            return response;
        }


        #endregion

        //public Hospital GetImageById(int id, string imageType)
        //{
        //    using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
        //    {
        //        StringBuilder strSql = new StringBuilder();
        //        strSql.AppendLine(" Select Id,User_Id,Name,Description,Email,HospitalCategory_Id,Country,City,Full_Address,Phone,Image,Status from Hospital where Status=1 ");
        //        strSql.AppendLine(" and Id=@Id ");
        //        var models = connection.Query<Hospital>(strSql.ToString(), new { Id = id, }).FirstOrDefault();
        //        return models;
        //    }
        //}

    }
}
