namespace SecurityService_Core.Models.Enums
{
    /// <summary>
    /// Статус заявки
    /// </summary>
    public enum OrderStatus
    {
        Error = -3,
        DeclinedByUser = -2,
        Declined = -1,
        New = 0,
        Registered = 1,
        Successful = 2
    }
}