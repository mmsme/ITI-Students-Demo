using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ITI_Students_Demo.Models;

namespace ITI_Students_Demo.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Department);
            return View(students.ToList());
        }

        [Authorize(Roles = "Student")]
        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [Authorize(Roles = "Ins")]
        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.DeptId = new SelectList(db.Departments, "Id", "DeptName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,Address,Img,DeptId")] Student student, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {

                if (img != null)
                {
                    string filename = student.Name + DateTime.Now.Millisecond + "." + img.FileName.Split('.')[1];
                    img.SaveAs(Server.MapPath("~/Imgs/") + filename);
                    student.Img = filename;
                }
                else
                {
                    student.Img = "/noone.png";
                }
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeptId = new SelectList(db.Departments, "Id", "DeptName", student.DeptId);
            return View(student);
        }

        [Authorize(Roles = "Ins")]
        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeptId = new SelectList(db.Departments, "Id", "DeptName", student.DeptId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,Address,Img,DeptId")] Student student, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {

                if (img != null)
                {
                    string filename = student.Name + student.Id + "." + img.FileName.Split('.')[1];
                    img.SaveAs(Server.MapPath("~/Imgs/") + filename);
                    student.Img = filename;
                }
                else
                {
                    student.Img = "/noone.png";
                }
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeptId = new SelectList(db.Departments, "Id", "DeptName", student.DeptId);
            return View(student);
        }

        [Authorize(Roles = "Ins")]
        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
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
