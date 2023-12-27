namespace SecurityService_Core.Security.Auth
{
    /// <summary>
    /// Опции авторизации
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Время (минуты) блокировки пользователя в случае неуспешного входа в аккаунт
        /// </summary>
        public int BlockTime { get; set; }
        /// <summary>
        /// Количество разрешённых неудачных попыток входа перед блокировкой
        /// </summary>
        public int FailedAttempCountLimit { get; set; }
    }
}
