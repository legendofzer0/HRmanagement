﻿using HRmanagement.Data;
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

        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company + "," + SD.Role_User_Management)]
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

                return RedirectToAction("Index");
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

        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company + "," + SD.Role_User_Management)]
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return View("404NotFound");
            }

            // Fetch the active employees for the dropdown
            var employees = await _userManager.Users
                .Where(e => e.Status.Equals(EmpStatus.Active))
                .Select(e => new { e.Id, e.Name })
                .ToListAsync();

            // Get the logged-in user's ID
            var assigneeId = _userService.GetLoggedInUserId(); // Ensure this is synchronous or adjust accordingly
            ViewBag.AssigneeId = assigneeId;
            ViewBag.Employees = new SelectList(employees, "Id", "Name");

            // Find the task and include related employee data (creator and assignee)
            var getTask = await _db.Tasks
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (getTask == null)
            {
                return View("404NotFound");
            }

            return  View(getTask);
        }

        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company + "," + SD.Role_User_Management)]
        [HttpPost]
        public async Task<IActionResult> View(TaskGiven obj)
        {
            return View();
        }
    }
}