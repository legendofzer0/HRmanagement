using HRmanagement.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRmanagement.Utility
{
    public class DesignationService
    {
        private readonly AppDbContext _db;

        public DesignationService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<SelectListItem>> GetDesignationListAsync()
        {
            return await _db.Designations
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToListAsync();
        }
    }
}