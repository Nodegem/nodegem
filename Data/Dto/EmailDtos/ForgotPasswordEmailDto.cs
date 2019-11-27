namespace Nodegem.Data.Dto.EmailDtos
{
    public class ForgotPasswordEmailDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string ResetPasswordToken { get; set; }
        public string Host { get; set; }

        public string Url => $"{Host}/reset-forgot-password/{UserId}/{ResetPasswordToken}";
    }
}