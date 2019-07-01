namespace Nodester.Data.Dto.UserDtos
{
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string PreviousPassword { get; set; }
        public string NewPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}