import { Table, TableBody, TableCaption, TableCell, TableFooter, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { useFetchOrders } from "@/hooks/useOrders";
import { format } from 'date-fns';

const OrderPage = () => {
    const { data } = useFetchOrders();
    const totalAmount = data?.reduce((sum, invoice) => sum + invoice.totalAmount, 0) || 0;

    return (
        <Table>
            <TableHeader>
                <TableRow>
                    <TableHead className="w-[100px]">Invoice</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead>Date</TableHead>
                    <TableHead className="text-right">Amount</TableHead>
                </TableRow>
            </TableHeader>
            <TableBody>
                {data?.map((invoice) => (
                    <TableRow key={invoice.orderId}>
                        <TableCell className="font-medium">{invoice.orderId}</TableCell>
                        <TableCell className={
                            invoice.status === 'Processing' ? 'text-green-500' :
                                invoice.status === 'Cancelled' ? 'text-red-500' :
                                    invoice.status === 'Pending' ? 'text-orange-500' : ''
                        }>
                            {invoice.status}
                        </TableCell>
                        <TableCell>{format(new Date(invoice.orderDate), 'dd-MM-yyyy HH:mm:ss')}</TableCell>
                        <TableCell className="text-right">${invoice.totalAmount}</TableCell>
                    </TableRow>
                ))}
            </TableBody>
            <TableFooter>
                <TableRow>
                    <TableCell colSpan={3}>Total</TableCell>
                    <TableCell className="text-right">${totalAmount}</TableCell>
                </TableRow>
            </TableFooter>
        </Table>
    );
};

export default OrderPage;