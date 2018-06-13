using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCPhotoSliderEF.Models
{
    public class PhotoDeleter : PhotoFileManager
    {
        public string Delete(string deletFileName)
        {
            string fullPath = Path.Combine(HttpContext.Current.Server.MapPath("~/" + StorageDirectory), deletFileName);
            string resultStr = string.Empty;
            if (System.IO.File.Exists(fullPath))
            {
                try
                {
                    System.IO.File.Delete(fullPath);
                    resultStr = "Successfully 1 file deleted";
                }
                catch (Exception ex)
                {
                    resultStr = ex.Message;
                }

            }

            return resultStr;
        }
    }
}