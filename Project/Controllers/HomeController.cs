using Project.Models;
using System.Linq;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private LibraryDbContext db = new LibraryDbContext();

        public ActionResult Index()
        {
            var members = db.GroupMembers.ToList();
            System.Diagnostics.Debug.WriteLine("Member count: " + members.Count);
            ViewBag.GroupMembers = members;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
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