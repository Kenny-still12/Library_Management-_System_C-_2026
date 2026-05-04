using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class ReportsController : Controller
    {
        private LibraryDbContext db = new LibraryDbContext();

        // Dashboard - summary of all stats
        public ActionResult Index()
        {
            ViewBag.TotalBooks = db.Books.Count();
            ViewBag.TotalMembers = db.Members.Count();
            ViewBag.TotalLibraries = db.Libraries.Count();
            ViewBag.ActiveBorrowings = db.BorrowingTransactions
                .Count(t => t.Status == BorrowingStatus.Borrowed);
            ViewBag.OverdueCount = db.BorrowingTransactions
                .Count(t => t.Status == BorrowingStatus.Overdue);
            ViewBag.OutstandingFines = db.BorrowingTransactions
                .Where(t => !t.FinePaid && t.FineAmount > 0)
                .Sum(t => (decimal?)t.FineAmount) ?? 0;

            return View();
        }

        // Overdue books report
        public ActionResult OverdueBooks()
        {
            var overdue = db.BorrowingTransactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .Where(t => t.Status == BorrowingStatus.Overdue
                         || (t.Status == BorrowingStatus.Borrowed
                             && t.DueDate < DateTime.Now))
                .OrderBy(t => t.DueDate)
                .ToList();

            return View(overdue);
        }

        // Most popular books by borrow count
        public ActionResult PopularBooks()
        {
            var popular = db.Books
                .Include(b => b.BorrowingTransactions)
                .OrderByDescending(b => b.BorrowingTransactions.Count)
                .Take(10)
                .ToList();

            return View(popular);
        }

        // Most active members by borrow count
        public ActionResult ActiveMembers()
        {
            var active = db.Members
                .Include(m => m.BorrowingTransactions)
                .OrderByDescending(m => m.BorrowingTransactions.Count)
                .Take(10)
                .ToList();

            return View(active);
        }

        // Monthly borrowing trends
        public ActionResult BorrowingTrends()
        {
            var trends = db.BorrowingTransactions
                .GroupBy(t => new { t.BorrowDate.Year, t.BorrowDate.Month })
                .Select(g => new BorrowingTrendItem
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(t => t.Year)
                .ThenBy(t => t.Month)
                .ToList();

            ViewBag.Trends = trends;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }

    // Simple class to hold trend data — avoids dynamic keyword issues
    public class BorrowingTrendItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }

        public string Period
        {
            get { return Month + "/" + Year; }
        }
    }
}