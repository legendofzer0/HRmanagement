using HRmanagement.Data;
using HRmanagement.Models;
using HRmanagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HRmanagement.Controllers
{
    [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Company)]
    public class DesignationController : Controller
    {
        private readonly AppDbContext _db;

        public DesignationController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Designation> objDesignation = _db.Designations.ToList();
            return View(objDesignation);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Designation obj)
        {
            if (ModelState.IsValid)
            {
                _db.Designations.Add(obj);
                _db.SaveChanges();
                TempData["SucessMessage"] = "Designation Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id <= 0 )
            {
                return View("404NotFound");
            }
            Designation? designationFromDb = _db.Designations.Find(id);
            if (designationFromDb == null)
            {
                return View("404NotFound");
            }
            return View(designationFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Designation obj)
        {
            if (ModelState.IsValid)
            {
                _db.Designations.Update(obj);
                _db.SaveChanges();
                TempData["SucessMessage"] = "Designation Edited Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == 0)
            {
                return View("404NotFound");
            }
            Designation? designationFromDb = _db.Designations.Find(id);
            if (designationFromDb == null)
            {
                return View("404NotFound");
            }
            return View(designationFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Designation? obj = _db.Designations.Find(id);
            if (obj == null)
            {
                return View("404NotFound");
            }
            _db.Designations.Remove(obj);
            _db.SaveChanges();
            TempData["SucessMessage"] = "Designation Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}