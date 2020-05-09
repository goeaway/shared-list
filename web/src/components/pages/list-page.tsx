import React, { FC, useState, useEffect } from "react";
import { useParams, useHistory } from "react-router";
import styled from "styled-components";
import List from "../list";
import { ListDTO } from "../../types";
import { createNewList } from "../../utils/create-new-list";
import { useToasts } from "react-toast-notifications";
import { HubConnectionBuilder, HubConnection } from "@aspnet/signalr";

const ListPage : FC = ({}) => {
    const { id } = useParams();
    const { replace } = useHistory();
    const [ready, setReady] = useState(false);
    const [list, setList] = useState<ListDTO>();
    const [connection, setConnection] = useState<HubConnection>(null);
    const { addToast } = useToasts();

    useEffect(() => {
        if(connection) {
            const startConnection = async () => {
                connection.on("updatelist", (data: ListDTO) => {
                    setList(data);
                });

                await connection.start();
                await connection.invoke("joinlist", id);
            }

            startConnection();
        }
    }, [connection]);

    useEffect(() => {
        if(id) {
            const timerStart = performance.now();
            fetch(`https://localhost:44327/list/get/${id}`)
                .then(response => {
                    if(response.ok) {
                        response.json().then((data: ListDTO) => {
                            setList(data);
                            // establish connection here
                            setConnection(new HubConnectionBuilder()
                                .withUrl("https://localhost:44327/listHub")
                                .build());
                        });
                    } else {
                        addToast(<span>Couldn't find your list. A new one has been created</span>, {
                            appearance: 'error',
                            autoDismiss: true
                        });
                        setList(createNewList());
                    }
                })
                .catch(reason => {
                    addToast(<span>Couldn't find your list. A new one has been created</span>, {
                        appearance: 'error',
                        autoDismiss: true
                    });
                    setList(createNewList());
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

    const onListChangeHandler = async (newList: ListDTO) => {
        // if we have an id just update, otherwise create
        if(newList.id) {
            if(connection) {
                await connection.invoke("updatelist", newList);
            } else {
                const result = await fetch("https://localhost:44327/list/update", {
                    method: "PUT",
                    headers: new Headers({ 'content-type': 'application/json' }),
                    body: JSON.stringify(newList)
                });
            }
        } else {
            const result = await fetch("https://localhost:44327/list/create", {
                method: "POST",
                headers: new Headers({ 'content-type': 'application/json' }),
                body: JSON.stringify(newList)
            });

            if(result.ok) {
                const newId = await result.text();
                replace(newId);
            }
        }
    }

    return (
        <PageContainer>
            <ContentContainer>
                {!ready && <LoadingContainer>Loading...</LoadingContainer>}
                {ready && list && <ListContainer><List list={list} canCopy={!!id} onChange={onListChangeHandler} /> </ListContainer>}
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