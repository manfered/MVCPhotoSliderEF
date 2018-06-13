using MVCPhotoSliderEF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCPhotoSliderEF.DataContext
{
    public class SlidesContextDb : DbContext
    {
        public SlidesContextDb() : base("defaultConnection")
        {

        }

        public DbSet<Slide> Slides { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}