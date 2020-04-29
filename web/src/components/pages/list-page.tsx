import React, { FC, useState, useEffect } from "react";
import { useParams, useHistory } from "react-router";
import styled from "styled-components";
import List from "../list";
import { ListDTO } from "../../types";
import { createNewList } from "../../utils/create-new-list";
import { useToasts } from "react-toast-notifications";

const ListPage : FC = ({}) => {
    const { id } = useParams();
    const [ready, setReady] = useState(false);
    const [list, setList] = useState<ListDTO>();
    const { addToast } = useToasts();

    useEffect(() => {
        if(id) {
            fetch(`http://localhost:3000/lists/${id}`)
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

    return (
        <AppContainer>
            {!ready && <LoadingContainer>Loading...</LoadingContainer>}
            {ready && list && <ListContainer><List list={list} canCopy={false} /> </ListContainer>}
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