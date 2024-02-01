using GroceryList.Application.Abstractions;
using GroceryList.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace GroceryList.Infrastructure.Services;
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpOptions _smtpConfig;

    public EmailService(IConfiguration configuration, IOptions<SmtpOptions> smtpOptions)
    {
        _configuration = configuration;
        _smtpConfig = smtpOptions.Value;
    }

    public async Task SendTokenEmailAsync(string userEmail, string token)
    {
        string toEmail = userEmail;
        string subject = "Test Email";
        string body = @$"<html><body>
                        <a href=""{_configuration.GetValue<string>("clientUrls:ConfirmEmail")}?token={EncodeTo64(token)}&email={userEmail}"">Confirm Email</a>
                        </body></html>";

        using SmtpClient smtpClient = new(_smtpConfig.Server, _smtpConfig.Port);
        smtpClient.Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);
        smtpClient.EnableSsl = true;

        var mailMessage = new MailMessage(_smtpConfig.FromEmail, toEmail, subject, body);
        await smtpClient.SendMailAsync(mailMessage);
    }

    private static string EncodeTo64(string toEncode)
    {

        byte[] toEncodeAsBytes

              = ASCIIEncoding.ASCII.GetBytes(toEncode);

        string returnValue

              = Convert.ToBase64String(toEncodeAsBytes);

        return returnValue;

    }
}
