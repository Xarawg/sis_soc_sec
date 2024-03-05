namespace SecurityService_Core.Models.Enums
{
    /// <summary>
    /// Действие с пользователем
    /// </summary>
    public enum UserProcessingAction
    {
        /// <summary>
        /// Присвоить пользователю статус "Заблокированный" с ID статуса = - 2
        /// </summary>
        Block = 0,

        /// <summary>
        /// Присвоить пользователю статус "Отклоненный" с ID статуса = - 1
        /// </summary>
        Decline = 1,

        /// <summary>
        /// Присвоить пользователю статус "Зарегистрированный пользователь" с ID статуса = 1
        /// </summary>
        Register = 2

    }
}