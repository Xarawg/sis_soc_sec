import { AbstractControl, ValidationErrors } from "@angular/forms";

/**
 * Проверка на соответствие введённой строки форматированию ИНН - число длинной в 10 или 12 символов.
 * @param control Поле для ввода числа ИНН.
 * @returns результат валидации строки.
 */
export function inn(control: AbstractControl): ValidationErrors | null {
    
    /**
     * Регулярное выражение для определения валидной ИНН строки с условием:
     */
    const pattern: RegExp = /^[\d+]{10,12}$/;

    if (!pattern.test(control.value)) {
        return { snils: true };
    }

    /**
     * Проверка длины. Длина либо 10, либо 12 цифр.
     */
    const stringValue: string = control.value.toString().replace(/\D/g, "");
    if (stringValue.length === 10 || stringValue.length === 12) {
        return null;
    } else {
        return { snils: true };
    }
}
