import axios, {AxiosResponse} from "axios";
import { Client, ClientFormValues } from "../models/client";
import { IOrder, IOrderDisplay } from "../models/order";
import { IInquiry } from "../models/inquiry";
import { User, UserFormValues } from "../models/user";

axios.defaults.baseURL = 'http://localhost:5001/api';
// axios.defaults.headers.common['Authorization'] = `Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlZsYWRhIiwibmFtZWlkIjoiN2E3NDY4ZGItNzJiZi00MGNhLWFkMDItOWIyMzdmMzg3OWM1IiwiZW1haWwiOiJjM0B0ZXN0LmNvbSIsIm5iZiI6MTcwNTYxNTI0NSwiZXhwIjoxNzA2MjIwMDQ1LCJpYXQiOjE3MDU2MTUyNDV9.-uWObBqDQXQC0IyzWK6ua-eklLyuzUmf2k3IrUXe9yY02_Xoy4dAIue4ftKG4RtpjThfeO-exWchgURvENvDgQ`;
// axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('access_token')}`;

const responseBody = <T>(response: AxiosResponse<T>)=> response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Account = {
    current:() => requests.get<User>('/account'),
    login: (client: UserFormValues) => requests.post<User>('/account/login', client),
    register: (client: UserFormValues) => requests.post<User>('/account/register', client),
}

const Orders = {
    post: (order: IOrder) => requests.post<IOrder>('/Order', order),
    get: () => requests.get<IOrderDisplay []>('/Order')
}

const Inquiries = {
    get: () => requests.get<IInquiry []>('inquiries')
}

const Courier = {
    take: (orderId: number, courierId: number) => requests.post<User>(`TakeOrder/${orderId}`, {'courierId': courierId}),
    pickup: (orderId: number, courierId: number) => requests.post<User>(`PickupOrder/${orderId}`, {'courierId': courierId}),
    deliver: (orderId: number, courierId: number) => requests.post<User>(`DeliverOrder/${orderId}`, {'courierId': courierId}),
    cannotDeliver: (orderId: number, courierId: number) => requests.post<User>(`CannotDeliverOrder/${orderId}`, {'courierId': courierId}),
}

const agent = {
    Account,
    Orders,
    Inquiries,
    Courier
}

export default agent;