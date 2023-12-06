namespace SecurityService_AspNetCore.Configurations
{
    /// <summary>
    /// Настройки web-приложения.
    /// </summary>
    public class ProjectOptions
    {
        /// <summary>
        /// Соединения с базами данных.
        /// </summary>
        public Dictionary<string, ConnectionStringOptions> ConnectionStrings { get; set; } = new Dictionary<string, ConnectionStringOptions>();
    }
}
