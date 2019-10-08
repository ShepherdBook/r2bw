
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using r2bw.Areas.Identity.Pages.Account;
using r2bw.Data;

namespace r2bw.Models
{
    public class UserRegisterModel : RegisterModel
    {
        public UserRegisterModel(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<RegisterModel> logger, IEmailSender emailSender, ApplicationDbContext context) : base(userManager, signInManager, logger, emailSender, context)
        {
        }
    }
}