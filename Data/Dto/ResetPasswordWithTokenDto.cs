using System;

namespace Nodegem.Data.Dto
{
    public class ResetPasswordWithTokenDto
    {
        public Guid UserId { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}