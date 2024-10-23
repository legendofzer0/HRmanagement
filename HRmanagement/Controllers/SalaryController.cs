using HRmanagement.Data;
using HRmanagement.Data.enums;
using HRmanagement.Models;
using HRmanagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRmanagement.Controllers
{
	[Authorize(Roles = SD.Role_User_Accounts + "," + SD.Role_User_Company)]
	public class SalaryController : Controller
	{
		private readonly AppDbContext _db;
		private readonly UserManager<EmployeeUser> _usermanager;

		public SalaryController(AppDbContext db, UserManager<EmployeeUser> usermanager)
		{
			_db = db;
			_usermanager = usermanager;
		}

		public IActionResult Index()
		{
			var SalaryQuery = _db.salaries.ToList();
			return View(SalaryQuery);
		}

		public async Task<IActionResult> New()
		{
			var getUsersDetails = await _usermanager.Users
				.Where(e => e.Status.Equals(EmpStatus.Active))
				.ToListAsync();
			ViewBag.Employees = new SelectList(getUsersDetails, "Id", "Name");
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetEmployeeDetails(string id)
		{
			var employee = await _usermanager.Users
				.Where(e => e.Id == id && e.Status.Equals(EmpStatus.Active))
				.Select(e => new
				{
					e.Name,
					e.PhoneNumber,
					e.AccountNumber,
					e.BaseSalary
				})
				.FirstOrDefaultAsync();

			if (employee == null)
			{
				return NotFound();
			}

			return Json(employee); // Return employee details as JSON
		}

		[HttpPost]
		public async Task<IActionResult> New(Salary salary)
		{
			try
			{
				// Save the salary record to the database
				_db.salaries.Add(salary);
				await _db.SaveChangesAsync();

				TempData["SucessMessage"] = "Salary record created successfully!";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				var getUsersDetails = await _usermanager.Users
					.Where(e => e.Status.Equals(EmpStatus.Active))
					.ToListAsync();
				ViewBag.Employees = new SelectList(getUsersDetails, "Id", "Name");
				TempData["Error"] = "Something went wrong!";
				return View(salary);
			}
		}

		public IActionResult More(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var salaryDetail = _db.salaries.FirstOrDefault(s => s.Id == id);
			if (salaryDetail == null)
			{
				return NotFound();
			}
			return View(salaryDetail);
		}
	}
}