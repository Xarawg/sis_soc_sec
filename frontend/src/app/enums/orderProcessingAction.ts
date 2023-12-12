export enum OrderProcessingAction {
    /** Отправить в обработку, изменив статус с 0 на 1 */
    Send = 0,
    /** Отменить заявку, сменив статус с 0 или 1 на -2 */
    Decline = 1,
    /** Дублировать заявку, сделав её копию под новым ID, но со статусом 0 (вновь заведённая) */
    Double = 2
  }
  