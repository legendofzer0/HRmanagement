using HRmanagement.Data;
using HRmanagement.Data.enums;
using HRmanagement.Models;
using HRmanagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRmanagement.Controllers
{
    public class TaskGivenController : Controller
    {
        public readonly AppDbContext _db;
        private readonly UserManager<EmployeeUser> _userManager;
        private readonly UserService _userService;

        public TaskGivenController(AppDbContext db,
            UserManager<EmployeeUser> userManager,
            UserService userService)
        {
            _db = db;
            _userManager = userManager;
            _userService = userService;
        }

        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company)]
        public IActionResult Index()
        {
            var tasks = _db.Tasks
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .ToList();

            return View(tasks);
        }

        [Authorize(Roles = SD.Role_User_Management)]
        public IActionResult IndexManagement()
        {
            var assigneeId = _userService.GetLoggedInUserId();
            if (assigneeId == null)
            {
                return NotFound();
            }
            var tasks = _db.Tasks
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .Where(t => t.CreatedBy == assigneeId)
                .ToList();
            return View(tasks);
        }

        [Authorize(Roles = SD.Role_User_Company + "," + SD.Role_User_Management)]
        public async Task<IActionResult> Create()
        {
            var employees = await _userManager.Users
                .Where(e => e.Status.Equals(EmpStatus.Active))
                .Select(e => new { e.Id, e.Name })
                .ToListAsync();
            var assigneeId = _userService.GetLoggedInUserId();
            ViewBag.AssigneeId = assigneeId;
            ViewBag.Employees = new SelectList(employees, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskGiven obj)
        {
            // Automatically assign the logged-in user as the creator
            obj.CreatedBy = _userService.GetLoggedInUserId();

            if (obj != null)
            {
                // Add the new task to the database
                _db.Tasks.Add(obj);
                await _db.SaveChangesAsync();

                // Success message
                TempData["SucessMessage"] = "New Task Created Sucessfully";
                return RedirectToAction("IndexManagement");
            }

            // If something went wrong, refill the ViewBag and return the view with an error message
            var employees = await _userManager.Users
                .Where(e => e.Status.Equals(EmpStatus.Active))
                .Select(e => new { e.Id, e.Name })
                .ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "Name");
            ViewBag.AssigneeId = _userService.GetLoggedInUserId();
            TempData["ErrorMessage"] = "Failed to create the task. Please check the input data.";
            return View();
        }

        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company + "," + SD.Role_User_Management + "," + SD.Role_User_Normal)]
        public async Task<IActionResult> ViewMore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employees = await _userManager.Users
               .Where(e => e.Status.Equals(EmpStatus.Active))
               .Select(e => new { e.Id, e.Name })
               .ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "Name");
            ViewBag.AssigneeId = _userService.GetLoggedInUserId();

            var tasks = await _db.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.Creator)
                .FirstOrDefaultAsync(t => t.Id == id);
            return View(tasks);
        }

        [Authorize(Roles = SD.Role_User_Company + "," + SD.Role_User_Management)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employees = await _userManager.Users
               .Where(e => e.Status.Equals(EmpStatus.Active))
               .Select(e => new { e.Id, e.Name })
               .ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "Name");

            var tasks = await _db.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.Creator)
                .FirstOrDefaultAsync(t => t.Id == id);
            return View(tasks);
        }

        [Authorize(Roles = SD.Role_User_Company + "," + SD.Role_User_Management)]
        [HttpPost]
        public async Task<IActionResult> Edit(TaskGiven obj)
        {
            try
            {
                _db.Tasks.Update(obj);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Task Edited Successfully";
                return RedirectToAction("IndexManagement");
            }
            catch (Exception ex)
            {
                // Re-populate the employee dropdown if validation fails
                var employees = await _userManager.Users
                    .Where(e => e.Status == EmpStatus.Active)
                    .Select(e => new { e.Id, e.Name })
                    .ToListAsync();
                ViewBag.Employees = new SelectList(employees, "Id", "Name");

                TempData["Error"] = "Something Went Wrong";
                return View(obj);
            }
        }
    }
}