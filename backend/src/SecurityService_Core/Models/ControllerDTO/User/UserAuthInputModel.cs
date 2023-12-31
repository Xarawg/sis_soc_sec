using System.ComponentModel.DataAnnotations;

namespace SecurityService_Core.Models.ControllerDTO.User
{
    /// <summary>
    /// Входная модель аутентификации пользователя
    /// </summary>
    public class UserAuthInputModel
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <example>SuperAdmin</example>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        /// <example>SuperAdmin</example>
        [Required]
        public string Password { get; set; }
    }
}