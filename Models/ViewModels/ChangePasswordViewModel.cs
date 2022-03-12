using Store.Entities;

namespace Store.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}