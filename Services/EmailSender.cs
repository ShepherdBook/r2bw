using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;

namespace r2bw.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            this.logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            SecretBundle secret = await keyVaultClient.GetSecretAsync("https://r2bw-alpha-key-vault.vault.azure.net/secrets/SendGridKey")
                .ConfigureAwait(false);

            logger.LogInformation($"Using secret: {secret.Value}");

            await Execute(secret.Value, subject, message, email);
        }

        private Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-reply@running2bwell.org", "Running2BWell"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}