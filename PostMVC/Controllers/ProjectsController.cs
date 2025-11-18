using Microsoft.AspNetCore.Mvc;
using PostMVC.Data;
using PostMVC.Models;

namespace PostMVC.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly PostMVCContext _context;

        public ProjectsController(PostMVCContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var projects = _context.Projects.ToList();
            return View(projects);
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(Projects project)
        {
            if (ModelState.IsValid)
            {
                project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                project.EndDate   = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);

                _context.Projects.Add(project);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

    }
}
