using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project.Models;

namespace Project.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private LibraryDbContext db = new LibraryDbContext();

        // Librarian sees all feedback
        [Authorize(Roles = "Librarian")]
        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks
                .Include(f => f.Book)
                .Include(f => f.Member)
                .OrderByDescending(f => f.SubmittedDate)
                .ToList();
            return View(feedbacks);
        }

        // Member submits feedback on a book
        [Authorize(Roles = "Member")]
        public ActionResult Create(int bookId)
        {
            var book = db.Books.Find(bookId);
            if (book == null)
                return HttpNotFound();

            ViewBag.BookTitle = book.Title;
            ViewBag.BookId = bookId;

            return View(new Feedback
            {
                BookId = bookId,
                SubmittedDate = DateTime.Now,
                Rating = 5
            });
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var member = db.Members
                    .FirstOrDefault(m => m.UserId == userId);

                if (member == null)
                {
                    TempData["Error"] = "Member profile not found.";
                    return RedirectToAction("Index", "Home");
                }

                // Check if member already reviewed this book
                bool alreadyReviewed = db.Feedbacks.Any(
                    f => f.BookId == feedback.BookId
                      && f.MemberId == member.MemberId);

                if (alreadyReviewed)
                {
                    TempData["Error"] = "You have already reviewed this book.";
                    return RedirectToAction("Details", "Books",
                        new { id = feedback.BookId });
                }

                feedback.MemberId = member.MemberId;
                feedback.SubmittedDate = DateTime.Now;
                feedback.IsApproved = false;

                db.Feedbacks.Add(feedback);
                db.SaveChanges();

                TempData["Success"] = "Thank you for your review! It will appear after approval.";
                return RedirectToAction("Details", "Books",
                    new { id = feedback.BookId });
            }

            var book = db.Books.Find(feedback.BookId);
            ViewBag.BookTitle = book?.Title;
            ViewBag.BookId = feedback.BookId;
            return View(feedback);
        }

        // Librarian approves feedback
        [Authorize(Roles = "Librarian")]
        public ActionResult Approve(int id)
        {
            var feedback = db.Feedbacks.Find(id);
            if (feedback == null)
                return HttpNotFound();

            feedback.IsApproved = true;
            db.SaveChanges();

            TempData["Success"] = "Feedback approved.";
            return RedirectToAction("Index");
        }

        // Librarian deletes inappropriate feedback
        [Authorize(Roles = "Librarian")]
        public ActionResult Delete(int id)
        {
            var feedback = db.Feedbacks
                .Include(f => f.Book)
                .Include(f => f.Member)
                .FirstOrDefault(f => f.FeedbackId == id);

            if (feedback == null)
                return HttpNotFound();

            return View(feedback);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Librarian")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();

            TempData["Success"] = "Review removed.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}