import React, { FC, useState, useEffect } from "react";
import { useParams, useHistory } from "react-router";
import styled from "styled-components";
import List from "../list";
import { ListDTO } from "../../types";
import { createNewList } from "../../utils/create-new-list";
import { useToasts } from "react-toast-notifications";
import { HubConnectionBuilder, HubConnection, HubConnectionState } from "@aspnet/signalr";
import useAuth from "../../hooks/use-auth";

const ListPage : FC = ({}) => {
    const { id } = useParams();
    const { replace } = useHistory();
    const [ready, setReady] = useState(false);
    const [list, setList] = useState<ListDTO>();
    const [connection, setConnection] = useState<HubConnection>(null);
    const { addToast } = useToasts();
    const { authData, setAuthentication } = useAuth();

    useEffect(() => {
        if(connection) {
            const startConnection = async () => {
                connection.on("updatelist", (data: ListDTO, user: string) => {
                    setList(data);
                    addToast(<span>{user} updated the list.</span>, {
                        appearance: "info",
                        autoDismiss: true
                    });
                });

                await connection.start();
                await connection.invoke("joinlist", id);
            }

            startConnection();

            return async () => {
                await connection.stop();
            }
        }
    }, [connection]);

    useEffect(() => {
        if(id) {
            const timerStart = performance.now();
            const errorHandler = () => {
                addToast(<span>Couldn't find your list. A new one has been created</span>, {
                    appearance: 'error',
                    autoDismiss: false
                });
                setList(createNewList());
                replace("/list");
            }

            fetch(`https://localhost:44327/list/get/${id}`, {
                method: "GET",
                headers: { 
                    'Authorization': `Bearer ${authData.jwt}` 
                },
            })
            .then(response => {
                if(response.ok) {
                    response.json().then((data: ListDTO) => {
                        setList(data);
                        // establish connection here
                        setConnection(new HubConnectionBuilder()
                            .withUrl("https://localhost:44327/listHub", { accessTokenFactory: () => authData.jwt })
                            .build());
                    });
                } else {
                    if(response.status === 401) {
                        setAuthentication(undefined);
                    }
                    errorHandler();
                }
            })
            .catch(reason => {
                errorHandler();
            })
            .finally(() => {
                // avoid jumps by forcing us to wait for at least 400 milliseconds
                const timerStop = performance.now();
                const waitTime = 400 - (timerStop - timerStart);
                if(waitTime > 0) {
                    setTimeout(() => {
                        setReady(true);
                    }, waitTime);
                } else {
                    setReady(true);
                }
            });
        } else {
            setReady(true);
            setList(createNewList());
        }
    }, [id]);

    async function onListChangeHandler (newList: ListDTO) {
        // if we have an id just update, otherwise create
        if(newList.id) {
            if(connection) {
                await connection.invoke("updatelist", newList);
            } else {
                const result = await fetch("https://localhost:44327/list/update", {
                    method: "PUT",
                    headers: { 'content-type': 'application/json', 'Authorization': `Bearer ${authData.jwt}` },
                    body: JSON.stringify(newList)
                });
            }
        } else {
            const handleCreateError = () => {
                addToast(<span>Couldn't save your list.</span>, {
                    appearance: "error",
                    autoDismiss: false
                });
            }

            try {
                const result = await fetch("https://localhost:44327/list/create", {
                    method: "POST",
                    headers: { 
                        'content-type': 'application/json', 
                        'Authorization': `Bearer ${authData.jwt}` 
                    },
                    body: JSON.stringify(newList)
                });
    
                if(result.ok) {
                    const newId = await result.text();
                    replace(`/list/${newId}`);
                    addToast(<span>Your list has been saved. Share your list to collaborate with others.</span>, {
                        appearance: "info",
                        autoDismiss: true
                    });
                } else {
                    if(result.status === 401) {
                        setAuthentication(undefined);
                    }
                    handleCreateError();
                }
            } catch {
                handleCreateError();
            }
        }
    }

    return (
        <PageContainer>
            <ContentContainer>
                {/* {!ready && <LoadingContainer>Loading...</LoadingContainer>} */}
                {list && <ListContainer><List list={list} canCopy={!!id} showCopy onChange={onListChangeHandler} /> </ListContainer>}
            </ContentContainer>
        </PageContainer>
    );
}

export default ListPage;

const PageContainer = styled.div`
    height: 100vh;
    width: 100vw;
    display:flex;
    position: relative;
`

const ContentContainer = styled.div`
    display:flex;
    justify-content: center;
    overflow-x: hidden;
    flex: 1 1 auto;
    padding: 5rem 1rem;
`

const ListContainer = styled.div`
    width: 100%;
    transition: width .3s ease;

    @media(min-width: 800px) {
        width: 75%;
    }

    @media(min-width: 1100px) {
        width: 50%;
    }
`

const LoadingContainer = styled.div`
    padding: 5rem;
`