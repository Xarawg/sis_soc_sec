export interface operatorOrderGetModel {
  /** Размер выводимой выборки данных */
  limitRowCount: number;
  /** Количество записей, которые нужно вырезать из начала выборки при условии фильтрации по дате создания (по убыванию) */
  limitOffset: number;
}