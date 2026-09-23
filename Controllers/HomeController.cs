using Microsoft.AspNetCore.Mvc;
using ScienceTeamsApp.Data; // Трябва ни за базата
using ScienceTeamsApp.Models;
using System.Diagnostics;
using System.Linq; // Трябва ни за броенето (.Count())

namespace ScienceTeamsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Добавяме връзката с базата

        // Променяме конструктора да приема и базата данни
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // БИЗНЕС ЛОГИКА 4: Събираме статистика за Таблото
            ViewBag.TotalTeams = _context.Teams.Count();
            ViewBag.TotalTasks = _context.TaskItems.Count();
            ViewBag.CompletedTasks = _context.TaskItems.Count(t => t.Status == "Done" || t.Status == "Готова");
            ViewBag.ActiveTasks = _context.TaskItems.Count(t => t.Status != "Done" && t.Status != "Готова");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
