import { Menu, ShoppingBag } from "lucide-react";

import { cn } from "@/lib/utils";

import { Button, buttonVariants } from "@/components/ui/button";

import {
    navigationMenuTriggerStyle,
} from "@/components/ui/navigation-menu";

import {
    Sheet,
    SheetContent,
    SheetHeader,
    SheetTitle,
    SheetTrigger,
} from "@/components/ui/sheet";

import { ModeToggle } from "@/components/mode-toggle";

import AuthAction from "./AuthAction";
import { Link } from "react-router";
import { useCartStore } from "@/features/carts/cartsStore";
import { useAuth } from "react-oidc-context";

const Navbar = () => {
    const { totalItems } = useCartStore();
    const auth = useAuth();

    const handleGotoCart = () => {
        window.location.href = '/cart';
    }

    return (
        <section className="py-4">
            <div className="container">
                <nav className="hidden justify-between lg:flex">
                    <div className="flex items-center gap-6">
                        <div className="flex items-center gap-2">
                            <img src="https://shadcnblocks.com/images/block/block-1.svg" className="w-8" alt="logo" />
                            <span className="text-xl font-bold">Study Mart</span>
                        </div>
                        <div className="flex items-center">
                            <Link to='/' className={cn(
                                "text-muted-foreground",
                                navigationMenuTriggerStyle,
                                buttonVariants({
                                    variant: "ghost",
                                }),
                            )}>Home</Link>
                            <Link to='/cart' className={cn(
                                "text-muted-foreground",
                                navigationMenuTriggerStyle,
                                buttonVariants({
                                    variant: "ghost",
                                }),
                            )}>Cart</Link>
                            {auth?.isAuthenticated && <Link to='/orders' className={cn(
                                "text-muted-foreground",
                                navigationMenuTriggerStyle,
                                buttonVariants({
                                    variant: "ghost",
                                }),
                            )}>Orders</Link>}
                            <Link to="/about" className={cn(
                                "text-muted-foreground",
                                navigationMenuTriggerStyle,
                                buttonVariants({
                                    variant: "ghost",
                                }),
                            )}>About</Link>
                        </div>
                    </div>
                    <div className="flex gap-2">
                        <Button variant='outline' onClick={handleGotoCart} className='flex items-center rounded-full px-4 py-2'>
                            <ShoppingBag size={20} />
                            <span className='ml-2 text-sm font-medium'>{totalItems}</span>
                        </Button>
                        <ModeToggle />
                        <AuthAction />
                    </div>
                </nav>

                <div className="block lg:hidden">
                    <div className="flex items-center justify-between">
                        <div className="flex items-center gap-2">
                            <img src="https://shadcnblocks.com/images/block/block-1.svg" className="w-8" alt="logo" />
                            <span className="text-xl font-bold">Study Mart</span>
                        </div>
                        <Sheet>
                            <SheetTrigger asChild>
                                <Button variant="outline" size="icon">
                                    <Menu className="size-4" />
                                </Button>
                            </SheetTrigger>
                            <SheetContent className="overflow-y-auto">
                                <SheetHeader>
                                    <SheetTitle>
                                        <div className="flex items-center gap-2">
                                            <img
                                                src="https://shadcnblocks.com/images/block/block-1.svg"
                                                className="w-8"
                                                alt="logo"
                                            />
                                            <span className="text-xl font-bold">Study Mart</span>
                                        </div>
                                    </SheetTitle>
                                </SheetHeader>
                                <div className="mb-8 mt-8 flex flex-col gap-4">
                                    <Link to='/' className={cn(
                                        "text-muted-foreground",
                                        navigationMenuTriggerStyle,
                                        buttonVariants({
                                            variant: "ghost",
                                        }),
                                    )}>Home</Link>
                                    <Link to='/cart' className={cn(
                                        "text-muted-foreground",
                                        navigationMenuTriggerStyle,
                                        buttonVariants({
                                            variant: "ghost",
                                        }),
                                    )}>Cart</Link>
                                    {auth?.isAuthenticated && <Link to='/orders' className={cn(
                                        "text-muted-foreground",
                                        navigationMenuTriggerStyle,
                                        buttonVariants({
                                            variant: "ghost",
                                        }),
                                    )}>Orders</Link>}
                                    <Link to="/about" className={cn(
                                        "text-muted-foreground",
                                        navigationMenuTriggerStyle,
                                        buttonVariants({
                                            variant: "ghost",
                                        }),
                                    )}>About</Link>
                                </div>
                                <div className="border-t pt-4">
                                    <div className="mt-2 flex flex-col gap-3">
                                        <AuthAction />
                                    </div>
                                </div>
                            </SheetContent>
                        </Sheet>
                    </div>
                </div>
            </div>
        </section>
    );
};

export default Navbar;
