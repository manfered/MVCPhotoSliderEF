using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MVCPhotoSliderEF.Models
{
    public class PhotoFileManager
    {
        public string StorageDirectory = "images";

        private IEnumerable<HttpPostedFileBase> files;

        public PhotoFileManager()
        {

        }
        public PhotoFileManager(IEnumerable<HttpPostedFileBase> _files)
        {
            files = _files;
        }

        public JsonResult Upload()
        {
            var result = JSONBuilder(_status: "Failed",
                                     _ResultMessage: "Uploaded File list in null");
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string uploadedFileName = file.FileName;

                        var path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + StorageDirectory), uploadedFileName);

                        //if file exists we assign a new sequence of charachters at the end of file name
                        if (System.IO.File.Exists(path))
                        {
                            uploadedFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + StorageDirectory), uploadedFileName);
                        }

                        try
                        {
                            file.SaveAs(path);
                            result = JSONBuilder(_status: "Success",
                                     _ResultMessage: "File successfully uploaded",
                                     _uploadedFileDirectory: StorageDirectory,
                                     _uploadedFileName: uploadedFileName);
                        }
                        catch (Exception ex)
                        {
                            result = JSONBuilder(_status: "Failed",
                                     _ResultMessage: ex.Message);

                            break;
                        }
                    }
                }
            }


            return result;
        }


        public JsonResult Delete(string deleteFileName)
        {
            string fullPath = Path.Combine(HttpContext.Current.Server.MapPath("~/" + StorageDirectory), deleteFileName);
            var result = JSONBuilder(_status: "Failed",
                                     _ResultMessage: "Error in deleting file");

            if (System.IO.File.Exists(fullPath))
            {
                try
                {
                    System.IO.File.Delete(fullPath);
                    result = JSONBuilder(_status: "Success",
                                     _ResultMessage: "File successfully deleted");
                }
                catch (Exception ex)
                {
                    result = JSONBuilder(_status: "Failed",
                                     _ResultMessage: "File deletation failed!" + ex.Message);
                }

            }

            return result;
        }


        private JsonResult JSONBuilder(string _status,
                                       string _ResultMessage,
                                       string _uploadedFileDirectory = "",
                                       string _uploadedFileName = "")
        {

            return new JsonResult()
            {
                Data = new
                {
                    status = _status,
                    resultMessage = _ResultMessage,
                    uploadedFileDirectory = _uploadedFileDirectory,
                    uploadedFileName = _uploadedFileName
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        
    }
}