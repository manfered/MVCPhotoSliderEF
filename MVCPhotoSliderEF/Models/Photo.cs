using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCPhotoSliderEF.Models
{
    public class Photo
    {
        [ForeignKey("Slide")]
        public int PhotoID { get; set; }
        [Required]
        [Display(Name = "Source")]
        public string SRC { get; set; }

        public virtual Slide Slide { get; set; }
        
    }
}