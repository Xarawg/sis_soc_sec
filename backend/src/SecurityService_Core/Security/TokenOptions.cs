namespace SecurityService_Core.Security
{
    /// <summary>
    /// Настройки для работы с токенами
    /// </summary>
    public class TokenOptions
    {
        /// <summary>
        /// Список доступных хостов для разрешенного доступа
        /// </summary>
        public string[] AllowedAudiences { get; set; } = null!;
        /// <summary>
        /// Издатель токена
        /// </summary>
        public string Issuer { get; set; } = null!;
        /// <summary>
        /// Время истечения токена доступа
        /// </summary>
        public long AccessTokenExpiration { get; set; }
        /// <summary>
        /// Время истечения токена обновления
        /// </summary>
        public long RefreshTokenExpiration { get; set; }
        /// <summary>
        /// Секретный ключ для симметричного шифрования
        /// </summary>
        public string Secret { get; set; } = null!;
    }
}
