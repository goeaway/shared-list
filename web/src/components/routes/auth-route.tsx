import React, { FC } from "react";
import { Route, RouteProps, Redirect } from "react-router-dom";
import useAuth from "../../hooks/use-auth";

export interface AuthRouteProps extends RouteProps {
    component: any;
}

const AuthRoute : FC<AuthRouteProps> = ({ component: Component, ...rest}) => {
    const isAuthed = useAuth();

    return (
        <Route {...rest} render={(props) => 
            isAuthed ?
            (
            <Component {...props} />
            ) : (
                <Redirect to="/login" />
            )}/>
    )
}

export default AuthRoute;