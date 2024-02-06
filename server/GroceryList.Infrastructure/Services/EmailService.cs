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
        string subject = "Grocery List - Confirm your email";
        string body = @$"<html><body>
                        <p>Hi</p>
                        <p>Thank you for creating an account on the Grocery List app.<br/>
                        This app is used to create and manage grocery lists, mainly to gain some precious time in the store !</p>
                        <p>To use the app, please <a href=""{_configuration.GetValue<string>("clientUrls:ConfirmEmail")}?token={EncodeTo64(token)}&email={userEmail}"">confirm your email</a>.</p>
                        <p>Have a great day !</p>
                        <p>Scotex</p>
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
