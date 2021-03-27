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
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Ins, Admin")]
        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        [Authorize(Roles = "Ins, Admin")]
        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.stds = db.StudentCourses.Where(c => c.CourseId == id).Select(c => c.Student).ToList();
            ViewBag.depts = db.DepartmentCourses.Where(c => c.CourseID == id).Select(c => c.Department);
            return View(course);
        }

        [Authorize(Roles = "Ins")]
        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,CourseName")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [Authorize(Roles = "Ins")]
        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        [Authorize(Roles = "Ins")]
        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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

        [Authorize(Roles = "Ins")]
        [HttpGet]
        public ActionResult AddStudents(int id)
        {
            var course = db.Courses.Where(c => c.CourseId == id).FirstOrDefault();
            var departments = course.DepartmentCourses.ToList();
            List<Student> allStudents = new List<Student>();

            foreach (var item in departments)
            {
                var stdList = item.Department.Students.ToList();
                allStudents.AddRange(stdList);
            }

            var CourseStudents = db.StudentCourses.Where(c => c.CourseId == id).Select(c => c.Student);
            var StudentsList = allStudents.Except(CourseStudents).ToList();
            ViewBag.course = course;

            return View(StudentsList);
        }

        [HttpPost]
        public ActionResult AddStudents(int id, Dictionary<string, bool> stds)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in stds)
                {
                    if (item.Value == true)
                    {
                        db.StudentCourses.Add(new StudentCourse() { StudentId = int.Parse(item.Key), CourseId = id});
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Details/" + id, "Courses");
            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "Ins")]
        [HttpGet]
        public ActionResult deleteStudents(int id)
        {
            var course = db.Courses.Where(c => c.CourseId == id).FirstOrDefault();
            var studentsList = db.StudentCourses.Where(c => c.CourseId == id).Select(c => c.Student).ToList();
            ViewBag.course = course;
            return View(studentsList);
        }

        [HttpPost]
        public ActionResult deleteStudents(int id, Dictionary<string, bool> stds) 
        {
            foreach (var item in stds)
            {
                if (item.Value == true)
                {
                    int x = int.Parse(item.Key);
                    var std = db.StudentCourses.FirstOrDefault(d => d.CourseId == id && d.StudentId == x);
                    db.StudentCourses.Remove(std);
                }

            }
            db.SaveChanges();
            return RedirectToAction("Details/" + id, "Courses");

        }
    }
}
