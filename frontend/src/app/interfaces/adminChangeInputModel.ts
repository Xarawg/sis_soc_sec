import { UserRoles } from "../enums/userRoles";
import { UserStates } from "../enums/userStates";

export interface AdminChangeInputModel {
    userName: string;
    email: string;
    phoneNumber: string;
    fio: string;
    organization: string;
    inn: string;
    role: number;
    state: number;
    address: string;
}