import { UserRoles } from "../enums/userRoles";
import { UserStates } from "../enums/userStates";

export interface User {
    userName: string;
    userRole: UserRoles;
    fio: string;
    organization: string;
    inn: string;
    address: string;
    email: string;
    phoneNumber: string;
    password: string;
    status: UserStates;
}