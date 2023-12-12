import { OrderStates } from "../enums/orderStates";
import { OrderTypes } from "../enums/orderTypes";
import { Docscan } from "./docscan";

export interface Order {
    /** Номер заявки – идентификатор не редактируемый */
    id: string;    
    /** Дата заявки – не редактируется */
    date: string;
    /** Состояние заявки – результат обработки услуги госорганом, не редактируется */
    state: OrderStates;
    /** Статус заявки – системный статус (в таблице ниже), не редактируется */
    status: string;
    /** СНИЛС */
    snils: string;
    /** ФИО заявителя */
    fio: string;
    /** Контактные данные */
    contactData: string;
    /** Тип заявки – выбор: заявление на смену реквизитов в ПФР, запрос мер поддержки */
    type: OrderTypes;
    /** Тело заявки – сгенерированное в XML(JSON) для отправки на сервис СМЭВ */
    body: string;
    /** Ссылки на документы – при подаче заявлений прикрепляются сканы документов, ограничение одного файла 5 Мб */
    documents: Array<Docscan> | null;
    /** Меры поддержки – тело ответа из госоргана при подаче запроса */
    supportMeasures: string;
}
