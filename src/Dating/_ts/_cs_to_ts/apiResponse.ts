export class ApiResponse<T> {
    public success: boolean;
    public errorCode: string;
    public value: T;
}