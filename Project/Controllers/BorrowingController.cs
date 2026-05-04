using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project.Models;

namespace Project.Controllers
{
    [Authorize]
    public class BorrowingController : Controller
    {
        private LibraryDbContext db = new LibraryDbContext();

        // Show transactions — librarian sees all, member sees only theirs
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var transactions = db.BorrowingTransactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .AsQueryable();

            if (User.IsInRole("Member"))
            {
                var member = db.Members
                    .FirstOrDefault(m => m.UserId == userId);
                if (member != null)
                {
                    transactions = transactions
                        .Where(t => t.MemberId == member.MemberId);
                }
            }

            // Auto-update overdue status
            var overdues = transactions
                .Where(t => t.Status == BorrowingStatus.Borrowed
                         && t.DueDate < DateTime.Now)
                .ToList();

            foreach (var t in overdues)
            {
                t.Status = BorrowingStatus.Overdue;
                var daysLate = (DateTime.Now - t.DueDate).Days;
                t.FineAmount = daysLate * 1.00m;
            }
            if (overdues.Any())
                db.SaveChanges();

            return View(transactions.OrderByDescending(t => t.BorrowDate).ToList());
        }

        // Member borrows a book
        [Authorize(Roles = "Member")]
        public ActionResult Borrow(int bookId)
        {
            var userId = User.Identity.GetUserId();
            var member = db.Members.FirstOrDefault(m => m.UserId == userId);

            if (member == null)
            {
                TempData["Error"] = "Member profile not found.";
                return RedirectToAction("Index", "Books");
            }

            var book = db.Books.Find(bookId);
            if (book == null || !book.IsAvailable)
            {
                TempData["Error"] = "This book is not available.";
                return RedirectToAction("Index", "Books");
            }

            // Check borrowing limit
            int currentBorrows = db.BorrowingTransactions
                .Count(t => t.MemberId == member.MemberId
                         && (t.Status == BorrowingStatus.Borrowed
                          || t.Status == BorrowingStatus.Overdue));

            if (currentBorrows >= 5)
            {
                TempData["Error"] = "You have reached the borrowing limit of 5 books.";
                return RedirectToAction("Index", "Books");
            }

            var transaction = new BorrowingTransaction
            {
                BookId = book.BookId,
                MemberId = member.MemberId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                Status = BorrowingStatus.Borrowed,
                RenewalCount = 0,
                FineAmount = 0
            };

            db.BorrowingTransactions.Add(transaction);
            book.IsAvailable = false;
            db.SaveChanges();

            TempData["Success"] = "You borrowed '" + book.Title
                + "'. Due back by " + transaction.DueDate.ToShortDateString();
            return RedirectToAction("Index");
        }

        // Member renews a book
        [Authorize(Roles = "Member")]
        public ActionResult Renew(int id)
        {
            var transaction = db.BorrowingTransactions
                .Include(t => t.Book)
                .FirstOrDefault(t => t.TransactionId == id);

            if (transaction == null)
                return HttpNotFound();

            if (transaction.RenewalCount >= 2)
            {
                TempData["Error"] = "Maximum renewals (2) reached for this book.";
                return RedirectToAction("Index");
            }

            if (transaction.Status == BorrowingStatus.Overdue)
            {
                TempData["Error"] = "Overdue books cannot be renewed. Please return first.";
                return RedirectToAction("Index");
            }

            transaction.DueDate = transaction.DueDate.AddDays(14);
            transaction.RenewalCount++;
            db.SaveChanges();

            TempData["Success"] = "Renewed! New due date: "
                + transaction.DueDate.ToShortDateString();
            return RedirectToAction("Index");
        }

        // Librarian returns a book
        [Authorize(Roles = "Librarian")]
        public ActionResult Return(int id)
        {
            var transaction = db.BorrowingTransactions
                .Include(t => t.Book)
                .FirstOrDefault(t => t.TransactionId == id);

            if (transaction == null)
                return HttpNotFound();

            transaction.ReturnDate = DateTime.Now;
            transaction.Status = BorrowingStatus.Returned;

            // Calculate final fine if overdue
            if (DateTime.Now > transaction.DueDate)
            {
                var daysLate = (DateTime.Now - transaction.DueDate).Days;
                transaction.FineAmount = daysLate * 1.00m;
            }

            transaction.Book.IsAvailable = true;
            db.SaveChanges();

            string msg = "Book returned successfully.";
            if (transaction.FineAmount > 0)
                msg += " Fine: $" + transaction.FineAmount.ToString("0.00");

            TempData["Success"] = msg;
            return RedirectToAction("Index");
        }

        // Librarian marks fine as paid
        [Authorize(Roles = "Librarian")]
        public ActionResult MarkFinePaid(int id)
        {
            var transaction = db.BorrowingTransactions.Find(id);
            if (transaction == null)
                return HttpNotFound();

            transaction.FinePaid = true;
            db.SaveChanges();

            TempData["Success"] = "Fine marked as paid.";
            return RedirectToAction("Index");
        }

        // Member's full borrowing history
        [Authorize(Roles = "Member")]
        public ActionResult History()
        {
            var userId = User.Identity.GetUserId();
            var member = db.Members.FirstOrDefault(m => m.UserId == userId);

            if (member == null)
                return RedirectToAction("Index", "Home");

            var history = db.BorrowingTransactions
                .Include(t => t.Book)
                .Where(t => t.MemberId == member.MemberId)
                .OrderByDescending(t => t.BorrowDate)
                .ToList();

            return View(history);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}