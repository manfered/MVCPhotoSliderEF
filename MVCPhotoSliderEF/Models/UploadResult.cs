using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCPhotoSliderEF.Models
{
    public class UploadResult
    {
        public bool Result { get; set; }
        public string ResultString { get; set; }
        public string StorageDirectory { get; set; }
        public string UploadedFilename { get; set; }
    }
}