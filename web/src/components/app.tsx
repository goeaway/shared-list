import React, { FC, useState } from "react";
import styled, { ThemeProvider } from "styled-components";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import ListPage from "./pages/list-page";
import Light from "../themes/light";
import { ToastProvider } from "react-toast-notifications";
import ConnectivityToaster from "./utils/connectivity-toaster";
import AuthContext, { AuthenticationData } from "../context/auth";
import AuthRoute from "./routes/auth-route";
import LoginPage from "./pages/login-page";
import HomePage from "./pages/home-page";

const App : FC = () => {
    const [authData, setAuthData] = useState(JSON.parse(localStorage.getItem("authData")) as AuthenticationData);
    const isAuthed = authData && !!authData.jwt;

    const setTokensMiddleware = (authData: AuthenticationData) => {
        localStorage.setItem("authData", JSON.stringify(authData));
        setAuthData(authData);
    }
    
    const logout = () => {
        localStorage.removeItem("authData");
        setAuthData(undefined);
    }

    return (
        <ThemeProvider theme={Light}>
            <ToastProvider>
                <AuthContext.Provider value={{ authData, isAuthed, setAuthentication: setTokensMiddleware }}>
                    {
                        isAuthed && <button type="button" onClick={logout}>Logout</button>
                    }
                    <ConnectivityToaster />
                    <Router>
                        <AppContainer>
                            <Switch>
                                <AuthRoute path="/list/:id?" component={ListPage} />
                                <Route exact path="/" component={HomePage} />
                                <Route path="/login" component={props => <LoginPage {...props} />} />
                            </Switch>
                        </AppContainer>
                    </Router>
                </AuthContext.Provider>
            </ToastProvider>
        </ThemeProvider>
    );
}

export default App;

const AppContainer = styled.div`
    height: 100vh;
    width: 100vw;
    display:flex;
    overflow-x: hidden;
    color: ${p => p.theme.fontLight5};
    background: ${p => p.theme.background1};

    > * {
        flex: 1 1 auto;
    }
`