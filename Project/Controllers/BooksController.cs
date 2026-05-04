using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class BooksController : Controller
    {
        private LibraryDbContext db = new LibraryDbContext();

        // GET: Books
        public ActionResult Index(string search, string genre)
        {
            var books = db.Books.Include(b => b.Library).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(b => b.Title.Contains(search)
                                      || b.Author.Contains(search));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                books = books.Where(b => b.Genre == genre);
            }

            ViewBag.Genres = db.Books.Select(b => b.Genre)
                                     .Distinct()
                                     .ToList();

            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Book book = db.Books
                .Include(b => b.Library)
                .Include(b => b.Feedbacks.Select(f => f.Member))
                .FirstOrDefault(b => b.BookId == id);

            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.LibraryId = new SelectList(db.Libraries, "LibraryId", "Name");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,Title,Author,Genre,ISBN,IsAvailable,Summary,CoverImageUrl,PublicationYear,TotalCopies,LibraryId")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                TempData["Success"] = "Book added successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.LibraryId = new SelectList(db.Libraries, "LibraryId", "Name", book.LibraryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Book book = db.Books.Find(id);
            if (book == null)
                return HttpNotFound();

            ViewBag.LibraryId = new SelectList(db.Libraries, "LibraryId", "Name", book.LibraryId);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,Title,Author,Genre,ISBN,IsAvailable,Summary,CoverImageUrl,PublicationYear,TotalCopies,LibraryId")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Book updated successfully!";
                return RedirectToAction("Index");
            }
            ViewBag.LibraryId = new SelectList(db.Libraries, "LibraryId", "Name", book.LibraryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Book book = db.Books.Find(id);
            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            TempData["Success"] = "Book deleted successfully!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}