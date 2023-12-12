import { UserRoles } from "../enums/userRoles";
import { UserStates } from "../enums/userStates";

export interface AdminRegistrationInputModel {
    userName: string;
    email: string;
    phoneNumber: string;
    fio: string;
    organization: string;
    inn: string;
    role: UserRoles;
    status: UserStates;
    address: string;
    password: string;
}