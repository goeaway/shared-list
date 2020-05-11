import React, { FC } from "react";
import styled from "styled-components";
import useAuth from "../../hooks/use-auth";
import ListList from "../list-list";
import List from "../list";
import { v1 } from "uuid";
import GoogleLogin from "react-google-login";
import { useHistory } from "react-router";
import { AuthenticationResponse } from "../../types";

const HomePage : FC<any> = ({ location }) => {
    const { authData, isAuthed, setAuthentication } = useAuth();
    const { push } = useHistory();
    const demoList = { id: v1(), name: "demo", items: [] };

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
                    name: response.name,
                    image: response.image
                },
                expires: response.expires
            });

            // redirect to referrer
            push(location.state.referrer || "/");
        }
    }

    const failureHandler = (response) => {

    }

    return (
        <Container>
            <Title>Share The Shop</Title>
            <ContentContainer>
                {
                    !isAuthed(authData) && 
                        <>
                            <ContentLeft><List list={demoList} /></ContentLeft>
                            <ContentRight>
                                <Copy>
                                    Create and Share shopping lists. Get started now, for free, by signing in below.
                                </Copy>
                                <GoogleLogin 
                                    clientId="787759781218-fa57asnept105qrlmv80tf4877jgkhvk.apps.googleusercontent.com"
                                    onSuccess={successHandler}
                                    onFailure={failureHandler}
                                    cookiePolicy="single_host_origin"
                                />
                            </ContentRight>
                        </>
                }
                { 
                    isAuthed(authData) &&
                    <ListList />
                }
            </ContentContainer>
        </Container>
    );
}

export default HomePage;

const Container = styled.div`
    height: 100vh;
    width: 100vw;
    display:flex;
    position: relative;
    padding: 8rem 2rem;
    flex-direction: column;
    transition: padding 300ms ease;

    @media(min-width: 600px) {
        padding: 8rem;
    }

    @media(min-width: 900px) {
        padding: 8rem 12rem;
    }

    @media(min-width: 1200px) {
        padding: 8rem 18rem;
    }

    @media(min-width: 1500px) {
        padding: 8rem 28rem;
    }
`

const ContentContainer = styled.div`
    display:flex;
    justify-content: center;
    flex-direction: column;
    
    > * {
        width: 100%;
        transition: width 300ms ease;
    }

    > *:first-child {
        margin-bottom: 1rem;
    }
    
    @media(min-width: 800px) {
        flex-direction: row;
        
        > * {
            padding-top: 1rem;
            width: 50%;
        }

        > *:first-child {
            padding-right: 1rem;
            margin-bottom: 0;
        }
        
        > *:last-child {
            padding-left: 1rem;
        }
    }
`
    
const Title = styled.span`
    font-size: 30px;
    line-height: 42px;
`

const ContentLeft = styled.div`
    flex: 1 1 auto;
`

const ContentRight = styled.div`
    flex: 1 1 auto;
    display: flex;
    flex-direction: column;

    button {
        display: flex;
        justify-content: center;
    }
`

const Copy = styled.span`
    font-size: 20px;
    line-height: 32px;
    margin-bottom: 2rem;
`