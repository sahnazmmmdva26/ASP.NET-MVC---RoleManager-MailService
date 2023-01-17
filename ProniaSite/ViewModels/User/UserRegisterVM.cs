using System.ComponentModel.DataAnnotations;

namespace ProniaSite.ViewModels.User
{
    public class UserRegisterVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="zehmet olmasa shifreni tekrarlayin")]
        public string ConfirmPassword { get; set; }

    }
}
