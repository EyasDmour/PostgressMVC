using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostMVC.Data.Service;
using PostMVC.Models;

namespace PostMVC.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectsService _projectsService;
    
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }
        public async Task<ActionResult> Index()
        {
            var projects = await _projectsService.GetAll();
            return View(projects);
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> Create(Projects project)
        {
            if (ModelState.IsValid)
            {
                await _projectsService.Add(project);
                return RedirectToAction("Index");
            }
            return View(project);
        }

    }
}
