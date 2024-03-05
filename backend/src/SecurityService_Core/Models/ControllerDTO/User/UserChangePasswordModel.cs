using System.ComponentModel.DataAnnotations;

namespace SecurityService_Core.Models.ControllerDTO.User
{
    /// <summary>
    /// Входная модель аутентификации пользователя
    /// </summary>
    public class UserChangePasswordModel
    {
        /// <summary>
        /// Новый пароль пользователя
        /// </summary>
        /// <example>SuperAdmin</example>
        [Required]
        public string Password { get; set; }
    }
}