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
    [Authorize(Roles = "Ins, Admin")]
    public class DepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       
        // GET: Departments
        public ActionResult Index()
        {
            return View(db.Departments.ToList());
        }

        [Authorize(Roles = "Ins")]
        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.courses = db.DepartmentCourses.Where(d => d.DeptID == id).Select(a => a.Course);
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeptName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeptName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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
        public ActionResult addCourses(int id)
        {
            var allCourses = db.Courses.ToList();
            var deptCourses = db.DepartmentCourses.Where(d => d.DeptID == id).Select(a => a.Course);
            var courseList = allCourses.Except(deptCourses).ToList();
            ViewBag.dept = db.Departments.FirstOrDefault(d => d.Id == id);
            return View(courseList);
        }

        [HttpPost]
        public ActionResult addCourses(int id, Dictionary<string, bool> crs)
        {
            foreach (var item in crs)
            {
                if (item.Value == true)
                {
                    db.DepartmentCourses.Add(new DepartmentCourse() { DeptID = id, CourseID = int.Parse(item.Key) });
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details/" + id, "Departments");
        }

        [Authorize(Roles = "Ins")]
        [HttpGet]
        public ActionResult deleteCourses(int id)
        {
            var deptCourses = db.DepartmentCourses.Where(d => d.DeptID == id).Select(a => a.Course).ToList();
            ViewBag.dept = db.Departments.FirstOrDefault(d => d.Id == id);
            return View(deptCourses);
        }

        [HttpPost]
        public ActionResult deleteCourses(int id, Dictionary<string, bool> crs)
        {
            foreach (var item in crs)
            {
                if (item.Value == true)
                {
                    int x = int.Parse(item.Key);
                    var c = db.DepartmentCourses.FirstOrDefault(d => d.DeptID == id && d.CourseID == x);
                    db.DepartmentCourses.Remove(c);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details/" + id, "Departments");
        }

      
    }
}
