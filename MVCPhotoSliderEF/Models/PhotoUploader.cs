using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCPhotoSliderEF.Models
{
    public class PhotoUploader : PhotoFileManager
    {
        private IEnumerable<HttpPostedFileBase> files;
        public PhotoUploader(IEnumerable<HttpPostedFileBase> _files)
        {
            files = _files;
        }

        public void Upload(UploadResult uploadResult)
        {
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
                            uploadResult.Result = true;
                            uploadResult.StorageDirectory = StorageDirectory;
                            uploadResult.UploadedFilename = uploadedFileName;
                            uploadResult.ResultString = "File successfully uploaded";
                        }
                        catch (Exception ex)
                        {
                            //exception message
                            uploadResult.Result = false;
                            uploadResult.ResultString = ex.Message;

                            break;
                        }
                    }
                }
            }
        }
    }
}