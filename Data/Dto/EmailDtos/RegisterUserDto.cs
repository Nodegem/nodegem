using System.Web;

namespace Nodester.Data.Dto.EmailDtos
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string EmailConfirmationToken { get; set; }
        public string UserId { get; set; }
        public string Host { get; set; }
        public string Url => $"{Host}/email-confirmation?userId={UserId:N}&token={HttpUtility.UrlEncode(EmailConfirmationToken)}";
    }
}