import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { regExpConstants } from '../constants/regexp.patterns.constants';
import { snils } from '../validators/snils.validator';
import { inn } from '../validators/inn.validator';


@Injectable({
  providedIn: 'root'
})
export class FormBuilderService {  
  constructor(private formBuilder: FormBuilder) { 
  }
  

  /**
   * Генерация формы для создания новой заявки 
   * через панель оператора, с незаполненными полями формы.
   */
  generateFormForCreatingNewOrder(): FormGroup {
    return this.formBuilder.group({
      snils: [
        '', 
        [
          snils,
        ], 
      ],
      fio: [
        '', 
        [
          Validators.required, 
          Validators.minLength(6), 
          Validators.maxLength(100)
        ], 
      ],
      contactData: [
        '', 
        [
          Validators.required, 
          Validators.minLength(10), 
          Validators.maxLength(256)
        ], 
      ],
      type: [
        '', 
        [
          Validators.required,
        ], 
      ],
      documents: [
        '', 
      ],
    });
  }

  /**
   * Генерация формы для создания нового пользователя 
   * через панель администратора, с незаполненными полями формы.
   */
  generateFormForNewUserRegisterByAdmin(): FormGroup {
    return this.formBuilder.group({
      userName: [
        '', 
        [
          Validators.required, 
          Validators.minLength(6), 
          Validators.maxLength(100), 
          Validators.pattern(regExpConstants.userNamePattern)
        ], 
      ],
      userRole: [
        '', 
        Validators.required,
      ],
      fio: [
        '', 
        [
          Validators.required,
          Validators.minLength(6), 
          Validators.maxLength(100),
        ], 
      ],
      organization: [
        '', 
        [
          Validators.required,
          Validators.minLength(10), 
          Validators.maxLength(100),
        ], 
      ],
      inn: [
        '', 
        [
          inn
        ],
      ],
      address: [
        '', 
        [
          Validators.required,
          Validators.minLength(10), 
          Validators.maxLength(256)
        ],
      ],
      email: [
        '', 
        [
          Validators.required, 
          Validators.minLength(4), 
          Validators.maxLength(100), 
          Validators.pattern(regExpConstants.emailPattern)
        ], 
      ],
      phoneNumber: [
        '', 
        [
          Validators.required, 
          Validators.minLength(11), 
          Validators.maxLength(100)
        ], 
      ],
      password: [
        '', 
        [
          Validators.required, 
          Validators.minLength(8), 
          Validators.maxLength(20), 
          Validators.pattern(regExpConstants.passwordPattern)
        ], 
      ],
      state: ['', Validators.required],
    });
  }

  /**
   * Генерация формы для создания нового пользователя, 
   * с незаполненными полями формы.
   */
  generateFormForNewUserRegister(): FormGroup {
    return this.formBuilder.group({
      userName: [
        '', 
        [
          Validators.required, 
          Validators.minLength(6), 
          Validators.maxLength(100), 
          Validators.pattern(regExpConstants.userNamePattern)
        ], 
      ],
      fio: [
        '', 
        [
          Validators.required,
          Validators.minLength(6), 
          Validators.maxLength(100),
        ], 
      ],
      email: [
        '', 
        [
          Validators.required, 
          Validators.minLength(4), 
          Validators.maxLength(100), 
          Validators.pattern(regExpConstants.emailPattern)
        ], 
      ],
      organization: [
        '', 
        [
          Validators.required,
          Validators.minLength(10), 
          Validators.maxLength(100),
        ], 
      ],
      inn: [
        '', 
        [
          inn
        ],
      ],
      address: [
        '', 
        [
          Validators.required,
          Validators.minLength(10), 
          Validators.maxLength(256)
        ],
      ],
      phoneNumber: [
        '', 
        [
          Validators.required, 
          Validators.minLength(11), 
          Validators.maxLength(100)
        ], 
      ],
      password: [
        '', 
        [
          Validators.required, 
          Validators.minLength(8), 
          Validators.maxLength(20), 
          Validators.pattern(regExpConstants.passwordPattern)
        ], 
      ],
    });
  }

}
