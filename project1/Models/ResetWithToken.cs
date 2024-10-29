using System.ComponentModel.DataAnnotations;

namespace Project1.Models
{
    public class ResetWithToken
    {
        public string Token { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$",
       ErrorMessage = "Password must be 8 characters long, contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
        public string NewPassword { get; set; }



        [Required(ErrorMessage = "Confirm new password is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
