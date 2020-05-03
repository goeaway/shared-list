import React, { FC, useState, useEffect } from "react";
import { useParams, useHistory } from "react-router";
import styled from "styled-components";
import List from "../list";
import { ListDTO } from "../../types";
import { createNewList } from "../../utils/create-new-list";
import { useToasts } from "react-toast-notifications";

const ListPage : FC = ({}) => {
    const { id } = useParams();
    const { replace } = useHistory();
    const [ready, setReady] = useState(false);
    const [list, setList] = useState<ListDTO>();
    const { addToast } = useToasts();

    useEffect(() => {
        if(id) {
            fetch(`https://localhost:44327/list/get/${id}`)
                .then((response) => {
                    if(response.ok) {
                        response.json().then((data: ListDTO) => {
                            setList(data);
                        });
                    } else {
                        addToast(<span>Couldn't find your list. A new one has been created</span>, {
                            appearance: 'error',
                            autoDismiss: true
                        });
                        setList(createNewList());
                    }
                })
                .catch((reason) => {
                    addToast(<span>Couldn't find your list. A new one has been created</span>, {
                        appearance: 'error',
                        autoDismiss: true
                    });
                    setList(createNewList());
                })
                .finally(() => {
                    setReady(true);
                });
        } else {
            setReady(true);
            setList(createNewList());
        }
    }, [id]);

    const onListChangeHandler = async (newList: ListDTO) => {
        // if we have an id just update, otherwise create
        if(newList.id) {
            const result = await fetch("https://localhost:44327/list/update", {
                method: "PUT",
                headers: new Headers({ 'content-type': 'application/json' }),
                body: JSON.stringify(newList)
            });

            if(!result.ok) {
                addToast(<span>Couldn't update list. Your changes might not be synchronised.</span>, {
                    appearance: 'error'
                });
            }
        } else {
            const result = await fetch("https://localhost:44327/list/create", {
                method: "POST",
                headers: new Headers({ 'content-type': 'application/json' }),
                body: JSON.stringify(newList)
            });

            if(!result.ok) {
                addToast(<span>Couldn't create list. Your changes might not be synchronised.</span>, {
                    appearance: 'error'
                });
            } else {
                const newId = await result.text();
                replace(newId);
            }
        }
    }

    return (
        <AppContainer>
            {!ready && <LoadingContainer>Loading...</LoadingContainer>}
            {ready && list && <ListContainer><List list={list} canCopy={false} onChange={onListChangeHandler} /> </ListContainer>}
        </AppContainer>
    );
}

export default ListPage;

const AppContainer = styled.div`
    height: 100vh;
    width: 100vw;
    display:flex;
    justify-content: center;
    overflow-x: hidden;
`

const ListContainer = styled.div`
    width: 100%;
    margin: 5rem 2rem;
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