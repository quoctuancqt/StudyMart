export interface Order {
    orderId: number;
    orderDate: Date;
    totalAmount: number;
    status?: string; 
}