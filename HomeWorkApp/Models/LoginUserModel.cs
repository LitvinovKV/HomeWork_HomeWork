using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HomeWorkApp.Models
{
    public class LoginUserModel
    {
        [Required]
        [DisplayName("Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}