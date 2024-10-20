using HRmanagement.Models;
using HRmanagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HRmanagement.Data;
using Microsoft.EntityFrameworkCore;
using HRmanagement.Data.enums;

namespace HRmanagement.Controllers
{
	[Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company)]
	public class TimeStorageController : Controller
	{
		private readonly UserManager<EmployeeUser> _userManager;
		private readonly AppDbContext _db;

		public TimeStorageController(UserManager<EmployeeUser> userManager, AppDbContext db)
		{
			_userManager = userManager;
			_db = db;
		}

		public IActionResult Index()
		{
			var timeObj = _db.Timings
				.Include(t => t.User)
				.ThenInclude(e => e.designation)
				.ToList();
			return View(timeObj);
		}

		public async Task<IActionResult> Create()
		{
			var employees = await _userManager.Users
				.Include(e => e.designation)
				.Where(e => e.Status.Equals(EmpStatus.Active))
				.ToListAsync();

			var viewModel = new TimeStorageViewModel
			{
				Employees = employees,
				TimeStorage = new TimeStorage()
			};

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Create(TimeStorageViewModel model)
		{
			// Check if an entry with the same userId and Date already exists
			var existingEntry = await _db.Timings
				.FirstOrDefaultAsync(t => t.userId == model.TimeStorage.userId && t.Date.Date == model.TimeStorage.Date.Date);

			// If an entry is found, return an error message
			if (existingEntry != null)
			{
				TempData["ErrorMessage"] = "Already Entered for today";
				return RedirectToAction("Index");
			}

			// If no duplicate entry is found, proceed with creating the new entry
			var user = await _userManager.FindByIdAsync(model.TimeStorage.userId);
			if (user != null)
			{
				_db.Timings.Add(model.TimeStorage);
				await _db.SaveChangesAsync();
				TempData["SucessMessage"] = "New Time Created";

				return RedirectToAction("Index");
			}

			// If user is not found, repopulate employees and return view
			model.Employees = await _userManager.Users
				.Where(e => e.Status.Equals("Active"))
				.ToListAsync();

			return View(model);
		}
	}
}