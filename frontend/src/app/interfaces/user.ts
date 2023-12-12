import { UserRoles } from "../enums/userRoles";
import { UserStates } from "../enums/userStates";

export interface User {
    userName: string;
    userRole: UserRoles;
    fio: string;
    organization: string;
    innOrganization: string;
    addressOrganization: string;
    email: string;
    phone: string;
    password: string;
    state: UserStates;
}