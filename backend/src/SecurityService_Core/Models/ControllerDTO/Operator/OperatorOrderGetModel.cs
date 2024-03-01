namespace SecurityService_Core.Models.ControllerDTO.Operator
{
    public class OperatorOrderGetModel
    {
        /// <summary>
        /// Размер выводимой выборки данных
        /// </summary>
        public int? LimitRowCount { get; set; } = null;
        /// <summary>
        /// Количество записей, которые нужно вырезать из начала выборки при условии фильтрации по дате создания (по убыванию)
        /// </summary>
        public int? LimitOffset { get; set; } = null;
    }
}
