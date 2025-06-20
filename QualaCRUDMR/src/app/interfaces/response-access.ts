export interface Token{
    expiration:string,
    token:string
}

export interface ResponseHttp<T>{
    result: string;
    data: T;
    message: string;
}