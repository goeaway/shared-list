import React, { FC, useState, useEffect } from "react";
import styled from "styled-components";
import useAuth from "../../hooks/use-auth";
import ListList from "../list-list";
import List from "../list";
import { v1 } from "uuid";
import GoogleLogin from "react-google-login";
import { useHistory } from "react-router";
import { AuthenticationResponse, ListDTO, ListPreviewDTO } from "../../types";
import { config } from "@config/production";

const HomePage : FC<any> = ({ location }) => {
    const { authData, isAuthed, setAuthentication } = useAuth();
    const { push } = useHistory();
    const [demoList, setDemoList] = useState<ListDTO>({id: v1(), name: "", items: []});
    const [userLists, setUserLists] = useState<Array<ListPreviewDTO>>([]);
    const [adding, setAdding] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if(isAuthed(authData)) {
            // make request for user lists
            const timerStart = performance.now();
            fetch(`${config.apiURL}/list/getlistpreviews`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${authData.jwt}`
                }
            })
            .then(response => {
                if(response.ok) {
                    response.json().then(json => {
                        setUserLists(json as Array<ListPreviewDTO>);
                    });
                } else if(response.status === 401) {
                    setAuthentication(undefined);
                }
            })
            .finally(() => {
                // avoid jumps by forcing us to wait for at least 400 milliseconds
                const timerStop = performance.now();
                const waitTime = 400 - (timerStop - timerStart);
                if(waitTime > 0) {
                    setTimeout(() => {
                        setLoading(false);
                    }, waitTime);
                } else {
                    setLoading(false);
                }
            });
        } else {
            fetch(`${config.apiURL}/list/getname`)
            .then(response => {
                if(response.ok) {
                    response.text().then(name => {
                        setDemoList({id: v1(), name, items: []});
                    });
                }
            })
        }
    }, []);

    const loginSuccess = async (response) => {
        // make request to API to register this user and get an API auth token
        const result = await fetch(`${config.apiURL}/auth/google?idToken=${response.tokenId}`, {
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
            push(location.state && location.state.referrer || "/");
        }
    }

    const loginFailure = (response) => {

    }

    const onDeleteListHandler = async (id: string) => {
        setUserLists(lists => {
            const copy = [...lists];
            const deleted = copy.findIndex(c => c.id === id);
            copy.splice(deleted, 1);
            return copy;
        });

        const result = await fetch(`${config.apiURL}/list/delete/${id}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${authData.jwt}`
            }
        });

        if(result.status === 401) {
            setAuthentication(undefined);
        }
    }

    const onAddListHandler = () => {
        if(userLists.length >= config.limits.lists) {
            return;
        }

        setAdding(true);

        fetch(`${config.apiURL}/list/createempty`, {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${authData.jwt}`
            }
        }).then(async result => {
            if(result.ok) {
                const newId = await result.text();
                push(`/list/${newId}`);
            } else if(result.status === 401) {
                setAuthentication(undefined);
            }
        }).finally(() => {
            setAdding(false);
        });
    }

    return (
        <Container>
            <Title>Share The Shop</Title>
            {
                !isAuthed(authData) && 
                <DemoContainer> 
                    <ContentLeft><List list={demoList}/></ContentLeft>
                    <ContentRight>
                        <Copy>
                            Create and collaborate on shopping lists in real time. Get started now by signing in below.
                        </Copy>
                        <GoogleLogin 
                            clientId={config.googleAuthClientId}
                            onSuccess={loginSuccess}
                            onFailure={loginFailure}
                            cookiePolicy="single_host_origin"
                        />
                    </ContentRight>
                </DemoContainer>
            }
            { 
                isAuthed(authData) && 
                    <ListList lists={userLists} onDelete={onDeleteListHandler} onAdd={onAddListHandler} loading={loading} adding={adding} />
            }
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

const DemoContainer = styled.div`
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
    margin-bottom: 1rem;
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

const ListContainer = styled.div`

`