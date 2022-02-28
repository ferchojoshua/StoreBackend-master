using Microsoft.AspNetCore.Identity;
using Store.Enums;

namespace Store.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string Address { get; set; }
        public UserType UserType { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}