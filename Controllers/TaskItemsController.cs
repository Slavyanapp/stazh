using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScienceTeamsApp.Data;
using ScienceTeamsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScienceTeamsApp.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public TaskItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TaskItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TaskItems.Include(t => t.AssignedUser).Include(t => t.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TaskItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .Include(t => t.AssignedUser)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItems/Create
        public IActionResult Create()
        {
            ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: TaskItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Deadline,Status,TeamId,AssignedUserId")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskItem);
                var currentUserId = _userManager.GetUserId(User);

                var log = new ActivityLog
                {
                    Action = "Create Task",
                    Description = $"Created task '{taskItem.Title}' with status {taskItem.Status}",
                    Timestamp = DateTime.Now,
                    UserId = currentUserId
                };

                _context.Add(log);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "UserName", taskItem.AssignedUserId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", taskItem.TeamId);
            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            // FORBIDDEN FOR OTHER USERS
            if (taskItem.AssignedUserId != currentUserId)
            {
                return Forbid();
            }

            ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "UserName", taskItem.AssignedUserId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", taskItem.TeamId);
            return View(taskItem);
        }

        // POST: TaskItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Deadline,Status,TeamId,AssignedUserId")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    var currentUserId = _userManager.GetUserId(User);
                    var log = new ActivityLog
                    {
                        Action = "Edit Task",
                        Description = $"Task Editted: '{taskItem.Title}'",
                        Timestamp = DateTime.Now,
                        UserId = currentUserId
                    };
                    _context.Add(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "UserName", taskItem.AssignedUserId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", taskItem.TeamId);
            return View(taskItem);
        }

        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .Include(t => t.AssignedUser)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (taskItem.AssignedUserId != currentUserId)
            {
                return Forbid();
            }

            return View(taskItem);
        }

        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                var currentUserId = _userManager.GetUserId(User);
                var log = new ActivityLog
                {
                    Action = "Delete Task",
                    Description = $"Task Deleted: '{taskItem.Title}'",
                    Timestamp = DateTime.Now,
                    UserId = currentUserId
                };
                _context.Add(log);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
