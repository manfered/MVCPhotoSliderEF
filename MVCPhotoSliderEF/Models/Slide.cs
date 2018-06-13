using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCPhotoSliderEF.Models
{
    public class Slide
    {
        public int SlideID { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public virtual Photo Photo { get; set; }
    }
}