using HRmanagement.Data;
using HRmanagement.Data.enums;
using HRmanagement.Models;
using HRmanagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company)]
public class EmployeeController : Controller
{

    private readonly UserManager<EmployeeUser> _userManager;
    private readonly AppDbContext _db;
    private readonly DesignationService _designationService;

    public EmployeeController(UserManager<EmployeeUser> userManager,
        AppDbContext db,
        DesignationService designationService)
    {
        _userManager = userManager;
        _db = db;
        _designationService = designationService;
    }

    public async Task<IActionResult> Index(string? name, EmpStatus? status)
    {
        var employees = _userManager.Users.Include(e => e.designation).AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            employees = employees.Where(e => e.Name.Contains(name));
        }

        if (status.HasValue)
        {
            employees = employees.Where(e => e.Status == status.Value);
        }

        var designationList = await _designationService.GetDesignationListAsync();
        ViewBag.DesignationList = designationList;

        return View(await employees.ToListAsync());
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return View("404NotFound");
        }

        var employee = await _userManager.FindByIdAsync(id);
        if (employee == null)
        {
            return View("404NotFound");
        }
        var designationList = await _designationService.GetDesignationListAsync();

        ViewBag.DesignationList = designationList;
        ViewBag.EmploymentType = GetEmployementType.GetEmployementTypeList();
        ViewBag.MaritalStatusList = GetMaritalStatusList.GetMaritalStatus();

        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, EmployeeUser model)
    {
        var employee = await _userManager.FindByIdAsync(id);
        if (employee == null)
        {
            return View("404NotFound");
        }

        employee.Name = model.Name;
        employee.MaritalStatus = model.MaritalStatus;
        employee.Email = model.Email;
        employee.BaseSalary = model.BaseSalary;
        employee.PhoneNumber = model.PhoneNumber;
        employee.PAN = model.PAN;
        employee.CitizenshipNumber = model.CitizenshipNumber;
        employee.AccountNumber = model.AccountNumber;
        employee.DesignationId = model.DesignationId;
        employee.Type = model.Type;

        var result = await _userManager.UpdateAsync(employee);

        if (result.Succeeded)
        {
            TempData["SucessMessage"] = "Employee Edited Successfully";
            return RedirectToAction("Index"); // Ensure this points to the correct action
        }

        // Handle update errors
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        // Repopulate ViewBags for the dropdowns in case of failure
        ViewBag.DesignationList = await _designationService.GetDesignationListAsync();
        ViewBag.EmploymentType = GetEmployementType.GetEmployementTypeList();
        ViewBag.MaritalStatusList = GetMaritalStatusList.GetMaritalStatus();

        return View(model);
    }

    public async Task<IActionResult> Status(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return View("404NotFound");
        }

        var employee = await _userManager.FindByIdAsync(id);
        if (employee == null)
        {
            return View("404NotFound");
        }
        ViewBag.EmpStatus = GetEmpStatusList.GetEmpStatusLists();

        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Status(string id, EmployeeUser model)
    {
        var employee = await _userManager.FindByIdAsync(id);
        if (employee == null)
        {
            return View("404NotFound");
        }

        employee.Status = model.Status;

        var result = await _userManager.UpdateAsync(employee);

        if (result.Succeeded)
        {
            TempData["SucessMessage"] = "Employee Edited Successfully";
            return RedirectToAction("Index");
        }

        // Handle update errors
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        ViewBag.EmpStatus = GetEmpStatusList.GetEmpStatusLists();
        return View(model);
    }
}