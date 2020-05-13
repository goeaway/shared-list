import React, { FC } from "react";
import styled from "styled-components";
import { ListDTO } from "../types";
import { useHistory } from "react-router";
import { FaPlus } from "react-icons/fa";

export interface ListListProps {
    lists: Array<ListDTO>;
}

const ListList : FC<ListListProps> = ({ lists }) => {
    const { push } = useHistory();

    const onListPreviewClickHandler = (id: string) => {
        push(`/list/${id}`);
    }

    const onNewListClickHandler = () => {
        push("/list");
    }

    return (
        <Container>
            <Title>Your Lists</Title>            
            <ControlBar>
                <NewListButton onClick={onNewListClickHandler}><FaPlus />&nbsp;Add New List</NewListButton>
            </ControlBar>
            <ListsContainer>
                {lists.length === 0 && <EmptyState>No Lists Yet.</EmptyState>}
                {
                    lists.length > 0 && 
                    lists.map(l => 
                        <ListPreview key={l.id} onClick={() => onListPreviewClickHandler(l.id)}>
                            {l.name}
                        </ListPreview>)
                }
            </ListsContainer>
        </Container>
    );
}

export default ListList;

const Container = styled.div`

`

const ControlBar = styled.div`

`

const Title = styled.span`

`

const EmptyState = styled.div`

`

const ListsContainer = styled.div`

`

const ListPreview = styled.div`

`

const NewListButton = styled.button`

`