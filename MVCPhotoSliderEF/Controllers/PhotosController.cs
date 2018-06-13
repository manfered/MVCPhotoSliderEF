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
            ViewBag.PhotoID = new SelectList(db.Slides, "SlideID", "Title", id);
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
        public ActionResult DeleteConfirmed(int id)
        {
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
            UploadResult uploadResult = new UploadResult();
            PhotoUploader uploader = new PhotoUploader(files);
            uploader.Upload(uploadResult);

            if (uploadResult.Result == false)
            {
                // if file upload was not successful 
                // we return a JSON to the controller as a result of failure
                return FailJSON(uploadResult);
            }

            return SuccessJSON(uploadResult);
        }

        public ActionResult AsyncDelete()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsyncDelete(string deletFileName)
        {
            PhotoDeleter photoDeleter = new PhotoDeleter();

            return Json(new { Data = photoDeleter.Delete(deletFileName), deleteFileName = deletFileName }, JsonRequestBehavior.AllowGet);
            //return new JsonResult { Data = "Successfully " + count + " file(s) uploaded" };
        }



        private JsonResult SuccessJSON(UploadResult uploadResult)
        {
            return Json(new
            {
                status = "Success",
                Data = uploadResult.ResultString,
                src = uploadResult.StorageDirectory + "/" + uploadResult.UploadedFilename,
                uploadedFileName = uploadResult.UploadedFilename
            }, JsonRequestBehavior.AllowGet);
        }

        private JsonResult FailJSON(UploadResult uploadResult)
        {
            return Json(new
            {
                status = "Fail",
                Data = uploadResult.ResultString
            }, JsonRequestBehavior.AllowGet);
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
