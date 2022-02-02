
namespace Puma.Models
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
