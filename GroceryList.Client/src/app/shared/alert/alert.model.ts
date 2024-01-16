export class Alert {
    type: AlertType;
    message: string;

    constructor(type: AlertType, message: string = '') {
        this.type=type;
        this.message=message;
    }
}

export enum AlertType {
    Success = "success",
    Error = "error",
    Info = "info",
    Warning = "warning",
    NoAlert = "noalert"
}