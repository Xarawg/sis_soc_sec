namespace SecurityService_Core.Models.Enums
{
    /// <summary>
    /// Статус пользователя
    /// </summary>
    public enum UserStatus
    {
        Blocked = -2,
        Declined = -1,
        New = 0,
        Registered = 1
    }
}