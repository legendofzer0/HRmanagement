using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using HRmanagement.Models;

namespace HRmanagement.Utility
{
    public class UserService
    {
        private readonly UserManager<EmployeeUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<EmployeeUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // Method to get the currently logged-in user
        public async Task<EmployeeUser> GetLoggedInUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userManager.FindByIdAsync(userId);
        }

        // Method to get the logged-in user's ID
        public string GetLoggedInUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // Method to get the logged-in user's name
        public string GetLoggedInUserName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        // Method to check if a user is authenticated
        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}