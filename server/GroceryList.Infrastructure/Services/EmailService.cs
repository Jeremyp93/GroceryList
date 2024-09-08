using GroceryList.Application.Abstractions;
using GroceryList.Application.Helpers;
using GroceryList.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace GroceryList.Infrastructure.Services;
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpOptions _smtpConfig;
    private readonly SmtpClient _smtpClient;

    public EmailService(SmtpClient smtpclient, IConfiguration configuration, IOptions<SmtpOptions> smtpOptions)
    {
        _configuration = configuration;
        _smtpConfig = smtpOptions.Value;
        _smtpClient = smtpclient;
    }

    public async Task SendTokenEmailAsync(string userEmail, string token)
    {
        string toEmail = userEmail;
        string subject = "Grocery List - Confirm your email";
        string body = @$"<html><body>
                        <p>Hi</p>
                        <p>Thank you for creating an account on the Grocery List app.<br/>
                        This app is used to create and manage grocery lists, mainly to gain some precious time in the store !</p>

                        <p>To use the app, please <a href=""{_configuration.GetValue<string>("clientUrls:ConfirmEmail")}?token={Base64Helper.EncodeTo64(token)}&email={userEmail}"">confirm your email</a>.</p>
                        <p>Have a great day !</p>
                        <p>Scotex</p>
                        </body></html>";

        var mailMessage = new MailMessage(_smtpConfig.FromEmail, toEmail, subject, body);
        await _smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendForgotPasswordLink(string userEmail, string token)
    {
        string toEmail = userEmail;
        string subject = "Grocery List - Reset your password";
        string body = @$"<html><body>
                        <p>Hi</p>
                        <p>Please click on the link to reset your password.</p>
                        <p><a href=""{_configuration.GetValue<string>("clientUrls:ResetPassword")}?token={Base64Helper.EncodeTo64(token)}&email={userEmail}"">Reset password</a></p>
                        <p>Have a great day !</p>
                        <p>Scotex</p>
                        </body></html>";

        var mailMessage = new MailMessage(_smtpConfig.FromEmail, toEmail, subject, body);
        await _smtpClient.SendMailAsync(mailMessage);
    }
}
