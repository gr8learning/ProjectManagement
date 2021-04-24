namespace ProjectManagement.Entities
{
    public class UserDto: BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }

    public class User : UserDto
    {
        public string Password { get; set; }
    }
}
