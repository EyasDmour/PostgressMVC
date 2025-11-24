using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostMVC.Data;
using PostMVC.Data.Service;
using PostMVC.Models;

namespace PostMVC.Controllers
{
    [Authorize]
    [Route("Tasks")]
    public class TasksController : Controller
    {
        private readonly ITasksService _tasksService;
        private readonly IProjectsService _projectsService;

        public TasksController(ITasksService tasksService, IProjectsService projectsService)
        {
            _tasksService = tasksService;
            _projectsService = projectsService;
        }

        [HttpGet("")]
        public async Task<ActionResult> Index(int? projectId)
        {
            ViewBag.Projects = await _projectsService.GetAll();

            if (projectId == null)
            {
                return View(new List<Tasks>());
            }

            var allTasks = await _tasksService.GetAll();
            var tasks = allTasks.Where(t => t.ProjectId == projectId).ToList();
            return View(tasks);
        }

        [HttpGet("Create")]
        public ActionResult Create([FromQuery] int? projectId)
        {
            var model = new Tasks();
            if (projectId.HasValue)
            {
                model.ProjectId = projectId.Value;
            }
            return View(model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tasks task)
        {
            if (!ModelState.IsValid) return View(task);
            await _tasksService.Add(task);
            return RedirectToAction(nameof(Index));
        }
    }
}
