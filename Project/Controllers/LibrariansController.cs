using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class LibrariansController : Controller
    {
        private LibraryDbContext db = new LibraryDbContext();

        // GET: Librarians
        public ActionResult Index()
        {
            var librarians = db.Librarians.Include(l => l.Library);
            return View(librarians.ToList());
        }

        // GET: Librarians/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Librarian librarian = db.Librarians.Find(id);
            if (librarian == null)
            {
                return HttpNotFound();
            }
            return View(librarian);
        }

        // GET: Librarians/Create
        public ActionResult Create()
        {
            ViewBag.LibraryId = new SelectList(db.Libraries, "LibraryId", "Name");
            return View();
        }

        // POST: Librarians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LibrarianId,FirstName,LastName,Email,EmployeeId,HireDate,LibraryId,UserId")] Librarian librarian)
        {
            if (ModelState.IsValid)
            {
                db.Librarians.Add(librarian);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LibraryId = new SelectList(db.Libraries, "LibraryId", "Name", librarian.LibraryId);
            return View(librarian);
        }

        // GET: Librarians/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Librarian librarian = db.Librarians.Find(id);
            if (librarian == null)
            {
                return HttpNotFound();
            }
            ViewBag.LibraryId = new SelectList(db.Libraries, "LibraryId", "Name", librarian.LibraryId);
            return View(librarian);
        }

        // POST: Librarians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LibrarianId,FirstName,LastName,Email,EmployeeId,HireDate,LibraryId,UserId")] Librarian librarian)
        {
            if (ModelState.IsValid)
            {
                db.Entry(librarian).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LibraryId = new SelectList(db.Libraries, "LibraryId", "Name", librarian.LibraryId);
            return View(librarian);
        }

        // GET: Librarians/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Librarian librarian = db.Librarians.Find(id);
            if (librarian == null)
            {
                return HttpNotFound();
            }
            return View(librarian);
        }

        // POST: Librarians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Librarian librarian = db.Librarians.Find(id);
            db.Librarians.Remove(librarian);
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
