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
        public async Task<ActionResult> Index()
        {
            var projects = await _projectsService.GetAll();
            var allTasks = await _tasksService.GetAll();

            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") 
                           ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                           ?? User.FindFirst("sub");
            
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                ViewBag.CurrentUserId = userId;
            }

            var model = projects.Select(p => new ProjectBoardViewModel
            {
                Project = p,
                Tasks = allTasks.Where(t => t.ProjectId == p.Id).ToList()
            }).ToList();

            return View(model);
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

        [HttpPost("ToggleComplete/{id}")]
        public async Task<ActionResult> ToggleComplete(int id)
        {
            var task = await _tasksService.GetById(id);
            if (task == null) return NotFound();

            task.IsCompleted = !task.IsCompleted;
            await _tasksService.Update(task);

            return RedirectToAction("Index");
        }

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _tasksService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
