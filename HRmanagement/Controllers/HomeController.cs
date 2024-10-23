using HRmanagement.Data;
using HRmanagement.Data.enums;
using HRmanagement.Models;
using HRmanagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HRmanagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;
        private readonly UserManager<EmployeeUser> _userManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext db, UserManager<EmployeeUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company + "," + SD.Role_User_Management + "," + SD.Role_User_Accounts + "," + SD.Role_User_Normal)]
        public IActionResult Index()
        {
            DateTime today = DateTime.Today;
            var attendanceCount = _db.Timings
                                    .Where(e => EF.Functions.DateDiffDay(today, e.Date) == 0)
                                    .Count();
            var userCount = _userManager.Users.Where(u => u.Status == EmpStatus.Active).Count();
            var tasks = _db.Tasks;

            // Calculate the attendance and absence
            @ViewBag.attendanceCount = attendanceCount;
            @ViewBag.absent = Math.Abs(userCount - attendanceCount);

            // Group tasks by severity and get the counts
            var taskSeverityCounts = tasks
                .GroupBy(t => t.severity)
                .Select(g => new { Severity = g.Key, Count = g.Count() })
                .ToDictionary(g => g.Severity, g => g.Count);

            @ViewBag.taskSeverityCounts = taskSeverityCounts;
            // Group tasks by severity and get the counts
            var taskProcessCounts = tasks
                .GroupBy(t => t.taskStatus)
                .Select(g => new { TaskStatus = g.Key, Count = g.Count() })
                .ToDictionary(g => g.TaskStatus, g => g.Count);

            @ViewBag.taskProcessCounts = taskProcessCounts;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}