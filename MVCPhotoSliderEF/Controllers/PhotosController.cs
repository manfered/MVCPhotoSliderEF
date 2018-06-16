using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCPhotoSliderEF.DataContext;
using MVCPhotoSliderEF.Models;
using System.IO;

namespace MVCPhotoSliderEF.Controllers
{
    public class PhotosController : Controller
    {
        private SlidesContextDb db = new SlidesContextDb();

        // GET: Photos
        public ActionResult Index()
        {
            var photos = db.Photos.Include(p => p.Slide);
            return View(photos.ToList());
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create(int id)
        {
            //ViewBag.PhotoID = new SelectList(db.Slides, "SlideID", "Title", id);
            ViewBag.PhotoID = id;
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhotoID,SRC")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Photos.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Index" , "Slides");
            }

            ViewBag.PhotoID = new SelectList(db.Slides, "SlideID", "Title", photo.PhotoID);
            return View(photo);
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhotoID = new SelectList(db.Slides, "SlideID", "Title", photo.PhotoID);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhotoID,SRC")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Slides");
            }
            ViewBag.PhotoID = new SelectList(db.Slides, "SlideID", "Title", photo.PhotoID);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string deleteFileName)
        {
            PhotoFileManager manager = new PhotoFileManager();
            manager.Delete(deleteFileName);

            Photo photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Index", "Slides");
        }


        //upload async action
        public ActionResult AsyncUpload()
        {
            return View();
        }

        //upload async action post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsyncUpload(IEnumerable<HttpPostedFileBase> files)
        {
            PhotoFileManager manager = new PhotoFileManager(files);
            return manager.Upload();
        }

        public ActionResult AsyncDelete()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsyncDelete(string deleteFileName)
        {
            PhotoFileManager manager = new PhotoFileManager();

            return manager.Delete(deleteFileName);
        }



        



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
