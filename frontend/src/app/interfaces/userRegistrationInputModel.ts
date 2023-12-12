import { UserRoles } from "../enums/userRoles";
import { UserStates } from "../enums/userStates";

export interface UserRegistrationInputModel {
    userName: string;
    email: string;
    phoneNumber: string;
    fio: string;
    organization: string;
    inn: string;
    address: string;
    password: string;
}