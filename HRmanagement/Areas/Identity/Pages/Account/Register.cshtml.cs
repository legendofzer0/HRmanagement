// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using HRmanagement.Data;
using HRmanagement.Data.enums;
using HRmanagement.Models;
using HRmanagement.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRmanagement.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<EmployeeUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<EmployeeUser> _userManager;
        private readonly IUserStore<EmployeeUser> _userStore;
        private readonly IUserEmailStore<EmployeeUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _db;

        public RegisterModel(
            UserManager<EmployeeUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<EmployeeUser> userStore,
            SignInManager<EmployeeUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            AppDbContext db
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public decimal BaseSalary { get; set; }

            [Required]
            public EmpStatus Status { get; set; } = EmpStatus.Active;

            [Required]
            [DisplayName("Marital Status")]
            public MaritialStatus MaritalStatus { get; set; }

            public List<SelectListItem> MaritalStatusList { get; set; }

            [Required]
            [DisplayName("Employement Type")]
            public EmpTypes Type { get; set; }

            public List<SelectListItem> EmployementTypeList { get; set; }

            [DisplayName("PAN Number")]
            public string? PAN { get; set; }

            [DisplayName("Phone Number")]
            public string? PhoneNumber { get; set; }

            [DisplayName("Citizenship Number")]
            public string? CitizenshipNumber { get; set; }

            [DisplayName("Bank Account Number")]
            public string? AccountNumber { get; set; }

            public string? Role { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }

            [Required]
            [DisplayName("Designation")]
            public int DesignationId { get; set; }

            public List<SelectListItem> DesignationList { get; set; }
        }

        private List<SelectListItem> GetMaritalStatusList()
        {
            return Enum.GetValues(typeof(MaritialStatus))
                .Cast<MaritialStatus>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();
        }

        private List<SelectListItem> GetEmployementType()
        {
            return Enum.GetValues(typeof(EmpTypes))
                .Cast<EmpTypes>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();
        }

        private async Task<List<SelectListItem>> GetDesignationListAsync()
        {
            return await _db.Designations
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToListAsync();
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            // Ensure roles exist
            if (!_roleManager.RoleExistsAsync(SD.Role_User_Normal).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Normal));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Admin));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Management));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Company));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Accounts));
            }

            // Initialize Input
            Input = new InputModel
            {
                MaritalStatusList = GetMaritalStatusList(),
                EmployementTypeList = GetEmployementType(),
                DesignationList = await GetDesignationListAsync(),
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }).ToList() // Ensure RoleList is a List<SelectListItem>
            };

            ReturnUrl = returnUrl; // Set return URL
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(); // Get external logins
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Employee");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                // Set user properties based on the Input model
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                // Populate the employee user fields from Input
                user.Name = Input.Name; // Set the Name property
                user.BaseSalary = Input.BaseSalary;
                user.Status = Input.Status;
                user.MaritalStatus = Input.MaritalStatus;
                user.PAN = Input.PAN;
                user.CitizenshipNumber = Input.CitizenshipNumber;
                user.AccountNumber = Input.AccountNumber;
                user.DesignationId = Input.DesignationId;
                user.PhoneNumber = Input.PhoneNumber;

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!String.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_User_Normal);
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private EmployeeUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<EmployeeUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(EmployeeUser)}'. " +
                    $"Ensure that '{nameof(EmployeeUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<EmployeeUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<EmployeeUser>)_userStore;
        }
    }
}