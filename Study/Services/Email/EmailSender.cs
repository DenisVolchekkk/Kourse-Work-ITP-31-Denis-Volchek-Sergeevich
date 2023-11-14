using Microsoft.AspNetCore.Identity.UI.Services;

namespace Study.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //TODO Complete Email Sender
            await Task.CompletedTask;
        }
    }
}
