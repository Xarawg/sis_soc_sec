import { AbstractControl, ValidationErrors } from "@angular/forms";

/**
 * Проверка на соответствие введённой строки форматированию 
 * СНИЛС - число длинной в 11 символов в формате XXX-XXX-XXX YY, 
 * где YY - контрольная сумма предыдущих чисел, сложенных по определённому алгоритму.
 * @param control Поле для ввода числа СНИЛС.
 * @returns результат валидации строки.
 */
export function snils(control: AbstractControl): ValidationErrors | null {
    const pattern: RegExp = /^\d{3}-\d{3}-\d{3} \d{2}$/;

    if (!pattern.test(control.value)) {
        return { snils: true };
    }

    /**
     * Текстовое представление числа после удаления лишних символов.
     * Строка должна быть 11 символов в длину.
     */
    const stringValue: string = control.value.toString().replace(/\D/g, "");
    if (stringValue.length != 11) {
        return { snils: true };
    }

    /**
     * Целочисленное представление числа после удаления лишних символов.
     * Если переменная не является числом - это ошибка валидации.
     */
    const numericValue: number =+ stringValue;
    if (isNaN(numericValue)) {
        return { snils: true };
    }

    /**
     * Контрольная сумма не проверяется, 
     * если число меньше или равно номеру 001-001-998.
     */
    if (numericValue <= 1001998) {
        return null;
    }

    /**
     * Проверка алгоритма суммы чисел СНИЛС, соответствующих последним двум числам
     */
    let sum: number = 
        + stringValue[0] * 9 + 
        + stringValue[1] * 8 + 
        + stringValue[2] * 7 + 
        + stringValue[3] * 6 + 
        + stringValue[4] * 5 + 
        + stringValue[5] * 4 + 
        + stringValue[6] * 3 + 
        + stringValue[7] * 2 + 
       + stringValue[8];

    let lastTwoDigits: string = stringValue[9]+ stringValue[10];
    let lastDigitsOfSum: string = (sum % 101).toString();
    if (lastDigitsOfSum.length === 1) {
        lastDigitsOfSum = '0' + lastDigitsOfSum;
    }
    if (lastDigitsOfSum.length > 2) {
        lastDigitsOfSum = lastDigitsOfSum[1] + lastDigitsOfSum[2];
    }
    
    if (lastDigitsOfSum === lastTwoDigits) {
        return null;
    }

    return { snils: true };
}
