using Microsoft.AspNetCore.Mvc;
using PostMVC.Data;

namespace PostMVC.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly PostMVCContext _context;

        public ProjectsController(PostMVCContext context)
        {
            _context = context;
        }
        // GET: ProjectsController
        public ActionResult Index()
        {
            return View();
        }

    }
}
