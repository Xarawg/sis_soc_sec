export enum UserProcessingAction {
    /** Присвоить пользователю статус "Заблокированный" с ID статуса = - 2 */
    Block = 0,
    /** Присвоить пользователю статус "Отклоненный" с ID статуса = - 1 */
    Decline = 1,
    /** Присвоить пользователю статус "Зарегистрированный пользователь" с ID статуса = 1 */
    Register = 2
}
  