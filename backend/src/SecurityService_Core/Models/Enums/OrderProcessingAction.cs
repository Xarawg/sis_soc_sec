namespace SecurityService_Core.Models.Enums
{
    /// <summary>
    /// Действие с заявкой
    /// </summary>
    public enum OrderProcessingAction
    {
        /// <summary>
        /// Отправить в обработку, изменив статус с 0 на 1
        /// </summary>
        Send = 0,

        /// <summary>
        /// Отменить заявку, сменив статус с 0 или 1 на -2
        /// </summary>
        Decline = 1,

        /// <summary>
        /// Дублировать заявку, сделав её копию под новым ID, но со статусом 0 (вновь заведённая)
        /// </summary>
        Double = 2
    }
}