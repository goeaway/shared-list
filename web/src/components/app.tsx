import React, { FC } from "react";
import styled, { ThemeProvider } from "styled-components";
import { BrowserRouter as Router, Switch, Route, Redirect } from "react-router-dom";
import ListPage from "./pages/list-page";
import Light from "../themes/light";
import { ToastProvider } from "react-toast-notifications";
import ConnectivityToaster from "./utils/connectivity-toaster";
import AuthContext from "../context/auth";
import AuthRoute from "./routes/auth-route";
import LoginPage from "./pages/login-page";
import HomePage from "./pages/home-page";

const App : FC = () => {
    return (
        <ThemeProvider theme={Light}>
            <ToastProvider>
                <AuthContext.Provider value={false}>
                    <ConnectivityToaster />
                    <Router>
                        <AppContainer>
                            <Switch>
                                <AuthRoute path="/list/:id?" component={ListPage} />
                                <Route exact path="/" component={HomePage} />
                                <Route path="/login" component={LoginPage} />
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