
export const userColumnsConstants = {
  /** общий массив столбцов для хэдера */
  displayedColumns: [
    'login', 'role', 'fio', 'organization','innOrganization',
    'addressOrganization', 'email', 'phone', 'state', 'actions'
  ],
  /** описание полей */
  labelColumns: [
    'Логин сотрудника',
    'Роль сотрудника',
    'ФИО сотрудника',
    'Организация',
    'ИНН организации',
    'Адрес организации',
    'Почта',
    'Мобильный телефон',
    'Состояние'
  ],
  /** описание состояний */
  states: [
    'Заблокированный',
    'Отклоненный',
    'Зарегистрированный пользователь',
    'Вновь заведенный пользователь'
  ]
} as const;