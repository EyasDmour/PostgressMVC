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

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _projectsService.Delete(id);
            return RedirectToAction("Index", "Tasks");
        }

        [HttpPost]
        public async Task<ActionResult> AddMember(int projectId, string username)
        {
            try
            {
                await _projectsService.AddMember(projectId, username);
                return RedirectToAction("Index", "Tasks"); // Redirect back to board
            }
            catch
            {
                // Handle error (e.g. user not found)
                return RedirectToAction("Index", "Tasks");
            }
        }
    }
}
