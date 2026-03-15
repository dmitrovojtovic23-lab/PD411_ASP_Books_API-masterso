using System.Net;
using System.Net.Mail;

namespace PD411_Books.BLL.Services
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;

        public EmailService(IConfiguration configuration)
        {
            _smtpHost = configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpUser = configuration["EmailSettings:SmtpUser"] ?? "";
            _smtpPass = configuration["EmailSettings:SmtpPass"] ?? "";
            _fromEmail = configuration["EmailSettings:FromEmail"] ?? _smtpUser;
        }

        public async Task<ServiceResponse> SendConfirmationEmailAsync(string toEmail, string confirmationLink)
        {
            try
            {
                using var client = new SmtpClient(_smtpHost, _smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = "Підтвердження email адреси",
                    Body = $@"
                        <h2>Підтвердження реєстрації</h2>
                        <p>Дякуємо за реєстрацію! Будь ласка, підтвердіть вашу email адресу, перейшовши за посиланням:</p>
                        <p><a href='{confirmationLink}'>Підтвердити email</a></p>
                        <p>Якщо ви не реєструвалися, проігноруйте цей лист.</p>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);

                return new ServiceResponse
                {
                    Message = "Лист з підтвердженням надіслано"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Помилка відправки листа: {ex.Message}"
                };
            }
        }
    }
}
