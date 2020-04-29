import React, { FC, useRef } from "react";
import styled, { ThemeProvider } from "styled-components";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import ListPage from "./pages/list-page";
import Light from "../themes/light";
import { ToastProvider } from "react-toast-notifications";
import ConnectivityToaster from "./utils/connectivity-toaster";

const App : FC = () => {
    return (
        <ThemeProvider theme={Light}>
            <ToastProvider>
                <ConnectivityToaster />
                <Router>
                    <AppContainer>
                        <Switch>
                            <Route path="/:id?">
                                <ListPage />
                            </Route>
                        </Switch>
                    </AppContainer>
                </Router>
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