using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HomeWorkApp.Models
{
    public class RegisterUserModel
    {
        [Required]
        [DisplayName("Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Повторный пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfrim { get; set; }

    }
}