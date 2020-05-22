import React, { FC, useState, useCallback } from "react";
import styled, { ThemeProvider } from "styled-components";
import { BrowserRouter as Router, Switch, Route, useLocation } from "react-router-dom";
import ListPage from "./pages/list-page";
import Light from "../themes/light";
import { ToastProvider } from "react-toast-notifications";
import ConnectivityToaster from "./utils/connectivity-toaster";
import AuthContext, { AuthenticationData } from "../context/auth";
import AuthRoute from "./routes/auth-route";
import HomePage from "./pages/home-page";
import { storeAuthData, getAuthData } from "../utils/storage";
import { isAuthenticated } from "../utils/authentication";
import UserMenu from "./user-menu";
import { FaHome } from "react-icons/fa";
import { IconLink } from "./style/links";
import { CSSTransition, TransitionGroup } from "react-transition-group";
import NotFoundPage from "./pages/not-found-page";

const App : FC = () => {
    const [authData, setAuthData] = useState(getAuthData);
    const isAuthed = useCallback(() => isAuthenticated(authData), [authData]);
    const setTokensMiddleware = (authData: AuthenticationData) => {
        storeAuthData(authData);
        setAuthData(authData);
    }
    const location = useLocation();
    
    return (
        <ThemeProvider theme={Light}>
            <ToastProvider>
                <AuthContext.Provider value={{ authData, isAuthed, setAuthentication: setTokensMiddleware }}>
                    <ConnectivityToaster />
                        <AppContainer>
                            <ControlBar>
                                <HomeLink to="/" size={25}><FaHome /></HomeLink>
                                {isAuthed() && <UserMenu />}
                            </ControlBar>
                            <TransitionGroup>
                                <CSSTransition key={location.key} timeout={300} classNames="fade">
                                    <Section>
                                        <Switch>
                                            <AuthRoute path="/list/:id?" component={ListPage} />
                                            <Route exact path="/" component={props => <HomePage {...props} />} />
                                            <Route component={NotFoundPage} />
                                        </Switch>
                                    </Section>
                                </CSSTransition>
                            </TransitionGroup>
                        </AppContainer>
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
    position: relative;

    div.transition-group {
        position: relative;
    }

    .fade-enter {
        opacity: 0;
        visibility: hidden;
    }

    .fade-enter-active {
        opacity: 1;
        visibility: visible;
        transition: all 300ms;
    }

    .fade-exit {
        opacity: 1;
        visibility: visible;
    }

    .fade-exit-active {
        opacity: 0;
        visibility: hidden;
        transition: all 300ms;
    }
`

const HomeLink = styled(IconLink)`
    z-index: 1000;
    width: 35px;
    height: 35px;
    padding: .45rem;
`

const ControlBar = styled.div`
    background: transparent;
    width: 100%;
    position: absolute;
    display: flex;
    justify-content: space-between;
    z-index: 1000;
    padding: .75rem;
`

const Section = styled.section`
    position: absolute;
`