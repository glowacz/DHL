export interface User{
    name: string;
    email: string;
    token: string;
}

export interface UserFormValues{
    name?: string;
    email?: string;
    sub: string;
}