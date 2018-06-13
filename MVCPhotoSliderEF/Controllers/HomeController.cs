using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MVCPhotoSliderEF.DataContext;

namespace MVCPhotoSliderEF.Controllers
{
    public class HomeController : Controller
    {
        private SlidesContextDb db = new SlidesContextDb();
        public ActionResult Index()
        {
            var slides = db.Slides.Include(s => s.Photo);
            return View(slides.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}