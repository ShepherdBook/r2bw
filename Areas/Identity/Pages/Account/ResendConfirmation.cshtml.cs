using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using r2bw.Data;

namespace r2bw.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public ResendConfirmationModel(UserManager<User> userManager, IEmailSender emailSender, ILogger<ResendConfirmationModel> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ResendConfirmationConfirmation");
                }

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

            return Page();
        }
    }
}
