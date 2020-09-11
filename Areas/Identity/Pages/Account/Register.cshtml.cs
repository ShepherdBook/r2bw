using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using r2bw.Data;

namespace r2bw.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            [Required]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Sex (for apparel sizing only)")]
            public string Sex { get; set; }

            [Display(Name = "Size (for apparel sizing only)")]
            public string Size { get; set; }

            [Display(Name = "Shoe Size")]
            public string ShoeSize { get; set; }

            [Display(Name = "Street")]
            public string Street1 { get; set; }

            [Display(Name = "")]
            public string Street2 { get; set; }

            public string City { get; set; }

            [StringLength(2, ErrorMessage = "State must be two letters")]
            public string State { get; set; }

            [StringLength(5)]
            public string Zip { get; set; }

            //public Group Group { get; set; }

            //public int GroupId { get; set; }
        }

        public async void OnGet(string returnUrl = null)
        {
            List<Group> groups = await _context.Groups.Where(g => g.Active).ToListAsync();

            ViewData["Groups"] = groups.Select(g => new SelectListItem(g.Name, g.Id.ToString()));

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new User { 
                    UserName = Input.Email, 
                    Email = Input.Email, 
                    PhoneNumber = Input.PhoneNumber,
                    FirstName = Input.FirstName, 
                    DateOfBirth = Input.DateOfBirth.Date,
                    LastName = Input.LastName,
                    Sex = Input.Sex,
                    Size = Input.Size,
                    ShoeSize = Input.ShoeSize,
                    WaiverSignedOn = DateTimeOffset.Now,
                    Street1 = Input.Street1,
                    Street2 = Input.Street2,
                    City = Input.City,
                    Zip = Input.Zip,
                    //GroupId = Input.GroupId,
                    Active = true,
                    SecurityStamp = Guid.NewGuid().ToString()};
                
                var createResult = await _userManager.CreateAsync(user, Input.Password);

                if (createResult.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Add to user role
                    var authorizeResult = await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("User added to a role.");
                    
                    if (authorizeResult.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code },
                            protocol: Request.Scheme);

                        _logger.LogInformation("Sending confirmation email to user.");
                        await _emailSender.SendEmailAsync(Input.Email, "Welcome!  Please confirm your email",
                            $"<p>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.</p>" +
                            "<p>By confirming an account you are agreeing to the following:</p>" +
                            "<p>I agree and acknowledge that as a member of Running2bwell I know that walking, jogging and running in and volunteering for organized group runs, social events, and races with this club are potentially hazardous activities, which could cause injury or death. I will not participate in any club organized events, group training runs, walks and/or social events, unless I am medically able and properly trained, and by my signature, I certify that I am medically able to perform all activities associated with the club and am in good health, and I am properly trained.  </p>" +
                            "<p>I agree to abide by all rules established by the club, including the right of any official to deny or suspend my participation for any reason whatsoever. I attest that I have read the rules of the club and agree to abide by them.  I assume all risks associated with being a member of this club and participating in club activities which may include: falls, contact with other participants, the effects of the weather, including cold, ice snow, rain, wind, lightning, high heat and/or humidity, traffic and the conditions of the road. All such risks being known and appreciated by me. I understand that bicycles, skateboards, baby joggers, roller skates or roller blades, unleashed animals, and personal music players are not allowed to be used in club organized activities and I agree to abide by this rule.</p>" +
                            "<p>Having read this waiver and knowing these facts and in consideration of Running2bwell accepting my membership, I, for myself and anyone entitled to act on my behalf, waive and release Running2bwell, all club leaders and board members, their representatives and successors from all claims or liabilities of any kind arising out of my participation with the club, even though that liability may arise out of negligence or carelessness on the part of the persons named in this waiver.  I grant permission to all of the foregoing to use my photographs, motion pictures, recordings or any other record for any legitimate promotional purposes for the club.</p>");

                        return LocalRedirect(returnUrl);
                    }
                    else 
                    {
                        string[] errors = authorizeResult.Errors.Select(e => e.Description).ToArray();
                        _logger.LogError($"User \"{user.Email}\" was not added to role \"User\".\n{String.Join('\n', errors)}");
                    }

                    foreach (var error in authorizeResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            List<Group> groups = await _context.Groups.Where(g => g.Active).ToListAsync();
            ViewData["Groups"] = groups.Select(g => new SelectListItem(g.Name, g.Id.ToString()));
            return Page();
        }
    }
}
