using System.ComponentModel.DataAnnotations;
using Store.Entities;

namespace Store.Models.ViewModels
{
    public class AddUserViewModel
    {
        [Required]
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public int RolId { get; set; }

        [Required]
        public ICollection<Almacen> Stores { get; set; }
    }

    public class UpdateMyAccountViewModel
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string SecondLastName { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }
    }

    public class UpdateUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string SecondLastName { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int RolId { get; set; }

        [Required]
        public ICollection<Almacen> Stores { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string UserBrowser { get; set; }

        public string UserSO { get; set; }

        public bool RememberMe { get; set; }
    }
}
