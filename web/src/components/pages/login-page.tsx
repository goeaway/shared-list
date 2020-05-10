import React, { FC, useEffect } from "react";
import { GoogleLogin } from "react-google-login";
import useAuth from "../../hooks/use-auth";
import styled from "styled-components";
import { useHistory, Redirect } from "react-router";
import { AuthenticationResponse } from "../../types";
import { AuthenticationData } from "../../context/auth";

const LoginPage : FC<any> = ({ location }) => {
    const { isAuthed, setAuthentication } = useAuth();
    const { push } = useHistory();

    const successHandler = async (response) => {
        // make request to API to register this user and get an API auth token
        const result = await fetch(`https://localhost:44327/auth/authenticate?idToken=${response.tokenId}`, {
            method: "POST"
        });

        if(result.ok) {
            const response = (await result.json()) as AuthenticationResponse;
            setAuthentication({
                jwt: response.jwt,
                profile: {
                    email: response.email,
                    name: response.name
                }
            });
        }

        // redirect to referrer
        push(location.state.referrer || "/");
    }

    const failureHandler = (response) => {

    }

    return (
        <>
            {
                isAuthed ? <Redirect to="/" /> : 
                <PageContainer>
                    <GoogleLogin 
                        clientId="787759781218-fa57asnept105qrlmv80tf4877jgkhvk.apps.googleusercontent.com"
                        onSuccess={successHandler}
                        onFailure={failureHandler}
                        cookiePolicy="single_host_origin"
                    />
                </PageContainer>
            }
        </>
    );
}

export default LoginPage;

const PageContainer = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
`

const StyledLoginButton = styled.div``