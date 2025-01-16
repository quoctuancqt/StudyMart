import { useAuth } from "react-oidc-context";
import { Button } from "./ui/button";

const AuthAction = () => {
    const auth = useAuth();

    return (
        <>
            {auth.isAuthenticated ? (
                <Button variant='destructive' onClick={() => void auth.signoutRedirect()}>Logout</Button>
            ) : (
                <>
                    <Button variant='outline' onClick={() => void auth.signinRedirect()}>Login</Button>
                    <Button>Sign up</Button>
                </>

            )}
        </>
    )
};

export default AuthAction;