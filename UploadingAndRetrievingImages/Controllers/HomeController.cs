using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadingAndRetrievingImages.Models;

namespace UploadingAndRetrievingImages.Controllers
{
    
    public class HomeController : Controller
    {
        NewDBEntities db = new NewDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            var data = db.students.ToList();

            return View(data);
        }
        public ActionResult Create()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Create(student s)
        {
            string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
            string extension = Path.GetExtension(s.ImageFile.FileName);
            HttpPostedFileBase postedFile = s.ImageFile;
            int length = postedFile.ContentLength;

            if(extension.ToLower()==".jpg"|| extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
            {
                // Lenght is in byte 1000000=1mb
                if(length<=1000000)
                {
                    fileName = fileName + extension;
                    s.image_path = "~/images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                    s.ImageFile.SaveAs(fileName);

                    db.students.Add(s);

                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        ViewBag.Message = "<script>alert('Record Inserted')</script>";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = "<script>alert('Record Not Inserted')</script>";
                    }
                }
                else
                {
                    ViewBag.SizeMessage = "<script>alert('Size should be of 1MB')</script>";
                }
            }
            else
            {
                ViewBag.ExtensionMessage = "<script>alert('Image Not Supported')</script>";
            }

            return View();
        }
    }
}