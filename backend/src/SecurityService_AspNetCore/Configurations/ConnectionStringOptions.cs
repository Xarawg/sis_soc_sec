namespace SecurityService_AspNetCore.Configurations
{
    /// <summary>
    /// Настройки соединения с базой данных.
    /// </summary>
    public class ConnectionStringOptions
    {
        /// <summary>
        /// Строка соединения.
        /// </summary>
        public string? ConnectionString { get; set; }
    }
}
