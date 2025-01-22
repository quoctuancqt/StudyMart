import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'

import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form'
import { Button } from '@/components/ui/button'
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Textarea } from '@/components/ui/textarea'

const formSchema = z.object({
    firstName: z.string().min(1, { message: 'First name is required' }),
    lastName: z.string().min(1, { message: 'Last name is required' }),
    email: z.string().email({ message: 'Invalid email address' }),
    address: z.string().min(1, { message: 'Address is required' }),
    address2: z.string().optional(),
})

export default function BillingInfoForm({ onSubmitCallback }: { onSubmitCallback: (formValues: z.infer<typeof formSchema>) => Promise<void> }) {
    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            firstName: '',
            lastName: '',
            email: '',
            address: '',
            address2: '',
        },
    })

    async function onSubmit(values: z.infer<typeof formSchema>) {
        await onSubmitCallback(values);
    }

    return (
        <Card className="mt-16 rounded-lg border px-4 py-6 sm:p-6 lg:col-span-7 lg:mt-0 lg:p-8">
            <CardHeader>
                <CardTitle className="text-2xl">Billing Information</CardTitle>
                <CardDescription>
                    Please fill out the form below and we will get back to you shortly.
                </CardDescription>
            </CardHeader>
            <CardContent>
                <Form {...form}>
                    <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
                        <div className="grid gap-4">
                            <FormField
                                control={form.control}
                                name="firstName"
                                render={({ field }) => (
                                    <FormItem className="grid gap-2">
                                        <FormLabel htmlFor="firstName">First Name</FormLabel>
                                        <FormControl>
                                            <Input
                                                id="firstName"
                                                placeholder="John"
                                                type="text"
                                                autoComplete="firstName"
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="lastName"
                                render={({ field }) => (
                                    <FormItem className="grid gap-2">
                                        <FormLabel htmlFor="lastName">Last Name</FormLabel>
                                        <FormControl>
                                            <Input
                                                id="lastName"
                                                placeholder="Doe"
                                                type="text"
                                                autoComplete="lastName"
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="email"
                                render={({ field }) => (
                                    <FormItem className="grid gap-2">
                                        <FormLabel htmlFor="email">Email</FormLabel>
                                        <FormControl>
                                            <Input
                                                id="email"
                                                placeholder="johndoe@mail.com"
                                                type="email"
                                                autoComplete="email"
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="address"
                                render={({ field }) => (
                                    <FormItem className="grid gap-2">
                                        <FormLabel htmlFor="address">Address</FormLabel>
                                        <FormControl>
                                            <Textarea
                                                id="address"
                                                placeholder="Your primary address"
                                                autoComplete="off"
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="address2"
                                render={({ field }) => (
                                    <FormItem className="grid gap-2">
                                        <FormLabel htmlFor="address2">Address 2</FormLabel>
                                        <FormControl>
                                            <Textarea
                                                id="address2"
                                                placeholder="Your secondary address"
                                                autoComplete="off"
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <Button type="submit" className="w-full">
                                Checkout
                            </Button>
                        </div>
                    </form>
                </Form>
            </CardContent>
        </Card>
    )
}
